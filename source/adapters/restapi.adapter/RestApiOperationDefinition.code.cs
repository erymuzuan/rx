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

            request.AddProperty($"public {MethodName}Route Routes {{get;set;}} = new {MethodName}Route();");
            request.AddProperty($"public {MethodName}QueryString QueryStrings {{get;set;}} = new {MethodName}QueryString();");
            request.AddProperty($"public {MethodName}RequestHeader Headers {{get;set;}} = new {MethodName}RequestHeader();");
            request.AddProperty($"public {MethodName}RequestBody Body{{get;set;}} = new {MethodName}RequestBody();");

            list.Add(request);
            return list;
        }

        public override IEnumerable<Class> GenerateResponseCode()
        {
            var list = base.GenerateResponseCode().ToList();
            var response = new Class { Name = $"{MethodName}Response", Namespace = CodeNamespace };
            response.AddNamespaceImport<DomainObject,DateTime>();

            response.AddProperty($"public {MethodName}ResponseHeader Headers {{get;set;}} = new {MethodName}ResponseHeader();");
            response.AddProperty($"public {MethodName}ResponseBody Body{{get;set;}} = new {MethodName}ResponseBody();");
            var members = from m in this.ResponseMemberCollection.OfType<SimpleMember>()
                select new Property {Code = m.GeneratedCode()};
            response.PropertyCollection.AddRange(members);

            list.Add(response);
            return list;
        }

        protected override string GenerateAdapterActionBody(Adapter adapter)
        {
            var code = new StringBuilder();
            var opUri = this.GenerateRouteCode();

            code.AppendLine(this.GenerateRequestQueryStringsCode(ref opUri));
            code.AppendLine(this.GenerateRequestHeadersCode((RestApiAdapter)adapter));

            var contentType = this.RequestMemberCollection.Single(x => x.Name == "Headers")
                .MemberCollection.SingleOrDefault(x => x.FullName == "Content-Type");
            
            code.AppendLine($@"
            var contentType = {(contentType == null ? "application/json".ToVerbatim() : $"request.Headers.{contentType.Name};")}; 
            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(request.Body, setting);

            var content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);


            var url = $@""{opUri}"";
            var httpRequest  = new HttpRequestMessage{{Method = new HttpMethod(""{HttpMethod}""), Content = content}};
            httpRequest.RequestUri = new System.Uri(url, System.UriKind.Relative);

            var response = await m_client.SendAsync(httpRequest);

            ");


            code.AppendLine(this.GenerateProcessHttpResponseCode());

            return code.ToString();
        }

        protected string GenerateRequestHeadersCode(RestApiAdapter adapter)
        {
            var headerMembers = this.RequestMemberCollection.FirstOrDefault(x => x.Name.EndsWith("Headers"));
            if (null == headerMembers)
                return string.Empty;

            var code = new StringBuilder("var rqh = request.Headers;");
            code.AppendLine();
            code.AppendLine(@"m_client.DefaultRequestHeaders.Clear();");
            Func<SimpleMember, int, bool> checkContentHeader = (m, i) =>
              {
                  if (m.FullName.Equals("Expires", StringComparison.InvariantCultureIgnoreCase)) return false;
                  if (m.FullName.ToEmptyString().StartsWith("content-", StringComparison.InvariantCultureIgnoreCase)) return false;
                  return true;
              };
            var headers = headerMembers.MemberCollection.OfType<SimpleMember>().Where(checkContentHeader).ToList();
            foreach (var m in headers.Where(x => !x.AllowMultiple))
            {
                code.AppendLine($@"m_client.DefaultRequestHeaders.Add(""{m.FullName}"", $""{{rqh.{m.Name}}}"");");
            }
            foreach (var m in headers.Where(x => x.AllowMultiple))
            {
                code.AppendLine($@"m_client.DefaultRequestHeaders.Add(""{m.FullName}"", rqh.{m.Name}).Select(x => $""{{x}}""));");
            }
            if (!string.IsNullOrWhiteSpace(adapter.AuthenticationType))
            {
                code.AppendLine($@"var token = {adapter.DefaultValue.GenerateCode()};");
                code.AppendLine($@"m_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(""{adapter.AuthenticationType}"", token);");
            }


            return code.ToString();
        }

        protected string GenerateRouteCode()
        {
            var routeParameterMembers = this.RequestMemberCollection.FirstOrDefault(x => x.Name.EndsWith("RouteParameters"));
            var routes = routeParameterMembers?.MemberCollection.OfType<RouteParameterMember>();
            return null == routes ? string.Empty : routes.ToString("/", x => $@"{{request.Routes.{x.Name}{(string.IsNullOrWhiteSpace(x.Converter) ? "" : ":" + x.Converter)}}}");
        }

        protected string GenerateRequestQueryStringsCode(ref string opUri)
        {
            var qsMember = this.RequestMemberCollection.FirstOrDefault(x => x.Name.EndsWith("QueryStrings"));
            if (null == qsMember)
                return string.Empty;

            var code = new StringBuilder("var qs = request.QueryStrings;");
            var queryString = new StringBuilder();
            var nonNullableQueryStrings = qsMember.MemberCollection.OfType<SimpleMember>().Where(x => !x.IsNullable).ToArray();
            var nullableQueryString = qsMember.MemberCollection.OfType<SimpleMember>().Where(x => x.IsNullable);


            queryString.JoinAndAppend(nonNullableQueryStrings, "&", s => $"{s.FullName}={{qs.{s.Name}}}");
            if (nonNullableQueryStrings.Any())
                queryString.Append("&");
            queryString.JoinAndAppend(nullableQueryString, "", s => $@"{{(qs.{s.Name}.HasValue ? $@""&{s.FullName}={{qs.{s.Name}}}"" :"""" )}}");

            if (queryString.Length > 0)
            {

                opUri += "?" + queryString;
            }

            return code.ToString();
        }

        protected string GenerateProcessHttpResponseCode()
        {
            var code = new StringBuilder();
            code.AppendLine($"var result = new {MethodName}Response();");

            code.AppendLine("           result.StatusCode = (int)response.StatusCode;");
            code.AppendLine("           result.StatusText = response.StatusCode.ToString();");
            code.AppendLine("           var content2 = response.Content as StreamContent;");
            code.AppendLine("           if(null == content2) throw new Exception(\"Fail to read from response\");");
            // response value
            foreach (var m in this.ResponseMemberCollection.OfType<SimpleMember>().Where(x => x.Name.StartsWith("Content") && x.Type == typeof(string) && !x.AllowMultiple))
            {
                code.AppendLine($@"      result.{m.Name} = content2.Headers.{m.Name}.ToEmptyString();");
            }
            foreach (var m in this.ResponseMemberCollection.OfType<SimpleMember>().Where(x => x.Name.StartsWith("Content") && x.Type == typeof(string) && x.AllowMultiple))
            {
                code.AppendLine($@"      result.{m.Name}.AddRange(content2.Headers.{m.Name});");
            }
            code.AppendLine("           if(content2.Headers.Expires.HasValue)");
            code.AppendLine("              result.Expires =  content2.Headers.Expires.Value.DateTime;");
            code.AppendLine("           if(content2.Headers.LastModified.HasValue)");
            code.AppendLine("              result.LastModified = content2.Headers.LastModified.Value.DateTime;");
            code.AppendLine();


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


            code.AppendLine("         var text = await content2.ReadAsStringAsync();");
            code.AppendLine(@"        if(!response.IsSuccessStatusCode)");
            code.AppendLine(@"        {");
            code.AppendLine($@"            ObjectBuilder.GetObject<ILogger>().LogAsync(new LogEntry{{ 
                                                            Message = $""{{(int)response.StatusCode}} {HttpMethod} {{m_client.BaseAddress}}/{{url}}"",
                                                            Severity = Severity.Warning,
                                                            Log = EventLog.Subscribers,
                                                            Source = ""{Name}"",
                                                            Details = text + ""\r\n"" + JsonConvert.SerializeObject(request)
                                                        }});");
            code.AppendLine(@"            return result;");
            code.AppendLine(@"        }");
            code.AppendLine(@"        else");
            code.AppendLine(@"        {");
            code.AppendLine($@"            ObjectBuilder.GetObject<ILogger>().LogAsync(new LogEntry{{ 
                                                            Message = $""{{(int)response.StatusCode}} {HttpMethod} {{m_client.BaseAddress}}/{{url}}"",
                                                            Severity = Severity.Log,
                                                            Log = EventLog.Subscribers,
                                                            Source = ""{Name}"",
                                                            Details = text
                                                        }});");
            code.AppendLine(@"        }");
            code.AppendLine($"        result.Body = JsonConvert.DeserializeObject<{MethodName}ResponseBody>(text);");

            code.AppendLine("return result;");
            return code.ToString();
        }
    }
}