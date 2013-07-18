using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.SphCommercialSpaces.Domain;
using Lucene.Net.Analysis;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;
using Document = Lucene.Net.Documents.Document;

namespace Bespoke.Sph.Searching
{
    public class LuceneSearchProvider : ISearchProvider
    { 
        private Analyzer m_analyzer;
        private FSDirectory m_directory;
        private IndexSearcher m_searcher;
        public string IndexDirectoryPath { set; get; }

        
        public LuceneSearchProvider(string directory)
        {
            this.IndexDirectoryPath = directory;
            Initialized();
        }

        private void Initialized()
        {
            m_directory = FSDirectory.Open(new DirectoryInfo(IndexDirectoryPath));
            m_analyzer = new SimpleAnalyzer(); //StandardAnalyzer(Version.LUCENE_29);
            if (System.IO.Directory.Exists(this.IndexDirectoryPath) && null == m_searcher)
                m_searcher = new IndexSearcher(m_directory, true);
        }

        public Task<IEnumerable<SearchResult>> SearchAsync(string text)
        {
            Console.WriteLine("Searching....");
            Console.WriteLine(text);
            this.Initialized();
            var parser = new QueryParser(Version.LUCENE_29, "text", m_analyzer);
            Query query = parser.Parse(text);
            var docs = m_searcher.Search(query, 100);
            var hits = docs.ScoreDocs;

            int results = docs.TotalHits;
            Console.WriteLine("Found {0} results for {1}", results, query);
            var list = new ObjectCollection<SearchResult>();
            for (int i = 0; i < results; i++)
            {
                if (i >= hits.Length) continue;
                //Document doc = m_searcher.Doc(hits[i].Doc);
                Document doc = m_searcher.Doc(hits[i].Doc);
                float score = hits[i].Score;
                var id = int.Parse(doc.Get("id"));
                var type = doc.Get("type");
                var owner2 = doc.Get("ownercode");
                const string owner = "";
                if (!string.IsNullOrWhiteSpace(owner) && owner2 != owner) continue;
                if (list.Any(d => d.Type == type && d.Id == id)) continue; // remove duplicate
                var sc = new SearchResult
                {
                    Score = score,
                    Title = doc.Get("title"),
                    Id = id,
                    Status = doc.Get("status"),
                    OwnerCode = doc.Get("ownercode"),
                    Code = doc.Get("code"),
                    Summary = doc.Get("summary"),
                    Type = type
                };

                DateTime date;
                if (DateTime.TryParse(doc.Get("created"), out date))
                {
                    sc.CreatedDate = date;
                }
                list.Add(sc);
            }

            return Task.FromResult(list.AsEnumerable());
        }

        public void Close()
        {
            if (null != m_searcher)
            {
                m_searcher.Dispose();
            }
            if (null != m_directory)
            {
                m_directory.Dispose();
            }
            if (null != m_analyzer)
            {
                m_analyzer.Close();
                
            }

            m_searcher = null;
            m_directory = null;
            m_analyzer = null;


        }

        //public static void DoStreamingSearch(Searcher searcher, Query query)
        //{
        //    Collector streamingHitCollector = new AnonymousClassCollector();
        //    searcher.Search(query, streamingHitCollector);
        //}


    

        public void Dispose()
        {
            this.Close();
        }
    }
}
