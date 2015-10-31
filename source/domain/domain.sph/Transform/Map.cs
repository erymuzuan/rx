using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class Map : DomainObject
    {
        [JsonIgnore]
        [XmlIgnore]
        public TransformDefinition TransformDefinition { get; set; }

        public virtual Task<string> ConvertAsync(object source)
        {
            throw new NotImplementedException();
        }
        public virtual string  GenerateCode()
        {
            return $"// NOT IMPLEMENTED =>{this.GetType().Name}";
        }

        [JsonIgnore]
        [XmlIgnore]
        public Type SourceType
        {
            get { return Strings.GetType(this.SourceTypeName); }
            set
            {
                this.SourceTypeName = value.GetShortAssemblyQualifiedName();
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        public Type DestinationType
        {
            get { return Strings.GetType(this.DestinationTypeName); }
            set
            {
                this.DestinationTypeName = value.GetShortAssemblyQualifiedName();
                RaisePropertyChanged();
            }
        }

        public virtual Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            throw new NotImplementedException();
        }
    }
}