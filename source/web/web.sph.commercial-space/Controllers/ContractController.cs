using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class ContractController : Controller
    {
        public async Task<ActionResult> Create(Contract contract, int rentalApplicationId)
        {
            if (0 == rentalApplicationId) throw new ArgumentException("RentalApplicationId cannot be 0", "rentalApplicationId");
            if (0 != contract.ContractId) throw new ArgumentException("Contract must be new", "contract");

            var context = new SphDataContext();
            var application = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == rentalApplicationId);
            var cs = await context.LoadOneAsync<CommercialSpace>(c => c.CommercialSpaceId == application.CommercialSpaceId);
            var audit = new AuditTrail
            {
                Operation = "Contract is created from application",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(RentalApplication).Name,
                EntityId = rentalApplicationId,
                Note = "-"
            };


            using (var session = context.OpenSession())
            {
                contract.RentalApplicationId = rentalApplicationId;
                contract.Status = "Active";
                contract.CommercialSpace = cs;
                
                session.Attach(contract, audit);
                await session.SubmitChanges();
            }
            var audit2 = new AuditTrail
            {
                Operation = "Contract is created from application",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(Contract).Name,
                EntityId = rentalApplicationId,
                Note = "-"
            };

            using (var session = context.OpenSession())
            {
                session.Attach(audit2);
                await session.SubmitChanges();
            }

            return Json(contract);
        }

        public async Task<ActionResult> Generate(int rentalApplicationId, int templateId)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<ContractTemplate>(t => t.ContractTemplateId == templateId);
            var app = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == rentalApplicationId);
            var cs = await context.LoadOneAsync<CommercialSpace>(r => r.CommercialSpaceId == app.Offer.CommercialSpaceId);
            var setting = await context.LoadOneAsync<Setting>(s => s.Key == "Organization");
            var org = JsonConvert.DeserializeObject<Organization>(setting.Value);
            var contract = new Contract
                {
                    Title = string.Format("Kontrak sewaan dengan {0} dan {1}", app.CompanyName, org.Name),
                    ReferenceNo = string.Format("BSPB/2013/{0}", app.RegistrationNo),
                    Date = DateTime.Now,
                    InterestRate = template.InterestRate,
                    Owner = new Owner
                        {
                            Name = org.Name,
                            Email = org.Email,
                            TelephoneNo = org.OfficeNo,
                            FaxNo = org.FaxNo,
                            Address = org.Address
                        },
                    Type = template.Type,
                    CommercialSpace = cs,
                    Period = app.Offer.Period,
                    PeriodUnit = app.Offer.PeriodUnit,
                    Option = app.Offer.Option,
                    Value = app.Offer.Deposit + app.Offer.Period * 12 * app.Offer.Rent,
                    Tenant = new Tenant
                        {
                            IdSsmNo = app.CompanyRegistrationNo ?? app.Contact.IcNo,
                            RegistrationNo = app.RegistrationNo,
                            Name = app.CompanyName ?? app.Contact.Name,
                            Address = app.Address
                        },
                    ContractingParty = new ContractingParty
                        {
                            Contact = app.Contact,
                            Name = app.CompanyName ?? app.Contact.Name,
                            RegistrationNo = app.RegistrationNo,
                            Address = app.Address
                        },
                        StartDate = app.Offer.ExpiryDate,
                        EndDate = app.Offer.ExpiryDate.AddYears(1)
                };

            contract.TopicCollection.AddRange(template.TopicCollection);

            var json = await JsonConvert.SerializeObjectAsync(contract);
            this.Response.ContentType = "application/json";

            return Content(json);
        }

        public async Task<ActionResult> GenerateDocument(int id, string templateId, string title, string remarks)
        {

            var context = new SphDataContext();
            var contract = await context.LoadOneAsync<Contract>(r => r.ContractId == id);

            var audit = new AuditTrail
            {
                Operation = "Generate agreement document",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(RentalApplication).Name,
                EntityId = id,
                Note = remarks
            };


            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var file = await store.GetContentAsync(templateId);
            var temp = System.IO.Path.GetTempFileName() + ".docx";
            System.IO.File.WriteAllBytes(temp, file.Content);
            var word = ObjectBuilder.GetObject<IDocumentGenerator>();

            var sessionKey = Guid.NewGuid().ToString();
            var fileName = string.Format("{0}-{1:yyyyMMdd}.{2}.docx", contract.ReferenceNo, DateTime.Today, title);
            var doc = new Document { Title = title, Extension = ".docx" };
            doc.DocumentVersionCollection.Add(new DocumentVersion
                {
                    No = "1",
                    Date = DateTime.Now,
                    CommitedBy = User.Identity.Name,
                    Note = "Newly generated",
                    StoreId = sessionKey
                });

            var docItem = new BinaryStore
            {
                StoreId = sessionKey,
                Extension = ".docx",
                Content = System.IO.File.ReadAllBytes(temp),
                FileName = fileName
            };
            await store.AddAsync(docItem);

            contract.DocumentCollection.Add(doc);
            using (var session = context.OpenSession())
            {
                session.Attach(audit, contract);
                await session.SubmitChanges();
            }
            word.Generate(temp, contract, audit);
            return Json(doc);
        }

        public async Task<ActionResult> Save(Contract contract)
        {
            var audit2 = new AuditTrail
            {
                Operation = "Contract is changed",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(Contract).Name,
                EntityId = contract.ContractId,
                Note = "-"
            };

            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<Contract>(c => c.ContractId == contract.ContractId);
            if (dbItem != contract)
            {
                var changetracker = new ChangeGenerator();
                audit2.ChangeCollection.AddRange(changetracker.GetChanges(dbItem, contract));

                dbItem.Title = contract.Title;
                dbItem.ReferenceNo = contract.ReferenceNo;
                dbItem.Value = contract.Value;
                dbItem.Period = contract.Period;
                dbItem.PeriodUnit = contract.PeriodUnit;
                dbItem.Option = contract.Option;
                dbItem.Owner = contract.Owner;
                dbItem.ContractingParty = contract.ContractingParty;
                dbItem.Remarks = contract.Remarks;
                dbItem.DocumentCollection.ClearAndAddRange(contract.DocumentCollection);
                dbItem.TopicCollection.ClearAndAddRange(contract.TopicCollection);

            }

            using (var session = context.OpenSession())
            {
                session.Attach(dbItem, audit2);
                await session.SubmitChanges();
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await JsonConvert.SerializeObjectAsync(contract));

        }

        public async Task<ActionResult> GenerateLedger(int id = 0)
        {
            var contractId = id != 0 ? id : 1;
            var context = new SphDataContext();
            var contract = await context.LoadOneAsync<Contract>(c => c.ContractId == contractId);
            var invoiceLo = await context.LoadAsync(context.Invoices.Where(r => r.ContractNo == contract.ReferenceNo));

            var export = ObjectBuilder.GetObject<ILedgerExport>();
            var filename = string.Format("{0}-{1:MMyyyy}.lejer.xlsx", contract.Tenant.RegistrationNo, DateTime.Today);
            var temp = System.IO.Path.GetTempFileName() + ".xlsx";

            export.GenerateLedger(contract, invoiceLo.ItemCollection, temp);
            
            this.Response.ContentType = "application/json";
            return File(System.IO.File.ReadAllBytes(temp), MimeMapping.GetMimeMapping(".xlsx"), filename);
        }
    }

}
