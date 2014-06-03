using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public class DirectMap : Map
    {
        public string Source { get; set; }
        public string TypeName { get; set; }
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
    }
}