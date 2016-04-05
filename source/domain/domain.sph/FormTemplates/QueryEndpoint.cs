using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [StoreAsSource(HasDerivedTypes = true)]
    public partial class QueryEndpoint : Entity, IEntityDefinitionAsset
    {

        [ImportMany(typeof(IBuildDiagnostics))]
        [JsonIgnore]
        [XmlIgnore]
        public IBuildDiagnostics[] BuildDiagnostics { get; set; }

        public async Task<BuildValidationResult> ValidateBuildAsync(EntityDefinition ed)
        {
            if (null == this.BuildDiagnostics)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.BuildDiagnostics)
                throw new InvalidOperationException($"Fail to initialize MEF for {nameof(QueryEndpoint)}.BuildDiagnostics");

            var result = new BuildValidationResult();

            if (null == ed)
                result.Errors.Add(new BuildError(this.WebId, $"Cannot find EntityDefinition:{Entity}"));

            var errorTasks = this.BuildDiagnostics.Select(d => d.ValidateErrorsAsync(this, ed));
            var errors = (await Task.WhenAll(errorTasks)).SelectMany(x => x.ToArray());

            var warningTasks = this.BuildDiagnostics.Select(d => d.ValidateWarningsAsync(this, ed));
            var warnings = (await Task.WhenAll(warningTasks)).SelectMany(x => x.ToArray());

            result.Errors.AddRange(errors);
            result.Warnings.AddRange(warnings);


            result.Result = result.Errors.Count == 0;

            return result;
        }

        [JsonIgnore]
        public string Icon => "fa fa-cloud-download";
        [JsonIgnore]
        public string Url => $"query.endpoint.designer/{Id}";

        public string GenerateEsSortDsl()
        {
            var f = from s in this.SortCollection
                    select $"{{\"{s.Path}\":{{\"order\":\"{s.Direction.ToString().ToLowerInvariant()}\"}}}}";
            return "[" + string.Join(",\r\n", f.ToArray()) + "]";
        }

        public void AddFilter(string term, Operator @operator, Field field)
        {
            this.FilterCollection.Add(new Filter { Field = field, Operator = @operator, Term = term });
        }

        public Task<QueryEndpointSetting> LoadSettingAsync()
        {
            var cacheManager = ObjectBuilder.GetObject<ICacheManager>();
            var key = $"entity-query:setting:{Id}";
            var setting = cacheManager.Get<QueryEndpointSetting>(key);
            if (null != setting) return Task.FromResult(setting);

            var source = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(QueryEndpoint)}\\{this.Id}.setting.json";
            if (File.Exists(source))
            {
                setting = File.ReadAllText(source).DeserializeFromJson<QueryEndpointSetting>();
            }
            else
            {
                setting = new QueryEndpointSetting
                {
                    CacheProfile = this.CacheProfile,
                    CacheFilter = this.CacheFilter,
                };
            }
            cacheManager.Insert(key, setting, source);

            return Task.FromResult(setting);
        }
        public Task SaveSetttingAsync(QueryEndpointSetting setting)
        {
            var cacheManager = ObjectBuilder.GetObject<ICacheManager>();
            var key = $"entity-query:setting:{Id}";
            var source = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(QueryEndpoint)}\\{this.Id}.setting.json";
            File.WriteAllText(source, setting.ToJsonString(true));
            cacheManager.Insert(key, setting, source);

            return Task.FromResult(0);
        }

        public Task<string> GenerateEsQueryAsync()
        {
            var filter = Filter.GenerateElasticSearchFilterDsl(this, this.FilterCollection);
             var max = @",
    ""aggs"" : {
        ""filtered_max_date"" : { 
              ""filter"" : " + filter + @",
              ""aggs"": {
                        ""last_changed_date"": {
                           ""max"": {
                              ""field"": ""ChangedDate""
                            }
                        }
               }
        }
    }
";
            var query =
                @"{
                    ""filter"":" + filter +
                @"  
" + this.GetFields() + max + @" 

    }";

            return Task.FromResult(query);

        }

        private string GetFields()
        {
            if (!this.MemberCollection.Any()) return string.Empty;
            var fields = $@",
                ""fields"" :[ { string.Join(",", this.MemberCollection.Select(x => $"\"{x}\""))}]";
            return fields;
        }

        public string GenerateListCode()
        {
            var code = new StringBuilder();
            if (!this.MemberCollection.Any())
            {
                code.Append(@" 
                    var list = from f in json.SelectToken(""$.hits.hits"")
                               let webId = f.SelectToken(""_source.WebId"").Value<string>()
                               let id = f.SelectToken(""_id"").Value<string>()
                               let link = $""\""link\"" :{{ \""href\"" :\""{ConfigurationManager.BaseUrl}/api/" + this.Resource + @"/{id}\""}}""
                               select f.SelectToken(""_source"").ToString().Replace($""{webId}\"""",$""{webId}\"","" + link);
");
                return code.ToString();
            }

            code.Append(@"
            var list = from f in json.SelectToken(""$.hits.hits"")
                        let fields = f.SelectToken(""fields"")
                        let id = f.SelectToken(""_id"").Value<string>()
                        select JsonConvert.SerializeObject( new {");
            foreach (var g in this.MemberCollection.Where(x => !x.Contains(".")))
            {
                var mb = m_ed.GetMember(g) as SimpleMember;
                if (null == mb) throw new InvalidOperationException("You can only select SimpleMember field, and " + g + " is not");
                code.AppendLine(
                    mb.Type == typeof(string)
                        ? $"      {g} = fields[\"{g}\"] != null ? fields[\"{g}\"].First.Value<string>() : null,"
                        : $"      {g} = fields[\"{g}\"] != null ? fields[\"{g}\"].First.Value<{mb.Type.ToCSharp()}>() : new Nullable<{mb.Type.ToCSharp()}>(),");
            }
            code.Append(this.GenerateComplexMemberFields(this.MemberCollection.ToArray()));

            code.Append($@"
                            _links = new {{
                                rel = ""self"",
                                href = $""{{ConfigurationManager.BaseUrl}}/api/{Resource}/{{id}}""
                            }}
                        }});
");
            return code.ToString();
        }

        private string GenerateComplexMemberFields(string[] members)
        {
            var code = new StringBuilder();
            var parent = "";
            var complexFields = members.Where(x => x.Contains(".")).OrderBy(x => x).ToList();
            foreach (var g in complexFields)
            {
                var mb = m_ed.GetMember(g) as SimpleMember;
                if (null == mb) throw new InvalidOperationException("You can only select SimpleMember field, and " + g + " is not");
                var paths = g.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                //if (paths.Count > 2)
                //{
                //    paths.RemoveAt(paths.Count - 1);
                //    var similiar = members.Where(x => x.StartsWith(string.Join(".", paths))).ToArray();
                //    code.Append(this.GenerateComplexMemberFields(similiar));
                //    continue;
                //}

                var cp = paths.First();
                var m = paths.Last();
                if (!string.IsNullOrWhiteSpace(parent) && parent != cp) code.AppendLine("     },");
                if (parent != cp)
                {
                    code.AppendLine($"          {cp} = new {{");
                }
                code.AppendLine(
                    mb.Type == typeof(string)
                        ? $"      {m} = fields[\"{g}\"] != null ? fields[\"{g}\"].First.Value<string>() : null,"
                        : $"      {m} = fields[\"{g}\"] != null ? fields[\"{g}\"].First.Value<{mb.Type.ToCSharp()}>() : new Nullable<{mb.Type.ToCSharp()}>(),");

                parent = cp;
            }
            if (complexFields.Count > 0) code.AppendLine("     },");

            return code.ToString();
        }
    }
}