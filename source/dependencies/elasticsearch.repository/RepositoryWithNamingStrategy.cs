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
            /*
             {
    "actions" : [
        { "add" : { "index" : "test1", "alias" : "alias1" } }
    ]
}
             */
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

        protected string GetIndexAlias(Filter[] filters)
        {
            // TODO : figure out the index alias, based on filters
            var start = filters.SingleOrDefault(x => x.Term.Equals("Time", StringComparison.InvariantCultureIgnoreCase) && (x.Operator == Operator.Ge || x.Operator == Operator.Gt));
            var to = filters.SingleOrDefault(x => x.Term.Equals("Time", StringComparison.InvariantCultureIgnoreCase) && (x.Operator == Operator.Le || x.Operator == Operator.Lt));
            if (null == start || null == to) return BaseIndexName;

            if (start.Field.GetValue(default) is DateTime startDate &&
                to.Field.GetValue(default) is DateTime endDate)
            {
                if (startDate.IsTheSameDay(endDate) && IndexNamingStrategy <= IndexNamingStrategy.Daily)
                    return $"{BaseIndexName}_{startDate:yyyyMMdd}";
                if (startDate.IsTheSameWeek(endDate) && IndexNamingStrategy <= IndexNamingStrategy.YearAndWeek)
                    return $"{BaseIndexName}_{startDate:yyyy}W{startDate.GetIso8601WeekOfYear()}";
                if (startDate.IsTheSameMonth(endDate) && IndexNamingStrategy <= IndexNamingStrategy.YearAndMonth)
                    return $"{BaseIndexName}_{startDate:yyyyMM}";
                if (startDate.Year == endDate.Year)
                    return $"{BaseIndexName}_{startDate:yyyy}";
                return BaseIndexName;
            }


            return BaseIndexName;

        }

    }

    public static class DateTimeExtension
    {
        public static bool IsTheSameDay(this DateTime one, DateTime two)
        {
            return one.Year == two.Year && one.Month == two.Month && one.Day == two.Day;
        }
        public static bool IsTheSameWeek(this DateTime one, DateTime two)
        {
            return one.Year == two.Year && one.GetIso8601WeekOfYear() == two.GetIso8601WeekOfYear();
        }
        public static bool IsTheSameMonth(this DateTime one, DateTime two)
        {
            return one.Year == two.Year && one.Month == two.Month;
        }
        public static bool IsTheSameYear(this DateTime one, DateTime two)
        {
            return one.Year == two.Year;
        }

        public static int GetIso8601WeekOfYear(this DateTime time)
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
    }
}