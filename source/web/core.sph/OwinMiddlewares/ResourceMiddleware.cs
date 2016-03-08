using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Bespoke.Sph.Domain;
using Microsoft.Owin;
using AppFunc = System.Func<
System.Collections.Generic.IDictionary<string, object>,
System.Threading.Tasks.Task
>;

namespace Bespoke.Sph.Web.OwinMiddlewares
{
    public class ResourceMiddleware
    {
        private readonly AppFunc m_next;
        private readonly string m_namespace;
        private readonly bool m_debugging;

        public ResourceMiddleware(AppFunc next, string @namespace, bool debugging)
        {
            m_next = next;
            m_namespace = @namespace;
            m_debugging = debugging;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var ctx = new OwinContext(environment);
            var fileName = ctx.Request.Path.Value.Remove(0, 1).Replace("/", ".");

            var contentType = MimeMapping.GetMimeMapping(Path.GetExtension(fileName));
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"Bespoke.Sph.Web.{m_namespace}.{fileName}";


            var file = $"{ConfigurationManager.WebPath}\\{m_namespace.Replace(".", "\\")}\\{fileName}";
            if (File.Exists(file))
            {
                SetStaticFileCacheability(ctx, file, "images\\form.element");
                var bytes = File.ReadAllBytes(file);
                await ctx.Response.WriteAsync(bytes);
                return;
            }

            var stream = assembly.GetManifestResourceStream(resourceName);
            if (null != stream)
            {
                this.SetCoreResourceCacheability(ctx);
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    var bytes = memoryStream.ToArray();
                    ctx.Response.ContentType = contentType;
                    await ctx.Response.WriteAsync(bytes);
                    return;
                }
            }


            if (fileName.EndsWith(".js"))
            {
                //Console.WriteLine(RouteData);
                var controller = fileName.Replace("viewmodels.", "")
                 .Replace(".js", "")
                 .Replace(".", "");
                var location = $"/app/{controller}/js";
                ctx.Response.StatusCode = 302;
                ctx.Response.Headers.Add("Location", new[] { location });
                return;

            }

            if (fileName.EndsWith(".html"))
            {
                var controller = fileName
                    .Replace(".html", "")
                    .Replace(".", "");

                var location = $"/app/{controller}/html";
                ctx.Response.StatusCode = 302;
                ctx.Response.Headers.Add("Location", new[] { location });
                return;
            }
            await m_next(environment);

        }



        private void SetCoreResourceCacheability(IOwinContext context)
        {
            if (m_debugging)
            {
                context.Response.Expires = DateTime.UtcNow.AddDays(-1);
                context.Response.Headers.Add("cache-control", new[] { "no-store", "no-cache" });
                return;
            }
            var lastAccessTimeUtc = File.GetLastAccessTimeUtc(($"{ConfigurationManager.WebPath}\\bin\\core.sph.dll"));

            context.Response.Expires = DateTime.UtcNow.AddDays(ConfigurationManager.StaticFileCache);
            context.Response.Headers.Add("cache-control", new[] { "public" });
            context.Response.ETag = GetMd5Hash(lastAccessTimeUtc.ToString(CultureInfo.InvariantCulture));

            context.Response.Headers.Add("Last-Modified", new[] { lastAccessTimeUtc.ToString("s") });
        }

        private void SetStaticFileCacheability(IOwinContext context, string file, params string[] cachePattern)
        {
            var cached = cachePattern.Any(file.Contains);
            if (m_debugging && !cached)
            {
                context.Response.Expires = DateTime.UtcNow.AddDays(-1);
                context.Response.Headers.Add("cache-control", new[] { "no-store", "no-cache" });
                return;
            }
            var lastAccessTimeUtc = File.GetLastAccessTimeUtc(file);
            context.Response.Expires = DateTime.UtcNow.AddDays(ConfigurationManager.StaticFileCache);
            context.Response.Headers.Add("cache-control", new[] { "public" });
            context.Response.ETag = GetMd5Hash(lastAccessTimeUtc.ToString(CultureInfo.InvariantCulture));

            context.Response.Headers.Add("Last-Modified", new[] { lastAccessTimeUtc.ToString("s") });
        }

        static string GetMd5Hash(string input)
        {
            using (var md5Hash = MD5.Create())
            {
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                return string.Join("", data.Select(x => x.ToString("x2")));

            }
        }
    }
}