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

        [ImportMany("QueryParser", typeof(IQueryParser), AllowRecomposition = true)]
        // ReSharper disable once MemberCanBePrivate.Global
        public IQueryParser[] QueryParserProviders { get; set; }

        public IQueryParser Get(string name, string version = "")
        {
            if (string.IsNullOrEmpty(version))
                return this.QueryParserProviders.SingleOrDefault(x => x.Provider == name);
            return this.QueryParserProviders.SingleOrDefault(x => x.Provider == name && x.Version == version);
        }
    }
}