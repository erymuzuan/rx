using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class RestApiOperationDefinition : OperationDefinition
    {
        public RestApiOperationDefinition()
        {

        }

        public RestApiOperationDefinition(JToken entry)
        {
            var request = entry["request"];
            var response = entry["response"];
            ExtractRequet(request);
            ExtractResponse(response);
        }

        private void ExtractRequet(JToken request)
        {
            if (null == request) return;
            this.BaseAddress = request["url"]?.Value<string>();
            this.HttpMethod = request["method"]?.Value<string>();
            this.HttpVersion = request["httpVersion"]?.Value<string>();
            this.RequestBodySample = request["postData"]?["text"]?.Value<string>();
            this.RequestContentType = request["postData"]?["mimeType"]?.Value<string>();

            foreach (var q in request["queryString"])
            {
                this.QueryStrings.Add(q["name"]?.Value<string>(), q["value"]?.Value<string>());
            }
            foreach (var q in request["headers"])
            {
                this.RequestHeaders.Add(q["name"]?.Value<string>(), q["value"]?.Value<string>());
            }
        }

        private void ExtractResponse(JToken response)
        {
            // response
            if (null == response) return;
            this.ResponseStatusText = response["statusText"]?.Value<string>();
            this.ResponseStatus = response["status"]?.Value<int>();
            this.ResponseBodySize = response["bodySize"]?.Value<long>();
            this.ResponseBodySample = response["content"]?["text"]?.Value<string>();
            this.ResponseContentType = response["content"]?["mimeType"]?.Value<string>();


            foreach (var q in response["headers"])
            {
                this.ResponseHeaders.Add(q["name"]?.Value<string>(), q["value"]?.Value<string>());
            }
        }

        public string HarStoreId { get; set; }
        public string RequestHeaderSample { get; set; }
        public string RequestBodySample { get; set; }
        public string RequestContentType { get; set; }
        public string ResponseHeaderSample { get; set; }
        public string ResponseBodySample { get; set; }
        public string ResponseContentType { get; set; }
        public string BaseAddress { get; set; }
        public string HttpMethod { get; set; }
        public string Route { get; set; }
        public string HttpVersion { get; set; }

        public IDictionary<string, string> QueryStrings { get; } = new Dictionary<string, string>();
        public IDictionary<string, string> RequestHeaders { get; } = new Dictionary<string, string>();
        public IDictionary<string, string> ResponseHeaders { get; } = new Dictionary<string, string>();
        public string ResponseStatusText { get; set; }
        public int? ResponseStatus { get; set; }
        public long? ResponseBodySize { get; set; }

        protected override string GenerateAdapterActionBody(Adapter adapter)
        {
            throw new NotImplementedException();
        }

        public Task BuildAsync()
        {
            var qms = from q in this.QueryStrings.Keys
                      select new SimpleMember { Name = q.ToPascalCase(), Type = typeof(string) };
            var hms = from q in this.RequestHeaders.Keys
                      select new SimpleMember { Name = q.ToPascalCase(), Type = typeof(string) };

            var querySrings = new ComplexMember { Name = "QueryStrings", AllowMultiple = false, TypeName = "QueryString" };
            querySrings.MemberCollection.AddRange(qms);

            var requestHeaders = new ComplexMember { Name = "Headers", TypeName = "RequestHeader", AllowMultiple = false };
            requestHeaders.MemberCollection.AddRange(hms);

            this.RequestMemberCollection.Clear();
            this.RequestMemberCollection.AddRange(querySrings, requestHeaders);
            if (!string.IsNullOrWhiteSpace(this.RequestBodySample))
            {

                var requestContent = JObject.Parse(this.RequestBodySample);
                var requestBodyMembers = from j in requestContent.Children()
                                         select GetContentMember((JProperty)j);

                var requestBody = new ComplexMember { Name = "Body", TypeName = "RequestBody" };
                requestBody.MemberCollection.AddRange(requestBodyMembers.Where(x => null != x));
                this.RequestMemberCollection.Add(requestBody);
            }
            
            var responseHeaderMembers = from k in this.ResponseHeaders.Keys
                                        select new SimpleMember { Name = k.ToPascalCase(), Type = typeof(string), PropertyAttribute = $@"[JsonProperty(""{k}"")]" };
            var responseHeader = new ComplexMember { AllowMultiple = false, Name = "Headers", TypeName = "Header" };
            responseHeader.MemberCollection.AddRange(responseHeaderMembers);
            var responseBody = new ComplexMember { Name = "Body", AllowMultiple = false };
            var json = JObject.Parse(this.ResponseBodySample);
            var members = from j in json.Children()
                          select GetContentMember((JProperty)j);
            responseBody.MemberCollection.AddRange(members.Where(x => null != x));
            // response
            this.ResponseMemberCollection.Clear();
            this.ResponseMemberCollection.Add(responseHeader);
            this.ResponseMemberCollection.Add(responseBody);

            this.Name = this.HttpMethod.ToLower().ToPascalCase() + "1";
            this.MethodName = this.HttpMethod.ToLower().ToPascalCase() + "1";
            this.Schema = "";
            this.WebId = Guid.NewGuid().ToString();
            this.Uuid = this.WebId;


            return Task.FromResult(0);
        }

        private Member GetContentMember(JProperty jp)
        {
            if (!jp.HasValues) return null;
            if (jp.Value.GetType() == typeof(JArray))
            {
                var ja = (JArray)jp.Value;
                var member = new ComplexMember { Name = jp.Name.ToPascalCase(), TypeName = $"{jp.Name.ToPascalCase()}Item", AllowMultiple = true, PropertyAttribute = $@"[JsonProperty(""{jp.Name}"")]" };
                // recurse
                var fr = (JObject)ja.First();
                var chilren = from f in fr.Children()
                              select GetContentMember((JProperty)f);
                member.MemberCollection.AddRange(chilren);
                return member;
            }
            if (jp.Value.GetType() == typeof(JObject))
            {
                var jo = (JObject)jp.Value;
                var member = new ComplexMember { Name = jp.Name.ToPascalCase(), TypeName = $"{jp.Name.ToPascalCase()}Item", AllowMultiple = false, PropertyAttribute = $@"[JsonProperty(""{jp.Name}"")]" };
                // recurse
                var chilren = from f in jo.Children()
                              select GetContentMember((JProperty)f);
                member.MemberCollection.AddRange(chilren);
                return member;
            }
            if (jp.Value.GetType() == typeof(JValue))
            {
                var jv = (JValue)jp.Value;
                var type = typeof(string);
                switch (jv.Type)
                {
                    case JTokenType.Date:
                        type = typeof(DateTime);
                        break;
                    case JTokenType.Integer:
                        type = typeof(int);
                        break;
                    case JTokenType.Boolean:
                        type = typeof(bool);
                        break;
                    case JTokenType.Float:
                        type = typeof(decimal);
                        break;
                }
                return new SimpleMember { Name = jp.Name.ToPascalCase(), PropertyAttribute = $@"[JsonProperty(""{jp.Name}"")]", Type = type };

            }
            throw new Exception($"Cannot determin type for '{jp.Value.GetType().Namespace}' for '{jp.Name}'");
        }
    }
}
