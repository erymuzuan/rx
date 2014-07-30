using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class TransformDefinition
    {
        private string m_inputTypeName;
        private string m_outputTypeName;

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

        public string OutputTypeName
        {
            get { return m_outputTypeName; }
            set
            {
                m_outputTypeName = value;
                RaisePropertyChanged();
            }
        }

        public string InputTypeName
        {
            get { return m_inputTypeName; }
            set
            {
                m_inputTypeName = value;
                RaisePropertyChanged();
            }
        }
    }
}
