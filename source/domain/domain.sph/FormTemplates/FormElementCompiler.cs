using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Bespoke.Sph.Domain
{
    public class FormElementCompiler
    {
        internal virtual string GenerateEditorTemplate(FormElement element, IProjectProvider project)
        {
            throw new Exception("Not implemented");
        }
        internal virtual string GenerateDisplayTemplate(FormElement element, IProjectProvider project)
        {
            throw new Exception("Not implemented");
        }

        public virtual IImmutableList<Diagnostic> GetDiagnostics(FormElement element, ExpressionDescriptor expression, IProjectProvider entity)
        {
            throw new NotImplementedException();
        }
    }

    public class FormElementCompiler<T> : FormElementCompiler where T : FormElement
    {

        public virtual string GenerateEditor(T element, IProjectProvider project)
        {
            throw new Exception("Not implemented");
        }
        public virtual string GenerateDisplay(T element, IProjectProvider project)
        {
            throw new Exception("Not implemented");
        }

        internal override string GenerateEditorTemplate(FormElement element, IProjectProvider project)
        {
            return this.GenerateEditor((T)element, project);
        }

        internal override string GenerateDisplayTemplate(FormElement element, IProjectProvider project)
        {
            return this.GenerateDisplay((T)element, project);
        }
    }
}