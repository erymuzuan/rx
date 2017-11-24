using System;
using System.Globalization;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
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