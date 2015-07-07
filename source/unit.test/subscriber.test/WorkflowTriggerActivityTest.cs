using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WorkflowTriggerSubscriptions;
using Humanizer;
using Moq;
using NUnit.Framework;

namespace subscriber.test
{
    [TestFixture]
    public class WorkflowTriggerActivityTest
    {
        private Mock<Bespoke.Sph.SubscribersInfrastructure.INotificationService> m_console;

        [SetUp]
        public void SetUp()
        {
            m_console = new Mock<Bespoke.Sph.SubscribersInfrastructure.INotificationService>(MockBehavior.Strict);
            m_console.Setup(x => x.Write(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback((string format, object[] f) => Console.WriteLine(format, f));
        }
        [Test]
        public async Task WeeklyTriggerTest()
        {
            var sub = new WorkflowSchedulerTriggerSubscriber(@"d:\project\tools\n.exe")
            {
                NotificicationService = m_console.Object
            };
            var wd = new WorkflowDefinition
            {
                IsActive = true,
                Name = "Weekly Trigger Test",
                Id = "weekly-trigger-test"
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


            await sub.Test(wd, null);
        }
        [Test]
        public async Task MonthlyTriggerTest()
        {
            var sub = new WorkflowSchedulerTriggerSubscriber(@"d:\project\tools\n.exe")
            {
                NotificicationService = m_console.Object
            };

            var wd = new WorkflowDefinition
            {
                IsActive = true,
                Name = "Monthly Trigger Test",
                Id = "monthly-trigger-test"
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
                IsLastDay = true,
                IsApril = true,
                IsMarch = true
            };
            trigger.IntervalScheduleCollection.Add(month);
            wd.ActivityCollection.Add(trigger);


            await sub.Test(wd, null);
        }


        [Test]
        public async Task DailyTrigger()
        {
            var sub = new WorkflowSchedulerTriggerSubscriber(@"d:\project\tools\n.exe")
            {
                NotificicationService = m_console.Object
            };
            var wd = new WorkflowDefinition
            {
                IsActive = true,
                Name = "Test 01",
                Id = "test-01"
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


            await sub.Test(wd, null);
        }
    }
}
