using System;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(ScriptFunctoid))]
    public partial class Functoid : DomainObject
    {
        public virtual T Convert<T, TArg>(TArg arg)
        {
            throw new Exception("whooaaa " + this.GetType().Name);
        }

        
        public virtual Task<string> ConvertAsync(object source)
        {
            throw new Exception("Not implemented");
        }

        public virtual string GeneratePreCode(FunctoidMap map)
        {
            return string.Empty;
        }
        public virtual string GenerateCode()
        {
            return string.Format("// NOT IMPLEMENTED => {0}", this.GetType().Name);
        }
    }
}