using System;
using System.IO;
using System.Linq;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace sqlbuilder.update
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var file = args.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(file))
            {
                Console.WriteLine(@"We need a file to convert");
                return;
            }
            var originalJson = File.ReadAllText(file);
            var jo = JObject.Parse(originalJson);
            var members = jo.SelectToken("$.MemberCollection.$values");
            var properties = members.Select(@t => ((JObject)t).UpdateMember()).SelectMany(x =>x).ToList();
            var ed = jo.ToString().DeserializeFromJson<EntityDefinition>();

            var settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All};
            File.WriteAllText(file.Replace(".json", ".AttachedProperties"), JsonConvert.SerializeObject(properties, Formatting.Indented, settings));
            
            var backup = $"{file}-{DateTime.Now:yyyyMMdd-HHmmss}.backup";
            File.Copy(file, backup, true);
            File.WriteAllText(file, jo.ToString());
            
            Console.WriteLine($@"Your EntityDefinition {ed.Name} was successfully converted and a backup file is created {Path.GetFileName(backup)}");

        }
    }
}
