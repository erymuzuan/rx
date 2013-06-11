using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;
using System.Linq;

namespace Bespoke.Sph.Commerspace.Web.Controllers
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

                    st.Value = settings.Single(s => s.Key == key1).Value;
                    var interest = settings.Single(s => s.Key == key1).InterestCollection;
                    var rebate = settings.Single(s => s.Key == key1).RebateCollection;
                    if (null != interest) st.InterestCollection.AddRange(interest);
                    if (null != rebate) st.RebateCollection.AddRange(rebate);
                   
                    session.Attach(st);

                }
                await session.SubmitChanges();
            }

            return Json(true);

        }

    }
}
