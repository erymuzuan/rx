using System;
using System.Collections.Generic;
using FileHelpers;
using Newtonsoft.Json;

namespace domain.test.receive.ports
{
    [DelimitedRecord(",")]
    public class CsvSalesOrder
    {
        [FieldHidden]
        private readonly IList<CsvOrderItem> m_item = new List<CsvOrderItem>();
        public IList<CsvOrderItem> Items => m_item;
        [JsonIgnore]
        public string RowTagRaw;
        [JsonIgnore]
        public string NameRaw;
        [JsonIgnore]
        public string OrderNoRaw;
        [JsonIgnore]
        public string AccountNoRaw;
        [JsonIgnore]
        [FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
        public DateTime DateRaw;

        public string RowTag => RowTagRaw.Replace("<comma>", ",");
        public string Name => NameRaw.Replace("<comma>", ",");
        public string OrderNo => OrderNoRaw.Replace("<comma>", ",");
        public string AccountNo => AccountNoRaw.Replace("<comma>", ",");
        public DateTime Date => DateRaw;
    }
}