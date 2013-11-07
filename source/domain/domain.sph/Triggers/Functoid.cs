using System;
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
    }
}