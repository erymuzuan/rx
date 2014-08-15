using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Mapping", TypeName = "Mapping", Description = "Run a data transform")]
    public partial class MappingActivity : Activity
    {
        public string[] Source { get; set; }
        
    }
}