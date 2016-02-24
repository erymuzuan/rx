using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public class ReadonlyRepository<T> : IDisposable, IReadonlyRepository<T> where T : Entity
    {
        private readonly HttpClient m_client;

        public ReadonlyRepository()
        {
            m_client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) };
        }

        public ReadonlyRepository(HttpMessageHandler httpMessageHandler, bool disposeHandler)
        {
            m_client = new HttpClient(httpMessageHandler, disposeHandler) { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) };
        }

        public async Task<LoadData<T>> LoadOneAsync(string id)
        {
            var url = $"{ ConfigurationManager.ApplicationName.ToLower()}/{typeof(T).Name.ToLower()}/{id}";
            var response = await m_client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return new LoadData<T>(null, null);

            var content = response.Content as StreamContent;
            if (null == content) throw new Exception("Cannot execute query on es ");
            var responseString = await content.ReadAsStringAsync();

            var esJson = JObject.Parse(responseString);
            var source = esJson.SelectToken("$._source");
            var version = esJson.SelectToken("$._version").Value<string>();
            var item = source.ToString().DeserializeFromJson<T>();

            return new LoadData<T>(item, version) { Json = source.ToString() };
        }

        public void Dispose()
        {
            m_client.Dispose();
        }
    }
}