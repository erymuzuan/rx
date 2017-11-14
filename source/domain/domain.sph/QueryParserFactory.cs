using System;
using System.ComponentModel.Composition;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public class QueryParserFactory
    {
        private static QueryParserFactory m_instance;
        private static readonly object m_lock = new object();

        private QueryParserFactory()
        {
        }

        public static QueryParserFactory Instance
        {
            get
            {
                lock (m_lock)
                {
                    if (null == m_instance)
                    {
                        m_instance = new QueryParserFactory();
                        ObjectBuilder.ComposeMefCatalog(m_instance);
                    }
                    if (null == m_instance.QueryParserProviders || m_instance.QueryParserProviders.Length == 0)
                        ObjectBuilder.ComposeMefCatalog(m_instance);
                    if (null == m_instance.QueryParserProviders || m_instance.QueryParserProviders.Length == 0)
                        throw new Exception("Fail to load any QueryParserProviders");
                    return m_instance;
                }
            }
        }
        // ReSharper disable once MemberCanBePrivate.Global

        [ImportMany("QueryParser", typeof(IQueryParser), AllowRecomposition = true)]
        public IQueryParser[] QueryParserProviders { get; set; }

        public IQueryParser Get(string provider, string contentType = "")
        {
            if (string.IsNullOrEmpty(contentType))
                return this.QueryParserProviders.SingleOrDefault(x => x.Provider == provider);
            if (string.IsNullOrEmpty(provider))
                return this.QueryParserProviders.SingleOrDefault(x => x.ContentType.StartsWith(contentType));
            return this.QueryParserProviders.SingleOrDefault(x => x.Provider == provider && x.ContentType == contentType);
        }
    }
}