using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Microsoft.AspNet.SignalR.Client;
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
            var stopFlag = new AutoResetEvent(false);
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, ce) =>
            {
                Console.WriteLine();
                Console.WriteLine($"[{DateTime.Now:T}]Processing your cancellation request..");
                // 1300 885 055 sunlife
                cts.Cancel();
                ce.Cancel = true;
            };
            StartAsync(id, notificationOnSuccess, notificationOnError, truncateData, cts.Token, stopFlag)
                .ContinueWith(_ =>
            {
                stopFlag.Set();
            }, cts.Token);

            stopFlag.WaitOne();
        }

        private static async Task StartAsync(string id, bool notificationOnSuccess, bool notificationOnError, bool truncateData, CancellationToken cst, AutoResetEvent flag)
        {
            var username = ConfigurationManager.AppSettings["username"] ?? "admin";
            var password = ConfigurationManager.AppSettings["password"] ?? "123456";

            var file = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(DataTransferDefinition)}\\{id}.json";
            if (!File.Exists(file)) return;
            var model = file.DeserializeFromJsonFile<DataTransferDefinition>();
            var container = new CookieContainer();
            GetCookie(username, password, container);
            var connection = new HubConnection(ConfigurationManager.BaseUrl) { CookieContainer = container };

            var hubProxy = connection.CreateHubProxy("DataImportHub");
            await connection.Start();

            try
            {
                var preview = (await hubProxy.Invoke<object>("preview", model) as JObject);
                var details = preview?.SelectToken("$.Details")?.Value<string>();
                if (!string.IsNullOrWhiteSpace(details))
                {
                    await NotifyErrorAsync(details, id);
                    flag.Set();
                    return;
                }

                if (truncateData)
                {
                    Console.WriteLine();
                    Console.Write($"\r[{DateTime.Now:T}]Please wait.. while we truncate the data....");
                    await hubProxy.Invoke("truncateData", id, model);
                    Console.Write($"\r[{DateTime.Now:T}]Done truncate data .........................");
                    Console.WriteLine();
                }
                var rows = 0;
                var sqlMessages = 0;
                var sqlRate = 0d;
                var sqlRows = 0;
                var esMessages = 0;
                var esRate = 0d;
                var esRows = 0;
                Action<ProgressData> progress = async p =>
                {
                    if (p.Rows > 0)
                        rows = p.Rows;
                    if (p.SqlServerQueue.MessagesCount > 0)
                        sqlMessages = p.SqlServerQueue.MessagesCount;
                    if (p.SqlServerQueue.Rate > 0d)
                        sqlRate = p.SqlServerQueue.Rate;
                    if (p.SqlRows > 0)
                        sqlRows = p.SqlRows;
                    if (p.ElasticsearchQueue.MessagesCount > 0)
                        esMessages = p.ElasticsearchQueue.MessagesCount;
                    if (p.ElasticsearchQueue.Rate > 0)
                        esRate = p.ElasticsearchQueue.Rate;
                    if (p.ElasticsearchRows > 0)
                        esRows = p.ElasticsearchRows;
                    Console.Write($"\r{DateTime.Now:T}Rows : {rows}\tSQL : {sqlMessages}/{sqlRate}({sqlRows})\t ES: {esMessages}/{esRate}({esRows})                                                 ");

                    if (cst.IsCancellationRequested)
                    {
                        await hubProxy.Invoke("requestCancel");
                    }

                };
                //hubProxy.Observe()
                var result = await hubProxy.Invoke<object, ProgressData>("execute", progress, id, model);
                Console.WriteLine();
                Console.WriteLine($@"{DateTime.Now:T}Done processing data transfer....");
                Console.WriteLine(result);
                if (notificationOnSuccess)
                    await NotifySuccessAsync(id);

                flag.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType().FullName);
                Console.WriteLine(e.Message);
                if (notificationOnError)
                {
                    await NotifyErrorAsync(e, id);
                }
                flag.Set();
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
        private static async Task NotifyErrorAsync(string exception, string id)
        {
            var smtp = new SmtpClient();
            var @from = ConfigurationManager.AppSettings["EmailFrom"] ?? "data-transfer-scheduler@bespoke.com.my";
            var to = ConfigurationManager.AppSettings["EmailTo"] ?? "admin@bespoke.com.my";
            var subject = $"Data transfer preview error for {id}";
            var body = exception;

            Console.WriteLine(subject);
            Console.WriteLine(body);

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
                Console.WriteLine($"Login status : {response.StatusCode}");
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
}
