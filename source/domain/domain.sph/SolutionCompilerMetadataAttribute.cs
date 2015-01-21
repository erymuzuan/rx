using System;
using System.Composition;


namespace Bespoke.Sph.Domain
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class SolutionCompilerMetadataAttribute : ExportAttribute, ISolutionCompilerMetadata
    {
        public SolutionCompilerMetadataAttribute()
            : base(typeof(ISolutionCompilerMetadata))
        {
        }

        public string Description { get; set; }
        public Type Type { get; set; }
        public string Name { get; set; }
        public bool IsSupported { get; set; }
    }
}