using System.Linq;

namespace Bespoke.Sph.Domain.Api
{
    public partial class TableDefinition
    {
        public string Schema { get; set; }

        public ObjectCollection<TableDefinition> ParentTableCollection { get; } = new ObjectCollection<TableDefinition>();

        public ObjectCollection<TableDefinition> ChildTableCollection { get; } = new ObjectCollection<TableDefinition>();

        public ObjectCollection<Member> MemberCollection { get; } = new ObjectCollection<Member>();

        public Member PrimaryKey
        {

            get { return this.MemberCollection.FirstOrDefault(a => this.PrimaryKeyCollection.Contains(a.Name)); }
        }

        public ObjectCollection<string> PrimaryKeyCollection { get; } = new ObjectCollection<string>();

        public override string ToString()
        {
            return this.Name;
        }
    }
}
