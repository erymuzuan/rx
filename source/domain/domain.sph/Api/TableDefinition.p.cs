using System.Linq;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain.Api
{
    public partial class TableDefinition : DomainObject
    {
        public string ClrName { get; set; }
        [JsonIgnore]
        public Column PrimaryKey
        {
            get { return this.ColumnCollection.FirstOrDefault(a => this.PrimaryKeyCollection.Contains(a.Name)); }
        }

        //TODO : move to xsd
        public ObjectCollection<string> PrimaryKeyCollection { get; } = new ObjectCollection<string>();
        public ObjectCollection<ControllerAction> ControllerActionCollection { get; } = new ObjectCollection<ControllerAction>();

        public override string ToString()
        {
            return this.Name;
        }

        public string ComputeClrName(Adapter adapter)
        {
            var name = $"{Schema}.{Name}";
            switch (adapter.ColumnClrNameStrategy)
            {
                case "Auto":
                    this.ClrName = name.ToClrAuto();
                    break;
                case "camel":
                    this.ClrName = name.ToCamelCase();
                    break;
                case "_":
                    this.ClrName = name.ToIdFormat().Replace("-", "_");
                    break;
                // default to Pascal since the previous code just take Pascal 
                //case "pascal":
                default:
                    this.ClrName = name.ToPascalCase();
                    break;
            }
            return this.ClrName;
        }


    }
}
