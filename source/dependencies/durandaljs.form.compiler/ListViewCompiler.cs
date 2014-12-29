using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.FORM_ELEMENT_COMPILER_CONTRACT, typeof(FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.DURANDAL_JS, Type = typeof(ListView))]
    public class ListViewCompiler : DurandalJsElementCompiler<ListView>
    {
        private readonly ObjectCollection<string> m_inputEditorCollection = new ObjectCollection<string>();

        public ObjectCollection<string> InputEditorCollection
        {
            get { return m_inputEditorCollection; }
        }
        public override string GenerateEditor(ListView element, EntityDefinition entity)
        {
            this.InputEditorCollection.Clear();
            var editors = from x in element.ListViewColumnCollection
                          let c = x.Input.ElementId = string.Empty
                          let d = x.Input.IsUniqueName = true
                          select x.Input.GenerateEditorTemplate(Constants.DURANDAL_JS, entity);
            this.InputEditorCollection.AddRange(editors);
            return base.GenerateEditor(element, entity);
        }
    }
}