using System.Reflection;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public class CompilerOptions
    {
        private readonly ObjectCollection<Assembly> m_referencedAssembliesCollection = new ObjectCollection<Assembly>();
        public bool IsDebug { get; set; }
        public bool IsVerbose { get; set; }
        public string SourceCodeDirectory { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public ObjectCollection<Assembly> ReferencedAssemblies
        {
            get { return m_referencedAssembliesCollection; }
        }
    }
}