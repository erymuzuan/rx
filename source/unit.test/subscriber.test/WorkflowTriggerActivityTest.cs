using System;
using Bespoke.Sph.Domain;
using Humanizer;
using NUnit.Framework;
using subscriber.workflowscheduler;

namespace subscriber.test
{
    [TestFixture]
    public class WorkflowTriggerActivityTest
    {
        [Test]
        public void WeeklyTriggerTest()
        {
            var sub = new WorkflowSchedulerTriggerSubscriber(@"d:\project\tools\n.exe");
            var wd = new WorkflowDefinition
            {
                IsActive = true,
                Name = "Weekly Trigger Test"
            };
            var trigger = new ScheduledTriggerActivity
            {
                IsInitiator = true,
                WebId = "2",
                Name = "Weekly Trigger Test"
            };
            var week = new WeeklySchedule
            {
                Start = DateTime.Today,
                Expire = In.One.Year,
                IsEnabled = true,
                IsMonday = true,
                IsFriday = true,
                Recur = 1
            };
            trigger.IntervalScheduleCollection.Add(week);
            wd.ActivityCollection.Add(trigger);


            sub.Test(wd, null);
        }
        [Test]
        public void MonthlyTriggerTest()
        {
            var sub = new WorkflowSchedulerTriggerSubscriber(@"d:\project\tools\n.exe");
            var wd = new WorkflowDefinition
            {
                IsActive = true,
                Name = "Monthly Trigger Test"
            };
            var trigger = new ScheduledTriggerActivity
            {
                IsInitiator = true,
                WebId = "2",
                Name = "Monthly Trigger Test"
            };
            var month = new MonthlySchedule
            {
                Start = DateTime.Today,
                Expire = In.One.Year,
                IsEnabled = true,
                IsLastDay = true
            };
            trigger.IntervalScheduleCollection.Add(month);
            wd.ActivityCollection.Add(trigger);


            sub.Test(wd, null);
        }


        [Test]
        public void DailyTrigger()
        {
            var sub = new WorkflowSchedulerTriggerSubscriber(@"d:\project\tools\n.exe");
            var wd = new WorkflowDefinition
            {
                IsActive = true,
                Name = "Test 01"
            };
            var trigger = new ScheduledTriggerActivity
            {
                IsInitiator = true,
                WebId = "1",
                Name = "Daily Trigger Test"
            };
            var daily = new DailySchedule
            {
                Recur = 1,
                Repeat = 3600,
                Duration = 3600 * 12,
                Start = DateTime.Now,
                Expire = In.Three.Days,
                IsEnabled = true
            };
            var daily2 = new DailySchedule
            {
                Recur = 1,
                Repeat = 180,
                Duration = 3600 * 4,
                Start = DateTime.Now,
                Expire = In.Three.Days,
                IsEnabled = true
            };
            trigger.IntervalScheduleCollection.Add(daily);
            trigger.IntervalScheduleCollection.Add(daily2);
            wd.ActivityCollection.Add(trigger);


            sub.Test(wd, null);
        }
    }
}
