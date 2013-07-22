using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public class FuctionField : Field
    {
        [XmlIgnore]
        public IScriptEngine ScriptEngine { get; set; }
        public string Script { get; set; }

        public override object GetValue(Entity item)
        {
            return this.ScriptEngine.Evaluate(this.Script, item);
        }
    }

    public interface IScriptEngine
    {
        object Evaluate(string script, Entity item);
    }
}