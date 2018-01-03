using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Csharp.CompilersServices.Extensions
{
    public static class TransformDefinitionExtension
    {
        private static string GetCodeHeader(this TransformDefinition map)
        {
            var header = new StringBuilder();
            header.AppendLine("using " + typeof(Entity).Namespace + ";");
            header.AppendLine("using " + typeof(int).Namespace + ";");
            header.AppendLine("using " + typeof(Task<>).Namespace + ";");
            header.AppendLine("using " + typeof(IEnumerable<>).Namespace + ";");
            header.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            header.AppendLine();

            header.AppendLine("namespace " + map.CodeNamespace);
            header.AppendLine("{");
            return header.ToString();

        }


        public static Dictionary<string, string> GenerateCode(this TransformDefinition map)
        {
            var header = map.GetCodeHeader();
            var code = new StringBuilder(header);

            code.AppendLine("   public partial class " + map.ClassName);
            code.AppendLine("   {");


            code.AppendLine(map.GenerateTransformCode());
            code.AppendLine(map.GenerateTransformToArrayCode());

            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace

            var sourceCodes = new Dictionary<string, string> { { map.ClassName + ".cs", code.FormatCode() } };


            return sourceCodes;
        }
        public static bool GeneratePartialCode(this TransformDefinition map, out string file)
        {
            file = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(TransformDefinition)}\\{map.Id}.cs";
            var partial = new Class
            {
                IsPartial = true,
                Name = map.ClassName,
                FileName = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(TransformDefinition)}\\{map.Id}.cs",
                Namespace = map.CodeNamespace
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
                TypeName = map.InputTypeName,
            });

            partial.MethodCollection[0].ArgumentCollection.Add(new MethodArg
            {
                Name = "destination",
                TypeName = map.OutputTypeName,
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
                TypeName = map.InputTypeName,
            });
            partial.MethodCollection[1].ArgumentCollection.Add(new MethodArg
            {
                Name = "destination",
                TypeName = map.OutputTypeName,
            });

            if (!File.Exists(file))
            {
                File.WriteAllText(file, partial.GetCode());
                return true;
            }


            return false;
        }

    }
}
