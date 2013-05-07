using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Bespoke.Cycling.Domain;
using Bespoke.Station.Windows.Views.Utils;

namespace Bespoke.Station.Windows
{
    public static class Program
    {

        public const string EXCUTING_ASSEMBLE_EXE = "dashboard.shell.exe";
        private static App m_app;

        [STAThread]
        public static void Main(string[] args)
        {
            var cul = CultureInfo.CreateSpecificCulture("ms-MY");
            cul.DateTimeFormat.ShortDatePattern = "d/M/yyyy";
            cul.DateTimeFormat.ShortestDayNames = new[] { "Ahd", "Isn", "Sel", "Rab", "Kha", "Jum", "Sab" };
            cul.DateTimeFormat.ShortTimePattern = "h:m tt";
            cul.DateTimeFormat.LongDatePattern = "dd MMM yyyy";
            cul.DateTimeFormat.ShortTimePattern = "HH:mm";
            cul.DateTimeFormat.LongTimePattern = "hh:mm tt";

            cul.NumberFormat.CurrencyDecimalDigits = 2;
            cul.NumberFormat.CurrencySymbol = "RM ";
            cul.NumberFormat.CurrencyDecimalSeparator = ".";
            cul.NumberFormat.CurrencyGroupSeparator = " ";
            cul.NumberFormat.NumberDecimalSeparator = ".";
            cul.NumberFormat.NumberDecimalDigits = 2;
            cul.NumberFormat.NumberGroupSeparator = " ";

            Thread.CurrentThread.CurrentCulture = cul;
            Thread.CurrentThread.CurrentUICulture = cul;

            string username, password;
            ParseArguments(args, out username, out password);


            var login = new LoginWindow(username, password);
            login.ShowDialog();
            if (login.DialogResult ?? false)
            {
                var t = new Thread(ShowSplashScreen);
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                RegisterServices();
                m_app = new App();
                m_app.InitializeComponent();
                m_app.Run();
            }

        }

        private  static void ShowSplashScreen()
        {
            var splash = new SplashScreen();
            splash.Show();
            Task.Delay(TimeSpan.FromSeconds(5)).Wait();
            splash.Message = "Loading main windows";
            Task.Delay(TimeSpan.FromSeconds(5)).Wait();
            splash.Message = "Loading main components..";
            Task.Delay(TimeSpan.FromSeconds(5)).Wait();
            splash.Message = "Loading main data..";
            splash.Close();

        }

        private static void RegisterServices()
        {
            var spring = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "spring.xml");
            ObjectBuilder.RegisterSpring(new Uri(spring).AbsoluteUri);

        }

        private static void ParseArguments(IEnumerable<string> args, out string username, out string password)
        {
            password = string.Empty;
            username = string.Empty;
            foreach (string a in args)
            {
                if (a.StartsWith("/password:"))
                {
                    password = a.Replace("/password:", string.Empty);
                }

                if (a.StartsWith("/user:"))
                {
                    username = a.Replace("/user:", string.Empty);
                }
                if (a.StartsWith("/halt"))
                {
                    MessageBox.Show("Debug");
                }
            }

        }


    }
}