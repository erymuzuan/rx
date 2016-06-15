using System.Linq;

namespace Bespoke.Sph.Domain.Api
{
    public partial class TableRelation : DomainObject
    {
        public TableDefinition GetTable(Adapter adapter)
        {
            return adapter.TableDefinitionCollection.SingleOrDefault(x => x.Name == this.Table);
        }
    }
}