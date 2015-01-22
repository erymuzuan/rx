using System;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.FormCompilers.DurandalJs.FormRenderers
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class FormRendererMetadata : ExportAttribute, IFormRendererMetadata
    {
        public FormRendererMetadata()
        {
            
        }
        public FormRendererMetadata(Type type)
            : base(type)
        {

        }
        public FormRendererMetadata(string contract, Type type)
            : base(contract, type)
        {

        }
        public Type FormType { get; set; }
        public string Text { get; set; }
    }
}