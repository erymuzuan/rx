using System.IO;
using System.Web.Mvc;

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
    }
}