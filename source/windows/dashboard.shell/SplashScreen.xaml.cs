using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Bespoke.Station.Windows
{
    public partial class SplashScreen
    {
        public SplashScreen()
        {
            InitializeComponent();
            this.Loaded += SplashScreenLoaded;
          
        }

        async void SplashScreenLoaded(object sender, RoutedEventArgs e)
        {
            var sb = this.Resources["RotateSpinner"] as Storyboard;
            if (null != sb)
                sb.Begin(this);

            this.ShowProgress();


            await Task.Delay(TimeSpan.FromSeconds(15));
            this.Close();
        }

// ReSharper disable FunctionRecursiveOnAllPaths
        private async void ShowProgress()
// ReSharper restore FunctionRecursiveOnAllPaths
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            title.Text = title.Text == "Station SS .. please wait"
                             ? "Station SS .. please wait"
                             : "Station SS .. please wait..";
            ShowProgress();
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof (string), typeof (SplashScreen),
                                        new FrameworkPropertyMetadata(".......",
                                                                      FrameworkPropertyMetadataOptions
                                                                          .BindsTwoWayByDefault));

        public string Message
        {
            get { return (string) GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
    }
}
