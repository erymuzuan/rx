using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace Bespoke.Sph.Messaging
{
    public class LuceneIndexer
    {
        private Analyzer m_analyzer;
        private FSDirectory m_directory;
        private IndexSearcher m_searcher;
        public string IndexDirectoryPath { set; get; }

        private class AnonymousClassCollector : Collector
        {
            private Scorer m_scorer;
            private int m_docBase;

            // simply print docId and score of every matching document
            public override void Collect(int doc)
            {
                Console.Out.WriteLine("doc=" + doc + m_docBase + " score=" + m_scorer.Score());
            }

            public override void SetNextReader(IndexReader reader, int docBase)
            {
                this.m_docBase = docBase;
            }

            public override bool AcceptsDocsOutOfOrder
            {
                get { return true; }
            }


            public override void SetScorer(Scorer scorer)
            {
                this.m_scorer = scorer;
            }
        }

        public LuceneIndexer(string directory)
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

        public static void DoStreamingSearch(Searcher searcher, Query query)
        {
            Collector streamingHitCollector = new AnonymousClassCollector();
            searcher.Search(query, streamingHitCollector);
        }

        public void AddDocuments(params ISearchable[] list)
        {
            this.Initialized();
            var writer = new IndexWriter(m_directory, m_analyzer, IndexWriter.MaxFieldLength.UNLIMITED);

            foreach (var item in list)
            {
                var doc = new Document();
                doc.Add(new Field("id", item.Id.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NO));
                var title = new Field("title", item.Title, Field.Store.YES, Field.Index.ANALYZED) { Boost = 1.5f };
                doc.Add(title);

                var code = new Field("code", item.Code, Field.Store.YES, Field.Index.NOT_ANALYZED) { Boost = 1.3f };
                doc.Add(code);
                if (!string.IsNullOrWhiteSpace(item.Status))
                {
                    //Console.WriteLine("{0}-{1}", item.Title, item.Status);
                    doc.Add(new Field("status", item.Status, Field.Store.YES, Field.Index.ANALYZED));
                }
                if (!string.IsNullOrWhiteSpace(item.OwnerCode))
                    doc.Add(new Field("ownercode", item.OwnerCode, Field.Store.YES, Field.Index.NOT_ANALYZED));


                var datestring = DateTools.DateToString(item.Created, DateTools.Resolution.DAY);
                doc.Add(new Field("created", datestring, Field.Store.YES, Field.Index.NOT_ANALYZED));


                if (null != item.CustomFields)
                {
                    foreach (var field in item.CustomFields.Keys)
                    {
                        var val = item.CustomFields[field];
                        if (null == val) continue;
                        if (val is DateTime)
                        {
                            var datestring2 = DateTools.DateToString((DateTime)val, DateTools.Resolution.DAY);
                            doc.Add(new Field(field.ToLower(), datestring2, Field.Store.YES, Field.Index.NOT_ANALYZED));
                            continue;
                        }

                        if (val is int)
                        {
                            //var number = new NumericField(field.ToLower(), Field.Store.YES, true);
                            //doc.Add(number);
                            //number.SetIntValue((int)val);
                            doc.Add(new Field(field.ToLower(), string.Format("{0:d4}", val), Field.Store.YES, Field.Index.NOT_ANALYZED));

                            continue;
                        }
                        if (val is decimal || val is double || val is float)
                        {
                            //var number = new NumericField(field.ToLower(), Field.Store.YES, true);
                            //number.SetDoubleValue(Convert.ToDouble(val));
                            //doc.Add(number);
                            doc.Add(new Field(field.ToLower(), string.Format("{0:0000.00}", val), Field.Store.YES, Field.Index.NOT_ANALYZED));

                            continue;
                        }
                        var s = val as string;
                        if (s != null)
                        {
                            doc.Add(new Field(field.ToLower(), s, Field.Store.YES, Field.Index.ANALYZED));
                        }
                    }
                }

                doc.Add(new Field("type", item.Type, Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("uniqueid", item.Type + item.Id, Field.Store.YES, Field.Index.NOT_ANALYZED));

                doc.Add(new Field("summary", item.Summary, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("text", item.Text, Field.Store.YES, Field.Index.ANALYZED));
                writer.AddDocument(doc);
            }
            writer.Commit();
            writer.Dispose();
        }

        public void RemoveDocuments(params ISearchable[] list)
        {
            var writer = new IndexWriter(m_directory, m_analyzer, IndexWriter.MaxFieldLength.UNLIMITED);

            foreach (var item in list)
            {
                var uniqueid = new Term("uniqueid", item.Type + item.Id);
                Console.WriteLine("Deleting {0} \tid : {1}", item.Type, item.Id);
                writer.DeleteDocuments(uniqueid);

            }
            writer.Commit();
            writer.Dispose();
        }


        public void Dispose()
        {
            this.Close();
        }
    }

    public interface ISearchable
    {
        int Id { get; set; }
        string Type { get; set; }
        string Summary { get; set; }
        string Text { get; set; }
        DateTime Created { get; set; }
        string Title { get; set; }
        Dictionary<string, object> CustomFields { get; set; }
        string Code { get; set; }
        string Status { get; set; }
        string OwnerCode { get; set; }
    }
}
