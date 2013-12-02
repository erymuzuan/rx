using System;
using Bespoke.Sph.Domain;
using Microsoft.Win32.TaskScheduler;
using Trigger = Microsoft.Win32.TaskScheduler.Trigger;

namespace Bespoke.Sph.WorkflowTriggerSubscriptions
{
    public static class TaskHelper
    {
        public static Trigger GeTrigger(this IntervalSchedule t)
        {
            var ds = t as DailySchedule;
            var ws = t as WeeklySchedule;
            var ms = t as MonthlySchedule;
            Trigger trigger = null;
            if (null != ds)
            {
                var daily = new DailyTrigger
                {
                    Enabled = t.IsEnabled,
                    DaysInterval = Convert.ToInt16(ds.Recur)

                };

                if (t.Delay.HasValue)
                    daily.RandomDelay = TimeSpan.FromSeconds(t.Delay.Value);

                trigger = daily;

            }
            if (null != ws)
            {
                var sunday = ws.IsSunday ? (int)DaysOfTheWeek.Sunday : 0;
                var monday = ws.IsMonday ? (int)DaysOfTheWeek.Monday : 0;
                var tuesday = ws.IsTuesday ? (int)DaysOfTheWeek.Tuesday : 0;
                var wed = ws.IsWednesday ? (int)DaysOfTheWeek.Wednesday : 0;
                var thur = ws.IsThursday ? (int)DaysOfTheWeek.Thursday : 0;
                var fri = ws.IsFriday ? (int)DaysOfTheWeek.Friday : 0;
                var sat = ws.IsSaturday ? (int)DaysOfTheWeek.Saturday : 0;

                var days = sunday + monday + tuesday + wed + thur + fri + sat;
                var weekly = new WeeklyTrigger
                {
                    Enabled = t.IsEnabled,
                    DaysOfWeek = (DaysOfTheWeek)days,
                    WeeksInterval = Convert.ToInt16(ws.Recur)
                };

                if (t.Delay.HasValue)
                    weekly.RandomDelay = TimeSpan.FromSeconds(t.Delay.Value);
                trigger = weekly;

            }
            if (null != ms)
            {
                var jan = ms.IsJanuary ? (int)MonthsOfTheYear.January : 0;
                var feb = ms.IsFebruary ? (int) MonthsOfTheYear.February : 0;
                var mac = ms.IsMarch ? (int) MonthsOfTheYear.March : 0;
                var apr = ms.IsApril ? (int) MonthsOfTheYear.April : 0;
                var may = ms.IsMay ? (int) MonthsOfTheYear.May : 0;
                var jun = ms.IsJune ? (int) MonthsOfTheYear.June : 0;
                var jul = ms.IsJuly ? (int) MonthsOfTheYear.July : 0;
                var aug = ms.IsAugust ? (int) MonthsOfTheYear.August : 0;
                var sep = ms.IsSeptember ? (int) MonthsOfTheYear.September : 0;
                var oct = ms.IsOctober ? (int) MonthsOfTheYear.October : 0;
                var nov = ms.IsNovember ? (int) MonthsOfTheYear.November : 0;
                var dec = ms.IsDecember ? (int) MonthsOfTheYear.December : 0;

                var months = jan + feb + mac + apr + may + jun + jul + aug + sep + oct + nov + dec;

                var monthly = new MonthlyTrigger
                {
                    Enabled = t.IsEnabled,
                    RunOnLastDayOfMonth = ms.IsLastDay,
                    DaysOfMonth = ms.Days.ToArray(),
                    MonthsOfYear = (MonthsOfTheYear)months
                };

                if (t.Delay.HasValue)
                    monthly.RandomDelay = TimeSpan.FromSeconds(t.Delay.Value);
                trigger = monthly;

            }
            if (null != trigger)
            {
                trigger.Enabled = t.IsEnabled;
                trigger.StartBoundary = t.Start;
                if (t.Expire.HasValue)
                    trigger.EndBoundary = t.Expire.Value;

                if (t.Repeat.HasValue && t.Duration.HasValue)
                {
                    trigger.Repetition.Duration = TimeSpan.FromSeconds(t.Duration.Value);
                    trigger.Repetition.Interval = TimeSpan.FromSeconds(t.Repeat.Value);
                }
            }


            return trigger;
        }
    }
}