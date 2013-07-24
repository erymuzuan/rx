using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class TriggerController : Controller
    {

        private Field GetField(dynamic field)
        {

            Field left = null;
            // function field
            try
            {
                var ff = new FunctionField
                {
                    Note = field.Note,
                    Name = field.Name,
                    Script = field.Script
                };
                left = ff;
            }
            catch { }

            try
            {
                var df = new DocumentField
                {
                    Note = field.Note,
                    Name = field.Name,
                    XPath = field.XPath,
                    Path = field.Path,
                    TypeName = field.TypeName
                };
                left = df;
            }
            catch { }
            try
            {
                var cf = new ConstantField
                {
                    Note = field.Note,
                    Name = field.Name,
                    Value = field.Value,
                    TypeName = field.TypeName
                };
                left = cf;
            }
            catch { }
            return left;
        }

        public async Task<ActionResult> Save(Trigger trigger)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(trigger);
                await session.SubmitChanges("Submit trigger");
            }


            return Json(true);
        }

    }
}
