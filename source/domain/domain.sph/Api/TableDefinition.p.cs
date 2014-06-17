using System.Linq;

namespace Bespoke.Sph.Domain.Api
{
    public partial class TableDefinition
    {
        public string Schema { get; set; }
        private readonly ObjectCollection<Member> m_memberCollection = new ObjectCollection<Member>();
        private readonly ObjectCollection<TableDefinition> m_childTableCollection = new ObjectCollection<TableDefinition>();
        private readonly ObjectCollection<TableDefinition> m_parentTableCollection = new ObjectCollection<TableDefinition>();

        public ObjectCollection<TableDefinition> ParentTableCollection
        {
            get { return m_parentTableCollection; }
        }

        public ObjectCollection<TableDefinition> ChildTableCollection
        {
            get { return m_childTableCollection; }
        }

        public ObjectCollection<Member> MemberCollection
        {
            get { return m_memberCollection; }
        }

        public Member PrimaryKey
        {

            get { return this.MemberCollection.FirstOrDefault(a => this.PrimaryKeyCollection.Contains(a.Name)); }
        }

        private readonly ObjectCollection<string> m_primaryKeyCollection = new ObjectCollection<string>();

        public ObjectCollection<string> PrimaryKeyCollection
        {
            get { return m_primaryKeyCollection; }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
