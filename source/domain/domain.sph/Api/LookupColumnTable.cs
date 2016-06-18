using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain.Api
{
    [DebuggerDisplay("{Name} {Table}({KeyColumn}/{ValueColumn})")]
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