using System.IO;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [StoreAsSource]
    public partial class ViewTemplate : Entity
    {
        private readonly string m_html;
        private readonly string m_js;
        private readonly string m_htmlPath;
        private readonly string m_jsPath;

        public ViewTemplate()
        {
            m_htmlPath = $"{ConfigurationManager.SphSourceDirectory}\\ViewTemplate\\{this.Name}.html.cshtml";
            m_jsPath = $"{ConfigurationManager.SphSourceDirectory}\\ViewTemplate\\{this.Name}.js.cshtml";
        }

        public ViewTemplate(string html, string js)
        {
            m_html = html;
            m_js = js;
        }

        public void StoreHtml(string html)
        {
            File.WriteAllText(m_htmlPath, html);
        }
        public void StoreJs(string js)
        {
            File.WriteAllText(m_jsPath, js);
        }
        [JsonIgnore]
        public string Html => !string.IsNullOrWhiteSpace(m_html) ? m_html : File.ReadAllText(m_htmlPath);
        [JsonIgnore]
        public string Js => !string.IsNullOrWhiteSpace(m_js) ? m_js : File.ReadAllText(m_jsPath);
    }
}