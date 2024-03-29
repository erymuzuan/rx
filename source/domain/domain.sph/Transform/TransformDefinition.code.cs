﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.Domain.Compilers;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class TransformDefinition
    {
        [JsonIgnore]
        public string ClassName => this.Name.ToPascalCase();
        [JsonIgnore]
        public string FullTypeName => $"{CodeNamespace}.{ClassName}";
        [JsonIgnore]
        public string AssemblyName => $"{ConfigurationManager.ApplicationName}.{Name}";
        [JsonIgnore]
        public string CodeNamespace => $"{ConfigurationManager.CompanyName}.{ConfigurationManager.ApplicationName}.Integrations.Transforms";

        private string GetCodeHeader()
        {
            var header = new StringBuilder();
            header.AppendLine("using " + typeof(Entity).Namespace + ";");
            header.AppendLine("using " + typeof(int).Namespace + ";");
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

            code.AppendLine("   public partial class " + this.ClassName);
            code.AppendLine("   {");


            code.AppendLine(this.GenerateTransformCode());
            code.AppendLine(this.GenerateTransformToArrayCode());

            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace

            var sourceCodes = new Dictionary<string, string> { { this.ClassName + ".cs", code.FormatCode() } };


            return sourceCodes;
        }
        public bool GeneratePartialCode(out string file)
        {
            file = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(TransformDefinition)}\\{this.Id}.cs";
            var partial = new Class
            {
                IsPartial = true,
                Name = this.ClassName,
                FileName = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(TransformDefinition)}\\{this.Id}.cs",
                Namespace = this.CodeNamespace
            };
            partial.MethodCollection.Add(new Method
            {
                Name = "BeforeTransform",
                IsPartial = true,
                ReturnType = typeof(void),
                Body = "",
                AccessModifier = Modifier.Private

            });
            partial.MethodCollection[0].ArgumentCollection.Add(new MethodArg
            {
                Name = "item",
                TypeName = this.InputTypeName,
            });

            partial.MethodCollection[0].ArgumentCollection.Add(new MethodArg
            {
                Name = "destination",
                TypeName = this.OutputTypeName,
            });


            partial.MethodCollection.Add(new Method
            {
                Name = "AfterTransform",
                IsPartial = true,
                ReturnType = typeof(void),
                Body = "",
                AccessModifier = Modifier.Private


            });
            partial.MethodCollection[1].ArgumentCollection.Add(new MethodArg
            {
                Name = "item",
                TypeName = this.InputTypeName,
            });
            partial.MethodCollection[1].ArgumentCollection.Add(new MethodArg
            {
                Name = "destination",
                TypeName = this.OutputTypeName,
            });

            if (!File.Exists(file))
            {
                File.WriteAllText(file, partial.GetCode());
                return true;
            }


            return false;
        }


        public string[] SaveSources(Dictionary<string, string> sources)
        {
            var folder = $"{ConfigurationManager.GeneratedSourceDirectory}\\{nameof(TransformDefinition)}.{Name}";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            foreach (var cs in sources.Keys)
            {
                var file = Path.Combine(folder, cs);
                File.WriteAllText(file, sources[cs]);
            }
            return sources.Keys.ToArray()
                    .Select(f => $"{folder}\\{f}")
                    .ToArray();
        }

        public string GenerateTransformCode()
        {
            const string GAP = "               ";
            var methodDeclaration = new StringBuilder();

            var args = "";
            if (!string.IsNullOrWhiteSpace(this.InputTypeName))
                args = $"{this.InputType.FullName} item";
            if (this.InputCollection.Count > 0)
            {
                methodDeclaration.AppendLine(" class Input");
                methodDeclaration.AppendLine("{");
                foreach (var input in this.InputCollection)
                {
                    var type = Strings.GetType(input.TypeName);
                    if (null == type) continue;
                    methodDeclaration.AppendLinf(" public {0} {1} {{ get; set; }}", type.FullName, input.Name);
                }
                methodDeclaration.AppendLine("   ");
                methodDeclaration.AppendLine("}");
                var list = from p in this.InputCollection
                           let type = Strings.GetType(p.TypeName)
                           where null != type
                           let name = p.Name.ToCamelCase()
                           select $"{type.FullName} {name}";
                args = string.Join(", ", list);
            }

            this.FunctoidCollection.Select((x, i) => x.Index = i).ToList().ForEach(x => { });

            this.FunctoidCollection.ForEach(x => x.TransformDefinition = this);
            this.MapCollection.ForEach(x => x.TransformDefinition = this);


            var code = new StringBuilder();
            if (this.InputCollection.Count > 0)
            {
                code.AppendLinf(" var item = new {0}.Input();", this.Name);
                foreach (var input in this.InputCollection)
                {
                    var type = Strings.GetType(input.TypeName);
                    if (null == type) continue;
                    code.AppendLinf("item.{0} =  {1};", input.Name, input.Name.ToCamelCase());
                }
            }

            code.AppendLinf("               var dest =  new {0}();", this.OutputType.FullName);
            code.AppendLine("               this.BeforeTransform(item, dest);");
            // functoids statement

            var functoidStatements = from f in this.FunctoidCollection.OrderBy(x =>x)
                                     let statement = f.GenerateStatementCode()
                                     where !string.IsNullOrWhiteSpace(statement)
                                     && (!statement.Contains("Collection.") || (f.GetType() == typeof(LoopingFunctoid)))
                                     select $@"
{GAP}//{f.Name}:{f.GetType().Name}:{f.WebId}
{GAP}{statement}";
            code.AppendLine(string.Concat(functoidStatements.ToArray()));
            code.AppendLine();

            var mappingCodes = from m in this.MapCollection
                               select $@"{GAP}//NoName:Map:{m.WebId}
{GAP}{m.GenerateCode()}";
            code.JoinAndAppendLine(mappingCodes, "\r\n");
            code.AppendLine();

            code.AppendLine("               this.AfterTransform(item, dest);");

            var asyncCode = CodeExpression.Load(code).HasAsyncAwait;
            code.AppendLinf(asyncCode ?
                "               return dest;" :
                "               return Task.FromResult(dest);");

            methodDeclaration.AppendLine($"           public{ (asyncCode ? " async" : "") } Task<{this.OutputType.FullName}> TransformAsync({args})");
            methodDeclaration.AppendLine("           {");
            methodDeclaration.Append(code);

            methodDeclaration.AppendLine("           }");

            // partial method
            if (this.InputCollection.Count > 0)
            {
                methodDeclaration.AppendLine($"partial void BeforeTransform(Input item,{this.OutputType.FullName} destination);");
                methodDeclaration.AppendLine($"partial void AfterTransform(Input item,{this.OutputType.FullName} destination);");
            }
            else
            {
                methodDeclaration.AppendLine($"partial void BeforeTransform({this.InputType.FullName} item,{this.OutputType.FullName} destination);");
                methodDeclaration.AppendLine($"partial void AfterTransform({this.InputType.FullName} item,{this.OutputType.FullName} destination);");
            }

            if (!string.IsNullOrWhiteSpace(this.InputTypeName))
                methodDeclaration.Replace("{SOURCE_TYPE}", this.InputType.FullName);
            else
                methodDeclaration.Replace("{SOURCE_TYPE}", this.Name + ".Input");

            methodDeclaration.Replace("{DEST_TYPE}", this.OutputType.FullName);
            return methodDeclaration.ToString();
        }

        public string GenerateTransformToArrayCode()
        {
            return ("//TODO : return the list of destinations objects");
        }

    }
}