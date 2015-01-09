using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.FORM_ELEMENT_COMPILER_CONTRACT, typeof(FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.DURANDAL_JS, Type = typeof(ListView))]
    public class ListViewCompiler : DurandalJsElementCompiler<ListView>
    {

        public override string GenerateDisplay(ListView element, IProjectProvider entity)
        {
            m_entity = entity;
            return base.GenerateDisplay(element, entity);
        }

        public string RenderInput(FormElement input)
        {
            var download = input as DownloadLink;
            if (null != download)
            {
                var dc = new DownloadLinkCompiler();
                return "<td>" + dc.GenerateDisplay(download, m_entity) + "</td>";
            }

            var image = input as ImageElement;
            if (null != image)
            {
                var ic = new ImageElementCompiler();
                return "<td>" + ic.GenerateDisplay(image, m_entity) + "</td>";
            }
            return string.Format("<td data-bind=\"text:{0}\"></td>", input.Path.ConvertJavascriptObjectToFunction());

        }
        public string Expression
        {
            get { return this.Element.Path.ConvertJavascriptObjectToFunction(); }
        }
        private readonly ObjectCollection<string> m_inputEditorCollection = new ObjectCollection<string>();
        private IProjectProvider m_entity;

        public ObjectCollection<string> InputEditorCollection
        {
            get { return m_inputEditorCollection; }
        }
        public override string GenerateEditor(ListView element, IProjectProvider entity)
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