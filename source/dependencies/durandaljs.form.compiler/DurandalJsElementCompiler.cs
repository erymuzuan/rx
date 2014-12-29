using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs.Properties;

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
            var booleanCompiler = new BooleanExpressionCompiler();
            this.Element = element.Clone();

            this.Element.Visible = booleanCompiler.Compile(element.Visible, entity);
            this.Element.Enable = booleanCompiler.Compile(element.Enable, entity);

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
    }
}