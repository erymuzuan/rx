using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Humanizer;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace scheduler.data.import
{
    class Program
    {
        static void Main(string[] args)
        {
            var id = args.FirstOrDefault();
            if (null == args) return;
            StartAsync(id).Wait();
        }

        private static async Task StartAsync(string id)
        {
            var username = ConfigurationManager.AppSettings["username"];
            var password = ConfigurationManager.AppSettings["password"];
            
            var file = $"{ConfigurationManager.WebPath}\\App_Data\\data-imports\\{id}.json";
            if (!File.Exists(file)) return;
            var json = File.ReadAllText(file);
            var container = new CookieContainer();
            GetCookie(username, password, container);
            var connection = new HubConnection(ConfigurationManager.BaseUrl) {CookieContainer = container};

            var hubProxy = connection.CreateHubProxy("DataImportHub");
            await connection.Start();

            await hubProxy.Invoke<ProgressData>("execute", p => { }, json);
        }

        private static void GetCookie(string user, string password, CookieContainer container)
        {
            var request = (HttpWebRequest)WebRequest.Create($"{ConfigurationManager.BaseUrl}/Sph/SphAccount/Login");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = container;

            var authCredentials = "UserName=" + user + "&Password=" + password;
            var bytes = System.Text.Encoding.UTF8.GetBytes(authCredentials);
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
