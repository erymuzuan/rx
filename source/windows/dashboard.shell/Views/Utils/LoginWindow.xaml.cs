using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Bespoke.CommercialSpace.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using Bespoke.Sph.Windows.Helpers;
using Telerik.Windows.Controls;

namespace Bespoke.Station.Windows.Views.Utils
{
    public partial class LoginWindow
    {
        public LoginWindow(string username, string password)
        {
            StyleManager.ApplicationTheme = new Windows8Theme();
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Loaded += LoginWindowLoaded;
            this.KeyDown += LoginWindowKeyDown;
            this.userNameTextBox.Text = username;
            this.passwordTextBox.Password = password;


        }

        void LoginWindowKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                this.LoginButtonClick(this, new RoutedEventArgs());
            }

        }

        async void LoginWindowLoaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(150);
            this.Post(() => userNameTextBox.Focus());
            if (string.IsNullOrWhiteSpace(userNameTextBox.Text) || string.IsNullOrWhiteSpace(passwordTextBox.Password)) return;
            await Task.Delay(500);
            this.LoginButtonClick(this, new RoutedEventArgs());
        }

        private async void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                messageLabel.Text = string.Empty;
                this.busyIndicator.IsBusy = true;
                var request = await WebRequestHelper.CreateHttpWebRequest("Account/JsonLogOn2")
                    .SetPostJsonDataAsync(new { username = userNameTextBox.Text, password = passwordTextBox.Password });

                var response = (HttpWebResponse)(await request.GetResponseAsync());
                var stream = response.GetResponseStream();
                var logon = await stream.DeserializeJsonAsync<logonJson>();
                if (logon.success)
                {
                    // get the cookies
                    var setCookie = response.Headers["Set-Cookie"];

                    const string pattern = @".angrychains=(?<angrychains>.*?);";
                    const RegexOptions option = RegexOptions.IgnoreCase | RegexOptions.Multiline;
                    var matches = Regex.Matches(setCookie, pattern, option);

                    var cookies = new Dictionary<string, string>();
                    var station = matches[0].Groups["angrychains"].Value;
                    cookies.Add(".angrychains", station);
                    
                    CredentialProvider.Initiliaze(cookies);
                    this.DialogResult = true;
                    this.Close();
                    return;
                }
                messageLabel.Text = "Sila betulkan " + string.Join(",", logon.errors)
                    ;
            }
            catch (WebException exc)
            {
                MessageBox.Show(exc.Message, "Tiada internet", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            this.busyIndicator.IsBusy = false;
        }

// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Local
        class logonJson
        {
            public bool success { get; set; }
            public string[] errors { get; set; }
            public string[] roles { get; set; }

        }
// ReSharper restore UnusedAutoPropertyAccessor.Local
// ReSharper restore InconsistentNaming
// ReSharper restore ClassNeverInstantiated.Local
    }
}
