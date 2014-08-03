using System;
using System.ComponentModel.Composition;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{

    [Export("FunctoidDesigner", typeof(Functoid))]
    [FunctoidDesignerMetadata(Name = "Constant", FontAwesomeIcon = "sort-numeric-asc")]
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