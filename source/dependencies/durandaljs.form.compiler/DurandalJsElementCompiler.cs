using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs.Properties;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public class DurandalJsElementCompiler<T> : FormElementCompiler<T> where T : FormElement
    {
        public T Element { get; private set; }

        protected virtual string EditorRazorTemplate
        {
            get
            {
                var razor = EditorTemplateResources.ResourceManager.GetString("editor_template_" + typeof(T).Name);
                if (string.IsNullOrWhiteSpace(razor))
                    return @"<span class=""error"">No editor template defined for " + this.GetType().GetShortAssemblyQualifiedName() + "</span>";
                return razor;
            }
        }

        protected virtual string DisplayRazorTemplate
        {
            get
            {
                var razor = DisplayTemplateResource.ResourceManager.GetString("display_template_" + typeof(T).Name);
                if (string.IsNullOrWhiteSpace(razor))
                    return @"<span class=""error"">No display template defined for " + this.GetType().GetShortAssemblyQualifiedName() + "</span>";
                return razor;
            }
        }


        public override string GenerateEditor(T element, EntityDefinition entity)
        {
            var booleanCompiler = new ExpressionCompiler();
            this.Element = element.Clone();

            var visibleResult = booleanCompiler.CompileAsync<bool>(element.Visible, entity).Result;
            var enableResult = booleanCompiler.CompileAsync<bool>(element.Enable, entity).Result;
            if (!visibleResult.Success)
                return string.Format("<span class=\"error\">{0}</span>", string.Join("<br/>", visibleResult.DiagnosticCollection.Select(x => x.ToString())));
            if (!enableResult.Success)
                return string.Format("<span class=\"error\">{0}</span>", string.Join("<br/>", enableResult.DiagnosticCollection.Select(x => x.ToString())));

            this.Element.Visible = visibleResult.Code;
            this.Element.Enable = enableResult.Code;

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return razor.GenerateAsync(this.EditorRazorTemplate, this).Result;
        }


        public override string GenerateDisplay(T element, EntityDefinition entity)
        {
            this.Element = element;
            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return razor.GenerateAsync(this.DisplayRazorTemplate, this).Result;
        }

        public virtual string GetKnockoutDisplayBindingExpression()
        {
            var path = this.Element.Path;
            return string.Format("text: {0}", path);
        }

        public override IImmutableList<Diagnostic> GetDiagnostics(FormElement element, ExpressionDescriptor expression, EntityDefinition entity)
        {
            var func = expression.Field.Compile();
            var code = func(element);

            var file = new StringBuilder();
            file.AppendLine("using System;");
            file.AppendLine("namespace Bespoke." + ConfigurationManager.ApplicationName + "_" + entity.Id + ".Domain");
            file.AppendLine("{");
            file.AppendLine("   public class BooleanExpression");
            file.AppendLine("   {");
            file.AppendLinf("       public {1} Evaluate({0} item)  ", entity.Name, expression.ReturnType.ToCSharp());
            file.AppendLine("       {");
            file.AppendLinf("           return {0};", code);
            file.AppendLine("       }");
            file.AppendLine("   }");
            file.AppendLine("}");

            var trees = new ObjectCollection<CSharpSyntaxTree>();

            var tree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(file.ToString());
            trees.Add(tree);


            var codes = from c in entity.GenerateCode()
                        where !c.Key.EndsWith("Controller")
                        where !c.Key.EndsWith("Controller.cs")
                        let x = c.Value.Replace("using Bespoke.Sph.Web.Helpers;", string.Empty)
                        .Replace("using System.Web.Mvc;", string.Empty)
                        .Replace("using System.Linq;", string.Empty)
                        .Replace("using System.Threading.Tasks;", string.Empty)
                        select (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(x);
            trees.AddRange(codes.ToArray());

            var compilation = CSharpCompilation.Create("eval")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReference<object>()
                .AddReference<XmlAttributeAttribute>()
                .AddReference<EntityDefinition>()
                .AddSyntaxTrees(trees);


            var diagnostics = compilation.GetDiagnostics().Where(x => x.Id != "CS8019");
            return diagnostics.ToImmutableList();
        }
    }
}