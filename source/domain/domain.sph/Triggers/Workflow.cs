using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    public partial class Workflow : Entity
    {
        [XmlAttribute]
        public string CurrentActivityWebId { get; set; }
        [XmlAttribute]
        public string SerializedDefinitionStoreId { get; set; }

        public virtual Task<ActivityExecutionResult> StartAsync()
        {
            return Task.FromResult(new ActivityExecutionResult { Status = ActivityExecutionStatus.None });
        }

        public virtual Task<ActivityExecutionResult> ExecuteAsync()
        {
            return Task.FromResult(new ActivityExecutionResult { Status = ActivityExecutionStatus.None });
        }

        public Activity GetCurrentActivity()
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var s = store.GetContent(this.SerializedDefinitionStoreId);
            using (var stream = new MemoryStream(s.Content))
            {
                var wd = stream.DeserializeFromXml<WorkflowDefinition>();
                if (string.IsNullOrWhiteSpace(this.CurrentActivityWebId))
                    return wd.ActivityCollection.Single(d => d.IsInitiator);
            }

            return null;
        }

        public Activity GetNexActivity()
        {
            return null;
        }

        public async virtual Task SaveAsync(string activityId)
        {
            await Task.Delay(500);
            Console.WriteLine("saving....." + activityId);
        }

    }
}