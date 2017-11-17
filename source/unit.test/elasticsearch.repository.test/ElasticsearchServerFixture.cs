using System;
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
            this.Repository = new ReadOnlyRepository<Patient>(URL, INDEX);
            // TODO : RUN source\unit.test\sample-data-patients\create-index.linq
            this.InitializeIndexAsync()
                .ContinueWith(_ =>
                {
                    if (_.Exception != null)
                        throw _.Exception;
                }).Wait();

        }

        public async Task InitializeIndexAsync()
        {
            var response = await this.Client.GetAsync($"{INDEX}/{TYPE}/_count");
            response.EnsureSuccessStatusCode();

            var json = await response.ReadContentAsJsonAsync();
            var count = json["count"].Value<int>();
            if (count != 100) throw new InvalidOperationException("Remove the test index and create them with TODO : RUN source\\unit.test\\sample-data-patients\\create-index.linq");

        }

        public void Dispose()
        {
            Client?.Dispose();
        }

        public HttpClient Client { get; private set; }
    }
}