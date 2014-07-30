using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class DirectMap : Map
    {
        [JsonIgnore]
        public Type Type
        {
            get
            {
                return Type.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }


        public  override Task<string> ConvertAsync(object source)
        {
            var context = new RuleContext((Entity)source);
            var df = new DocumentField {Path = this.Source};
            var json = string.Format("\"{0}\":\"{1}\"", this.Destination, df.GetValue(context));
            return Task.FromResult(json);
        }

        public override string GenerateCode()
        {
            return string.Format("dest.{1} = item.{0};", this.Source, this.Destination);
        }
    }
}