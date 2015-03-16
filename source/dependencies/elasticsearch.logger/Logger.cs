using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bespoke.Sph.ElasticSearchLogger
{
    [Export(typeof(ILogger))]
    public class Logger : ILogger
    {
        public void Log(string operation, string message, Severity severity = Severity.Info, LogEntry entry = LogEntry.Application)
        {
            Console.WriteLine("");
        }

        public async Task LogAsync(string operation, string message, Severity severity = Severity.Info, LogEntry entry = LogEntry.Application)
        {
            var ad = ObjectBuilder.GetObject<IDirectoryService>();
            string user;
            try
            {
                user = ad.CurrentUserName;
            }
            catch
            {
                user = "Fail to get username";
            }

            var item = new Log
            {
                Operation = operation,
                Message = message,
                Severity = severity,
                Entry = entry,
                DateTime = DateTime.Now,
                UserName = user
            };
            var setting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            setting.Converters.Add(new StringEnumConverter());
            setting.Formatting = Formatting.None;
            var json = JsonConvert.SerializeObject(item, setting);

            var content = new StringContent(json);


            var url = string.Format("{0}/{1}_sys/log/{2}", ConfigurationManager.ElasticSearchHost,
                ConfigurationManager.ApplicationName.ToLowerInvariant(),
                Guid.NewGuid());

            using (var client = new HttpClient())
            {
                var response = await client.PutAsync(url, content);
                Console.WriteLine(response.StatusCode);
            }

        }

        public Task LogAsync(Exception e, IReadOnlyDictionary<string, object> properties)
        {
            //
            // keep in session
            //context.Trace.Warn("Last Server Error", e.ToString());
            /* your error logging */
            var message = new StringBuilder();

            var errorId = Guid.NewGuid().ToString();
            message.AppendFormat("\r\nError ID : {0}", errorId);
            message.AppendFormat("\r\nDate time : {0}", DateTime.Now);
            // message.AppendFormat("\r\nUrl : {0}", context.Request.Url);
            // message.AppendFormat("\r\nRefereral Url : {0}", context.Request.UrlReferrer);

            #region "exception details"
            var exc = e;
            var count = 1;
            while (null != exc)
            {
                message.AppendLine();
                message.AppendFormat(" --------------- {0} -----------------", count);
                message.AppendLine();
                message.AppendFormat("Type : {0}", exc.GetType().FullName);

                message.AppendLine();
                message.AppendFormat("Message : {0}", exc.Message);

                exc = exc.InnerException;
                count++;
            }

            message.AppendLine();
            message.Append("\r\n\r\nDetails : -------------------------------");
            message.AppendLine();
            message.Append(e);

            #endregion

            return this.LogAsync("Exception", message.ToString(), Severity.Error);
        }


        public void Log(Exception e, IReadOnlyDictionary<string, object> properties)
        {

            var message = new StringBuilder();

            var errorId = Guid.NewGuid().ToString();
            message.AppendFormat("\r\nError ID : {0}", errorId);
            message.AppendFormat("\r\nDate time : {0}", DateTime.Now);
            
            #region "exception details"
            var exc = e;
            var count = 1;
            while (null != exc)
            {
                message.AppendLine();
                message.AppendFormat(" --------------- {0} -----------------", count);
                message.AppendLine();
                message.AppendFormat("Type : {0}", exc.GetType().FullName);

                message.AppendLine();
                message.AppendFormat("Message : {0}", exc.Message);

                exc = exc.InnerException;
                count++;
            }

            message.AppendLine();
            message.Append("\r\n\r\nDetails : -------------------------------");
            message.AppendLine();
            message.Append(e);

            #endregion

            var ad = ObjectBuilder.GetObject<IDirectoryService>();
            string user;
            try
            {
                user = ad.CurrentUserName;
            }
            catch
            {
                user = "Fail to get username";
            }

            var item = new Log
            {
                Operation = "Exception",
                Message = message.ToString(),
                Severity = Severity.Error,
                Entry = LogEntry.Application,
                DateTime = DateTime.Now,
                UserName = user
            };
            var setting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            setting.Converters.Add(new StringEnumConverter());
            setting.Formatting = Formatting.None;
            var json = JsonConvert.SerializeObject(item, setting);

            var content = new StringContent(json);


            var url = string.Format("{0}/{1}_sys/log/{2}", ConfigurationManager.ElasticSearchHost,
                ConfigurationManager.ApplicationName.ToLowerInvariant(),
                Guid.NewGuid());

            using (var client = new HttpClient())
            {
                var response = client.PutAsync(url, content).Result;
                Console.WriteLine(response.StatusCode);
            }
        }


    }
}