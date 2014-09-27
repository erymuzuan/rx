using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class CustomAction : DomainObject
    {
        public virtual void Execute(RuleContext context)
        {
            throw new Exception("NotImplemented");
        }

        public virtual Task ExecuteAsync(RuleContext context)
        {
            throw new Exception("NotImplemented");
        }
        public virtual bool UseAsync
        {
            get
            {
                throw new Exception("NotImplemented");
            }
        }
        public virtual bool UseCode
        {
            get
            {
                return false;
            }
        }

        public virtual string GetEditorViewModel()
        {
            throw new Exception(this.GetType().FullName + " does not implement GetEditorViewModel method");
        }

        public virtual string GetEditorView()
        {
            throw new Exception(this.GetType().FullName + " does not implement GetEditorView method");
        }

        public virtual string GeneratorCode()
        {
            throw new Exception(this.GetType().FullName + " does not implement GeneratorCode method");
        }
    }
}
