namespace Bespoke.Sph.Domain.Api
{
    public partial class OperationDefinition
    {
        public string Name { get; set; }
        public string MethodName { get; set; }
        public bool IsOneWay { get; set; }
        public bool IsEvent { get; set; }
        public ParameterDefinition RequestDefinition { get; set; }
        public ParameterDirection ResponseDefinition { get; set; }
        public string CodeNamespace { get; set; }

        private readonly ObjectCollection<Member> m_requestMemberCollection = new ObjectCollection<Member>();

        public ObjectCollection<Member> RequestMemberCollection
        {
            get { return m_requestMemberCollection; }
        }

        private readonly ObjectCollection<Member> m_responseMemberCollection = new ObjectCollection<Member>();

        public ObjectCollection<Member> ResponseMemberCollection
        {
            get { return m_responseMemberCollection; }
        }

        public virtual string GenerateRequestCode()
        {
            throw new System.NotImplementedException();
        }
        public virtual string GenerateResponseCode()
        {
            throw new System.NotImplementedException();
        }

        public string Uuid { get; set; }
    }
}
