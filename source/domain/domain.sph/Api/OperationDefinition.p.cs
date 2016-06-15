using System.Collections.Generic;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain.Api
{
    public partial class OperationDefinition : DomainObject
    {
        public string CodeNamespace { get; set; }

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
