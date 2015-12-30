using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class HttpOperationDefinition : OperationDefinition
    {
        [ImportMany(typeof(IHarProcessor))]
        [JsonIgnore]
        [XmlIgnore]
        public IHarProcessor[] HarProcessors { get; set; }

        public HttpOperationDefinition()
        {

        }

        public HttpOperationDefinition(JToken jt)
        {
            if (null == this.HarProcessors)
                ObjectBuilder.ComposeMefCatalog(this);

            if (null == this.HarProcessors)
                throw new Exception("Cannot do MEF for HarProcessors");

            var mt = jt.SelectToken("response.content.mimeType");
            if (null != mt)
                this.ResponseMimeType = mt.Value<string>();
            foreach (var harProcessor in this.HarProcessors)
            {
                if (harProcessor.CanProcess(this, jt))
                    harProcessor.Process(this, jt);
            }
        }


        public static readonly string[] ImportDirectives =
        {


           typeof(Entity).Namespace,
           typeof(Int32).Namespace ,
           typeof(Task<>).Namespace ,
           typeof(Enumerable).Namespace,
           typeof(JsonConvert).Namespace,
           typeof(CamelCasePropertyNamesContractResolver).Namespace ,
           typeof(StringEnumConverter).Namespace,
           typeof(XmlAttributeAttribute).Namespace ,
            "System.Web.Http",
            "System.Net",
            "System.Net.Http"

        };

        public override IEnumerable<Class> GenerateResponseCode()
        {

            var responseTypeName = (this.HttpMethod + "_" + this.Name).ToCsharpIdentitfier() + "Response";
            var @class = new Class { Name = responseTypeName, BaseClass = nameof(DomainObject), Namespace = CodeNamespace };
            @class.ImportCollection.AddRange(ImportDirectives);
            var sources = new ObjectCollection<Class> { @class };
            @class.AddProperty("       [JsonIgnore]public string ResponseText{ get; private set;}");


            var loadAsync = new Method { ReturnType = typeof(Task), Name = "LoadAsync" };
            loadAsync.ArgumentCollection.Add(new MethodArg { Name = "respopnse", Type = typeof(HttpResponseMessage) });


            loadAsync.AppendLine("           var content = response.Content as StreamContent;");
            loadAsync.AppendLine("           if(null == content) throw new Exception(\"Fail to read from response\");");
            loadAsync.AppendLine("           this.ResponseText = await content.ReadAsStringAsync();");

            foreach (var m in this.ResponseMemberCollection.OfType<RegexMember>().Where(m => !string.IsNullOrWhiteSpace(m.Group) && !string.IsNullOrWhiteSpace(m.Pattern)))
            {
                loadAsync.AppendLine(m.GenerateParseCode("this"));
            }
            //objects
            foreach (var m in this.ResponseMemberCollection.OfType<RegexMember>().Where(m => m.Type == typeof(object)))
            {
                var m1 = m;
                loadAsync.AppendLine($"           //{m1.Name}");
                loadAsync.AppendLine($"           this.{m1.Name} = new {m1.Name}();");
                var objectsChildren = from mc in m.MemberCollection.OfType<RegexMember>()
                                      select mc.GenerateParseCode("this." + m1.Name);
                objectsChildren.ToList().ForEach(x => loadAsync.AppendLine(x));

            }

            // array
            var count = 1;
            foreach (var m in this.ResponseMemberCollection.OfType<RegexMember>().Where(m => m.Type == typeof(Array)))
            {
                var count1 = count;
                var first = m.MemberCollection.OfType<RegexMember>().FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Group) && !string.IsNullOrWhiteSpace(x.Pattern));
                if (null == first) continue;

                loadAsync.AppendLine("           // " + m.Name);
                loadAsync.AppendLine($"           var values{count} = Strings.RegexValues(this.ResponseText, {first.Pattern.ToLiteral()}, \"{first.Group}\");");
                var cname = m.Name.Replace("Collection", "");
                loadAsync.AppendLine($"           var colls{count} = new ObjectCollection<{cname}>(Enumerable.Range(0,values{count}.Length).Select(x => new {cname}() ));");


                loadAsync.AppendLine($"           for(int i = 0 ; i < values{count}.Length; i++)");
                loadAsync.AppendLine("           {");

                var collectionChildrenCodes = from cm in m.MemberCollection.OfType<RegexMember>()
                                              where !string.IsNullOrWhiteSpace(cm.Group)
                                                    && !string.IsNullOrWhiteSpace(cm.Pattern)
                                              select cm.GenerateParseCode("   colls" + count1 + "[i]");
                collectionChildrenCodes.ToList().ForEach(x => loadAsync.AppendLine(x));


                loadAsync.AppendLine("           }");
                loadAsync.AppendLine($"           this.{m.Name}.AddRange(colls{count});");
                count++;
            }

            // for ajax request
            if (this.ResponseMimeType == "application/json; charset=utf-8")
            {
                loadAsync.AppendLine();
                if (this.ResponseIsJsonArray)
                    loadAsync.AppendLine($"           var result = JsonConvert.DeserializeObject<{responseTypeName}>(\"{{ \\\"list\\\" :\" + this.ResponseText + \"}}\");");
                else
                    loadAsync.AppendLine($"           var result = JsonConvert.DeserializeObject<{responseTypeName}>(this.ResponseText);");
                foreach (var member in this.ResponseMemberCollection)
                {
                    // TODO : lots more stuff here
                    loadAsync.AppendLine(
                       /* member.Type == typeof(Array)
                            ? $"           this.{member.Name}.ClearAndAddRange(result.{member.Name});"
                            :*/ $"           this.{member.Name} = result.{member.Name};");
                }
            }

            @class.MethodCollection.Add(loadAsync);
            var properties = this.ResponseMemberCollection.Select(x => new Property {Code = x.GeneratedCode("    ")});
            @class.PropertyCollection.AddRange(properties);



            var childrenClasses = this.ResponseMemberCollection.Select(
                x => x.GeneratedCustomClass(CodeNamespace, ImportDirectives))
                .SelectMany(x => x.ToArray());
            sources.AddRange(childrenClasses);
            
            return sources;
        }

        public override IEnumerable<Class> GenerateRequestCode()
        {

            var typeName = (this.HttpMethod + "_" + this.Name).ToCsharpIdentitfier() + "Request";
            var code = new Class { Name = typeName, Namespace = CodeNamespace, BaseClass = nameof(DomainObject) };
            code.ImportCollection.AddRange(ImportDirectives);
            var sources = new ObjectCollection<Class> { code };

            if (!string.IsNullOrWhiteSpace(this.RequestRouting))
            {
                var body = $"           return \"" + this.RequestRouting.Replace("{", "\" + ").Replace("}", ".EscapeUriString() + \"") + "\";";
                var generateUrl = new Method { Name = "GenerateUrl", AccessModifier = Modifier.Public, ReturnType = typeof(string), Body = body };
                generateUrl.ArgumentCollection.Add(new MethodArg { Name = "url", Type = typeof(string) });
                code.MethodCollection.Add(generateUrl);
            }


            var postData = new StringBuilder();
            postData.AppendLine("       [JsonIgnore]");
            postData.AppendLine("       [XmlIgnore]");
            postData.AppendLine("       public string PostData");
            postData.AppendLine("       {");
            postData.AppendLine("           get");
            postData.AppendLine("           {");
            postData.AppendLine(PostDataCodeGenerator.Create(this).GenerateCode(this));
            postData.AppendLine("           }");
            postData.AppendLine("       }");
            code.AddProperty(postData.ToString());

            var members = this.RequestMemberCollection.Select(m => m.GeneratedCode(" "))
                .Select(m => new Property { Code = m });
            code.PropertyCollection.AddRange(members);


            var childrenClasses = this.RequestMemberCollection.Select(
                x => x.GeneratedCustomClass(CodeNamespace, ImportDirectives))
                .SelectMany(x => x.ToArray());
            sources.AddRange(childrenClasses);


            return sources;
        }


        public string GenerateActionCode(HttpAdapter adapter, string methodName)
        {
            var code = new StringBuilder();
            var op = this;
            code.AppendLine(CreateMethodCode(adapter, methodName, op));
            code.AppendLine(OperationLoginCode(op));
            code.AppendLine("           var client = m_client;");

            code.AppendLine(!string.IsNullOrWhiteSpace(op.RequestRouting)
                ? "           var url = request.GenerateUrl(REQUEST_URL);"
                : "           var url = REQUEST_URL;");

            var sendCode = SendCode.Create(op).GenerateCode(op);


            foreach (var c in sendCode.Split(new[] { Environment.NewLine, "\r\n", "\n" }, StringSplitOptions.None))
            {
                code.AppendLine("           " + c);
            }
            // custom headers
            var automaticHeaders = new[] { "content-type", "content-length", "cookie", "accept-encoding" };
            foreach (var hd in this.RequestHeaderDefinitionCollection)
            {
                if (automaticHeaders.Contains(hd.Name.ToLowerInvariant())) continue;
                code.AppendLinf("           requestMessage.Headers.Add(\"{0}\", \"{1}\");", hd.Name, hd.Field.GetValue(null));
            }

            code.AppendLine("           var response = await client.SendAsync(requestMessage);");
            code.AppendLine(CreateResponseCode(op, methodName));
            code.AppendLine("       }");

            return code.ToString();
        }


        private string CreateMethodCode(HttpAdapter adapter, string methodName, HttpOperationDefinition op)
        {
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<{0}Response> {0}Async({0}Request request)",
                methodName.ToCsharpIdentitfier());
            code.AppendLine("       {");
            code.AppendLinf("           const string REQUEST_URL = \"{0}\";", op.Url.Replace(adapter.BaseAddress, ""));

            return code.ToString();
        }

        private string CreateResponseCode(HttpOperationDefinition op, string methodName)
        {
            var code = new StringBuilder();
            if (op.EnsureSuccessStatusCode)
                code.AppendLine("           response.EnsureSuccessStatusCode();");

            code.AppendLine("           if(response.IsSuccessStatusCode)");
            code.AppendLine("           {");

            if (op.IsLoginOperation)
            {
                code.AppendLine("           this.IsAuthenticated = true;// TODO - verify with the response");
            }

            code.AppendLinf("               var result =  new {0}Response();", methodName.ToCsharpIdentitfier());
            code.AppendLine("               await result.LoadAsync(response);");
            code.AppendLine("               return result;");
            code.AppendLine("           }");
            code.AppendLine("           return null;");

            return code.ToString();
        }

        private string OperationLoginCode(HttpOperationDefinition op)
        {
            if (op.IsLoginRequired)
            {
                return
                    "           " +
                    "if(!this.IsAuthenticated)" +
                    " throw new InvalidOperationException(\"You must be logged in\");";
            }
            return string.Empty;
        }
    }
}
