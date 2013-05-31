using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class ContractController : Controller
    {


        public async Task<ActionResult> Create(Contract contract)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(contract);
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
                    Date = DateTime.Today,
                    Owner = new Owner
                        {
                            
                        },
                    Type = template.Type,
                    CommercialSpace = cs,
                    Period = app.Offer.Period,
                    PeriodUnit = app.Offer.PeriodUnit,
                    Option = app.Offer.Option,
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
                    EndDate = app.Offer.ExpiryDate.AddYears(app.Offer.Period),
                    Title = ""

                };

            contract.TopicCollection.AddRange(template.TopicCollection);

            var json = await JsonConvert.SerializeObjectAsync(contract);
            this.Response.ContentType = "application/json";

            return Content(json);
        }

    }
}
