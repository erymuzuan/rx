using System.Diagnostics;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Solutions
{
    // ReSharper disable InconsistentNaming
    [DebuggerDisplay("{text} {icon}")]
    public class SolutionItem
    {
        public string id { get; set; }
        public string changedType { get; set; }
        public SolutionItem item { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string dialog { get; set; }
        public string createDialog { get; set; }
        public string createdUrl { get; set; }
        public string codeEditor { get; set; }

        public override bool Equals(object obj)
        {
            var sol = obj as SolutionItem;
            if (null == sol) return false;
            return this.Equals(sol);
        }

        protected bool Equals(SolutionItem other)
        {
            return string.Equals(id, other.id) && string.Equals(text, other.text);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((id?.GetHashCode() ?? 0)*397) ^ (text?.GetHashCode() ?? 0);
            }
        }

        public void AddItems(params SolutionItem[] items)
        {
            foreach (var x in items)
            {
                if (null == x) continue;
                if (this.itemCollection.Contains(x)) continue;
                this.itemCollection.Add(x);
            }
        }
        public ObjectCollection<SolutionItem> itemCollection { get; } = new ObjectCollection<SolutionItem>();
        [JsonIgnore]
        public object Tag { get; set; }
    }
}