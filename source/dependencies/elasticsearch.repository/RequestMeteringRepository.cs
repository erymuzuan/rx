using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Bespoke.Sph.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public class RequestMeteringRepository : IMeteringRepository
    {
        private readonly string m_index;
        private readonly HttpClient m_client;
        public IndexNamingStrategy IndexNamingStrategy { get; set; } = IndexNamingStrategy.Daily;

        public RequestMeteringRepository(string host, string index)
        {
            m_index = index;
            m_client = new HttpClient { BaseAddress = new Uri(host) };
        }

        public RequestMeteringRepository() : this(EsConfigurationManager.ElasticSearchHost, EsConfigurationManager.ElasticSearchSystemIndex)
        {

        }

        public void Initialize()
        {
            // TODO : create index alias base on IndexNamingStrategy

        }

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        private string GetIndexName()
        {
            switch (IndexNamingStrategy)
            {
                case IndexNamingStrategy.None:
                    return m_index;
                case IndexNamingStrategy.Hourly:
                    return $"{m_index}_{DateTime.Now:yyyyMMdd_HH}";
                case IndexNamingStrategy.Daily:
                    return $"{m_index}_{DateTime.Now:yyyyMMdd}";
                case IndexNamingStrategy.YearAndWeek:
                    return $"{m_index}_{DateTime.Now:yyyy}_{GetIso8601WeekOfYear(DateTime.Today)}";
                case IndexNamingStrategy.YearAndMonth:
                    return $"{m_index}_{DateTime.Now:yyyyMM}";
                case IndexNamingStrategy.Year:
                    return $"{m_index}_{DateTime.Now:yyyy}";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public void Log(HttpRequestLog request)
        {

            var setting = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var json = JsonConvert.SerializeObject(request, setting);
            var content = new StringContent(json);
            var index = GetIndexName();
            m_client.PostAsync(index + "/request_log", content)
                .ContinueWith(_ => { })
                .ConfigureAwait(false);
        }

        public async Task<LoadOperation<HttpRequestLog>> SearchAsync(Filter[] filters)
        {
            var alias = GetIndexAlias(filters);
            var url = $"{EsConfigurationManager.ElasticSearchIndex}_logs/{alias}/_search";
            using (var client = new HttpClient { BaseAddress = new Uri(EsConfigurationManager.ElasticsearchLogHost) })
            {
                var query = default(Entity).GetFilterDsl(filters);
                var response = await client.PostAsync(url, new StringContent(query));
                var lo = await response.ReadContentAsLoadOperationAsync<HttpRequestLog>();
                // TODO : read 
                return lo;


            }
        }

        private string GetIndexAlias(Filter[] filters)
        {
            // TODO : figure out the index alias, based on filters
            var start = filters.SingleOrDefault(x => x.Term == "Time" && (x.Operator == Operator.Ge || x.Operator == Operator.Gt));
            var to = filters.SingleOrDefault(x => x.Term == "Time" && (x.Operator == Operator.Le || x.Operator == Operator.Lt));
            switch (IndexNamingStrategy)
            {
                case IndexNamingStrategy.None:
                    return m_index;
                case IndexNamingStrategy.Hourly:
                    return $"{m_index}_{DateTime.Now:yyyyMMdd_HH}";
                case IndexNamingStrategy.Daily:
                    return $"{m_index}_{DateTime.Now:yyyyMMdd}";
                case IndexNamingStrategy.YearAndWeek:
                    return $"{m_index}_{DateTime.Now:yyyy}_{GetIso8601WeekOfYear(DateTime.Today)}";
                case IndexNamingStrategy.YearAndMonth:
                    return $"{m_index}_{DateTime.Now:yyyyMM}";
                case IndexNamingStrategy.Year:
                    return $"{m_index}_{DateTime.Now:yyyy}";
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
    }
}
