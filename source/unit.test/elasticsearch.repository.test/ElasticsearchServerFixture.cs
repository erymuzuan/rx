﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository;
using Bespoke.Sph.Extensions;
using Newtonsoft.Json.Linq;
using Xunit;

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
            var mapping = JObject.Parse(System.Text.Encoding.UTF8.GetString(Properties.Resources.Patient));
            this.Repository = new ReadOnlyRepository<Patient>(URL, INDEX, mapping);
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

    [CollectionDefinition(ELASTICSEARCH_COLLECTION)]
    public class ElasticsearchServerCollection : ICollectionFixture<ElasticsearchServerFixture>
    {
        public const string ELASTICSEARCH_COLLECTION = "Elasticsearch collection";
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}