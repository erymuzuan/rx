using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class TransformDefinition
    {
        private string GetCodeHeader()
        {
            var header = new StringBuilder();
            header.AppendLine("using " + typeof(Entity).Namespace + ";");
            header.AppendLine("using " + typeof(Int32).Namespace + ";");
            header.AppendLine("using " + typeof(Task<>).Namespace + ";");
            header.AppendLine("using " + typeof(IEnumerable<>).Namespace + ";");
            header.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            header.AppendLine();

            header.AppendLine("namespace " + this.CodeNamespace);
            header.AppendLine("{");
            return header.ToString();

        }

        public Dictionary<string, string> GenerateCode()
        {
            var header = this.GetCodeHeader();
            var code = new StringBuilder(header);

            code.AppendLine("   public class " + this.Name);
            code.AppendLine("   {");


            code.AppendLine(this.GenerateTransformCode());
            code.AppendLine(this.GenerateTransformToArrayCode());


            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace

            var sourceCodes = new Dictionary<string, string> { { this.Name + ".cs", code.FormatCode() } };


            return sourceCodes;
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
                    .Select(f => $"{ConfigurationManager.UserSourceDirectory}\\{this.Name}\\{f}")
                    .ToArray();
        }
        public string CodeNamespace => $"{ConfigurationManager.ApplicationName}.Integrations.Transforms";

        public string GenerateTransformCode()
        {
            const string GAP = "               ";
            var code = new StringBuilder();

            var args = "";
            if (!string.IsNullOrWhiteSpace(this.InputTypeName))
                args = $"{this.InputType.FullName} item";
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
                           select $"{type.FullName} {name}";
                args = string.Join(", ", list);
            }

            this.FunctoidCollection.Select((x, i) => x.Index = i).ToList().ForEach(x => { });

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
                code.Replace("{SOURCE_TYPE}", this.Name + ".Input");

            code.Replace("{DEST_TYPE}", this.OutputType.FullName);
            return code.ToString();
        }

        public string GenerateTransformToArrayCode()
        {
            return ("//TODO : return the list of destinations objects");
        }

    }
}