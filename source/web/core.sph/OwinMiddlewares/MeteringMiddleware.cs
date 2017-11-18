using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Microsoft.Owin;
using AppFunc = System.Func<
System.Collections.Generic.IDictionary<string, object>,
System.Threading.Tasks.Task
>;
namespace Bespoke.Sph.Web.OwinMiddlewares
{
    public class MeteringMiddleware
    {
        private readonly AppFunc m_next;

        public MeteringMiddleware(AppFunc next)
        {
            m_next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var context = new OwinContext(environment);
            var sw = new Stopwatch();
            sw.Start();
            await m_next(environment);
            sw.Stop();

            var accepts = context.Request.Accept.ToEmptyString();
            var request = new HttpRequestLog
            {
                Elapsed = sw.ElapsedMilliseconds,
                Time = DateTime.Now.ToString("s"),
                User = context.Request.User.Identity.Name,
                Controller = context.Get<string>("rx:controller"),
                Action = context.Get<string>("rx:action"),
                Request = new HttpRequestPayload
                {   
                    Path = context.Request.Path.ToString(),
                    PathBase = context.Request.PathBase.ToString(),
                    Host = context.Request.Host.ToString(),
                    RemoteIpAddress = context.Request.RemoteIpAddress,
                    Accepts = accepts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToArray(),
                    CacheControl = context.Request.CacheControl,
                    IsSecure = context.Request.IsSecure,
                    LocalIpAddress = context.Request.LocalIpAddress,
                    Method = context.Request.Method,
                    Protocol = context.Request.Protocol,
                    Scheme = context.Request.Scheme
                },
                Response = new HttpResponsePayload
                {
                    ContentLength = context.Response.ContentLength,
                    ContentType =  context.Response.ContentType,
                    ReasonPhrase = context.Response.ReasonPhrase,
                    StatusCode = context.Response.StatusCode
                }
            };
            try
            {
                var repos = ObjectBuilder.GetObject<IMeteringRepository>();
                repos.Log(request);

            }
            catch
            {
                //ignore
            }
        }
    }

    public static class TaskExtension
    {
        public static async void FireAndForget(this Task task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch
            {
                // ignore
            }
        }
    }
}