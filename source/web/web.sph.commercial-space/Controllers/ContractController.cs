using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class ContractController : Controller
    {
        public async Task<ActionResult> Create(Contract contract, int rentalApplicationId)
        {
            if(0 == rentalApplicationId)throw new ArgumentException("RentalApplicationId cannot be 0", "rentalApplicationId");
            if(0 != contract.ContractId)throw new ArgumentException("Contract must be new", "contract");

            var context = new SphDataContext();

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
                session.Attach(contract,audit);
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

            var contract = new Contract
                {
                    Title = string.Format("Kotrak sewaan dengan {0} dan {1}", app.CompanyName, "Bespoke"),
                    ReferenceNo = string.Format("BSPB/2013/{0}", app.RegistrationNo),
                    Date = DateTime.Today,
                    Owner = new Owner
                        {
                            Name = "To get from settings",
                            Email = "someone@bespoke.com.my"
                        },
                    Type = template.Type,
                    CommercialSpace = cs,
                    Period = app.Offer.Period,
                    PeriodUnit = app.Offer.PeriodUnit,
                    Option = app.Offer.Option,
                    Value = app.Offer.Deposit + app.Offer.Period * 12 * app.Offer.Rent,
                    Tenant = new Tenant
                        {
                            Id = app.CompanyRegistrationNo ?? app.Contact.IcNo,
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
                    EndDate = app.Offer.ExpiryDate.AddYears(app.Offer.Period)

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



    }
}
