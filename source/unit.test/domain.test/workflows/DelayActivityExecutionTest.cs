﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Moq;
using NUnit.Framework;

namespace domain.test.workflows
{
    public class DelayActivityExecutionTest : WorkflowTestBase
    {


        [Test]
        public async Task DelayInit()
        {
            var scheduledTask = new List<string>();
            var ts = new Mock<ITaskScheduler>(MockBehavior.Strict);
            ts.Setup(x => x.AddTaskAsync(It.IsAny<DateTime>(), It.IsAny<ScheduledActivityExecution>()))
                .Callback((DateTime dt, ScheduledActivityExecution se) => scheduledTask.Add(se.Name))
                .Returns(Task.FromResult(0));
            ObjectBuilder.AddCacheList(ts.Object);

            var wd = this.Create(25);
            wd.ActivityCollection.Add(new ScreenActivity { Name = "Start isi borang", IsInitiator = true, WebId = "A", NextActivityWebId = "B" });
            wd.ActivityCollection.Add(new DelayActivity { Name = "Wait Delay", Seconds = 1, WebId = "B", NextActivityWebId = "C" });
            wd.ActivityCollection.Add(new EndActivity { WebId = "C", Name = "Habis" });
            var result = this.Compile(wd);
            dynamic wf = this.Run(wd, result.Output, Console.WriteLine);

            var resultA = await wf.ExecuteAsync("A");
            CollectionAssert.Contains(resultA.NextActivities, "B");

            var initB = await wf.InitiateAsyncExecDelayActivityWaitDelay_BAsync();//this.GetType().Name, name, unique)
            Assert.IsNotNullOrEmpty(initB.Correlation);
            CollectionAssert.Contains(scheduledTask, "Wait Delay");


        }

        [Test]
        public async Task DelayExecute()
        {
            var scheduledTask = new List<string>();
            var ts = new Mock<ITaskScheduler>(MockBehavior.Strict);
            ts.Setup(x => x.AddTaskAsync(It.IsAny<DateTime>(), It.IsAny<ScheduledActivityExecution>()))
                .Callback((DateTime dt, ScheduledActivityExecution se) => scheduledTask.Add(se.Name))
                .Returns(Task.FromResult(0));
            ObjectBuilder.AddCacheList(ts.Object);

            var wd = this.Create(586);
            wd.ActivityCollection.Add(new ScreenActivity { Name = "Start isi borang", IsInitiator = true, WebId = "A", NextActivityWebId = "B" });
            wd.ActivityCollection.Add(new DelayActivity { Name = "Wait Delay", Seconds = 1, WebId = "B", NextActivityWebId = "C" });
            wd.ActivityCollection.Add(new EndActivity { WebId = "C", Name = "Habis" });
            var result = this.Compile(wd);
            dynamic wf = this.Run(wd, result.Output, Console.WriteLine);

            var resultA = await wf.ExecuteAsync("A");
            CollectionAssert.Contains(resultA.NextActivities, "B");

            var initB = await wf.InitiateAsyncExecDelayActivityWaitDelay_BAsync();//this.GetType().Name, name, unique
            Assert.IsNotNullOrEmpty(initB.Correlation);
            CollectionAssert.Contains(scheduledTask, "Wait Delay");


            var resultB = await wf.ExecuteAsync("B", initB.Correlation);
            CollectionAssert.Contains(resultB.NextActivities, "C");
        }
    }
}