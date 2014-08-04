using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public override bool Validate()
        {
            throw new Exception("Not implemented, use ValidateAsync");
        }

        public virtual void RemoveInvalidArgument()
        {
            this.ArgumentCollection.RemoveAll(x => x.Functoid == null);
        }
        public virtual async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errors = new List<ValidationError>();
            var nf = from a in this.ArgumentCollection
                     where null == a.Functoid
                     select new ValidationError { PropertyName = a.Name, Message = string.Format("[{0}] Functoid is null", a.Name) };
            errors.AddRange(nf);
            var vfTasks = from a in this.ArgumentCollection
                          where null != a.Functoid
                          select a.Functoid.ValidateAsync();

            var vf = (await Task.WhenAll(vfTasks)).SelectMany(x => x.ToArray());
            errors.AddRange(vf);

            return errors;
        }
    }
}