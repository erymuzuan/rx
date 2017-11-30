using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [PersistenceOption(HasDerivedTypes = true, IsSource = true)]
    public partial class QueryEndpoint : Entity, IProjectDefinition
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

            var filterErrorTasks = this.FilterCollection.Select(x => x.ValidateErrorsAsync());
            var filterErrors = (await Task.WhenAll(filterErrorTasks)).SelectMany(x => x.ToArray());
            result.Errors.AddRange(filterErrors);

            var filterWarningTasks = this.FilterCollection.Select(x => x.ValidateWarningsAsync());
            var filterWarnings = (await Task.WhenAll(filterWarningTasks)).SelectMany(x => x.ToArray());
            result.Warnings.AddRange(filterWarnings);

            // route
            var routeParameters = Strings.RegexValues(this.Route, "\\{(?<p>.*?)}", "p");
            foreach (var pr in routeParameters)
            {
                var filter = this.FilterCollection.Select(x => x.Field).OfType<RouteParameterField>()
                    .SingleOrDefault(x => x.Name == pr);
                if (null == filter)
                    result.Warnings.Add(new BuildError(this.WebId, $@"You should define a filter with RouteParameterField for ""{pr}"" route parameter "));
            }

            result.Result = result.Errors.Count == 0;

            return result;
        }

        [Obsolete("Move to elasticsearch implementation")]
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

            var source = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(QueryEndpoint)}\\{this.Id}.setting";
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
                    CachingSetting = new CachingSetting
                    {
                        CacheControl = this.CacheProfile
                    }
                };
                File.WriteAllText(source, setting.ToJsonString());
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

    }
}