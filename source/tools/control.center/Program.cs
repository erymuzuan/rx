using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace Bespoke.Sph.ControlCenter
{
    class Program
    {
        static readonly Mutex mutex = new Mutex(true, "{90084509-6E92-4022-8074-9D0192414ED0}");
        [STAThread]
        public static void Main()
        {
            var projectJson = AppDomain.CurrentDomain.BaseDirectory + "/../project.json";
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {

                var setup = File.Exists(projectJson);
                if (!setup)
                {
                    var wizard = new SetupWizardWindow();
                    wizard.ShowDialog();
                    if (!File.Exists(projectJson))
                    {
                        mutex.ReleaseMutex();
                        return;
                    }

                    var app = new App();
                    app.InitializeComponent();
                    app.Run();

                }
                else
                {
                    var app = new App();
                    app.InitializeComponent();
                    app.Run();
                }

                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("There's another instance of Reactive Developer control center running", "ReactiveDeveloper",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}
