using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class TransformDefinition
    {
        private readonly ObjectCollection<ReferencedAssembly> m_referencedAssemblyCollection = new ObjectCollection<ReferencedAssembly>();
        private readonly ObjectCollection<string> m_inputTypeNameCollection = new ObjectCollection<string>();

        public ObjectCollection<string> InputTypeNameCollection
        {
            get { return m_inputTypeNameCollection; }
        }

        public ObjectCollection<ReferencedAssembly> ReferencedAssemblyCollection
        {
            get { return m_referencedAssemblyCollection; }
        }

        [XmlIgnore]
        [JsonIgnore]
        public Type OutputType
        {
            get
            {
                return Type.GetType(this.OutputTypeName);
            }
            set
            {
                this.OutputTypeName = value.GetShortAssemblyQualifiedName();
                RaisePropertyChanged();
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        public Type InputType
        {
            get
            {
                return Type.GetType(this.InputTypeName);
            }
            set
            {
                this.InputTypeName = value.GetShortAssemblyQualifiedName();
                RaisePropertyChanged();
            }
        }

       
    }
}
