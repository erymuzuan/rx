using System;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.OdataQueryCompilers
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class IdentifierCompilerMetadataAttribute : ExportAttribute, IIdentifierCompilerMetadata
    {
        public IdentifierCompilerMetadataAttribute()
        {
            
        }
        public IdentifierCompilerMetadataAttribute(Type type)
            : base(type)
        {

        }
        public IdentifierCompilerMetadataAttribute(string contract, Type type)
            : base(contract, type)
        {

        }
        public string TypeName { get; set; }
        public string Text { get; set; }
    }
}