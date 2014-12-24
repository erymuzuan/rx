using System;

namespace Bespoke.Sph.Domain
{
    public class FormElementCompiler
    {
        internal virtual string GenerateEditorTemplate(FormElement element)
        {
            throw new Exception("Not implemented");
        }
        internal virtual string GenerateDisplayTemplate(FormElement element)
        {
            throw new Exception("Not implemented");
        }
    }

    public class FormElementCompiler<T> : FormElementCompiler where T : FormElement
    {

        public virtual string GenerateEditor(T element)
        {
            throw new Exception("Not implemented");
        }
        public virtual string GenerateDisplay(T element)
        {
            throw new Exception("Not implemented");
        }

        internal override string GenerateEditorTemplate(FormElement element)
        {
            return this.GenerateEditor((T)element);
        }

        internal override string GenerateDisplayTemplate(FormElement element)
        {
            return this.GenerateDisplay((T) element);
        }
    }
}