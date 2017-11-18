using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.RxPs.Domain
{
    public class ReceiveLocation : Entity
    {
        public bool IsPublished { get; set; }
        public DateTime? Compiled { get; set; }
        public DateTime? Deployed { get; set; }

        public static EntityDefinition Parse(JObject x)
        {
            var ed = new EntityDefinition
            {
                Id = x.SelectToken("$.Id").Value<string>(),
                Name = x.SelectToken("$.Name").Value<string>(),
                IsPublished = x.SelectToken("$.IsPublished").Value<bool>(),
                CreatedDate = x.SelectToken("$.CreatedDate").Value<DateTime>(),
                ChangedDate = x.SelectToken("$.ChangedDate").Value<DateTime>(),

            };

            var dll = new FileInfo(
                $@"{ConfigurationManager.CompilerOutputPath}\{ConfigurationManager.RxApplicationName}.{ed.Name}.dll");
            if (dll.Exists)
                ed.Compiled = dll.LastWriteTime;

            return ed;
        }
    }
}