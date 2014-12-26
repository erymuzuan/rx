using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public class DurandalJsElementCompiler<T> : FormElementCompiler<T> where T : FormElement
    {
        public T Element { get; private set; }

        protected virtual string EditorRazorTemplate
        {
            get
            {
                var razor = Properties.EditorTemplateResources.ResourceManager.GetString("editor_template_" + typeof(T).Name);
                if (string.IsNullOrWhiteSpace(razor))
                    return @"<span class=""error"">No editor template defined for " + this.GetType().GetShortAssemblyQualifiedName() + "</span>";
                return razor;
            }
        }

        protected virtual string DisplayRazorTemplate
        {
            get
            {
                var razor = Properties.DisplayTemplateResource.ResourceManager.GetString("display_template_" + typeof(T).Name);
                if (string.IsNullOrWhiteSpace(razor))
                    return @"<span class=""error"">No display template defined for " + this.GetType().GetShortAssemblyQualifiedName() + "</span>";
                return razor;
            }
        }
        public override string GenerateEditor(T element)
        {
            this.Element = element;
            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return razor.GenerateAsync(this.EditorRazorTemplate, this).Result;
        }


        public override string GenerateDisplay(T element)
        {
            this.Element = element;
            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return razor.GenerateAsync(this.DisplayRazorTemplate, this).Result;
        }
    }
}