using System;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;
using Humanizer;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [Authorize(Roles = "developers")]
    [RoutePrefix("developer-service")]
    public class DeveloperServiceController : BaseApiController
    {
        [Route("environment-variables")]
        public IHttpActionResult GetEnvironmentVariables()
        {
            var json = JsonConvert.SerializeObject(Environment.GetEnvironmentVariables());
            return Json(json);
        }

        [Route("configs")]
        public IHttpActionResult GetConfigs()
        {
            var type = typeof(ConfigurationManager);
            var configs = from m in type.GetProperties(BindingFlags.Public | BindingFlags.Static)
                          where m.Name != "ConnectionStrings"
                          && m.Name != "AppSettings"
                          select new
                          {
                              Key = m.Name,
                              Value = m.GetValue(null, null)
                          };
            var json = JsonConvert.SerializeObject(configs.ToDictionary(k => k.Key, v => v.Value));
            return Json(json);
        }
        [Route("sqlserver-instances")]
        public async Task<IHttpActionResult> GetSqlServerInstances()
        {
            var cache = ObjectBuilder.GetObject<ICacheManager>();
            const string KEY = "developers-service:sqlserver-instances";
            var servers = cache.Get<string[]>(KEY);
            if (null == servers)
            {
                var table = System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources();
                var instances = from DataRow r in table.Rows
                                select $"{r["ServerName"]}\\{r["InstanceName"]}";
                var localdb = await LoadSqlLocaldbInstances();
                servers = instances.Concat(localdb).ToArray();
                cache.Insert(KEY, servers, 30.Minutes());

            }
            var json = servers.ToJson();
            return Json(json);
        }


        private Task<string[]> LoadSqlLocaldbInstances()
        {
            var tcs = new TaskCompletionSource<string[]>();
            var workerInfo = new ProcessStartInfo
            {
                FileName = "SqlLocalDB.exe",
                Arguments = "i",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            using (var p = Process.Start(workerInfo))
            {
                if (null == p)
                {
                    tcs.TrySetResult(Array.Empty<string>());
                    return tcs.Task;
                }
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                var list = new ConcurrentBag<string>();
                p.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                        list.Add("(localdb)\\" + e.Data);
                };
                p.WaitForExit();
                tcs.SetResult(list.ToArray());
            }
            return tcs.Task;
        }





    }
}