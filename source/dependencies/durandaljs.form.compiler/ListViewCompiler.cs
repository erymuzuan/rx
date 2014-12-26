using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.CONTRACT, typeof(FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.COMPILER_NAME, Type = typeof(ListView))]
    public class ListViewCompiler : DurandalJsElementCompiler<ListView>
    {
        private readonly ObjectCollection<string> m_inputEditorCollection = new ObjectCollection<string>();

        public ObjectCollection<string> InputEditorCollection
        {
            get { return m_inputEditorCollection; }
        }
        public override string GenerateEditor(ListView element)
        {
            this.InputEditorCollection.Clear();
            var editors = from x in element.ListViewColumnCollection
                          let c = x.Input.ElementId = string.Empty
                          let d = x.Input.IsUniqueName = true
                          select x.Input.GenerateEditorTemplate(Constants.COMPILER_NAME);
            this.InputEditorCollection.AddRange(editors);
            return base.GenerateEditor(element);
        }
    }
}