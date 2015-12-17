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
                var text = reader.ReadToEnd();
                return text;
            }
        }

        public static T GetRequestJson<T>(this Controller controller)
        {
            if (null == controller.Request) return default(T);
            if (null == controller.Request.InputStream) return default(T);
            if (controller.Request.InputStream.CanSeek)
                controller.Request.InputStream.Position = 0;
            using (var reader = new StreamReader(controller.Request.InputStream))
            {
                var json = reader.ReadToEnd();
                return json.DeserializeFromJson<T>();
            }
        }
    }
}