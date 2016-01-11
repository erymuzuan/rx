using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Domain.Api
{
    public partial class TableDefinition
    {
        public TableDefinition()
        {

        }

        public TableDefinition(AdapterTable table)
        {
            this.Name = table.Name;
            var tables = from a in table.ChildRelationCollection
                         select new TableDefinition
                         {
                             Name = a.Table,
                             CodeNamespace = this.CodeNamespace,
                             Schema = this.Schema
                         };
            this.ChildTableCollection.ClearAndAddRange(tables);

        }

        private static readonly string[] ImportDirectives =
        {

            typeof(Entity).Namespace ,
            typeof(int).Namespace ,
            typeof(Task<>).Namespace ,
            typeof(Enumerable).Namespace ,
            typeof(JsonConvert).Namespace ,
            typeof(CamelCasePropertyNamesContractResolver).Namespace ,
            typeof(StringEnumConverter).Namespace ,
            typeof(XmlAttributeAttribute).Namespace ,
            typeof(MediaTypeFormatter).Namespace,
            "System.Web.Http",
            "System.Net",
            "System.Net.Http"
};



        public IEnumerable<Class> GenerateCode(Adapter adapter)
        {
            var @adapteClass = new Class { Name = Name, BaseClass = nameof(DomainObject), Namespace = this.CodeNamespace };

            var list = new ObjectCollection<Class> { @adapteClass };

            if (!string.IsNullOrWhiteSpace(ClassAttribute))
                @adapteClass.AttributeCollection.Add(ClassAttribute);
            

            var pk = "\"\"";
            if (null != this.PrimaryKey)
                pk = this.PrimaryKey.Name;
            var toString = $"public override string ToString(){{ return \"{Name}:\" + {pk};}}";
            @adapteClass.MethodCollection.Add(new Method {Code = toString});


            var properties = this.MemberCollection.Select(x => new Property {Code = x.GeneratedCode("   ")});
            @adapteClass.PropertyCollection.AddRange(properties);

            var otherClasses = this.MemberCollection.Select(
                x => x.GeneratedCustomClass(this.CodeNamespace, ImportDirectives))
                .SelectMany(x => x.ToArray());
            list.AddRange(otherClasses);

            var controller = this.GenerateController(adapter);
            list.Add(controller);


            return list;
        }

        public string Name { get; set; }
        public string CodeNamespace { get; set; }
        public string ClassAttribute { get; set; }
        public string WebId { get; set; }
    }
}