using System;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DesignerMetadataAttribute : ExportAttribute, IDesignerMetadata
    {
        public DesignerMetadataAttribute() : base(typeof (IDesignerMetadata))
        {
        }

        public string Description { get;  set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public Type Type { get;  set; }
        public string FontAwesomeIcon{ get; set; }
        public string BootstrapIcon{ get; set; }
        public string PngIcon{ get; set; }
    }
}