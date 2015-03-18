using System;
using System.Windows.Threading;
using Bespoke.Sph.ControlCenter.Model;
using Bespoke.Station.Windows;

namespace Bespoke.Sph.ControlCenter
{
    public partial class App
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var entry = new LogEntry(e.ExceptionObject as Exception);
            var message = entry.ToString();

            var window = new ErrorWindow(message);
            window.ShowDialog();
            this.Shutdown(-1);
            
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var entry = new LogEntry(e.Exception);
            var message = entry.ToString();

            var window = new ErrorWindow(message);
            window.ShowDialog();
            e.Handled = true;
            this.Shutdown(-1);
        }
    }
}
