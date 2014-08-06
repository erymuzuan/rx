using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [FunctoidDesignerMetadata(Name = "Parse int", FontAwesomeIcon = "sort-numeric-asc")]
    public partial class Int32Functoid : Functoid
    {
        public override bool Initialize()
        {
            this.ArgumentCollection.Clear();
            this.ArgumentCollection.Add(new FunctoidArg { Name = "source", Type = typeof(object) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "styles", Type = typeof(string), IsOptional = true, Default = "None" });
            return true;
        }

        private int m_number;
        public override string GeneratePreCode()
        {
            this.NumberStyles = "None";
            m_number = GetRunningNumber();
            var code = new StringBuilder();
            code.AppendLinf("               var val{0} = {1};", m_number,
                this["source"].GetFunctoid(this.TransformDefinition).GenerateCode());
            return code.ToString();
        }

        public override string GenerateCode()
        {
            return string.Format("int.Parse(val{0}, System.Globalization.NumberStyles.{1})", m_number, this.NumberStyles);
        }
    }
}