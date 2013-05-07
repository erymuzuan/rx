using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using System.Net;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using Bespoke.Station.Windows.Helpers;
using Bespoke.Station.Windows.Infrastructure;
using GalaSoft.MvvmLight.Command;

namespace Bespoke.Station.Windows.ViewModels
{
    [Export]
    public class ServerReportsViewModel : StationViewModelBase<Ride>
    {
        public RelayCommand<string> LoadUrlCommand { get; set; }

        public ServerReportsViewModel()
        {
            if(this.IsInDesignMode)return;

            this.LoadUrlCommand = new RelayCommand<string>(Loadurl, s => !string.IsNullOrWhiteSpace(s));
        }

        private void Loadurl(string url)
        {
            
        }

        protected async override void OnViewReady()
        {
            var request = (HttpWebRequest)WebRequestHelper
                .CreateHttpWebRequest("Reports/Kpdnhep",method:"GET",json:false)
                .SetCredential();

            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            var response = await request.GetResponseAsync();
            if (null == response || null == response.GetResponseStream())
            {
                return;
            }
            var responseStream = response.GetResponseStream();
            if (null != responseStream)
            {
                using (var sr = new StreamReader(responseStream))
                {
                    var web = ConfigurationManager.AppSettings["DataServiceurl"];
                    string result = await sr.ReadToEndAsync();
                    result = result
                        .Replace("src=\"/", "src=\"" + web + "/")
                        .Replace("href=\"/", "href=\"" + web + "/");
                    this.HtmlText = result;

                }
            }
        }
        private string m_htmlText;


        public string HtmlText
        {
            get { return m_htmlText; }
            set
            {
                m_htmlText = value;
                RaisePropertyChanged("HtmlText");
            }
        }
    }
}