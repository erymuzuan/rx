using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.CosmosDbRepository;
using Bespoke.Sph.CosmosDbRepository.Extensions;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Tests.CosmosDb.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Tests.CosmosDb
{
    public class CosmosDbFixture : IDisposable
    {
        private readonly string m_databaseId;
        private readonly string m_collectionId;
        private readonly DocumentClient m_client;
        public int OfferThroughput { get; set; } = 400;
        public IReadOnlyRepository<Patient> Repository { get; }

        public CosmosDbFixture()
        {
            m_databaseId = "unit_test";
            m_collectionId = nameof(Patient);

            m_client = new DocumentClient(new Uri("https://localhost:8081"), "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            this.Repository = new ReadOnlyRepository<Patient>();
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
            await CreateDatabaseIfNotExistsAsync();
            await CreateCollectionIfNotExistsAsync();
        }

        public void Dispose()
        {
            m_client?.Dispose();
        }


        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await m_client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(m_databaseId));
            }
            catch (DocumentClientException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await m_client.CreateDatabaseAsync(new Database { Id = m_databaseId });
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await m_client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(m_databaseId, m_collectionId));
            }
            catch (DocumentClientException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await m_client.CreateDocumentCollectionAsync(
                    UriFactory.CreateDatabaseUri(m_databaseId),
                    new DocumentCollection { Id = m_collectionId },
                    new RequestOptions { OfferThroughput = OfferThroughput });

            }

            //

            var files = Directory.GetFiles($@"{ConfigurationManager.Home}\..\source\unit.test\sample-data-patients\", "*.json").ToArray();
            var done = 0;
            const int BATCH_SIZE = 10;
            for (var i = 0; i < 100; i++)
            {
                var tasks = from file in files.Skip(BATCH_SIZE * i).Take(BATCH_SIZE)
                            let id = Path.GetFileNameWithoutExtension(file)
                            let text = File.ReadAllText(file)
                            let patient = JObject.Parse(text).CreateIdNodeWith("$.Id")
                            select m_client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(m_databaseId, m_collectionId), patient);


                await Task.WhenAll(tasks);
                done += BATCH_SIZE;
                if (done >= files.Length) break;

            }
        }

    }
}