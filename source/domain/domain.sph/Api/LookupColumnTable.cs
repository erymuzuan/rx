using System;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain.Api
{
    public partial class LookupColumnTable : DomainObject
    {
        [JsonIgnore]
        public Type Type
        {
            get { return Strings.GetType(this.TypeName); }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }
    }
}