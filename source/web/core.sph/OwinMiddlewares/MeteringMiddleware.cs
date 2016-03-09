using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AppFunc = System.Func<
System.Collections.Generic.IDictionary<string, object>,
System.Threading.Tasks.Task
>;
namespace Bespoke.Sph.Web.OwinMiddlewares
{
    public class MeteringMiddleware
    {
        private readonly AppFunc m_next;
        private readonly HttpClient m_client;

        public MeteringMiddleware(AppFunc next)
        {
            m_next = next;
            m_client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) };
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var context = new OwinContext(environment);
            var sw = new Stopwatch();
            sw.Start();
            await m_next(environment);
            sw.Stop();

            var request = new
            {
                Elapsed = sw.ElapsedMilliseconds,
                Time = DateTime.Now,
                User = context.Request.User.Identity.Name,
                Request = new
                {
                    Path = context.Request.Path.ToString(),
                    PathBase = context.Request.PathBase.ToString(),
                    Host = context.Request.Host.ToString(),
                    context.Request.RemoteIpAddress,
                    Accepts = context.Request.Accept.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToArray(),
                    context.Request.CacheControl,
                    context.Request.IsSecure,
                    context.Request.LocalIpAddress,
                    context.Request.Method,
                    context.Request.Protocol,
                    context.Request.Scheme
                },
                Response = new
                {
                    context.Response.ContentLength,
                    context.Response.ContentType,
                    context.Response.ReasonPhrase,
                    context.Response.StatusCode
                }
            };
            var setting = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var json = JsonConvert.SerializeObject(request, setting);
            var content = new StringContent(json);
            await m_client.PostAsync($"{ConfigurationManager.ApplicationName.ToLowerInvariant()}/request_log", content);
        }
    }
}