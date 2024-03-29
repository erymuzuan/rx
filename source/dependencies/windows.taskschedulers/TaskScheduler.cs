﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32.TaskScheduler;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Messaging;

namespace Bespoke.Sph.WindowsTaskScheduler
{
    public class TaskScheduler : ITaskScheduler
    {
        private string m_executable;
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EncryptedPassword { get; set; }
        public string FolderName { get; set; }

        public TaskScheduler()
        {
            this.FolderName = "Bespoke";
        }

        public TaskScheduler(string executable)
        {
            this.Executable = executable;
            this.FolderName = "Bespoke";
        }

        public string Executable
        {
            get
            {
                if (string.IsNullOrWhiteSpace(m_executable))
                {
                    return m_executable = ConfigurationManager.DelayActivityExecutable;
                }
                return m_executable;
            }
            set { m_executable = value; }
        }

        public System.Threading.Tasks.Task AddTaskAsync(DateTime dateTime, ScheduledActivityExecution info)
        {
            var path = GetPath(info);
            using (var ts = new TaskService())
            {
                var folder = ts.RootFolder.SubFolders.FirstOrDefault(f => f.Name == this.FolderName) ??
                             ts.RootFolder.CreateFolder(FolderName);

                var td = ts.NewTask();
                td.Settings.Compatibility = TaskCompatibility.V2;
                td.RegistrationInfo.Source = "sph";
                td.Settings.Hidden = true;
                td.RegistrationInfo.Description = "Scheduled task for Delay Activity for " + info;

                // one time trigger
                td.Triggers.Add(new TimeTrigger(dateTime));
                var action = new ExecAction(this.Executable, $"{info.ActivityId} {info.InstanceId}")
                {
                    WorkingDirectory = System.IO.Path.GetDirectoryName(this.Executable)
                };
                td.Actions.Add(action);

                if (!string.IsNullOrWhiteSpace(this.Password) && !string.IsNullOrWhiteSpace(UserName))
                    folder.RegisterTaskDefinition(path, td, TaskCreation.Create, this.UserName, this.Password, TaskLogonType.Password);
                else
                    folder.RegisterTaskDefinition(path, td);
            }
            return System.Threading.Tasks.Task.FromResult(0);

        }
        public System.Threading.Tasks.Task DeleteAsync(ScheduledActivityExecution info)
        {
            var path = GetPath(info);
            try
            {
                using (var ts = new TaskService())
                {
                    var folder = ts.RootFolder.SubFolders.FirstOrDefault(f => f.Name == this.FolderName) ??
                                 ts.RootFolder.CreateFolder(FolderName);
                    folder.DeleteTask(path);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // NOTE : when a ReceiveActivity is invoked from ListenActivity branch, a delay might be cancelled
                // and this is invoke from ASP.net, thus we have to delegate this operation to the deletedelay subscriber
                var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();
                var item = new Entity[] { new Tracker { WebId = path,Id = Guid.NewGuid().ToString()} };
                publisher.PublishDeleted("DeleteDelayActivity", item, new Dictionary<string, object>());
            }

            return System.Threading.Tasks.Task.FromResult(0);
        }

        private static string GetPath(ScheduledActivityExecution info)
        {
            var guid = $"DELAY_{info.InstanceId}_{info.ActivityId}";
            var path = guid.Replace(" ", string.Empty);
            return path;
        }
    }
}
