using System.IO;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Helpers
{
    public static class ControllerHelpers
    {

        public static string GetRequestBody(this Controller controller)
        {

            using (var reader = new StreamReader(controller.Request.InputStream))
            {
                string text = reader.ReadToEnd();
                return text;
            }
        }

        public static T GetRequestJson<T>(this Controller controller)
        {
            using (var reader = new StreamReader(controller.Request.InputStream))
            {
                string json = reader.ReadToEnd();
                return JsonSerializerService.DeserializeFromJson<T>(json);
            }
        }
    }
}