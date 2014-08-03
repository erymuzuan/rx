using System;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FunctoidDesignerMetadataAttribute : ExportAttribute, IFunctoidDesignerMetadata
    {
        public FunctoidDesignerMetadataAttribute() : base(typeof (IFunctoidDesignerMetadata))
        {
        }

        public FunctoidCategory Category { get; set; }
        public string Name { get; set; }
        public string FontAwesomeIcon{ get; set; }
        public string BootstrapIcon{ get; set; }
        public string PngIcon{ get; set; }
    }
}