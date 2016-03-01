using System.Globalization;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Moq;
using Xunit;

namespace domain.test.workflows
{
    public class TrackerTest : WorkflowTestBase
    {
        public TrackerTest()
        {
            var ds = new Mock<IDirectoryService>(MockBehavior.Strict);
            ds.SetupGet(x => x.CurrentUserName)
                .Returns("erymuzuan");
            ObjectBuilder.AddCacheList(ds.Object);
        }


        [Fact]
        public void GetTracker()
        {
            var wf = new TestWorkflowForTracker { WorkflowDefinitionId = 5.ToString(CultureInfo.CurrentCulture), Id = "1" };
            var tracker = wf.GetTrackerAsync();
            Assert.NotNull(tracker);

        }

        [Fact]
        public async Task StartScreen()
        {
            var wd = new WorkflowDefinition
            {
                Id = 1.ToString(CultureInfo.CurrentCulture),
                Name = "Test start screen"
            };
            wd.ActivityCollection.Add(new ReceiveActivity
            {
                IsInitiator = true,
                Name = "Start screen",
                WebId = "A",
                Performer = new Performer { IsPublic = true }
            });
            var wf = new TestWorkflowForTracker { WorkflowDefinition = wd, WorkflowDefinitionId = "1", Id = "0" };
            var tracker = await wf.GetTrackerAsync();
            Assert.NotNull(tracker);

            var canExecute = tracker.CanExecute("A", null);
            Assert.True(canExecute);
        }

        [Fact]
        public async Task InitiateScreenAsync()
        {
            var wd = new WorkflowDefinition
            {
                Id = "1",
                Name = "Test start screen"
            };

            wd.ActivityCollection.Add(new ReceiveActivity
            {
                IsInitiator = true,
                Name = "Start screen",
                WebId = "A",
                Performer = new Performer { IsPublic = true },
                NextActivityWebId = "B"
            });
            wd.ActivityCollection.Add(new ReceiveActivity
            {
                Name = "Start B",
                WebId = "B",
                Performer = new Performer { UserProperty = "UserName", Value = "admin"}
            });

            var wf = new TestWorkflowForTracker { WorkflowDefinition = wd, WorkflowDefinitionId = "1", Id = "0" };
            var tracker = await wf.GetTrackerAsync();

            var resultA = await wf.ExecuteAsync("A");
            Assert.Contains("B", resultA.NextActivities);

            // initiate
            tracker.AddInitiateActivity(wd.ActivityCollection[1], new InitiateActivityResult { Correlation = "1234" });
            Assert.True(tracker.WaitingAsyncList.ContainsKey("B"));
            Assert.Contains("1234", tracker.WaitingAsyncList["B"]);
            Assert.True(tracker.CanExecute("B", "1234"));
        }


        [Fact]
        public async Task ExecuteInitiatedScreenAsync()
        {
            var wd = new WorkflowDefinition
            {
                Id = "1",
                Name = "Test start screen"
            };

            wd.ActivityCollection.Add(new ReceiveActivity
            {
                IsInitiator = true,
                Name = "Start screen",
                WebId = "A",
                Performer = new Performer { IsPublic = true },
                NextActivityWebId = "B"
            });
            wd.ActivityCollection.Add(new ReceiveActivity
            {
                Name = "Start B",
                WebId = "B",
                Performer = new Performer { UserProperty = "UserName", Value = "Admin"}
            });
            wd.ActivityCollection.Add(new EndActivity { Name = "C", WebId = "C" });

            var wf = new TestWorkflowForTracker { WorkflowDefinition = wd, WorkflowDefinitionId = "1", Id = "0" };
            var tracker = await wf.GetTrackerAsync();

            var resultA = await wf.ExecuteAsync("A");
            Assert.Contains("B", resultA.NextActivities);

            // initiate
            tracker.AddInitiateActivity(wd.ActivityCollection[1], new InitiateActivityResult { Correlation = "ABC" });
            Assert.True(tracker.WaitingAsyncList.ContainsKey("B"));

            // now execute
            var resultB = await wf.ExecuteAsync("B", "ABC");
            Assert.Contains("C", resultB.NextActivities);
            Assert.DoesNotContain("ABC", tracker.WaitingAsyncList["B"]);
            Assert.False(tracker.CanExecute("B", "1234"));
        }

        [Fact]
        public async Task CancelScreenActivity()
        {
            var wd = new WorkflowDefinition
            {
                Id = "1",
                Name = "Test start screen"
            };

            var r1 = new ReceiveActivity
            {
                IsInitiator = true,
                Name = "Start screen",
                WebId = "A",
                Performer = new Performer { IsPublic = true },
                NextActivityWebId = "B"
            };
            wd.ActivityCollection.Add(r1);
            var r2 = new ReceiveActivity
            {
                Name = "Start B",
                WebId = "B",
                Performer = new Performer { UserProperty = "UserName", Value = "admin"}
            };
            wd.ActivityCollection.Add(r2);
            wd.ActivityCollection.Add(new EndActivity { Name = "C", WebId = "C" });

            var wf = new TestWorkflowForTracker { WorkflowDefinition = wd, WorkflowDefinitionId = "1", Id = "0" };
            var tracker = await wf.GetTrackerAsync();

            var resultA = await wf.ExecuteAsync("A");
            Assert.Contains("B", resultA.NextActivities);

            // initiate
            tracker.AddInitiateActivity(wd.ActivityCollection[1], new InitiateActivityResult { Correlation = "ABC" });
            Assert.True(tracker.WaitingAsyncList.ContainsKey("B"));

            // now cancel
            await r2.CancelAsync(wf);
            Assert.DoesNotContain("ABC", tracker.WaitingAsyncList["B"]);
            Assert.False(tracker.CanExecute("B", "1234"));
        }
    }
}
