﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize(Roles = "developers, administrators")]
    [RoutePrefix("adapter")]
    public class AdapterController : BaseApiController
    {

        [Route("installed-adapters")]
        [HttpGet]
        public IHttpActionResult InstalledAdapters()
        {
            var actions = from a in this.DeveloperService.Adapters
                          select
                              $@"
{{
    ""designer"" : {JsonConvert.SerializeObject(a.Metadata)},
    ""adapter"" : {a.Value.ToJsonString()}
}}";
           

            return Json("[" + string.Join(",", actions) + "]");

        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync(string id)
        {
            var context = new SphDataContext();
            var ef = await context.LoadOneAsync<Adapter>(x => x.Id == id);
            if (null == ef)
                return NotFound("Cannot find adapter with id " + id);

            using (var session = context.OpenSession())
            {
                session.Delete(ef);
                await session.SubmitChanges("Delete");
            }
            return Ok(new { success = true, status = "OK", id = ef.Id });
        }
        [Route("")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync([JsonBody]Adapter adapter)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Delete(adapter);
                await session.SubmitChanges("Delete");
            }
            return Ok(new { success = true, status = "OK", id = adapter.Id });
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Create([JsonBody]Adapter adapter)
        {
            if (null == adapter)
                return BadRequest("Cannot deserialize adapter");

            var context = new SphDataContext();
            adapter.Id = adapter.Name.ToIdFormat();

            using (var session = context.OpenSession())
            {
                session.Attach(adapter);
                await session.SubmitChanges("Save");
            }


            var result = new
            {
                success = true,
                status = "OK",
                id = adapter.Id,
                link = new
                {
                    rel = "self",
                    href = "adapter/" + adapter.Id
                }
            };
            return Created($"/api/adapters/{adapter.Id}", result);
        }



        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> Save([JsonBody]Adapter adapter, string id)
        {
            if (null == adapter)
                return NotFound("Cannot deserialize adapter");

            adapter.Id = id;
            var vr = (await adapter.ValidateAsync()).ToArray();
            if (vr.Any())
            {
                return Json(new { success = false, status = "Not Valid", errors = vr });
            }

            var context = new SphDataContext();
            var baru = adapter.IsNewItem;
            if (baru)
            {
                adapter.Id = adapter.Name.ToIdFormat();
            }

            using (var session = context.OpenSession())
            {
                session.Attach(adapter);
                await session.SubmitChanges("Save");
            }
            var result = new
            {
                success = true,
                status = "OK",
                id = adapter.Id,
                link = new
                {
                    rel = "self",
                    href = "adapter/" + adapter.Id
                }
            };
            if (baru) return Created($"/api/adapters/{adapter.Id}", result);
            return Ok(result);
        }

        


    }
}