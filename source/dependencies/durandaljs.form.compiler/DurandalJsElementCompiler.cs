using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public class DurandalJsElementCompiler<T> : FormElementCompiler<T> where T : FormElement
    {
        public T Element { get; private set; }
        protected virtual string EditorRazorTemplate { get { throw new Exception("Not implemented"); } }
        protected virtual string DisplayRazorTemplate { get { return null; } }
        public override string GenerateEditor(T element)
        {
            this.Element = element;
            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return razor.GenerateAsync(this.EditorRazorTemplate, this).Result;
        }


        public override string GenerateDisplay(T element)
        {
            this.Element = element;
            if (string.IsNullOrWhiteSpace(this.DisplayRazorTemplate))
                return string.Format(@"<span data-bind=""text:{0}""></span> ", element.Path);

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return razor.GenerateAsync(this.DisplayRazorTemplate, this).Result;
        }
    }
}