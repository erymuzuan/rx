using System;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class DesignerMetadataAttribute : ExportAttribute, IDesignerMetadata
    {
        public DesignerMetadataAttribute()
            : base(typeof(IDesignerMetadata))
        {
            this.Order = 100;
        }


        public bool IsEnabled { get; set; }
        public double Order { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string Route { get; set; }
        public Type Type { get; set; }
        public Type RouteTableProvider { get; set; }
        public string FontAwesomeIcon { get; set; }
        public string BootstrapIcon { get; set; }
        public string PngIcon { get; set; }
    }
}