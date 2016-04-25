using System;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [JsonConverter(typeof(RawJsonSerializer))]
    public class WorkflowPresentation : DomainObject
    {
        private readonly string m_json;
        public string Id { get; set; }
        public string Version { get; set; }
        public string WorkflowDefinitionId { get; set; }
        public int WorkflowDefinitionVersion { get; set; }

        public WorkflowPresentation()
        {
            
        }
        public WorkflowPresentation(string id, string version, string json)
        {
            m_json = json;
            this.Id = id;
            Version = version;
        }
        public override string ToString()
        {
            var json = m_json.Remove(m_json.LastIndexOf("}", StringComparison.Ordinal))
                       + $@",
                ""__link"" : {{
                    ""rel"" : ""self"",
                    ""href"" : ""{ConfigurationManager.BaseUrl}/wf/{WorkflowDefinitionId}/v{WorkflowDefinitionVersion}/{Id}""
                }}
            }}";
            return json;
        }

    }
}