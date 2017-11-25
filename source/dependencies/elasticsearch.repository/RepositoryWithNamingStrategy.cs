using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public abstract class RepositoryWithNamingStrategy : IDisposable
    {

        protected HttpClient Client { get; }

        protected RepositoryWithNamingStrategy(string host, string baseIndexName)
        {
            this.BaseIndexName = baseIndexName;
            Client = new HttpClient { BaseAddress = new Uri(host) };
        }

        public string BaseIndexName { get; set; }
        public IndexNamingStrategy IndexNamingStrategy { get; set; } = IndexNamingStrategy.Daily;

        private DateTime m_aliasIsCreated = DateTime.MinValue;

        protected async Task CreateAliasesQueryAsync(DateTime dateTime)
        {
            // create aliases ?? how much impact, hopefuly not much , it's just once a day
            if (m_aliasIsCreated.Date == DateTime.Today) return;

            var index = this.GetIndexName();
            var aliases = this.GenerateAliasesName(dateTime);
            var action = $@"         {{
    ""actions"" : [
        {{ ""add"" : {{ ""index"" : ""{index}"", ""alias"" : ""{aliases[0]}"" }} }},
        {{ ""add"" : {{ ""index"" : ""{index}"", ""alias"" : ""{aliases[1]}"" }} }},
        {{ ""add"" : {{ ""index"" : ""{index}"", ""alias"" : ""{aliases[2]}"" }} }}
    ]
}}";
            var response = await this.Client.PostAsync("_aliases", new StringContent(action));
            response.EnsureSuccessStatusCode();
            m_aliasIsCreated = DateTime.Today;
        }


        protected string GetIndexName()
        {
            switch (IndexNamingStrategy)
            {
                case IndexNamingStrategy.None:
                    return BaseIndexName;
                case IndexNamingStrategy.Hourly:
                    return $"{BaseIndexName}_{DateTime.Now:yyyyMMdd_HH}";
                case IndexNamingStrategy.Daily:
                    return $"{BaseIndexName}_{DateTime.Now:yyyyMMdd}";
                case IndexNamingStrategy.YearAndWeek:
                    return $"{BaseIndexName}_{DateTime.Now:yyyy}W{DateTime.Today.GetIso8601WeekOfYear()}";
                case IndexNamingStrategy.YearAndMonth:
                    return $"{BaseIndexName}_{DateTime.Now:yyyyMM}";
                case IndexNamingStrategy.Year:
                    return $"{BaseIndexName}_{DateTime.Now:yyyy}";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected string GetIndexAlias(Filter[] filters, string defaultDateTimeField = nameof(Entity.CreatedDate))
        {
            var from = filters.FirstOrDefault(x =>
                x.Term == defaultDateTimeField && (x.Operator == Operator.Ge || x.Operator == Operator.Gt));
            var to = filters.FirstOrDefault(x =>
                x.Term == defaultDateTimeField && (x.Operator == Operator.Le || x.Operator == Operator.Lt));

            return GetIndexAlias(from, to);

        }

        protected string GetIndexAlias(Filter @from, Filter to)
        {
            if (null == @from || null == to) return BaseIndexName;

            if (@from.Field.GetValue(default) is DateTime startDate &&
                to.Field.GetValue(default) is DateTime endDate)
            {
                if (startDate.IsTheSameDay(endDate) && IndexNamingStrategy <= IndexNamingStrategy.Daily)
                    return $"{BaseIndexName}_{startDate:yyyyMMdd}";
                if (startDate.IsTheSameWeek(endDate) && IndexNamingStrategy <= IndexNamingStrategy.YearAndWeek)
                    return $"{BaseIndexName}_{startDate:yyyy}W{startDate.GetIso8601WeekOfYear():00}";
                if (startDate.IsTheSameMonth(endDate) && IndexNamingStrategy <= IndexNamingStrategy.YearAndMonth)
                    return $"{BaseIndexName}_{startDate:yyyyMM}";
                if (startDate.Year == endDate.Year)
                    return $"{BaseIndexName}_{startDate:yyyy}";
                return BaseIndexName;
            }

            return BaseIndexName;

        }

        protected string[] GenerateAliasesName(DateTime dateTime)
        {
            return new[]
            {
                $"{BaseIndexName}_{dateTime:yyyy}",
                $"{BaseIndexName}_{dateTime:yyyyMM}",
                $"{BaseIndexName}_{dateTime:yyyy}W{dateTime.GetIso8601WeekOfYear():00}"
            };
        }

        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}