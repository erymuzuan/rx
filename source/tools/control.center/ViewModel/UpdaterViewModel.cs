using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public class UpdaterViewModel : ViewModelBase
    {
        public RelayCommand CheckUpdateCommand { get; set; }
        private bool m_isBusy;

        public bool IsBusy
        {
            get { return m_isBusy; }
            set
            {
                m_isBusy = value;
                OnPropertyChanged();
            }
        }
        public UpdaterViewModel()
        {
            this.CheckUpdateCommand = new RelayCommand(CheckUpdate);
        }

        private async void CheckUpdate()
        {
            var file = @".\version.json".TranslatePath();
            if (!File.Exists(file))
            {
                MessageBox.Show("Can't find " + file + " in your root", "Rx Developer", MessageBoxButton.OK,
                    MessageBoxImage.Stop);
                return;
            }
            this.IsBusy = true;
            var text = File.ReadAllText(file);
            var json = JObject.Parse(text);
            var build = json.SelectToken("$.build").Value<int>();
            using (var client = new HttpClient { BaseAddress = new Uri("http://www.bespoke.com.my/") })
            {
                var url = "download/" + build + ".json";

                try
                {
                    var response = await client.GetAsync(url);
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        MessageBox.Show("Now new update is found, Please check again in the future", "Rx Developer", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        return;

                    }
                    var content = response.Content as StreamContent;
                    if (null == content) return;
                    var responseJson = await content.ReadAsStringAsync();
                    if (string.IsNullOrWhiteSpace(responseJson))
                    {
                        MessageBox.Show("Too bad, not getting any update info", Strings.Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;

                    }

                    var jo = JObject.Parse(responseJson);
                    var vnext = jo.SelectToken("$.vnext").Value<int>();
                    if (vnext > build)
                    {
                        var updateScript = jo.SelectToken("$.update-script").Value<string>();
                        var ps1 = await client.GetByteArrayAsync("download/" + updateScript);
                        File.WriteAllBytes(updateScript.TranslatePath(), ps1);

                        var info = new ProcessStartInfo
                        {
                            FileName = "powershell",
                            Arguments = updateScript.TranslatePath(),
                            WorkingDirectory = Path.GetDirectoryName(updateScript.TranslatePath())
                        };
                        var ps = Process.Start(info);
                        if (null == ps) throw new InvalidOperationException("Cannot start Powershell");
                        ps.WaitForExit();
                        MessageBox.Show( updateScript+ " has been downloaded to your working directory, use Powershell to execute the update", Strings.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("No update is available", Strings.Title, MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show(e.ToString());
                }
                finally
                {
                    this.IsBusy = false;
                }
            }
        }
    }
}
