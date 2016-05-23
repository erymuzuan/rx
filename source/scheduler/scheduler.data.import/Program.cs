using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Humanizer;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace scheduler.data.import
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var id = args.FirstOrDefault();
            var notificationOnError = ParseArgExist("notificationOnError");
            var notificationOnSuccess = ParseArgExist("notificationOnSuccess");
            var truncateData = ParseArgExist("truncateData");
            var debug = ParseArgExist("debug");

            if (debug)
            {
                Console.WriteLine(@"Press [ENTER] to continue");
                Console.ReadLine();
            }
            if (null == args) return;
            StartAsync(id, notificationOnSuccess, notificationOnError, truncateData).Wait();
        }

        private static async Task StartAsync(string id, bool notificationOnSuccess, bool notificationOnError, bool truncateData)
        {
            var username = ConfigurationManager.AppSettings["username"] ?? "admin";
            var password = ConfigurationManager.AppSettings["password"] ?? "123456";

            var file = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(DataTransferDefinition)}\\{id}.json";
            if (!File.Exists(file)) return;
            var json = File.ReadAllText(file);
            var container = new CookieContainer();
            GetCookie(username, password, container);
            var connection = new HubConnection(ConfigurationManager.BaseUrl) { CookieContainer = container };

            var hubProxy = connection.CreateHubProxy("DataImportHub");
            await connection.Start();

            if (truncateData)
                await hubProxy.Invoke("truncateData");
            try
            {
                var preview = await hubProxy.Invoke<object>("preview", json);
                Console.WriteLine(preview);
                await hubProxy.Invoke<ProgressData>("execute", p => { }, json);
                if (notificationOnSuccess)
                    await NotifySuccessAsync(id);
            }
            catch (Exception e)
            {
                if (notificationOnError)
                {
                    await NotifyErrorAsync(e, id);
                }
            }

        }

        private static async Task NotifySuccessAsync(string id)
        {
            var smtp = new SmtpClient();
            var @from = ConfigurationManager.AppSettings["EmailFrom"] ?? "data-transfer-scheduler@bespoke.com.my";
            var to = ConfigurationManager.AppSettings["EmailTo"] ?? "admin@bespoke.com.my";
            var subject = $"Data transfer was successful - {id}";
            var body = $@"
Your scheduled data transfer was successfuly executed on {DateTime.Now}";

            await smtp.SendMailAsync(@from, to, subject, body);
        }

        private static async Task NotifyErrorAsync(Exception exception, string id)
        {
            var smtp = new SmtpClient();
            var @from = ConfigurationManager.AppSettings["EmailFrom"] ?? "data-transfer-scheduler@bespoke.com.my";
            var to = ConfigurationManager.AppSettings["EmailTo"] ?? "admin@bespoke.com.my";
            var subject = $"Data transfer Error for {id}";
            var body = GetExceptionBody(exception);

            await smtp.SendMailAsync(@from, to, subject, body);
        }

        private static string GetExceptionBody(Exception exception)
        {
            var details = new StringBuilder();

            var exc = exception;
            var aeg = exc as AggregateException;
            if (null != aeg)
            {
                foreach (var ie in aeg.InnerExceptions)
                {
                    details.AppendLine(" ========================== ");
                    details.AppendLine(ie.GetType().FullName);
                    details.AppendLine(ie.Message);
                    details.AppendLine(ie.StackTrace);
                    details.AppendLine();
                    details.AppendLine();
                }

            }

            var rlex = exc as ReflectionTypeLoadException;
            if (null != rlex)
            {
                foreach (var lex in rlex.LoaderExceptions)
                {
                    details.AppendLine(" ========================== ");
                    details.AppendLine(lex.GetType().FullName);
                    details.AppendLine(lex.Message);
                    details.AppendLine();
                    details.AppendLine();
                }
            }

            while (null != exc)
            {

                details.AppendLine(" ========================== ");
                details.AppendLine(exc.GetType().FullName);
                details.AppendLine(exc.Message);
                details.AppendLine(exc.StackTrace);
                details.AppendLine();
                details.AppendLine();
                exc = exc.InnerException;
            }
            return details.ToString();
        }

        private static void GetCookie(string user, string password, CookieContainer container)
        {
            var request = (HttpWebRequest)WebRequest.Create($"{ConfigurationManager.BaseUrl}/Sph/SphAccount/Login");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = container;

            var authCredentials = $"UserName={user}&Password={password}&ReturnUrl=&submit=";
            var bytes = Encoding.UTF8.GetBytes(authCredentials);
            request.ContentLength = bytes.Length;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Console.WriteLine(response.StatusCode);
            }

        }


        public static string ParseArg(string name)
        {
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
            return val?.Replace("/" + name + ":", string.Empty);
        }

        private static bool ParseArgExist(string name)
        {
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name));
            return null != val;
        }
    }
    public class ProgressData
    {
        public Exception Exception { get; set; }
        public object Data { get; set; }
        public string ErrorId { get; set; }
        public int Rows { get; set; }
        public int ElasticsearchRows { get; set; }
        public int SqlRows { get; set; }
        [JsonIgnore]
        public TimeSpan Elapsed { set; get; }
        public string TotalTime => this.Elapsed.Humanize();
        public Queue ElasticsearchQueue { get; set; } = new Queue("es.data-import", 0, 0);
        public Queue SqlServerQueue { get; set; } = new Queue("persistence", 0, 0);

        public ProgressData(int rows)
        {
            Rows = rows;
        }
        public ProgressData(Exception exception, object data, string errorId)
        {
            Exception = exception;
            Data = data;
            ErrorId = errorId;
        }

        public static ProgressData Parse(string es, string sql)
        {
            var pd = new ProgressData(-1)
            {
                SqlServerQueue = ParseQueue(sql),
                ElasticsearchQueue = ParseQueue(es)
            };


            return pd;
        }
        private static Queue ParseQueue(string json)
        {
            var jo = JObject.Parse(json);
            var name = jo.SelectToken("$.name").Value<string>();
            var messages = jo.SelectToken("$.messages").Value<int>();
            var deliveries = jo.SelectToken("$.message_stats.deliver_details.rate").Value<double>();
            var unacked = jo.SelectToken("$.messages_unacknowledged").Value<int>();

            return new Queue(name, messages, deliveries, unacked);

        }
    }

    public class Queue
    {
        public string Name { get; set; }
        public int MessagesCount { get; set; }
        public double Rate { get; set; }
        public int Unacked { get; set; }

        public Queue(string name, int count, double rate)
        {
            this.Name = name;
            this.MessagesCount = count;
            this.Rate = rate;
        }

        public Queue(string name, int count, double rate, int unacked)
        {
            this.Name = name;
            this.MessagesCount = count;
            this.Rate = rate;
            this.Unacked = unacked;
        }
    }

}
