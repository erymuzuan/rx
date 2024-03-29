﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using System.Linq;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class SettingController : Controller
    {
        public async Task<ActionResult> Save(IEnumerable<Setting> settings)
        {
            settings = settings.ToList();

            var keys = settings.Select(s => s.Key).ToArray();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                foreach (var key in keys)
                {
                    var key1 = key;
                    var st = await context.LoadOneAsync<Setting>(s => s.Key == key1)
                        ?? settings.Single(s => s.Key == key1);
                    if (string.IsNullOrWhiteSpace(st.Id) || st.Id == "0")
                        st.Id = key;

                    st.Value = settings.Single(s => s.Key == key1).Value;
                    session.Attach(st);

                }
                await session.SubmitChanges();
            }

            return Json(true);

        }

    }
}
