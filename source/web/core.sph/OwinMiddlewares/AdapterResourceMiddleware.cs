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
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.WebApi;
using Microsoft.Owin;

namespace Bespoke.Sph.Web.OwinMiddlewares
{
    public class AdapterResourceMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> m_next;
        private readonly string m_namespace;
        private readonly bool m_debugging;

        public AdapterResourceMiddleware(Func<IDictionary<string, object>, Task> next, string @namespace, bool debugging)
        {
            DeveloperService.Init();
            m_next = next;
            m_namespace = @namespace;
            m_debugging = debugging;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var ctx = new OwinContext(environment);
            var paths = ctx.Request.Path.Value.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var folder = "";
            if (paths.Length > 1)
                folder = "\\" + string.Join("\\", paths.Take(paths.Length - 1));
            var fileName = paths.Last() ?? "";
            if (!fileName.StartsWith("adapter."))
            {
                await m_next(environment);
                return;
            }
            var adapterName = fileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1];

            var contentType = "text/plain";
            if (!string.IsNullOrWhiteSpace(fileName)) contentType = MimeMapping.GetMimeMapping(Path.GetExtension(fileName));

            var file = $"{ConfigurationManager.WebPath}\\{m_namespace}{folder}\\{fileName}";
            // for development of adapters, so we don't need to compile
            if (!File.Exists(file)) file = file.Replace("web\\web.sph", $"adapters\\{adapterName}.adapter");
            if (File.Exists(file))
            {
                SetStaticFileCacheability(ctx, file, "images\\form.element");
                var bytes = File.ReadAllBytes(file);
                ctx.Response.ContentType = contentType;
                await ctx.Response.WriteAsync(bytes);
                return;
            }
            // get it from AdapterDesigner
            var developerService = ObjectBuilder.GetObject<IDeveloperService>();
            var adapter = developerService.Adapters.Single(x => x.Metadata.Name == adapterName);
            var routeProvider = (IRouteTableProvider)Activator.CreateInstance(adapter.Metadata.RouteTableProvider);
            if (fileName.EndsWith(".js"))
            {
                var vm = routeProvider.GetEditorViewModel(new JsRoute {ModuleId = $"viewmodels/{fileName.Replace(".js", "")}"});
                if (!string.IsNullOrWhiteSpace(vm))
                {
                    ctx.Response.ContentType = contentType;
                    await ctx.Response.WriteAsync(vm);
                    return;
                }
            }
            if (fileName.EndsWith(".html"))
            {
                var vm = routeProvider.GetEditorView(new JsRoute {ModuleId = $"viewmodels/{fileName.Replace(".js", "")}"});
                if (!string.IsNullOrWhiteSpace(vm))
                {
                    ctx.Response.ContentType = contentType;
                    await ctx.Response.WriteAsync(vm);
                    return;
                }
            }

            // evertyhing else, just query the embedded resources
            var resourceName = $"Bespoke.Sph.Integrations.Adapters.{m_namespace}.{folder}.{fileName}";
            var assembly = adapter.Value.GetType().Assembly;
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
            // 

            ctx.Response.StatusCode = 404;
            ctx.Response.ContentType = "text/plain";
            await ctx.Response.WriteAsync($"Cannot find '{resourceName}' resource in assembly '{assembly.FullName}'");

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