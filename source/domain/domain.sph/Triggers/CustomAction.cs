using System;
using System.Drawing;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
        [JsonIgnore]
        public virtual bool UseAsync
        {
            get
            {
                throw new Exception("NotImplemented");
            }
        }
        [JsonIgnore]
        public virtual bool UseCode => false;

        public virtual string GetEditorViewModel()
        {
            throw new Exception(this.GetType().FullName + " does not implement GetEditorViewModel method");
        }

        public virtual string GetEditorView()
        {
            throw new Exception(this.GetType().FullName + " does not implement GetEditorView method");
        }

        public virtual Bitmap GetPngIcon()
        {
            return Properties.Resources.Clapper_Board;
        }

        public virtual string GeneratorCode()
        {
            throw new Exception(this.GetType().FullName + " does not implement GeneratorCode method");
        }
    }
}
