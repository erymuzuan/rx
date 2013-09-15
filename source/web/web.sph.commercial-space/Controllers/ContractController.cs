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
            var cs = await context.LoadOneAsync<Space>(c => c.CommercialSpaceId == application.CommercialSpaceId);
            cs.IsAvailable = false;

            using (var session = context.OpenSession())
            {
                contract.RentalApplicationId = rentalApplicationId;
                contract.Status = "Active";
                contract.Space = cs;
                
                session.Attach(contract,cs);
                await session.SubmitChanges("Contract is created from application");
            }

            return Json(contract);
        }

        public async Task<ActionResult> Generate(int rentalApplicationId, int templateId)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<ContractTemplate>(t => t.ContractTemplateId == templateId);
            var app = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == rentalApplicationId);
            var cs = await context.LoadOneAsync<Space>(r => r.CommercialSpaceId == app.Offer.CommercialSpaceId);
            var setting = await context.LoadOneAsync<Setting>(s => s.Key == "Organization");
            var org = JsonConvert.DeserializeObject<Organization>(setting.Value);
            var contract = new Contract
                {
                    Title = string.Format("Kontrak sewaan {0} dan {1}", app.CompanyName ?? app.Contact.Name, org.Name),
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
                    Space = cs,
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

        public async Task<ActionResult> Terminate(int id,Termination termination)
        {
            
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<Contract>(c => c.ContractId == id);
            dbItem.Termination = termination;
            dbItem.EndDate = termination.Date;
            dbItem.IsEnd = true;

            using (var session = context.OpenSession())
            {
                session.Attach(dbItem);
                await session.SubmitChanges(string.Format("Contract was terminate on {0}", termination.Date));
            }
            return Json(true);
        }

        public async Task<ActionResult> Extend(int id, Extension extension)
        {
           var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<Contract>(c => c.ContractId == id);
            dbItem.Extension = extension;
            dbItem.EndDate = extension.Date;

            using (var session = context.OpenSession())
            {
                session.Attach(dbItem);
                await session.SubmitChanges(string.Format("Contract extended to date {0}", extension.Date));
            }
            return Json(true);
        }

        public async Task<ActionResult> GenerateLedger(int id = 0)
        {
            var contractId = id != 0 ? id : 1;
            var context = new SphDataContext();
            var contract =await context.LoadOneAsync<Contract>(c => c.ContractId == contractId);


            var invoiceQuery = context.Invoices.Where(r => r.ContractNo == contract.ReferenceNo);
            var rebateQuery = context.Rebates.Where(r => r.ContractNo == contract.ReferenceNo);
            var paymentQuery = context.Payments.Where(p => p.ContractNo == contract.ReferenceNo);

            var invoiceLoTask = context.LoadAsync(invoiceQuery,includeTotalRows:true);
            var rebateLoTask = context.LoadAsync(rebateQuery, includeTotalRows:true);
            var paymentLoTask = context.LoadAsync(paymentQuery, includeTotalRows:true);

            await Task.WhenAll(invoiceLoTask, rebateLoTask, paymentLoTask);
            var invoiceLo = await invoiceLoTask;
            var rebateLo = await rebateLoTask;
            var paymentLo = await paymentLoTask;

            var invoices = new ObjectCollection<Invoice>(invoiceLo.ItemCollection);
            while (invoiceLo.HasNextPage)
            {
                invoiceLo = await context.LoadAsync(invoiceQuery, invoiceLo.CurrentPage + 1, includeTotalRows: true);
                invoices.AddRange(invoiceLo.ItemCollection);
            }

            var rebates = new ObjectCollection<Rebate>(rebateLo.ItemCollection);
            while (rebateLo.HasNextPage)
            {
                rebateLo = await context.LoadAsync(rebateQuery, rebateLo.CurrentPage + 1, includeTotalRows: true);
                rebates.AddRange(rebateLo.ItemCollection);

            }

            var payments = new ObjectCollection<Payment>(paymentLo.ItemCollection);
            while (paymentLo.HasNextPage)
            {
                paymentLo = await context.LoadAsync(paymentQuery, paymentLo.CurrentPage + 1, includeTotalRows: true);
                payments.AddRange(paymentLo.ItemCollection);

            }

            var export = ObjectBuilder.GetObject<ILedgerExport>();
            var filename = string.Format("{0}-{1:MMyyyy}.lejer.xlsx", contract.Tenant.RegistrationNo, DateTime.Today);
            var temp = System.IO.Path.GetTempFileName() + ".xlsx";

            export.GenerateLedger(contract,invoices,rebates,payments, temp);
            
            this.Response.ContentType = "application/json";
            return File(System.IO.File.ReadAllBytes(temp), MimeMapping.GetMimeMapping(".xlsx"), filename);
        }
    }

}
