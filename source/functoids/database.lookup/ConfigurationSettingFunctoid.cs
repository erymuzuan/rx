using System;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Config", FontAwesomeIcon = "tasks", Category = FunctoidCategory.DATABASE)]
    public class ConfigurationSettingFunctoid : Functoid
    {
        public string Section { get; set; }
        public string Key { get; set; }

        public override string GenerateAssignmentCode()
        {
            return $"__config{this.Index}";
        }

        public override string GenerateStatementCode()
        {
            if (this.Section == "EnvironmentVariable")
                return $"var __config{Index} = ConfigurationManager.GetEnvironmentVariable(\"{Key}\");";
             if (this.Section == "ConnectionString")
                return $"var __config{Index} = ConfigurationManager.ConnectionStrings[\"{Key}\"].ConnectionString;";
            if (this.Section == "AppSetting")
                return $"var __config{Index} = ConfigurationManager.AppSettings[\"{Key}\"];";
            throw new InvalidOperationException("Cannot recognized section " + this.Section);
        }
        public override string GetEditorView()
        {
            return database.lookup.Properties.Resources.ConfigView;
        }

        public override string GetEditorViewModel()
        {
            return database.lookup.Properties.Resources.ConfigViewModel;
        }
    }
}