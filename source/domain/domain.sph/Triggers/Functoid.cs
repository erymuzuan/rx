using System;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    public partial class ScriptFunctoid : Functoid
    {
        public override object Convert(object arg)
        {
            var script = ObjectBuilder.GetObject<IScriptEngine>();
            return script.Evaluate<object, object>(this.Expression, arg);
        }
    }

    [XmlInclude(typeof(ScriptFunctoid))]
    public partial class Functoid : DomainObject
    {
        public virtual object Convert(object arg)
        {
            throw new Exception("whooaaa");
        }
    }
}