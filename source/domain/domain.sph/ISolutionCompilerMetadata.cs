using System;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    public interface ISolutionCompilerMetadata
    {
        string Name { get; }
    }

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