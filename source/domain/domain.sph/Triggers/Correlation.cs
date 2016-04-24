using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public class Correlation
    {
        [JsonProperty("wid")]
        public string WorfklowId { get; set; }
        [JsonProperty("wdid")]
        public string WorkflowDefinitionId { get; set; }
        [JsonProperty("tracker")]
        public Tracker Tracker { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}