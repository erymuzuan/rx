using System;
using System.ComponentModel.Composition;
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
                $@"                 ""from"": {size * (page - 1)}," +
                $@"                 ""size"": {size}" +
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
                    var list
");
                return code.ToString();
            }

            code.Append(@"
            var list = from f in jo.SelectToken(""$.hits.hits"")
                        let fields = f.SelectToken(""fields"")
                        let id = f.SelectToken(""_id"").Value<string>()
                        select new {");
            foreach (var g in this.MemberCollection)
            {
                var mb = ed.GetMember(g) as SimpleMember;
                if (null == mb) throw new InvalidOperationException("You can only select SimpleMember field, and " + g + " is not");
                code.AppendLine(
                    mb.Type == typeof (string)
                        ? $"      {g} = fields[\"{g}\"] != null ? fields[\"{g}\"].First.Value<string>() : null,"
                        : $"      {g} = fields[\"{g}\"] != null ? fields[\"{g}\"].First.Value<{mb.Type.ToCSharp()}>() : new Nullable<{mb.Type.ToCSharp()}>(),");
            }


            code.Append($@"
                            links = new {{
                                href = $""{{ConfigurationManager.BaseUrl}}/{Entity.ToLowerInvariant()}/{{id}}""
}}
                        }};
");
            return code.ToString();
        }
    }
}