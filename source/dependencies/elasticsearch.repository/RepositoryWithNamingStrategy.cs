using System;
using System.Globalization;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public abstract class RepositoryWithNamingStrategy
    {
        public string BaseIndexName { get; set; }
        public IndexNamingStrategy IndexNamingStrategy { get; set; } = IndexNamingStrategy.Daily;

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
                    return $"{BaseIndexName}_{DateTime.Now:yyyy}_{GetIso8601WeekOfYear(DateTime.Today)}";
                case IndexNamingStrategy.YearAndMonth:
                    return $"{BaseIndexName}_{DateTime.Now:yyyyMM}";
                case IndexNamingStrategy.Year:
                    return $"{BaseIndexName}_{DateTime.Now:yyyy}";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected string GetIndexAlias(Filter[] filters)
        {
            // TODO : figure out the index alias, based on filters
            var start = filters.SingleOrDefault(x => x.Term == "Time" && (x.Operator == Operator.Ge || x.Operator == Operator.Gt));
            var to = filters.SingleOrDefault(x => x.Term == "Time" && (x.Operator == Operator.Le || x.Operator == Operator.Lt));
            switch (IndexNamingStrategy)
            {
                case IndexNamingStrategy.None:
                    return BaseIndexName;
                case IndexNamingStrategy.Hourly:
                    return $"{BaseIndexName}_{DateTime.Now:yyyyMMdd_HH}";
                case IndexNamingStrategy.Daily:
                    return $"{BaseIndexName}_{DateTime.Now:yyyyMMdd}";
                case IndexNamingStrategy.YearAndWeek:
                    return $"{BaseIndexName}_{DateTime.Now:yyyy}_{GetIso8601WeekOfYear(DateTime.Today)}";
                case IndexNamingStrategy.YearAndMonth:
                    return $"{BaseIndexName}_{DateTime.Now:yyyyMM}";
                case IndexNamingStrategy.Year:
                    return $"{BaseIndexName}_{DateTime.Now:yyyy}";
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
}