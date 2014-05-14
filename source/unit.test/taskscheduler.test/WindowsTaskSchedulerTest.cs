using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bespoke.Sph.Domain;
using Microsoft.Win32.TaskScheduler;
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
            var job = new TaskScheduler(@"C:\project\work\sph\source\scheduler\scheduler.delayactivity\bin\Debug\scheduler.delayactivity.exe");
            job.AddTaskAsync(DateTime.Now.AddSeconds(5), info).Wait();


        }


        [TestMethod]
        public void HiddenTask()
        {
            const string name = "test-hidden";

            const string executable = "notepad.exe";
            const string folderName = "unit-test";

            using (var ts = new TaskService())
            {
                var folder = ts.RootFolder.SubFolders.FirstOrDefault(f => f.Name == folderName) ??
                             ts.RootFolder.CreateFolder(folderName);

                var td = ts.NewTask();
                td.Settings.Compatibility = TaskCompatibility.V2_2;
                td.RegistrationInfo.Source = "sph";
                td.RegistrationInfo.Description = "Unit test ";

                td.Settings.Hidden = true;


                td.Triggers.Add(new TimeTrigger(DateTime.Now.AddSeconds(10)));
                var action = new ExecAction(executable, "test 1234")
                {
                    WorkingDirectory = System.IO.Path.GetDirectoryName(executable)
                };
                td.Actions.Add(action);


                folder.RegisterTaskDefinition(name, td);
            }
        }
        [TestMethod]
        public void LoggedOnOrOffTask()
        {
            const string name = "test";
            const string userName = "administrator";
            const string password = "WajaWKL9459Silver";
            const string executable = "notepad.exe";
            const string folderName = "unit-test";

            using (var ts = new TaskService())
            {
                var folder = ts.RootFolder.SubFolders.FirstOrDefault(f => f.Name == folderName) ??
                             ts.RootFolder.CreateFolder(folderName);

                var td = ts.NewTask();
                td.Settings.Compatibility = TaskCompatibility.V2_2;
                td.RegistrationInfo.Source = "sph";
                td.RegistrationInfo.Description = "Unit test ";
                //td.Principal.UserId = userName;


                td.Triggers.Add(new TimeTrigger(DateTime.Now.AddSeconds(10)));
                var action = new ExecAction(executable, "test 1234")
                {
                    WorkingDirectory = System.IO.Path.GetDirectoryName(executable)
                };
                td.Actions.Add(action);


                //folder.RegisterTaskDefinition(name, td);
                folder.RegisterTaskDefinition(name, td, TaskCreation.CreateOrUpdate, userName, password, TaskLogonType.Password);
            }
        }
    }
}
