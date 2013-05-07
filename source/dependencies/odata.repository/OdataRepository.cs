using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bespoke.Station.Domain;

namespace Bespoke.Station.OdataRepository
{
    public class OdataRepository<T> : IRepository<T> where T : Entity
    {
        public string ServiceUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(m_serviceUrl))
                    m_serviceUrl = ConfigurationManager.AppSettings["DataServiceUrl"];
                
                return m_serviceUrl;
            }
            set { m_serviceUrl = value; }
        }

        private readonly List<PageToken> m_tokens = new List<PageToken>();
        private string m_serviceUrl;

        class PageToken
        {
            public string Query { get; set; }
            public int Page { get; set; }
            public int? NextSkipToken { get; set; }
        }

        public async Task<T> LoadOneAsync(IQueryable<T> query)
        {
            var lo = await this.LoadAsync(query);
            return lo.ItemCollection.SingleOrDefault();
        }

        public async Task<int> GetCountAsync(IQueryable<T> query)
        {
            return await ExecuteScalarAsync<int>(query, "*","count");
        }

        public async Task<TResult> GetSumAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            var column = this.GetMemberName(selector);
            return await ExecuteScalarAsync<TResult>(query, column,"sum");
        }
 
        public async Task<TResult> GetSumAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult?>> selector) where TResult : struct
        {
            var column = this.GetMemberName(selector);
            return await ExecuteScalarAsync<TResult>(query, column,"sum");
        }
 
        public async Task<TResult> GetMaxAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            var column = this.GetMemberName(selector);
            return await ExecuteScalarAsync<TResult>(query, column, "max");
        }

        public async Task<TResult> GetMinAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            var column = this.GetMemberName(selector);
            return await ExecuteScalarAsync<TResult>(query, column, "min");
        }

        public async Task<decimal> GetSumAsync(IQueryable<T> query, Expression<Func<T, decimal>> selector)
        {
            var column = this.GetMemberName(selector);
            return await ExecuteScalarAsync<decimal>(query, column, "sum");
        }

        public async Task<TResult> GetAverageAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            var column = this.GetMemberName(selector);
            return await ExecuteScalarAsync<TResult>(query, column, "avg");
        }

        public async Task<TResult> GetAverageAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult?>> selector) where TResult : struct
        {
            var column = this.GetMemberName(selector);
            return await ExecuteScalarAsync<TResult>(query, column, "avg");
        }

        public async Task<bool> ExistAsync(IQueryable<T> query)
        {
            return (await this.GetCountAsync(query)) > 0;
        }

        public async Task<TResult> GetScalarAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)
        {
            var column = this.GetMemberName(selector);
            return await ExecuteScalarAsync<TResult>(query, column, "scalar");
        }

        public async Task<IEnumerable<TResult>> GetListAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)
        {
            var column = this.GetMemberName(selector);
            var url = string.Format("{0}/list/{4}?{1}&column={2}&table={3}", this.ServiceUrl, query, column,
                                    typeof(T).Name, "")
                            .Replace("$filter=", "filter=");


            var request = (HttpWebRequest)WebRequest.Create(new Uri(url)).SetCredential();
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("X-Requested-With: XMLHttpRequest");
            request.Headers.Add("Accept-Encoding: gzip, deflate");
            request.Headers.Add("Accept-Language: en-my");
            request.Method = "GET";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            var t = await request.GetResponseAsync();
            var stream = t.GetResponseStream();
            var result = await stream.DeserializeJsonAsync<IEnumerable<TResult>>();
            return result;
        }

        public async Task<IEnumerable<Tuple<TResult, TResult2>>> GetList2Async<TResult, TResult2>(IQueryable<T> query, Expression<Func<T, TResult>> selector, Expression<Func<T, TResult2>> selector2)
        {
            var column = this.GetMemberName(selector);
            var column2 = this.GetMemberName(selector2);
            var url = string.Format("{0}/list/{4}?{1}&column={2}&column2={5}&table={3}", this.ServiceUrl, query, column,
                                    typeof(T).Name, "Tuple",column2)
                            .Replace("$filter=", "filter=");


            var request = (HttpWebRequest)WebRequest.Create(new Uri(url)).SetCredential();
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("X-Requested-With: XMLHttpRequest");
            request.Headers.Add("Accept-Encoding: gzip, deflate");
            request.Headers.Add("Accept-Language: en-my");
            request.Method = "GET";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            var t = await request.GetResponseAsync();
            var stream = t.GetResponseStream();
            var result = await stream.DeserializeJsonAsync<IEnumerable<Tuple<TResult, TResult2>>>();
            return result;
        }

        public async Task<LoadOperation<T>> LoadAsync(IQueryable<T> query, int page = 1, int size = 40, bool includeTotalRows = true)
        {
            Console.WriteLine(query);
            var type = typeof(T).Name + "s";
            if (typeof(T) == typeof(Delivery))
                type = "Deliveries";
            if (typeof(T) == typeof(Inventory))
                type = "Inventories";
            if (typeof(T) == typeof(DailySummary))
                type = "DailySummaries";
            var skiptoken = string.Empty;
            if (page > 1)
            {
                skiptoken = string.Format("&$skiptoken={0}", (page - 1) * size);
                var token = m_tokens.SingleOrDefault(t => t.Page == page - 1 && t.Query == query.ToString());
                if (null != token)
                {
                    skiptoken = string.Format("&$skiptoken={0}", token.NextSkipToken);
                }
            }
            var url = string.Format("{2}/Services/StationDataService.svc/{0}/?$inlinecount=allpages&{1}&$select=Data{3}",
                type, query, this.ServiceUrl, skiptoken);
            var uri = new Uri(url, UriKind.Absolute);

            var lo = await DownloadAsync(uri, page, size);
            lo.Query = query;
            lo.PageSize = size;
            lo.CurrentPage = page;

            if (lo.NextSkipToken.HasValue && !m_tokens.Any(t => t.Query == query.ToString() && t.Page == page))
            {
                m_tokens.Add(new PageToken
                    {
                        NextSkipToken = lo.NextSkipToken,
                        Page = page,
                        Query = query.ToString()
                    });
            }

            return lo;

        }


        private async Task<LoadOperation<T>> DownloadAsync(Uri query, int page, int size)
        {
            var request = (HttpWebRequest)WebRequest.Create(query).SetCredential();
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("X-Requested-With: XMLHttpRequest");
            request.Headers.Add("Accept-Encoding: gzip, deflate");
            request.Headers.Add("Accept-Language: en-my");
            request.Method = "GET";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            var t = await request.GetResponseAsync();
            var stream = t.GetResponseStream();
            var result = await stream.DeserializeJsonAsync<WcfDataServiceJson>();

            var list = from d in result.d.results
                       let id = GetId(d.__metadata.uri)
                       let data = XElement.Parse(d.Data).Deserialize<T>()
                       select SetId(data, id);



            int? nextSkipToken = null;
            if (!string.IsNullOrWhiteSpace(result.d.__next))
            {
                const RegexOptions option = RegexOptions.IgnoreCase | RegexOptions.Singleline;
                var matches = Regex.Matches(result.d.__next, "skiptoken=(?<skip>[0-9]{1,7})", option);
                if (matches.Count == 1)
                {
                    nextSkipToken = int.Parse(matches[0].Groups["skip"].Value.Trim());
                }
            }


            var lo = new LoadOperation<T>
            {
                CurrentPage = page,
                PageSize = size,
                TotalRows = result.d.__count,
                NextSkipToken = nextSkipToken
            };
            lo.ItemCollection.ClearAndAddRange(list);
            return lo;

        }

        private Type GetEntityType(Type itemType)
        {
            var attr = itemType.GetCustomAttribute<EntityTypeAttribute>();
            if (null != attr) return attr.Type;
            return itemType;
        }
        private T SetId(T data, int id)
        {
            var entityType = this.GetEntityType(typeof(T));
            var prop = entityType.GetProperty(entityType.Name + "Id");
            prop.SetValue(data, id);
            return data;
        }

        private int GetId(string uri)
        {
            const string pattern = @"s\((?<id>[0-9]{1,7})\)";
            var matches = Regex.Matches(uri, pattern);
            return int.Parse(matches[0].Groups["id"].Value);
        }


        private async Task<TResult> ExecuteScalarAsync<TResult>(IQueryable<T> query, string column, string aggregate) 
        {
            var url = string.Format("{0}/aggregate/{4}?{1}&column={2}&table={3}", this.ServiceUrl, query, column,
                                    typeof (T).Name, aggregate)
                            .Replace("$filter=", "filter=");


            var request = (HttpWebRequest)WebRequest.Create(new Uri(url)).SetCredential();
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("X-Requested-With: XMLHttpRequest");
            request.Headers.Add("Accept-Encoding: gzip, deflate");
            request.Headers.Add("Accept-Language: en-my");
            request.Method = "GET";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            var t = await request.GetResponseAsync();
            var stream = t.GetResponseStream();
            var result = await stream.DeserializeJsonAsync<TResult>();
            return result;
        }

        private string GetMemberName<TResult>(Expression<Func<T, TResult>> selector)
        {
            var me = selector as LambdaExpression;
            if (null == me) return null;
            var body = me.Body as MemberExpression;
            if (body == null)
            {
                //This will handle Nullable<T> properties.
                var ubody = selector.Body as UnaryExpression;

                if (ubody != null)
                    body = ubody.Operand as MemberExpression;
                if (body == null)
                    throw new ArgumentException("Expression is not a MemberExpression", "selector");

            }
            return body.Member.Name;
        }
       

    }

}
