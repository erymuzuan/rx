using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class FunctionField : Field
    {
        [XmlIgnore]
        [JsonIgnore]
        private IScriptEngine m_scriptEngine;

        [XmlIgnore]
        [JsonIgnore]
        public IScriptEngine ScriptEngine
        {
            get { return m_scriptEngine ?? (m_scriptEngine = ObjectBuilder.GetObject<IScriptEngine>()); }
            set { m_scriptEngine = value; }
        }

        public override object GetValue(RuleContext context)
        {
            return this.ScriptEngine.Evaluate(this.Script, context.Item);
        }
    }

    public interface IScriptEngine
    {
        object Evaluate(string script, Entity item);
    }
}