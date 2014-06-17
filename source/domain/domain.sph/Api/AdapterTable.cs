namespace Bespoke.Sph.Domain.Api
{
    public class AdapterTable
    {
        public string Name { get; set; }
        private readonly ObjectCollection<TableRelation> m_childRelations = new ObjectCollection<TableRelation>();
        private readonly ObjectCollection<TableRelation> m_parentRelations = new ObjectCollection<TableRelation>();

        public ObjectCollection<TableRelation> ChildRelationCollection
        {
            get { return m_childRelations; }
        }


        public ObjectCollection<TableRelation> ParentCollection
        {
            get { return m_parentRelations; }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class TableRelation
    {
        public string Table { get; set; }
        public string Constraint { get; set; }
        public string Column { get; set; }
        public string ForeignColumn { get; set; }
    }
}