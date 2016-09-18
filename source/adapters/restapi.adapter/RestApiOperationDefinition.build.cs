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
            var uri = new Uri(BaseAddress);
            if (string.IsNullOrWhiteSpace(name))
            {
                name = $"{HttpMethod.ToLowerInvariant()} {uri.LocalPath.Replace("/", " ")}".ToPascalCase();
            }
            this.Name = name;
            this.MethodName = name;
            this.Schema = "";
            this.WebId = Guid.NewGuid().ToString();
            this.Uuid = this.WebId;

            var qms = await m_builder.GetRequestQueryStringMembersAsync();
            var hms = await m_builder.GetRequestHeaderMembersAsync();

            var routeParameters = new ComplexMember
            {
                Name = "RouteParameters",
                AllowMultiple = false,
                TypeName = $"{name}Route"
            };
            var routes = from s in uri.AbsolutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                         select new RouteParameterMember
                         {
                             Name = s.ToPascalCase(),
                             DefaultValue = new ConstantField { Name = s.ToPascalCase(), Value = s, Type = typeof(string) },
                             FullName = s,
                             Type = s.TryGuessType()
                         };
            routeParameters.MemberCollection.AddRange(routes);



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
            this.RequestMemberCollection.AddRange(routeParameters, querySrings, requestHeaders);
            var requestBody = await this.GetRequestBodyMemberAsync();
            if (null != requestBody)
                this.RequestMemberCollection.Add(requestBody);

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
            this.ResponseMemberCollection.Add(new SimpleMember { Name = "Allow", FullName = "Allow", Type = typeof(string), AllowMultiple = true });
            this.ResponseMemberCollection.Add(new SimpleMember { Name = "StatusText", FullName = "StatusText", Type = typeof(string) });
            this.ResponseMemberCollection.Add(new SimpleMember { Name = "StatusCode", FullName = "StatusCode", Type = typeof(int) });
            this.ResponseMemberCollection.Add(new SimpleMember { Name = "ContentLength", FullName = "Content-Length", Type = typeof(int), IsNullable = true });
            // MediaTypeHeaderValue
            this.ResponseMemberCollection.Add(new SimpleMember { Name = "ContentType", FullName = "Content-Type", Type = typeof(string) });
            // ContentDispositionHeaderValue
            this.ResponseMemberCollection.Add(new SimpleMember { Name = "ContentDisposition", FullName = "Content-Disposition", Type = typeof(string) });

            this.ResponseMemberCollection.Add(new SimpleMember { Name = "ContentEncoding", FullName = "Content-Encoding", Type = typeof(string), AllowMultiple = true });
            this.ResponseMemberCollection.Add(new SimpleMember { Name = "ContentLanguage", FullName = "Content-Language", Type = typeof(string), AllowMultiple = true });

            //Uri
            this.ResponseMemberCollection.Add(new SimpleMember { Name = "ContentLocation", FullName = "Content-Location", Type = typeof(string) });
            // byte array
            //this.ResponseMemberCollection.Add(new SimpleMember { Name = "ContentMD5", FullName = "Content-MD5", Type = typeof(int), AllowMultiple = true});
            this.ResponseMemberCollection.Add(new SimpleMember { Name = "ContentRange", FullName = "Content-Range", Type = typeof(string) });
            this.ResponseMemberCollection.Add(new SimpleMember { Name = "Expires", FullName = "Expires", Type = typeof(DateTime), IsNullable = true });
            this.ResponseMemberCollection.Add(new SimpleMember { Name = "LastModified", FullName = "Last-Modified", Type = typeof(DateTime), IsNullable = true });

        }

        protected virtual async Task<Member> GetRequestBodyMemberAsync()
        {
            var requestBodyMembers = await m_builder.GetRequestBodyMembersAsync();

            var requestBody = new ComplexMember { Name = "Body", TypeName = $"{Name}RequestBody" };
            requestBody.MemberCollection.AddRange(requestBodyMembers.Where(x => null != x));
            return requestBody;

        }
    }
}