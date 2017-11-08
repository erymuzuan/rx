using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Bespoke.Sph.Extensions;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public class ReadonlyRepository<T> : IDisposable, IReadonlyRepository<T> where T : Entity
    {
        private readonly HttpClient m_client;
        private readonly string m_url;

        public ReadonlyRepository(string host, string indexName)
        {

            if (null == m_client)
                m_client = new HttpClient { BaseAddress = new Uri(host) };
            m_url = indexName + "/" + typeof(T).Name.ToLowerInvariant();

        }

        public ReadonlyRepository() : this(EsConfigurationManager.Host, EsConfigurationManager.Index)
        {
        }

        public ReadonlyRepository(string host, string indexName, HttpMessageHandler httpMessageHandler, bool disposeHandler) : this(host, indexName)
        {
            m_client = new HttpClient(httpMessageHandler, disposeHandler) { BaseAddress = new Uri(host) };
        }

        public ReadonlyRepository(HttpMessageHandler httpMessageHandler, bool disposeHandler) : this(EsConfigurationManager.Host, EsConfigurationManager.Index)
        {
            m_client = new HttpClient(httpMessageHandler, disposeHandler) { BaseAddress = new Uri(EsConfigurationManager.Host) };
        }

        public async Task<LoadData<T>> LoadOneAsync(string id)
        {
            var url = $"{m_url}/{id}";
            var response = await m_client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return new LoadData<T>(null, null);

            var esJson = await response.ReadContentAsJsonAsync();
            var source = esJson.SelectToken("$._source");
            var version = esJson.SelectToken("$._version").Value<string>();
            var item = source.ToString().DeserializeFromJson<T>();

            return new LoadData<T>(item, version) { Json = source.ToString() };
        }

        public async Task<LoadData<T>> LoadOneAsync(string field, string value)
        {
            var url = $"{ m_url}/_search?q={field}:{value}&version=true";
            var response = await m_client.GetAsync(url);
            var json = await response.ReadContentAsJsonAsync();
            var total = json.SelectToken("$.hits.total").Value<int>();
            if (total == 0)
                return new LoadData<T>(null, null);
            if (total > 1)
                throw new InvalidOperationException($"{typeof(T).Name} query returns more than one result - {field}:{value}");

            var source = json.SelectToken("$.hits.hits[0]._source");
            var version = json.SelectToken("$.hits.hits[0]._version").Value<string>();
            var item = source.ToString().DeserializeFromJson<T>();

            return new LoadData<T>(item, version) { Json = source.ToString() };
        }

        public async Task<LoadOperation<T>> SearchAsync(Query query)
        {
            var query = ((T)null).GenerateQueryDsl(query.Filters, query.Sorts, query.Skip, query.Size);
            var response = await m_client.PostAsync($"{m_url}/_search", new StringContent(query));
            var lo = await response.ReadContentAsLoadOperationAsync<T>(query.Skip, query.Size);

            return lo;
        }

        public async Task<LoadOperation<T>> SearchAsync(string query)
        {
            var request = new StringContent(query);
            var url = $"{m_url}/_search";
            var response = await m_client.PostAsync(url, request);
            return await response.ReadContentAsLoadOperationAsync<T>();
        }

        public async Task<string> SearchAsync(string query, string queryString)
        {
            var request = new StringContent(query);
            var url = $"{m_url}/_search?" + queryString;
            var response = await m_client.PostAsync(url, request);
            return await response.ReadContentAsStringAsync();
        }


        public async Task<int> GetCountAsync(Filter[] filters)
        {
            var query = (default(T)).GenerateBoolQueryDsl(filters);
            var request = new StringContent(query);
            var url = $"{m_url}/_count";

            var response = await m_client.PostAsync(url, request);

            var json = await response.ReadContentAsJsonAsync();
            var count = json.SelectToken("$.count").Value<int>();
            return count;
        }

        public Task<int> GetCountAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> GetListAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMaxAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMaxAsync<TResult>(Filter[] filters = null, Sort[] sorts = null, int skip = 0, int size = 20)
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {
            m_client.Dispose();
        }
    }
}