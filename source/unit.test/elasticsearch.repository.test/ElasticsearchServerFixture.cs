using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository;
using Bespoke.Sph.Extensions;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Tests.Elasticsearch
{
    public class ElasticsearchServerFixture : IDisposable
    {
        public const string INDEX = "test_index";
        public const string TYPE = "patient";
        public const string URL = "http://localhost:9200";
        public string Index => INDEX;
        public string Type => TYPE;
        public string Url => URL;
        public IReadOnlyRepository<Patient> Repository { get; }
        public ElasticsearchServerFixture()
        {
            Client = new HttpClient { BaseAddress = new Uri(URL) };
            this.Repository = new ReadOnlyRepository<Patient>(URL,INDEX);
            // this.InitializeIndexAsync().Wait();

        }

        public async Task InitializeIndexAsync()
        {
            var response = await this.Client.GetAsync("test_index/patient/_count");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.ReadContentAsJsonAsync();
                var count = json["count"].Value<int>();
                if (count == 100) return;
            }
            var delete = await this.Client.DeleteAsync(INDEX);
            Console.WriteLine(delete.StatusCode);
            var index = await this.Client.PostAsync(INDEX, new StringContent(""));
            index.EnsureSuccessStatusCode();

            var mapping = await this.Client.PutAsync($"{INDEX}/_mapping/{TYPE}", new ByteArrayContent(Properties.Resources.Patient));
            mapping.EnsureSuccessStatusCode();
            /* */
            var tasks = from file in Directory.GetFiles(
                    $"../../../sample-data-patients/", "*.json")
                        let id = Path.GetFileNameWithoutExtension(file)
                        let content = new StringContent(File.ReadAllText(file))
                        select this.Client.PostAsync($"/{INDEX}/{TYPE}/{id}", content);

            await Task.WhenAll(tasks);
        }

        public void Dispose()
        {
            Client?.Dispose();
        }

        public HttpClient Client { get; private set; }
    }
}