using System.Linq;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain.Api
{
    public partial class TableDefinition : DomainObject
    {
        [JsonIgnore]
        public Column PrimaryKey
        {
            get { return this.ColumnCollection.FirstOrDefault(a => this.PrimaryKeyCollection.Contains(a.Name)); }
        }

        //TODO : move to xsd
        public ObjectCollection<string> PrimaryKeyCollection { get; } = new ObjectCollection<string>();

        public override string ToString()
        {
            return this.Name;
        }


    }
}
