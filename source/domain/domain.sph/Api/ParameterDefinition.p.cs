namespace Bespoke.Sph.Domain.Api
{
    public partial class ParameterDefinition
    {
        public string Name { get; set; }
        public ParameterDirection Direction { get; set; }
        public ParameterFormat Format { get; set; }
        public string CodeNamespace { get; set; }

        public string WebId { get; set; }

        private readonly ObjectCollection<Member> m_memberCollection = new ObjectCollection<Member>();

        public ObjectCollection<Member> MemberCollection
        {
            get { return m_memberCollection; }
        }
    }

    public enum ParameterDirection
    {
        Request,
        Response
    }

    public enum ParameterFormat
    {
        Json,
        FormEncoded
    }
}
