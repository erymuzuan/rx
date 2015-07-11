using System.IO;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [StoreAsSource]
    public partial class ViewTemplate : Entity
    {
        private readonly string m_html;
        private readonly string m_js;
        [JsonIgnore]
        private string HtmlPath  =>$"{ConfigurationManager.SphSourceDirectory}\\ViewTemplate\\{this.Name}.html.cshtml";
        [JsonIgnore]
        private string JsPath =>$"{ConfigurationManager.SphSourceDirectory}\\ViewTemplate\\{this.Name}.js.cshtml";

        public ViewTemplate()
        {
        }

        public ViewTemplate(string html, string js)
        {
            m_html = html;
            m_js = js;
        }

        public void StoreHtml(string html)
        {
            File.WriteAllText(HtmlPath, html);
        }
        public void StoreJs(string js)
        {
            File.WriteAllText(JsPath, js);
        }
        [JsonIgnore]
        public string Html => !string.IsNullOrWhiteSpace(m_html) ? m_html : File.ReadAllText(HtmlPath);
        [JsonIgnore]
        public string Js => !string.IsNullOrWhiteSpace(m_js) ? m_js : File.ReadAllText(JsPath);
    }
}