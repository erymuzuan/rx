using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Fomatting object", FontAwesomeIcon = "file-text")]
    public partial class FormattingFunctoid : Functoid
    {
        public override bool Initialize()
        {
            this.ArgumentCollection.Clear();
            this.ArgumentCollection.Add(new FunctoidArg{ Name = "format", Type = typeof(string), IsOptional = true});
            this.ArgumentCollection.Add(new FunctoidArg{ Name = "value", Type = typeof(object)});

            return base.Initialize();
        }

        public override string GenerateCode()
        {
            return "string.Format(\"" + this.Format +"\", item."+ this.SourceField +")";
        }
    }
}