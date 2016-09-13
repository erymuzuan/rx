using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class RestApiOperationDefinition
    {
        public override IEnumerable<Class> GenerateRequestCode()
        {
            var list = base.GenerateRequestCode().ToList();
            var request = new Class { Name = $"{MethodName}Request", Namespace = CodeNamespace };

            request.AddProperty($"public {MethodName}QueryString QueryStrings {{get;set;}} = new {MethodName}QueryString();");
            request.AddProperty($"public {MethodName}RequestHeader Headers {{get;set;}} = new {MethodName}RequestHeader();");
            if (this.HttpMethod != "GET")
                request.AddProperty($"public {MethodName}RequestBody Body{{get;set;}} = new {MethodName}RequestBody();");

            list.Add(request);
            return list;
        }

        public override IEnumerable<Class> GenerateResponseCode()
        {
            var list = base.GenerateResponseCode().ToList();
            var request = new Class { Name = $"{MethodName}Response", Namespace = CodeNamespace };

            request.AddProperty($"public {MethodName}ResponseHeader Headers {{get;set;}} = new {MethodName}ResponseHeader();");
            request.AddProperty($"public {MethodName}ResponseBody Body{{get;set;}} = new {MethodName}ResponseBody();");

            list.Add(request);
            return list;
        }

        protected override string GenerateAdapterActionBody(Adapter adapter)
        {
            var uri = new Uri(this.BaseAddress);
            var opUri = uri.LocalPath;
            var code = new StringBuilder();

            var requestQueryStringMember =
                this.RequestMemberCollection.FirstOrDefault(x => x.Name.EndsWith("QueryStrings"));
            if (null != requestQueryStringMember)
            {
                code.AppendLine("var qs = request.QueryStrings;");
                var queryString = new StringBuilder();
                queryString.JoinAndAppendLine(requestQueryStringMember.MemberCollection, "&",
                    s => $"{s.FullName}={{qs.{s.Name}}}");
                if (queryString.Length > 0)
                {
                    opUri += "?" + queryString.ToString();
                }
            }
            
            if (this.HttpMethod == "GET")
            {
                code.AppendLine($@" 

            var url = $@""{opUri}"";
            var response = await m_client.GetAsync(url);

            ");
            }
            else
            {

                code.AppendLine($@" 
            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(request.Body, setting);

            var content = new StringContent(json);

            var url = $@""{opUri}"";
            var httpRequest  = new HttpRequestMessage{{Method = new HttpMethod(""{HttpMethod}""), Content = content}};
            var response = await m_client.SendAsync(httpRequest);

            ");
            }


            code.AppendLine("           var content2 = response.Content as StreamContent;");
            code.AppendLine("           if(null == content2) throw new Exception(\"Fail to read from response\");");
            code.AppendLine("           var text = await content2.ReadAsStringAsync();");

            code.AppendLine($"var result = new {MethodName}Response();");
            code.AppendLine($"result.Body = JsonConvert.DeserializeObject<{MethodName}ResponseBody>(text);");


            code.AppendLine("return result;");

            return code.ToString();
        }
    }
}