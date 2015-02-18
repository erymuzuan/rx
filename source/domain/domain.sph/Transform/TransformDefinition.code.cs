﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class TransformDefinition
    {


        public override string DefaultNamespace
        {
            get { return string.Format("{0}.Integrations.Transforms", ConfigurationManager.ApplicationName); }
        }

        public override Task<IEnumerable<Class>> GenerateCodeAsync()
        {
            var @classes = new List<Class>();


            var map = new Class { Name = this.Name };
            map.AddNamespaceImport<Int32>();
            map.AddNamespaceImport<Task>();
            map.AddNamespaceImport<EnumerableQuery>();
            @classes.Add(map);

            map.MethodCollection.Add(new Method { Name = "TransformAsync", Code = this.GenerateTransformCode() });
            map.MethodCollection.Add(new Method { Name = "TransformToArrayAsync", Code = this.GenerateTransformToArrayCode() });
            return Task.FromResult(@classes.AsEnumerable());
        }


        public string[] SaveSources(Dictionary<string, string> sources)
        {
            var folder = Path.Combine(ConfigurationManager.UserSourceDirectory, this.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            foreach (var cs in sources.Keys)
            {
                var file = Path.Combine(folder, cs);
                File.WriteAllText(file, sources[cs]);
            }
            return sources.Keys.ToArray()
                    .Select(f => string.Format("{0}\\{1}\\{2}", ConfigurationManager.UserSourceDirectory, this.Name, f))
                    .ToArray();
        }


        public string GenerateTransformCode()
        {
            const string GAP = "               ";
            var code = new StringBuilder();

            var args = "";
            if (!string.IsNullOrWhiteSpace(this.InputTypeName))
                args = string.Format("{0} item", this.InputType.FullName);
            if (this.InputCollection.Count > 0)
            {
                code.AppendLine(" class Input");
                code.AppendLine("{");
                foreach (var input in this.InputCollection)
                {
                    var type = Type.GetType(input.TypeName);
                    if (null == type) continue;
                    code.AppendLinf(" public {0} {1} {{ get; set; }}", type.FullName, input.Name);
                }
                code.AppendLine("   ");
                code.AppendLine("}");
                var list = from p in this.InputCollection
                           let type = Type.GetType(p.TypeName)
                           where null != type
                           let name = p.Name.ToCamelCase()
                           select string.Format("{0} {1}", type.FullName, name);
                args = string.Join(", ", list);
            }

            this.FunctoidCollection.Select((x, i) => x.Index = i).ToList().ForEach(Console.WriteLine);

            this.FunctoidCollection.ForEach(x => x.TransformDefinition = this);
            this.MapCollection.ForEach(x => x.TransformDefinition = this);

            code.AppendLinf("           public async Task<{0}> TransformAsync({1})", this.OutputType.FullName, args);
            code.AppendLine("           {");
            if (this.InputCollection.Count > 0)
            {
                code.AppendLinf(" var item = new {0}.Input();", this.Name);
                foreach (var input in this.InputCollection)
                {
                    var type = Type.GetType(input.TypeName);
                    if (null == type) continue;
                    code.AppendLinf("item.{0} =  {1};", input.Name, input.Name.ToCamelCase());
                }
            }

            code.AppendLinf("               var dest =  new {0}();", this.OutputType.FullName);

            // functoids statement
            var sorted = new List<Functoid>(this.FunctoidCollection);
            sorted.Sort(new FunctoidDependencyComparer());
            var functoidStatements = from f in sorted
                                     let statement = f.GenerateStatementCode()
                                     where !string.IsNullOrWhiteSpace(statement)
                                     && (!statement.Contains("Collection.") || (f.GetType() == typeof(LoopingFunctoid)))
                                     select string.Format("\r\n{4}//{0}:{1}:{2}\r\n{4}{3}", f.Name, f.GetType().Name, f.WebId, statement, GAP);
            code.AppendLine(string.Concat(functoidStatements.ToArray()));
            code.AppendLine();

            var mappingCodes = from m in this.MapCollection
                               select GAP + m.GenerateCode() + "\r\n";
            code.AppendLine(string.Concat(mappingCodes.ToArray()));
            code.AppendLine();


            if (code.ToString().Contains("await "))
                code.AppendLinf("               return dest;");
            else
            {
                code.Replace("public async Task<", "public Task<");
                code.AppendLinf("               return Task.FromResult(dest);");
            }

            code.AppendLine("           }");

            if (!string.IsNullOrWhiteSpace(this.InputTypeName))
                code.Replace("{SOURCE_TYPE}", this.InputType.FullName);
            else
                code.Replace("{SOURCE_TYPE}", this.Name +".Input");

            code.Replace("{DEST_TYPE}", this.OutputType.FullName);
            return code.ToString();
        }

        public string GenerateTransformToArrayCode()
        {
            return ("//TODO : return the list of destinations objects");
        }

    }
}