using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{

    [Export("FunctoidDesigner", typeof(Functoid))]
    [FunctoidDesignerMetadata(Name = "Constant", FontAwesomeIcon = "sort-numeric-asc", Category = FunctoidCategory.Common)]
    public partial class ConstantFunctoid : Functoid
    {
        public override string GenerateCode()
        {
            if (typeof (string) == this.Type)
                return "\"" + Value + "\"";
            if (typeof(double) == this.Type)
                return string.Format("{0}d", Value);
            if (typeof(decimal) == this.Type)
                return string.Format("{0}m", Value);

            return string.Format("{0}", Value) ;
        }

        public override async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errors = (await base.ValidateAsync()).ToList();
            if(string.IsNullOrWhiteSpace(this.TypeName))
                errors.Add(this.GetType().Name, "TypeName is not specified", this.WebId);
            if(!string.IsNullOrWhiteSpace(this.TypeName) && null == this.Type)
                errors.Add(this.GetType().Name, "TypeName is not recognized : " + this.TypeName, this.WebId);

            return errors;
        }

        [XmlIgnore]
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
    

    
    }
}