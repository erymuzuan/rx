using System.IO;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Helpers
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