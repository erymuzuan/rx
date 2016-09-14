using System;
using System.Collections.Generic;
using System.Linq;
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
            var code = new StringBuilder();
            var uri = new Uri(this.BaseAddress);
            var opUri = uri.LocalPath;

            code.AppendLine(this.GenerateRequestQueryStringsCode(ref opUri));
            code.AppendLine($@" 
            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(request.Body, setting);

            var content = new StringContent(json);

            var url = $@""{opUri}"";
            var httpRequest  = new HttpRequestMessage{{Method = new HttpMethod(""{HttpMethod}""), Content = content}};
            var response = await m_client.SendAsync(httpRequest);

            ");


            code.AppendLine(this.GenerateProcessHttpResonseCode());

            return code.ToString();
        }

        protected string GenerateRequestQueryStringsCode(ref string opUri)
        {
            var qsMember = this.RequestMemberCollection.FirstOrDefault(x => x.Name.EndsWith("QueryStrings"));
            if (null == qsMember)
                return string.Empty;

            var code = new StringBuilder("var qs = request.QueryStrings;");
            var queryString = new StringBuilder();
            queryString.JoinAndAppendLine(qsMember.MemberCollection, "&", s => $"{s.FullName}={{qs.{s.Name}}}");

            if (qsMember.MemberCollection.Any())
            {
                opUri += "?" + queryString;
            }

            return code.ToString();
        }

        protected string GenerateProcessHttpResonseCode()
        {
            var code = new StringBuilder();
            code.AppendLine("           var content2 = response.Content as StreamContent;");
            code.AppendLine("           if(null == content2) throw new Exception(\"Fail to read from response\");");
            code.AppendLine("           var text = await content2.ReadAsStringAsync();");

            code.AppendLine($"var result = new {MethodName}Response();");
            code.AppendLine($"result.Body = JsonConvert.DeserializeObject<{MethodName}ResponseBody>(text);");

            var headerParent = this.ResponseMemberCollection.Single(x => x.Name == "Headers");
            var stringHeaders = headerParent.MemberCollection.OfType<SimpleMember>().Where(x => x.Type == typeof(string)).ToArray();
            var dateTimeHeaders = headerParent.MemberCollection.OfType<SimpleMember>().Where(x => x.Type == typeof(DateTime)).ToArray();
            var int32Headers = headerParent.MemberCollection.OfType<SimpleMember>().Where(x => x.Type == typeof(int)).ToArray();
            var decimalHeaders = headerParent.MemberCollection.OfType<SimpleMember>().Where(x => x.Type == typeof(decimal)).ToArray();
            var booleanHeaders = headerParent.MemberCollection.OfType<SimpleMember>().Where(x => x.Type == typeof(bool)).ToArray();

            Func<SimpleMember, bool> checkContentHeader = m =>
            {
                if (m.FullName.Equals("Expires", StringComparison.InvariantCultureIgnoreCase)) return true;
                if (m.FullName.ToEmptyString().StartsWith("content-", StringComparison.InvariantCultureIgnoreCase)) return true;
                return false;
            };

            Func<SimpleMember, string> hasHeaderCode = m => checkContentHeader(m) ?
                                                                $@"response.Content.Headers.Contains(""{m.FullName}"")" :
                                                                $@"response.Headers.Contains(""{m.FullName}"")";

            Func<SimpleMember, string> readHeaderCode = m => checkContentHeader(m) ? 
                                                                $@"response.Content.Headers.GetValues(""{m.FullName}"")" : 
                                                                $@"response.Headers.GetValues(""{m.FullName}"")";

            #region "strings headers"
            foreach (var header in stringHeaders.Where(x => x.AllowMultiple))
            {
                code.AppendLine($@"if({hasHeaderCode(header)})");
                code.AppendLine($@"   result.Headers.{header.Name}.AddRange({readHeaderCode(header)});");

            }
            foreach (var h in stringHeaders.Where(x => !x.AllowMultiple))
            {
                code.AppendLine($@"if({hasHeaderCode(h)})");
                code.AppendLine($@"   result.Headers.{h.Name} = string.Join(""; "", {readHeaderCode(h)});");
            }
            #endregion

            #region "DateTime headers"
            foreach (var header in dateTimeHeaders.Where(x => !x.AllowMultiple))
            {
                code.AppendLine($@"if({hasHeaderCode(header)})");
                code.AppendLine("  {");
                if (header.IsNullable)
                {
                    code.AppendLine($@"    var header{header.Name}Raw = {readHeaderCode(header)}.FirstOrDefault();");
                    code.AppendLine($@"    DateTime header{header.Name};
                                           if(DateTime.TryParse(header{header.Name}Raw, out header{header.Name}))
                                              result.Headers.{header.Name} = header{header.Name};
");
                }
                else
                {
                    code.AppendLine($@"     var header{header.Name}Raw = {readHeaderCode(header)}.First();");
                    code.AppendLine($@"     result.Headers.{header.Name} = DateTime.Parse(header{header.Name}Raw);");
                }
                code.AppendLine("  }");
            }

            #endregion

            #region "Int32 headers"
            foreach (var header in int32Headers.Where(x => !x.AllowMultiple))
            {
                code.AppendLine($@"if({hasHeaderCode(header)})");
                code.AppendLine("  {");
                if (header.IsNullable)
                {
                    code.AppendLine($@"var header{header.Name}Raw = {readHeaderCode(header)}.FirstOrDefault();");
                    code.AppendLine($@"int header{header.Name};
                                       if(int.TryParse(header{header.Name}Raw, out header{header.Name}))
                                            result.Headers.{header.Name} = header{header.Name};
");
                }
                else
                {
                    code.AppendLine($@"var header{header.Name}Raw = {readHeaderCode(header)}.First();");
                    code.AppendLine($@"result.Headers.{header.Name} = int.Parse(header{header.Name}Raw);");
                }
                code.AppendLine("  }");
            }
            #endregion

            #region "decimals headers"
            foreach (var header in decimalHeaders.Where(x => !x.AllowMultiple))
            {
                code.AppendLine($@"if({hasHeaderCode(header)})");
                code.AppendLine("  {");
                if (header.IsNullable)
                {
                    code.AppendLine($@"var header{header.Name}Raw = {readHeaderCode(header)}.FirstOrDefault();");
                    code.AppendLine($@"decimal header{header.Name};
                                       if(decimal.TryParse(header{header.Name}Raw, out header{header.Name}))
                                            result.Headers.{header.Name} = header{header.Name};
");
                }
                else
                {
                    code.AppendLine($@"var header{header.Name}Raw = {readHeaderCode(header)}.First();");
                    code.AppendLine($@"result.Headers.{header.Name} = decimal.Parse(header{header.Name}Raw);");
                }
                code.AppendLine("  }");
            }
            #endregion

            #region "boolean headers"
            foreach (var header in booleanHeaders.Where(x => !x.AllowMultiple))
            {
                code.AppendLine($@"if({hasHeaderCode(header)})");
                code.AppendLine("  {");
                if (header.IsNullable)
                {
                    code.AppendLine($@"var header{header.Name}Raw = {readHeaderCode(header)}.FirstOrDefault();");
                    code.AppendLine($@"bool header{header.Name};
                                       if(bool.TryParse(header{header.Name}Raw, out header{header.Name}))
                                            result.Headers.{header.Name} = header{header.Name};
");
                }
                else
                {
                    code.AppendLine($@"var header{header.Name}Raw = {readHeaderCode(header)}.First();");
                    code.AppendLine($@"result.Headers.{header.Name} = bool.Parse(header{header.Name}Raw);");
                }
                code.AppendLine("  }");
            }
            #endregion

            code.AppendLine("return result;");
            return code.ToString();
        }
    }
}