using System.Globalization;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Moq;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class TrackerTest : WorkflowTestBase
    {
        private Mock<IDirectoryService> m_ds;
        [SetUp]
        public override void Init()
        {

            m_ds = new Mock<IDirectoryService>(MockBehavior.Strict);
            m_ds.SetupGet(x => x.CurrentUserName)
                .Returns("erymuzuan");
            ObjectBuilder.AddCacheList(m_ds.Object);
            base.Init();
        }


        [Test]
        public void GetTracker()
        {
            var wf = new TestWorkflowForTracker { WorkflowDefinitionId = 5.ToString(CultureInfo.CurrentCulture), Id = "1" };
            var tracker = wf.GetTrackerAsync();
            Assert.IsNotNull(tracker);

        }

        [Test]
        public async Task StartScreen()
        {
            var wd = new WorkflowDefinition
            {
                Id = 1.ToString(CultureInfo.CurrentCulture),
                Name = "Test start screen"
            };
            wd.ActivityCollection.Add(new ScreenActivity
            {
                IsInitiator = true,
                Name = "Start screen",
                WebId = "A",
                Performer = new Performer { IsPublic = true }
            });
            var wf = new TestWorkflowForTracker { WorkflowDefinition = wd, WorkflowDefinitionId = "1", Id= "0" };
            var tracker = await wf.GetTrackerAsync();
            Assert.IsNotNull(tracker);

            var canExecute = tracker.CanExecute("A", null);
            Assert.IsTrue(canExecute);
        }

        [Test]
        public async Task InitiateScreenAsync()
        {
            var wd = new WorkflowDefinition
            {
                Id = "1",
                Name = "Test start screen"
            };

            wd.ActivityCollection.Add(new ScreenActivity
            {
                IsInitiator = true,
                Name = "Start screen",
                WebId = "A",
                Performer = new Performer { IsPublic = true },
                NextActivityWebId = "B"
            });
            wd.ActivityCollection.Add(new ScreenActivity
            {
                Name = "Start B",
                WebId = "B",
                Performer = new Performer { UserProperty = "UserName", Value = "admin" }
            });

            var wf = new TestWorkflowForTracker { WorkflowDefinition = wd, WorkflowDefinitionId = "1", Id = "0" };
            var tracker = await wf.GetTrackerAsync();

            var resultA = await wf.ExecuteAsync("A");
            CollectionAssert.Contains(resultA.NextActivities, "B");

            // initiate
            tracker.AddInitiateActivity(wd.ActivityCollection[1], new InitiateActivityResult { Correlation = "1234" });
            Assert.IsTrue(tracker.WaitingAsyncList.ContainsKey("B"));
            CollectionAssert.Contains(tracker.WaitingAsyncList["B"], "1234");
            Assert.IsTrue(tracker.CanExecute("B","1234"));
        }


        [Test]
        public async Task ExecuteInitiatedScreenAsync()
        {
            var wd = new WorkflowDefinition
            {
                Id = "1",
                Name = "Test start screen"
            };

            wd.ActivityCollection.Add(new ScreenActivity
            {
                IsInitiator = true,
                Name = "Start screen",
                WebId = "A",
                Performer = new Performer { IsPublic = true },
                NextActivityWebId = "B"
            });
            wd.ActivityCollection.Add(new ScreenActivity
            {
                Name = "Start B",
                WebId = "B",
                Performer = new Performer { UserProperty = "UserName", Value = "admin" }
            });
            wd.ActivityCollection.Add(new EndActivity { Name = "C", WebId = "C" });

            var wf = new TestWorkflowForTracker { WorkflowDefinition = wd, WorkflowDefinitionId = "1", Id = "0" };
            var tracker = await wf.GetTrackerAsync();

            var resultA = await wf.ExecuteAsync("A");
            CollectionAssert.Contains(resultA.NextActivities, "B");

            // initiate
            tracker.AddInitiateActivity(wd.ActivityCollection[1], new InitiateActivityResult { Correlation = "ABC" });
            Assert.IsTrue(tracker.WaitingAsyncList.ContainsKey("B"));

            // now execute
            var resultB = await wf.ExecuteAsync("B", "ABC");
            CollectionAssert.Contains(resultB.NextActivities, "C");
            CollectionAssert.DoesNotContain(tracker.WaitingAsyncList["B"], "ABC");
            Assert.IsFalse(tracker.CanExecute("B", "1234"));
        }

        [Test]
        public async Task CancelScreenActivity()
        {
            var wd = new WorkflowDefinition
            {
                Id = "1",
                Name = "Test start screen"
            };

            wd.ActivityCollection.Add(new ScreenActivity
            {
                IsInitiator = true,
                Name = "Start screen",
                WebId = "A",
                Performer = new Performer { IsPublic = true },
                NextActivityWebId = "B"
            });
            var screenB = new ScreenActivity
            {
                Name = "Start B",
                WebId = "B",
                Performer = new Performer {UserProperty = "UserName", Value = "admin"}
            };
            wd.ActivityCollection.Add(screenB);
            wd.ActivityCollection.Add(new EndActivity { Name = "C", WebId = "C" });

            var wf = new TestWorkflowForTracker { WorkflowDefinition = wd, WorkflowDefinitionId = "1", Id = "0" };
            var tracker = await wf.GetTrackerAsync();

            var resultA = await wf.ExecuteAsync("A");
            CollectionAssert.Contains(resultA.NextActivities, "B");

            // initiate
            tracker.AddInitiateActivity(wd.ActivityCollection[1], new InitiateActivityResult { Correlation = "ABC" });
            Assert.IsTrue(tracker.WaitingAsyncList.ContainsKey("B"));

            // now cancel
            await screenB.CancelAsync(wf);
            CollectionAssert.DoesNotContain(tracker.WaitingAsyncList["B"], "ABC");
            Assert.IsFalse(tracker.CanExecute("B", "1234"));
        }
    }
}
