using System;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "ConfigurationSettingFunctoid", FontAwesomeIcon = "tasks", Category = FunctoidCategory.DATABASE)]
    public class ConfigurationSettingFunctoid : Functoid
    {
        public string Section { get; set; }
        public string Key { get; set; }

        public override string GenerateAssignmentCode()
        {
            return string.Format("__config{0}", this.Index);
        }

        public override string GenerateStatementCode()
        {
            if (this.Section == "ConnectionString")
                return string.Format("var __config{1} = ConfigurationManager.ConnectionStrings[\"{0}\"].ConnectionString;", this.Key, this.Index);
            if (this.Section == "AppSetting")
                return string.Format("var __config{1} = ConfigurationManager.AppSettings[\"{0}\"];", this.Key, this.Index);
            throw new InvalidOperationException("Cannot recognized section " + this.Section);
        }
    }
}