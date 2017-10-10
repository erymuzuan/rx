using System;
using System.IO;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Mangements
{
    public class MigrationPlan
    {
        public string PreviousCommitId { get; set; }
        public string CurrentCommitId { get; set; }
        public DateTime? DeployedDateTime { get; set; }

        public ObjectCollection<MemberChange> ChangeCollection { get; } = new ObjectCollection<MemberChange>();

        public static MigrationPlan ParseFile(string migrationPlan)
        {

            var jsonSource = File.ReadAllText(migrationPlan);
            var setting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            return JsonConvert.DeserializeObject<MigrationPlan>(jsonSource, setting);

        }
    }
}