﻿using System.IO;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Extensions
{
    public static class ControllerExtension
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
            if (controller.Request?.InputStream == null) return default;
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