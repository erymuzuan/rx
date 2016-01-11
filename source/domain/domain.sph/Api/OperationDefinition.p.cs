using System.Collections.Generic;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain.Api
{
    public partial class OperationDefinition
    {
        public string WebId { get; set; }
        public string Name { get; set; }
        public string MethodName { get; set; }
        public bool IsOneWay { get; set; }
        public bool IsEvent { get; set; }
        public ParameterDefinition RequestDefinition { get; set; }
        public ParameterDirection ResponseDefinition { get; set; }
        public string CodeNamespace { get; set; }

        public ObjectCollection<Member> RequestMemberCollection { get; } = new ObjectCollection<Member>();

        public ObjectCollection<Member> ResponseMemberCollection { get; } = new ObjectCollection<Member>();

        public virtual IEnumerable<Class> GenerateRequestCode()
        {
            throw new System.NotImplementedException();
        }
        public virtual IEnumerable<Class> GenerateResponseCode()
        {
            throw new System.NotImplementedException();
        }

        public string Uuid { get; set; }
    }
}
