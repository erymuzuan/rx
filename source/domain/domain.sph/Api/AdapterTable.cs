using System.Diagnostics;

namespace Bespoke.Sph.Domain.Api
{
    [DebuggerDisplay("{Name} version:{VersionColumn}, modified: {ModifiedDateColumn}")]
    public class AdapterTable
    {
        public string Name { get; set; }
        public string VersionColumn { get; set; }
        public string ModifiedDateColumn { get; set; }

        public ObjectCollection<TableRelation> ChildRelationCollection { get; } = new ObjectCollection<TableRelation>();

        public ObjectCollection<TableRelation> ParentCollection { get; } = new ObjectCollection<TableRelation>();

        public override string ToString()
        {
            return this.Name;
        }
    }
}