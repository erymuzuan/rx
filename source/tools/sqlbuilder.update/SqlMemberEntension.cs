using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace sqlbuilder.update
{
    internal static class SqlMemberEntension
    {
        public static void UpdateSourceFilesNamespace(this EntityDefinition ed)
        {
            var folder = $"{ConfigurationManager.SphSourceDirectory}\\{ed.Name}";
            var sources = Directory.GetFiles(folder, "*.json");
            Console.WriteLine($@"Converting {sources.Length} source files");
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




        public static IList<AttachedProperty> UpdateMember(this JObject member)
        {
            var list = new List<AttachedProperty>();
            var typeProp = member.Property("$type");
            var typeNameProp = member.Property("TypeName");

            var type = typeProp.Value.Value<string>();
            if (typeNameProp?.Value == null) return list;

            if (type == typeof(SimpleMember).GetShortAssemblyQualifiedName())
            {
                var filterable = member["IsFilterable"].Value<bool>();
                var boost = member["Boost"].Value<bool>();
                var notIndexed = member["IsNotIndexed"].Value<bool>();
                var analyzed = member["IsAnalyzed"].Value<bool>();
                var excludeInAll = member["IsExcludeInAll"].Value<bool>();
                var webId = member["WebId"].Value<string>();
                if (string.IsNullOrWhiteSpace(webId))
                {
                    webId = Guid.NewGuid().ToString();
                    member["WebId"] = webId;
                }
                

                list.Add(CreateFilterableAttachedProperty(filterable, webId));
                list.Add(CreateBoostAttachedProperty(boost, webId));
                list.Add(CreateNotIndexAttachedProperty(notIndexed, webId));
                list.Add(CreateAnalyzedAttachedProperty(analyzed, webId));
                list.Add(CreateExcludeInAllAttachedProperty(excludeInAll, webId));

                if ($"{typeNameProp.Value}" == "System.String, mscorlib")
                {
                    list.Add(new AttachedProperty
                    {
                        Name = "Length",
                        Value = 255,
                        AttachedTo = webId,
                        ProviderName = "SqlServer2016",
                        Type = typeof(int)
                    });
                    list.Add(new AttachedProperty
                    {
                        Name = "AllowUnicode",
                        Value = true,
                        AttachedTo = webId,
                        ProviderName = "SqlServer2016",
                        Type = typeof(bool)
                    });
                }


                member.Remove("IsFilterable");
                member.Remove("Boost");
                member.Remove("IsNotIndexed");
                member.Remove("IsAnalyzed");
                member.Remove("IsExcludeInAll");
            }
            var childMembers = member.SelectToken("MemberCollection.$values");
            foreach (var c1 in childMembers)
            {
                list.AddRange(UpdateMember((JObject)c1));
            }
            return list;
        }

        private static AttachedProperty CreateExcludeInAllAttachedProperty(bool excludeInAll, string webId)
        {
            return new AttachedProperty
            {
                AttachedTo = webId,
                Name = "IsExcludeInAll",
                Type = typeof(bool),
                ProviderName = "Elasticsearch.1.7.5",
                WebId = Guid.NewGuid().ToString(),
                Value = excludeInAll
            };
        }

        private static AttachedProperty CreateAnalyzedAttachedProperty(bool analyzed, string webId)
        {
            return new AttachedProperty
            {
                AttachedTo = webId,
                Name = "IsAnalyzed",
                Type = typeof(bool),
                ProviderName = "Elasticsearch.1.7.5",
                WebId = Guid.NewGuid().ToString(),
                Value = analyzed
            };
        }

        private static AttachedProperty CreateNotIndexAttachedProperty(bool notIndexed, string webId)
        {
            return new AttachedProperty
            {
                AttachedTo = webId,
                Name = "IsNotIndexed",
                Type = typeof(bool),
                ProviderName = "Elasticsearch.1.7.5",
                WebId = Guid.NewGuid().ToString(),
                Value = notIndexed
            };
        }

        private static AttachedProperty CreateBoostAttachedProperty(bool boost, string webId)
        {
            return new AttachedProperty
            {
                AttachedTo = webId,
                Name = "Boost",
                Type = typeof(bool),
                ProviderName = "Elasticsearch.1.7.5",
                WebId = Guid.NewGuid().ToString(),
                Value = boost
            };
        }

        private static AttachedProperty CreateFilterableAttachedProperty(bool filterable, string webId)
        {
            return new AttachedProperty
            {
                AttachedTo = webId,
                Name = "SqlIndex",
                Type = typeof(bool),
                ProviderName = "SqlServer2016",
                WebId = Guid.NewGuid().ToString(),
                Value = filterable
            };
        }
    }
}