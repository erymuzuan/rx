using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class RestApiOperationDefinition
    {
        public async Task BuildAsync(string name = "")
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                var uri = new Uri(BaseAddress);
                name = $"{HttpMethod.ToLowerInvariant()} {uri.LocalPath.Replace("/", " ")}".ToPascalCase();
            }
            this.Name = name;
            this.MethodName = name;
            this.Schema = "";
            this.WebId = Guid.NewGuid().ToString();
            this.Uuid = this.WebId;

            var qms = await m_builder.GetRequestQueryStringMembersAsync();
            var hms = await m_builder.GetRequestHeaderMembersAsync();

            var querySrings = new ComplexMember
            {
                Name = "QueryStrings",
                AllowMultiple = false,
                TypeName = $"{name}QueryString"
            };
            querySrings.MemberCollection.AddRange(qms);

            var requestHeaders = new ComplexMember
            {
                Name = "Headers",
                TypeName = $"{name}RequestHeader",
                AllowMultiple = false
            };
            requestHeaders.MemberCollection.AddRange(hms);

            this.RequestMemberCollection.Clear();
            this.RequestMemberCollection.AddRange(querySrings, requestHeaders);
            if (this.HttpMethod != "GET")
            {
                var requestBodyMembers = await m_builder.GetRequestBodyMembersAsync();

                var requestBody = new ComplexMember {Name = "Body", TypeName = $"{name}RequestBody"};
                requestBody.MemberCollection.AddRange(requestBodyMembers.Where(x => null != x));
                this.RequestMemberCollection.Add(requestBody);
            }

            var responseHeaderMembers = await m_builder.GetResponseHeaderMembersAsync();
            var responseHeader = new ComplexMember
            {
                AllowMultiple = false,
                Name = "Headers",
                TypeName = $"{Name}ResponseHeader"
            };
            responseHeader.MemberCollection.AddRange(responseHeaderMembers);


            var responseBody = new ComplexMember
            {
                Name = "Body",
                AllowMultiple = false,
                TypeName = $"{name}ResponseBody"
            };
            var members = await m_builder.GetResponseBodyMembersAsync();
            responseBody.MemberCollection.AddRange(members);
            // response
            this.ResponseMemberCollection.Clear();
            this.ResponseMemberCollection.Add(responseHeader);
            this.ResponseMemberCollection.Add(responseBody);
        }
    }
}