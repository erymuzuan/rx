using System;
using System.ComponentModel.Composition;


namespace Bespoke.Sph.Domain
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class ProjectProviderMetadataAttribute : ExportAttribute, IProjectProviderMetadata
    {
        public ProjectProviderMetadataAttribute()
            : base(typeof(IProjectProviderMetadata))
        {
        }

        public string Description { get; set; }
        public Type Type { get; set; }
        public string Name { get; set; }
        public bool IsSupported { get; set; }
    }
}