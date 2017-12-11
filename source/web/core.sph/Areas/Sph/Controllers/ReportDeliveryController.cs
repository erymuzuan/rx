﻿using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Extensions;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class ReportDeliveryController : Controller
    {
        public async Task<ActionResult> Save()
        {
            var rd = this.GetRequestJson<ReportDelivery>();
            var context = new SphDataContext();

            if (rd.IsNewItem) rd.Id = rd.ReportDefinitionId + "-" + Guid.NewGuid().ToString().Substring(1,4);

            using (var session = context.OpenSession())
            {
                session.Attach(rd);
                await session.SubmitChanges("Save");
            }

            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(JsonConvert.SerializeObject(rd.Id));

        }

    }
}
