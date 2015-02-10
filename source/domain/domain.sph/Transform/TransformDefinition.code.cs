using System;
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

            this.FunctoidCollection.Select((x, i) => x.Index = i).ToList().ForEach(Console.WriteLine);

            this.FunctoidCollection.ForEach(x => x.TransformDefinition = this);
            this.MapCollection.ForEach(x => x.TransformDefinition = this);

            const string GAP = "               ";
            var code = new StringBuilder();
            code.AppendLinf("           public async Task<{0}> TransformAsync({1} item)", this.OutputType.FullName, this.InputType.FullName);
            code.AppendLine("           {");
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

            code.Replace("{SOURCE_TYPE}", this.InputType.FullName);
            code.Replace("{DEST_TYPE}", this.OutputType.FullName);
            return code.ToString();
        }

        public string GenerateTransformToArrayCode()
        {
            return ("//TODO : return the list of destinations objects");
        }

    }
}