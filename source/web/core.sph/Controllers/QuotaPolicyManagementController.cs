using System;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("api/quota-policies")]
    public class QuotaPolicyManagementController : BaseApiController
    {
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Post(QuotaPolicy policy)
        {
            var context = new SphDataContext();
            var baru = policy.IsNewItem;
            if (baru)
                policy.Id = policy.Name.ToIdFormat();

            using (var session = context.OpenSession())
            {
                session.Attach(policy);
                await session.SubmitChanges("Save");
            }
            if (baru) return Created(new Uri($"/api/quota-polices/{policy.Id}", UriKind.Relative), new { success = true, status = "OK", id = policy.Id });
            return Ok();
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            var context = new SphDataContext();
            var policies = context.LoadFromSources<QuotaPolicy>();

            return Ok(policies);

        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            var context = new SphDataContext();
            var policy = context.LoadOneFromSources<QuotaPolicy>(x => x.Id == id);
            if (null == policy)
                return NotFound("Cannot find any Policy with id " + id);

            return Ok(policy);
        }

    }
}