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
    public partial class EntityQuery : Entity, IEntityDefinitionAsset
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
                throw new InvalidOperationException($"Fail to initialize MEF for EntityQuery.BuildDiagnostics");

            var result = new BuildValidationResult();
            var errorTasks = this.BuildDiagnostics.Select(d => d.ValidateErrorsAsync(this, ed));
            var errors = (await Task.WhenAll(errorTasks)).SelectMany(x => x.ToArray());

            var warningTasks = this.BuildDiagnostics.Select(d => d.ValidateWarningsAsync(this, ed));
            var warnings = (await Task.WhenAll(warningTasks)).SelectMany(x => x.ToArray());

            result.Errors.AddRange(errors);
            result.Warnings.AddRange(warnings);


            result.Result = result.Errors.Count == 0;

            return result;
        }

        public string Icon => "fa fa-search";
        public string Url => $"entity.query.designer/{Id}";

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

        public Task<EntityQuerySetting> LoadSettingAsync()
        {
            var cacheManager = ObjectBuilder.GetObject<ICacheManager>();
            var key = $"entity-query:setting:{Id}";
            var setting = cacheManager.Get<EntityQuerySetting>(key);
            if (null != setting) return Task.FromResult(setting);

            var source = $"{ConfigurationManager.SphSourceDirectory}\\EntityQuery\\{this.Id}.setting.json";
            if (File.Exists(source))
            {
                setting = File.ReadAllText(source).DeserializeFromJson<EntityQuerySetting>();
            }
            else
            {
                setting = new EntityQuerySetting
                {
                    CacheProfile = this.CacheProfile,
                    CacheFilter = this.CacheFilter,
                    Performer =  this.Performer
                };
            }
            cacheManager.Insert(key, setting, source);

            return Task.FromResult(setting);
        }
        public Task SaveSetttingAsync(EntityQuerySetting setting)
        {
            var cacheManager = ObjectBuilder.GetObject<ICacheManager>();
            var key = $"entity-query:setting:{Id}";
            var source = $"{ConfigurationManager.SphSourceDirectory}\\EntityQuery\\{this.Id}.setting.json";
            File.WriteAllText(source,setting.ToJsonString(true));
            cacheManager.Insert(key, setting, source);

            return Task.FromResult(0);
        }

        public Task<string> GenerateEsQueryAsync(int page = 1, int size = 20)
        {
            var query =
                @"{
                ""query"": {
                    ""filtered"": {
                        ""filter"":" +
                Filter.GenerateElasticSearchFilterDsl(this, this.FilterCollection) + @"
                    }
                }," +
                $@"                 ""from"": <<page-from>>," +
                $@"                 ""size"": <<page-size>>" +
                @"  
" + this.GetFields() + @" 

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

        public string GenerateListCode(EntityDefinition ed)
        {
            var code = new StringBuilder();
            if (!this.MemberCollection.Any())
            {
                code.Append(@" 
                    var list = from f in esJsonObject.SelectToken(""$.hits.hits"")
                               let webId = f.SelectToken(""_source.WebId"").Value<string>()
                               let id = f.SelectToken(""_id"").Value<string>()
                               let link = $""\""link\"" :{{ \""href\"" :\""{ConfigurationManager.BaseUrl}/api/" + this.Resource + @"/{id}\""}}""
                               select f.SelectToken(""_source"").ToString().Replace($""{webId}\"""",$""{webId}\"","" + link);
");
                return code.ToString();
            }

            code.Append(@"
            var list = from f in esJsonObject.SelectToken(""$.hits.hits"")
                        let fields = f.SelectToken(""fields"")
                        let id = f.SelectToken(""_id"").Value<string>()
                        select JsonConvert.SerializeObject( new {");
            foreach (var g in this.MemberCollection)
            {
                var mb = ed.GetMember(g) as SimpleMember;
                if (null == mb) throw new InvalidOperationException("You can only select SimpleMember field, and " + g + " is not");
                code.AppendLine(
                    mb.Type == typeof(string)
                        ? $"      {g} = fields[\"{g}\"] != null ? fields[\"{g}\"].First.Value<string>() : null,"
                        : $"      {g} = fields[\"{g}\"] != null ? fields[\"{g}\"].First.Value<{mb.Type.ToCSharp()}>() : new Nullable<{mb.Type.ToCSharp()}>(),");
            }


            code.Append($@"
                            _links = new {{
                                self = new {{ href = $""{{ConfigurationManager.BaseUrl}}/api/{Resource}/{{id}}"" }}
                            }}
                        }});
");
            return code.ToString();
        }
    }
}