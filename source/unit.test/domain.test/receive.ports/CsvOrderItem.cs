using FileHelpers;
using Newtonsoft.Json;

namespace domain.test.receive.ports
{
    [DelimitedRecord(",")]
    public class CsvOrderItem
    {
        [JsonIgnore]
        public string RowTagRaw;
        [JsonIgnore]
        public string ItemRaw;
        [JsonIgnore]
        public string QuatityRaw;
        [JsonIgnore]
        public string AmountRaw;

        public string RowTag => RowTagRaw.Replace("<comma>", ",");
        public string Item => ItemRaw.Replace("<comma>", ",");
        public int Quantity => int.Parse(QuatityRaw.Replace("<comma>", ","));
        public decimal Amount => decimal.Parse(AmountRaw.Replace("<comma>", ","));
    }
}