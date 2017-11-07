using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface ILoggerRepository
    {
        /*
         
            var url = $"{ ConfigurationManager.ElasticSearchSystemIndex}/log/{id}";
            string responseString;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticsearchLogHost);
                var response = await client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return NotFound();

                var content = response.Content as StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es ");
                responseString = await content.ReadAsStringAsync();

            }
            var esJson = JObject.Parse(responseString);
            var source = esJson.SelectToken("$._source");
            */
        Task<LogEntry> LoadOneAsync(string id);
    }

    public interface ILogger
    {
        Task LogAsync(LogEntry entry);
        void Log(LogEntry entry);
    }

    public enum Severity
    {
        Debug = 0,
        Verbose = 1,
        Info = 2,
        Log = 3,
        Warning = 4,
        Error = 5,
        Critical = 6
    }

    public enum EventLog
    {
        Application,
        Security,
        Schedulers,
        Subscribers,
        WebServer,
        Elasticsearch,
        SqlRepository,
        SqlPersistence,
        Logger
    }
}
