using System;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class FormCompilerMetadataAttribute : ExportAttribute, IFormCompilerMetadata
    {
        public const string CONTRACT = "FormElementCompiler";
        public FormCompilerMetadataAttribute()
            : base(typeof(IFormCompilerMetadata))
        {
        }

        public string Description { get; set; }
        public Type Type { get; set; }
        public string Name { get; set; }
        public bool IsSupported { get; set; }
    }
}