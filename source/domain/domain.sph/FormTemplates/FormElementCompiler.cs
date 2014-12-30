using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Bespoke.Sph.Domain
{
    public class FormElementCompiler
    {
        internal virtual string GenerateEditorTemplate(FormElement element, EntityDefinition entity)
        {
            throw new Exception("Not implemented");
        }
        internal virtual string GenerateDisplayTemplate(FormElement element, EntityDefinition entity)
        {
            throw new Exception("Not implemented");
        }

        public virtual IImmutableList<Diagnostic> GetDiagnostics(FormElement element, ExpressionDescriptor expression, EntityDefinition entity)
        {
            throw new NotImplementedException();
        }
    }

    public class FormElementCompiler<T> : FormElementCompiler where T : FormElement
    {

        public virtual string GenerateEditor(T element, EntityDefinition entity)
        {
            throw new Exception("Not implemented");
        }
        public virtual string GenerateDisplay(T element, EntityDefinition entity)
        {
            throw new Exception("Not implemented");
        }

        internal override string GenerateEditorTemplate(FormElement element, EntityDefinition entity)
        {
            return this.GenerateEditor((T)element, entity);
        }

        internal override string GenerateDisplayTemplate(FormElement element, EntityDefinition entity)
        {
            return this.GenerateDisplay((T) element, entity);
        }
    }
}