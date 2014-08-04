using System.Linq;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(ScriptFunctoid))]
    [XmlInclude(typeof(ConstantFunctoid))]
    [XmlInclude(typeof(StringConcateFunctoid))]
    public partial class Functoid : DomainObject
    {
        public const string DESIGNER_CONTRACT = "FunctoidDesigner";

        public virtual bool Initialize()
        {
            return true;
        }
        private static int m_number = 1;
        protected static int GetRunningNumber()
        {
            return m_number++;
        }
        public static void ResetRunningNumber()
        {
            m_number = 1;
        }

        public virtual string GeneratePreCode(FunctoidMap map)
        {
            return string.Empty;
        }
        public virtual string GenerateCode()
        {
            return string.Format("// NOT IMPLEMENTED => {0}", this.GetType().Name);
        }


        public new FunctoidArg this[string index]
        {
            get { return this.ArgumentCollection.Single(x => x.Name == index); }

        }
    }
}