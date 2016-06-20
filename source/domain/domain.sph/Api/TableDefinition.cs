using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Domain.Api
{
    public partial class TableDefinition
    {

        private static readonly string[] ImportDirectives =
        {

            typeof(Entity).Namespace ,
            typeof(int).Namespace ,
            typeof(Task<>).Namespace ,
            typeof(Enumerable).Namespace ,
            typeof(JsonConvert).Namespace ,
            typeof(JObject).Namespace ,
            typeof(CamelCasePropertyNamesContractResolver).Namespace ,
            typeof(StringEnumConverter).Namespace ,
            typeof(XmlAttributeAttribute).Namespace ,
            typeof(MediaTypeFormatter).Namespace,
            "System.Web.Http",
            "System.Net",
            "System.Net.Http",
            "Bespoke.Sph.WebApi"
};



        public IEnumerable<Class> GenerateCode(Adapter adapter)
        {
            var adapteClass = new Class { Name = ClrName, BaseClass = nameof(DomainObject), Namespace = this.CodeNamespace };
            adapteClass.AddNamespaceImport<System.DateTime, DomainObject, JsonIgnoreAttribute>();
            var list = new ObjectCollection<Class> { adapteClass };

            if (!string.IsNullOrWhiteSpace(ClassAttribute))
                adapteClass.AttributeCollection.Add(ClassAttribute);


            var pk = "\"\"";
            if (null != this.PrimaryKey)
                pk = this.PrimaryKey.Name;
            var toString = $"public override string ToString(){{ return \"{Name}:\" + {pk};}}";
            adapteClass.MethodCollection.Add(new Method { Code = toString });
            
            var properties = this.ColumnCollection.Select(x => new Property { Code = x.GeneratedCode() }).ToList();
            var lookupProperties = from c in this.ColumnCollection
                                   where c.LookupColumnTable.IsEnabled
                                   select c.GetLookupProperty(adapter, this);
            properties.AddRange(lookupProperties);

            adapteClass.PropertyCollection.AddRange(properties);

            var otherClasses = this.ColumnCollection.Select(
                x => x.GeneratedCustomClass(this.CodeNamespace, ImportDirectives))
                .SelectMany(x => x.ToArray());
            list.AddRange(otherClasses);

            var controller = this.GenerateController(adapter);
            list.Add(controller);


            return list;
        }
        [JsonIgnore]
        public string CodeNamespace { get; set; }
        [JsonIgnore]
        public string ClassAttribute { get; set; }

        
    }
}