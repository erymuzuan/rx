using System;
using System.Net;
using System.Net.Http;
using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public class UpdaterViewModel : ViewModelBase
    {
        public RelayCommand CheckUpdateCommand { get; set; }
        public UpdaterViewModel()
        {
            this.CheckUpdateCommand = new RelayCommand(CheckUpdate);
        }

        private async void CheckUpdate()
        {
            using (var client = new HttpClient { BaseAddress = new Uri("http://www.bespoke.com.my/") })
            {
                const string url = "download/version.json";

                try
                {
                    var response = await client.GetAsync(url);
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        return;
                    var content = response.Content as StreamContent;
                    if (null == content) return;
                    var json = await content.ReadAsStringAsync();
                    MessageBox.Show(json);
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show(e.ToString()) ;
                }
            }
        }
    }
}
