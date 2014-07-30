using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class Map : DomainObject
    {
        public virtual Task<string> ConvertAsync(object source)
        {
            throw new System.NotImplementedException();
        }
        public virtual string  GenerateCode()
        {
            return string.Format("// NOT IMPLEMENTED =>{0}", this.GetType().Name);
        }

        private Type m_destinationType;
        private Type m_sourceType;

        public Type SourceType
        {
            get { return m_sourceType; }
            set
            {
                m_sourceType = value;
                RaisePropertyChanged();
            }
        }

        public Type DestinationType
        {
            get { return m_destinationType; }
            set
            {
                m_destinationType = value;
                RaisePropertyChanged();
            }
        }
    }
}