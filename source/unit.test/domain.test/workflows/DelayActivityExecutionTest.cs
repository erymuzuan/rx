using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Moq;
using Xunit;

namespace domain.test.workflows
{
    [Trait("Category", "Workflow")]
    public class DelayActivityExecutionTest : WorkflowTestBase
    {
        [Fact]
        public async Task DelayInit()
        {
            var scheduledTask = new List<string>();
            var ts = new Mock<ITaskScheduler>(MockBehavior.Strict);
            ts.Setup(x => x.AddTaskAsync(It.IsAny<DateTime>(), It.IsAny<ScheduledActivityExecution>()))
                .Callback((DateTime dt, ScheduledActivityExecution se) => scheduledTask.Add(se.Name))
                .Returns(Task.FromResult(0));
            ObjectBuilder.AddCacheList(ts.Object);

            var wd = this.Create("Wf25");
            wd.ActivityCollection.Add(new ReceiveActivity { Name = "Start isi borang", IsInitiator = true, WebId = "A", NextActivityWebId = "B" });
            wd.ActivityCollection.Add(new DelayActivity { Name = "Wait Delay", Seconds = 1, WebId = "B", NextActivityWebId = "C" });
            wd.ActivityCollection.Add(new EndActivity { WebId = "C", Name = "Habis" });
            var result = await this.CompileAsync(wd);
            dynamic wf = this.CreateInstance(wd, result.Output);
            await wf.StartAsync();

            var resultA = await wf.ExecuteAsync("A");
            Assert.Contains(resultA.NextActivities, "B");

            var initB = await wf.InitiateAsyncWaitDelayAsync();//this.GetType().Name, name, unique)
            Assert.NotEmpty(initB.Correlation);
            Assert.Contains( "Wait Delay",scheduledTask);


        }

        [Fact]
        public async Task DelayExecute()
        {
            var scheduledTask = new List<string>();
            var ts = new Mock<ITaskScheduler>(MockBehavior.Strict);
            ts.Setup(x => x.AddTaskAsync(It.IsAny<DateTime>(), It.IsAny<ScheduledActivityExecution>()))
                .Callback((DateTime dt, ScheduledActivityExecution se) => scheduledTask.Add(se.Name))
                .Returns(Task.FromResult(0));
            ObjectBuilder.AddCacheList(ts.Object);

            var wd = this.Create("Wf586");
            wd.ActivityCollection.Add(new ReceiveActivity { Name = "Start isi borang", IsInitiator = true, WebId = "A", NextActivityWebId = "B" });
            wd.ActivityCollection.Add(new DelayActivity { Name = "Wait Delay", Seconds = 1, WebId = "B", NextActivityWebId = "C" });
            wd.ActivityCollection.Add(new EndActivity { WebId = "C", Name = "Habis" });
            var result =await this.CompileAsync(wd);
            dynamic wf = this.CreateInstance(wd, result.Output);
            await wf.StartAsync();

            var resultA = await wf.ExecuteAsync("A");
            Assert.Contains(resultA.NextActivities, "B");

            var initB = await wf.InitiateAsyncWaitDelayAsync();//this.GetType().Name, name, unique
            Assert.NotEmpty(initB.Correlation);
            Assert.Contains( "Wait Delay",scheduledTask);


            var resultB = await wf.ExecuteAsync("B", initB.Correlation);
            Assert.Contains(resultB.NextActivities, "C");
        }
    }
}