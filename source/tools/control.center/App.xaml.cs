using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Threading;
using Bespoke.Sph.ControlCenter.Model;
using Bespoke.Station.Windows;
using Mindscape.Raygun4Net;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ControlCenter
{
    public partial class App
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            InitLogger();
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var entry = new LogEntry(e.ExceptionObject as Exception);
            var message = entry.ToString();

            m_client.SendInBackground(e.ExceptionObject as Exception, new List<string>());
            var window = new ErrorWindow(message);
            window.ShowDialog();
            this.Shutdown(-1);
            
        }

        private  RaygunClient m_client;
        private void InitLogger()
        {
            var version = "unknown";
            string file = "..\\version.json";
            if (File.Exists(file))
            {
                var json = JObject.Parse(File.ReadAllText(file));
                var build = json.SelectToken("$.build").Value<int>();
                version = build.ToString();
            }
            m_client = new RaygunClient("imHU3x8eZamg84BwYekfMQ==")
            {
                ApplicationVersion = version
            };
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
