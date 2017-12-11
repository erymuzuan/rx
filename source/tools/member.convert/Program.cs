using System;
using System.IO;
using System.Linq;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Tools.Upgrades
{
    internal class Program
    {
        public static void Main(string[] args)
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
            foreach (var m in members)
            {
                var m1 = (JObject)m;
                m1.UpdateMember();

            }
            var ed = jo.ToString().DeserializeFromJson<EntityDefinition>();
            ed.Plural = ed.Plural.Replace(" ", "");
            if (ed.Plural == ed.Name)
                ed.Plural += "s";

            var backup = $"{file}-{DateTime.Now:yyyyMMdd-HHmmss}.backup";
            File.Copy(file, backup, true);
            File.WriteAllText(file, jo.ToString());

            if (ed.TreatDataAsSource)
            {
                try
                {
                    ed.UpdateSourceFilesNamespace();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            Console.WriteLine($@"Your EntityDefinition {ed.Name} was successfully converted and a backup file is created {Path.GetFileName(backup)}");

        }

    }
}
