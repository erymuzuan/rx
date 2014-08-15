using System;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FormMetadataAttribute : ExportAttribute, IFormRendereMetadata
    {
        public FormMetadataAttribute()
            : base(typeof(IDesignerMetadata))
        {
        }

        public string Name { get; set; }
    }
}