using System;
using System.IO;
using System.Threading;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Tools.Upgrades
{
    internal static class NamespaceEntension
    {
        public static void UpdateSourceFilesNamespace(this EntityDefinition ed)
        {
            var folder = $"{ConfigurationManager.SphSourceDirectory}\\{ed.Name}";
            var sources = Directory.GetFiles(folder, "*.json");
            Console.WriteLine($@"Converterting {sources.Length} source files");
            Console.WriteLine(new string('=', 50));
            Console.WriteLine();
            var count = 0;
            foreach (var file in sources)
            {
                var json = JObject.Parse(File.ReadAllText(file));
                json["$type"] = ed.FullTypeName;

                var backup = file + ".bak";
                var number = 0;
                while (File.Exists(backup))
                {
                    number++;
                    backup = $"{file}.{number:00}.bak";
                }
                File.Copy(file, backup, false);
                File.WriteAllText(file, json.ToString());
                Thread.Sleep(50);
                Console.Write($@"\r\t\t\t{++count:000}/{sources.Length:000}\t{json["Id"]}.....                                                  ");
            }
            Console.WriteLine();
            Console.WriteLine($@"Converted {sources.Length} source files..");
        }

        public static void RemoveObsoleteMembersFromComplexObject(this JObject member)
        {
            member.Remove("IsNullable");
            member.Remove("IsNotIndexed");
            member.Remove("IsAnalyzed");
            member.Remove("IsFilterable");
            member.Remove("IsExcludeInAll");
            member.Remove("DefaultValue");
            member.Remove("Boost");

        }

        public static void UpdateMember(this JObject member)
        {
            var typeProp = member.Property("$type");
            var typeNameProp = member.Property("TypeName");

            var type = typeProp.Value.Value<string>();
            if (typeNameProp?.Value == null) return;
            var typeName = typeNameProp.Value.Value<string>();

            if (type == "Bespoke.Sph.Domain.Member, domain.sph")
            {
                member["$type"] = "Bespoke.Sph.Domain.SimpleMember, domain.sph";
                member.Remove("FullName");
                if (typeName == "System.Object, mscorlib")
                {
                    member["$type"] = "Bespoke.Sph.Domain.ComplexMember, domain.sph";
                    member["TypeName"] = member["Name"].Value<string>();
                    if (member.Property("AllowMultiple") == null)
                        member.Add(new JProperty("AllowMultiple", false));
                    RemoveObsoleteMembersFromComplexObject(member);


                }
                if (typeName == "System.Array, mscorlib")
                {
                    member["$type"] = "Bespoke.Sph.Domain.ComplexMember, domain.sph";
                    member["TypeName"] = member["Name"].Value<string>().Replace("Collection", "");
                    if (member.Property("AllowMultiple") == null)
                        member.Add(new JProperty("AllowMultiple", true));
                    RemoveObsoleteMembersFromComplexObject(member);


                }
            }
            var childMembers = member.SelectToken("MemberCollection.$values");
            foreach (var c1 in childMembers)
            {
                UpdateMember((JObject)c1);
            }
        }
    }
}
