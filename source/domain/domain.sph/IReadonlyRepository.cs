using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IReadonlyRepository
    {
        /*
         
                // delete the elasticsearch data
                using (var client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) })
                {
                    var message = new HttpRequestMessage(HttpMethod.Delete,
                        $"{ConfigurationManager.ElasticSearchIndex}/{name.ToLowerInvariant()}/_query")
                    {
                        Content = new StringContent(
    @"{
   ""query"": {
      ""match_all"": {}
   }
}")
                    };
                    await client.SendAsync(message);
                }

 
             */
        /* using (var client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) })
  {
      var message = new HttpRequestMessage(HttpMethod.Delete,
          $"{ConfigurationManager.ElasticSearchIndex}/{model.Entity.ToLowerInvariant()}/_query")
      {
          Content = new StringContent(
@"{
""query"": {
""match_all"": {}
}
}")
      };
      await client.SendAsync(message);
  }*/
        Task TruncateAsync(EntityDefinition ed);


        /* using (var client = new HttpClient())
            {
                client.DeleteAsync(
                    $"{ConfigurationManager.ElasticSearchHost}/{ConfigurationManager.ElasticSearchIndex}/_mapping/{name.ToLowerInvariant()}")
                    .Wait(5000);
            }*/
        Task CleanAsync(EntityDefinition ed);


        /*using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.DeleteAsync(ConfigurationManager.ApplicationName);
                Console.WriteLine("DELETE {1} index : {0}", response.StatusCode, ConfigurationManager.ApplicationName);
                await client.PutAsync(ConfigurationManager.ApplicationName, new StringContent(""));

            }*/
        Task CleanAsync();



        /*
            var query = @"
                {
                    ""query"": {
                        ""query_string"": {
                           ""default_field"": ""_all"",
                           ""query"": """ + text + @"""
                        }
                    },
                   ""highlight"": {
                        ""fields"": {
                            " + records + @"
                        }
                    },  
                  ""from"": 0,
                  ""size"": 20
                }
            ";

            var request = new StringContent(query);
            var url = $"{ConfigurationManager.ElasticSearchIndex}/{types}/_search";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.PostAsync(url, request);
                var content = response.Content as StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es " + request);
                var result = await content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new SearchException("Cannot execute query for :" + text) { Query = query, Result = result };
                return Json(result);

            }*/
            //--
            /*        /*
            var request = new StringContent(query);
            var log = type == "log" || type == "request_log";
            var index = sys ? ConfigurationManager.ElasticSearchSystemIndex : ConfigurationManager.ElasticSearchIndex;
            if (log) index = $"{ConfigurationManager.ElasticSearchIndex}_logs";
            var url = $"{index}/{type.ToLowerInvariant()}/_search";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(log ? ConfigurationManager.ElasticsearchLogHost : ConfigurationManager.ElasticSearchHost);

                var response = await client.PostAsync(url, request);
                var content = response.Content as StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es " + request);

                var result = await content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    throw new SearchException("Cannot execute query for : " + type) { Query = query, Result = result };

                return Json(result);

            }*/*/
        Task<object> SearchAsync(string types, Filter[] filters);




        /*
            var json = await
                m_elasticsearchHttpClient.GetStringAsync(
                    $"{ConfigurationManager.ElasticSearchIndex}/{model.Entity.ToLowerInvariant()}/_count");
            var jo = JObject.Parse(json);
            return jo.SelectToken("$.count").Value<int>();*/
        Task<int> GetCountAsync(string entity);
    }
    public interface IReadonlyRepository<T> where T : Entity
    {
        Task<LoadData<T>> LoadOneAsync(string id);
        Task<LoadData<T>> LoadOneAsync(string field, string value);
        Task<LoadOperation<T>> SearchAsync(Filter[] filters, int skip, int size);
        Task<string> SearchAsync(string query);
        Task<string> SearchAsync(string query, string queryString);
        Task<int> GetCountAsync(string query, string queryString);
        Task<int> GetCountAsync(Expression<Func<T, bool>> query);
        Task<IEnumerable<TResult>> GetListAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
        Task<TResult> GetMaxAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
    }

    public class LoadData<T> where T : Entity
    {
        public LoadData(T source, string version)
        {
            Source = source;
            Version = version;
        }

        public T Source { get;  }
        public string Version { get; }
        public string Json { get; set; }
    }
}