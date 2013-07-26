using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class FunctionField : Field
    {
        private IScriptEngine m_scriptEngine;

        [XmlIgnore]
        public IScriptEngine ScriptEngine
        {
            get
            {
                if (null == m_scriptEngine)
                    m_scriptEngine = ObjectBuilder.GetObject<IScriptEngine>();
                return m_scriptEngine;
            }
            set { m_scriptEngine = value; }
        }

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