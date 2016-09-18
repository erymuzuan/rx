using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    [Export(typeof(IEndpointsBuilder))]
    public partial class HarEndpointsBuilder : IEndpointsBuilder
    {

        public HarEndpointsBuilder()
        {

        }


        public HarEndpointsBuilder(JToken x)
        {
            var request = x["request"];
            var response = x["response"];
            ExtractRequet(request);
            ExtractResponse(response);
        }

        public string ResponseHeaderSample { get; set; }
        public string ResponseContentType { get; set; }

        public Dictionary<string, string> QueryStrings { get; } = new Dictionary<string, string>();
        public Dictionary<string, string> RequestHeaders { get; } = new Dictionary<string, string>();
        public Dictionary<string, string> ResponseHeaders { get; } = new Dictionary<string, string>();
        public string ResponseStatusText { get; set; }
        public int? ResponseStatus { get; set; }
        public long? ResponseBodySize { get; set; }


        public string BaseAddress { get; set; }
        public string HttpMethod { get; set; }
        public string Route { get; set; }


        public static Encoding GetFileEncoding(string srcFile)
        {
            // *** Use Default of Encoding.Default (Ansi CodePage)
            Encoding enc = Encoding.Default;

            // *** Detect byte order mark if any - otherwise assume default
            byte[] buffer = new byte[5];
            FileStream file = new FileStream(srcFile, FileMode.Open);
            file.Read(buffer, 0, 5);
            file.Close();

            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                enc = Encoding.UTF8;
            else if (buffer[0] == 0xfe && buffer[1] == 0xff)
                enc = Encoding.Unicode;
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                enc = Encoding.UTF32;
            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
                enc = Encoding.UTF7;
            else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                // 1201 unicodeFFFE Unicode (Big-Endian)
                enc = Encoding.GetEncoding(1201);
            else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                // 1200 utf-16 Unicode
                enc = Encoding.GetEncoding(1200);


            return enc;
        }

        public async Task<RestApiOperationDefinition> BuildAsync()
        {
            var op = new RestApiOperationDefinition(this)
            {
                BaseAddress = this.BaseAddress,
                HttpMethod = this.HttpMethod
            };
            var requestWithoutBodies = new[] { "GET", "HEAD", "OPTIONS", "DELETE" };
            if (requestWithoutBodies.Contains(this.HttpMethod.ToUpperInvariant()))
            {
                op = new RequestWithoutBodyApiOperationDefinition(this)
                {
                    BaseAddress = this.BaseAddress,
                    HttpMethod = this.HttpMethod
                };
            }

            await op.BuildAsync();
            return op;
        }

        public async Task<IEnumerable<IEndpointsBuilder>> GetBuildersAsync()
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            try
            {
                var har = await store.GetContentAsync(this.StoreId);
                var temp = Path.GetTempFileName();
                File.WriteAllBytes(temp, har.Content);
                var text = File.ReadAllText(temp);
                var json = JObject.Parse(text);
                var entries = json.SelectToken("$.log.entries").ToList();
                return entries.Select(x => new HarEndpointsBuilder(x) { StoreId = this.StoreId });
            }
            catch
            {
                //ignore
            }

            return Array.Empty<IEndpointsBuilder>();
        }

        public Task<IEnumerable<Member>> GetRequestBodyMembersAsync()
        {

            var requestContent = JObject.Parse(this.RequestBodySample);
            var requestBodyMembers = from j in requestContent.Children()
                                     select GetContentMember((JProperty)j);
            return Task.FromResult(requestBodyMembers.Where(x => null != x));
        }

        public Task<IEnumerable<Member>> GetRequestHeaderMembersAsync()
        {
            var hms = from q in this.RequestHeaders.Keys
                      select new SimpleMember
                      {
                          Name = q.ToPascalCase(),
                          FullName = q,
                          Type = typeof(string),
                          DefaultValue = new ConstantField
                          {
                              Name = q,
                              Value = this.RequestHeaders[q],
                              Type = typeof(string),
                              WebId = Guid.NewGuid().ToString()
                          },
                          WebId = Guid.NewGuid().ToString()
                      };
            var members = new List<Member>();
            members.AddRange(hms);
            return Task.FromResult(members.Where(x => null != x));

        }

        public Task<IEnumerable<Member>> GetRequestQueryStringMembersAsync()
        {
            var qms = from q in this.QueryStrings.Keys
                      select new SimpleMember
                      {
                          Name = q.ToPascalCase(),
                          Type = typeof(string),
                          FullName = q,
                          DefaultValue = new ConstantField
                          {
                              Name = q,
                              Value = this.QueryStrings[q],
                              Type = typeof(string),
                              WebId = Guid.NewGuid().ToString()
                          },
                          WebId = Guid.NewGuid().ToString()
                      };
            var members = new List<Member>();
            members.AddRange(qms);
            return Task.FromResult(members.AsEnumerable());
        }

        public Task<IEnumerable<Member>> GetResponseBodyMembersAsync()
        {
            var json = JObject.Parse(this.ResponseBodySample);
            var members = from j in json.Children()
                          select GetContentMember((JProperty)j);
            return Task.FromResult(members.Where(x => null != x));

        }

        public Task<IEnumerable<Member>> GetResponseHeaderMembersAsync()
        {
            var list = new List<Member>();
            var members = from k in this.ResponseHeaders.Keys
                          select new SimpleMember
                          {
                              Name = k.ToPascalCase(),
                              Type = typeof(string),
                              FullName = k,
                              PropertyAttribute = $@"[JsonProperty(""{k}"")]"
                          };
            list.AddRange(members);
            return Task.FromResult(list.Where(x => null != x));

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
                var queryStringName = q["name"]?.Value<string>();
                if (null != queryStringName && !this.QueryStrings.ContainsKey(queryStringName))
                    this.QueryStrings.Add(queryStringName, q["value"]?.Value<string>());
            }
            foreach (var q in request["headers"])
            {
                var headerName = q["name"]?.Value<string>();
                if (null != headerName && !this.RequestHeaders.ContainsKey(headerName))
                    this.RequestHeaders.Add(headerName, q["value"]?.Value<string>());
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
                var name = q["name"]?.Value<string>();
                if (null != name && !this.ResponseHeaders.ContainsKey(name))
                    this.ResponseHeaders.Add(name, q["value"]?.Value<string>());
            }
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
                if (type == typeof(string))
                {
                    type = jv.ToString(CultureInfo.InvariantCulture).TryGuessType();
                }
                return new SimpleMember { Name = jp.Name.ToPascalCase(), PropertyAttribute = $@"[JsonProperty(""{jp.Name}"")]", Type = type };

            }
            throw new Exception($"Cannot determin type for '{jp.Value.GetType().Namespace}' for '{jp.Name}'");
        }
    }
}