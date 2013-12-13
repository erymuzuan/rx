using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class TrackerTest
    {
        [Test]
        public void GetTracker()
        {
            var wf = new TestWorkflowForTracker { WorkflowDefinitionId = 5, WorkflowId = 1 };
            var tracker = wf.GetTrackerAsync();
            Assert.IsNotNull(tracker);

        }

        [Test]
        public async Task StartScreen()
        {
            var wd = new WorkflowDefinition
            {
                WorkflowDefinitionId = 1,
                Name = "Test start screen"
            };
            wd.ActivityCollection.Add(new ScreenActivity
            {
                IsInitiator = true,
                Name = "Start screen",
                WebId = "A",
                Performer = new Performer { IsPublic = true }
            });
            var wf = new TestWorkflowForTracker { WorkflowDefinition = wd, WorkflowDefinitionId = 1, WorkflowId = 0 };
            var tracker = await wf.GetTrackerAsync();
            Assert.IsNotNull(tracker);

            var canExecute = tracker.CanExecute("A", null);
            Assert.IsTrue(canExecute);
        }
    }

    public class TestWorkflowForTracker : Workflow
    {
    }
}
