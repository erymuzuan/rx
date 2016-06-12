using System;
using Humanizer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace scheduler.data.import
{
    public class ProgressData
    {
        public Exception Exception { get; set; }
        public object Data { get; set; }
        public string ErrorId { get; set; }
        public int Rows { get; set; }
        public int ElasticsearchRows { get; set; }
        public int SqlRows { get; set; }
        [JsonIgnore]
        public TimeSpan Elapsed { set; get; }
        public string TotalTime => this.Elapsed.Humanize();
        public Queue ElasticsearchQueue { get; set; } = new Queue("es.data-import", 0, 0);
        public Queue SqlServerQueue { get; set; } = new Queue("persistence", 0, 0);

        public ProgressData()
        {

        }
        public ProgressData(int rows)
        {
            Rows = rows;
        }
        public ProgressData(Exception exception, object data, string errorId)
        {
            Exception = exception;
            Data = data;
            ErrorId = errorId;
        }

        public static ProgressData Parse(string es, string sql)
        {
            var pd = new ProgressData(-1)
            {
                SqlServerQueue = ParseQueue(sql),
                ElasticsearchQueue = ParseQueue(es)
            };


            return pd;
        }
        private static Queue ParseQueue(string json)
        {
            var jo = JObject.Parse(json);
            var name = jo.SelectToken("$.name").Value<string>();
            var messages = jo.SelectToken("$.messages").Value<int>();
            var deliveries = jo.SelectToken("$.message_stats.deliver_details.rate").Value<double>();
            var unacked = jo.SelectToken("$.messages_unacknowledged").Value<int>();

            return new Queue(name, messages, deliveries, unacked);

        }

        public void Merge(ProgressData p)
        {
            if (p.Rows > 0)
                this.Rows = p.Rows;
            if (p.SqlServerQueue.MessagesCount > 0)
                this.SqlServerQueue.MessagesCount = p.SqlServerQueue.MessagesCount;
            if (p.SqlServerQueue.Rate > 0d)
                this.SqlServerQueue.Rate = p.SqlServerQueue.Rate;
            if (p.SqlRows > 0)
                this.SqlRows = p.SqlRows;
            if (p.ElasticsearchQueue.MessagesCount > 0)
                this.SqlServerQueue.MessagesCount = p.ElasticsearchQueue.MessagesCount;
            if (p.ElasticsearchQueue.Rate > 0)
                this.ElasticsearchQueue.Rate = p.ElasticsearchQueue.Rate;
            if (p.ElasticsearchRows > 0)
                this.ElasticsearchRows = p.ElasticsearchRows;
        }
    }
}