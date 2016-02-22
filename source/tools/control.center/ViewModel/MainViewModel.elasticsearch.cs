using System;
using System.Net.Http;
using System.Threading;
using System.Windows;
using ElasticsearchInside;
using GalaSoft.MvvmLight.Command;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class MainViewModel
    {
        public RelayCommand StartElasticSearchCommand { get; set; }
        public RelayCommand StopElasticSearchCommand { get; set; }
        private Elasticsearch m_elasticsearch;

        public void SetupElasticsearch()
        {
            StartElasticSearchCommand = new RelayCommand(StartElasticsearch, () => !ElasticSearchServiceStarted && RabbitMqServiceStarted && !RabbitMqServiceStarting);
            StopElasticSearchCommand = new RelayCommand(StopElasticSearch, () => ElasticSearchServiceStarted);
        }

        public void StartElasticsearch()
        {
            this.IsBusy = true;
            this.QueueUserWorkItem(StartElasticsearchHelper);
        }

        private async void StartElasticsearchHelper()
        {

            Log("ElasticSearch...[INITIATING]");
            m_elasticsearch = new Elasticsearch(x => x
                .Port(this.Settings.ElasticsearchHttpPort)
                .EnableLogging()
                .LogTo((m, args) => Log(string.Format(m, args)))
            );

            var connected = false;
            // verify that elasticsearch started successfully
            var uri = new Uri($"http://localhost:{this.Settings.ElasticsearchHttpPort}");
            using (var client = new HttpClient() { BaseAddress = uri })
            {
                for (var i = 0; i < 20; i++)
                {
                    try
                    {
                        var ok = client.GetStringAsync("/").Result;
                        connected = ok.Contains("tagline");
                        break;
                    }
                    catch
                    {
                        // ignored
                        Thread.Sleep(500);
                    }
                }
            }
            if (connected)
            {
                ElasticSearchServiceStarted = true;
                ElasticSearchStatus = "Running";
                Log("ElasticSearch... [STARTED]");

                this.IsSetup = await this.FindOutSetupAsync();
                IsBusy = false;

            }
            else
            {
                this.Post(() =>
                {
                    this.IsBusy = false;
                    MessageBox.Show("Cannot start your Elasticsearch", "Reactive Developer", MessageBoxButton.OK, MessageBoxImage.Warning);
                });
            }

        }

        private void StopElasticSearch()
        {
            this.QueueUserWorkItem(() =>
            {
                m_elasticsearch.Dispose();
                m_elasticsearch = null;
                Log("ElasticSearch... [STOPPED]");
                ElasticSearchServiceStarted = false;
                ElasticSearchStatus = "Stopped";

            });
        }



    }
}
