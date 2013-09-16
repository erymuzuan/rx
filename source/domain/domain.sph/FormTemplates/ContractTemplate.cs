namespace Bespoke.Sph.Domain
{

    public partial class ContractTemplate : Entity
    {
        private readonly ObjectCollection<CustomField> m_customFieldCollection = new ObjectCollection<CustomField>();

        public ObjectCollection<CustomField> CustomFieldCollection
        {
            get { return m_customFieldCollection; }
        }
    }
}