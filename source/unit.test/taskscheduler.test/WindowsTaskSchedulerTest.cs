using System;
using System.Threading.Tasks;
using FluentDate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bespoke.Sph.Domain;
using TaskScheduler = Bespoke.Sph.WindowsTaskScheduler.TaskScheduler;

namespace taskscheduler.test
{
    [TestClass]
    public class WindowsTaskSchedulerTest
    {
        [TestMethod]
        public void AddAndRemove()
        {
            var info = new ScheduledActivityExecution { InstanceId = 1, ActivityId = "A", Name = "Unit test" };
            var job = new TaskScheduler { Executable = @"C:\project\work\sph\source\scheduler\scheduler.delayactivity\bin\Debug\scheduler.delayactivity.exe" };
            job.AddTaskAsync(DateTime.Now.AddSeconds(5), info).Wait();

           
        }
    }
}
