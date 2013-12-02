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
            var wf = new TestWorkflow_1_1();
            var tracker = wf.GetTrackerAsync();
            Assert.IsNotNull(tracker);

        }
    }

// ReSharper disable InconsistentNaming
    public class TestWorkflow_1_1 : Workflow
// ReSharper restore InconsistentNaming
    {
    }
}
