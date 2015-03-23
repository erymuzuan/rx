using System;
using System.Windows;

namespace Bespoke.Sph.ControlCenter
{
    class Program
    {
        [STAThread]
        public static void Main()
        {
            var process = System.Diagnostics.Process.GetProcessesByName("controlcenter");
            if (process.Length > 1)
            {
                MessageBox.Show("There's another instance of Reactive Developer control center running","ReactiveDeveloper",
                    MessageBoxButton.OK, 
                    MessageBoxImage.Information);
                return;
            }
            // --
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
