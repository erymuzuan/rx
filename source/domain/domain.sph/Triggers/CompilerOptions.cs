using System;
using System.Reflection;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public class CompilerOptions
    {
        private readonly ObjectCollection<string> m_referencedAssembliesCollection = new ObjectCollection<string>();
        public bool IsDebug { get; set; }
        public bool IsVerbose { get; set; }
        public string SourceCodeDirectory { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public ObjectCollection<string> ReferencedAssembliesLocation
        {
            get { return m_referencedAssembliesCollection; }
        }

        /// <summary>
        /// A helper to add referenced assembly location
        /// </summary>
        /// <param name="location"></param>
        public void AddReference(string location)
        {
            this.ReferencedAssembliesLocation.Add(location);
        }
        /// <summary>
        /// A helper to add referenced assembly location
        /// </summary>
        /// <param name="type">The assembly for the given type</param>
        public void AddReference(Type type)
        {
            this.ReferencedAssembliesLocation.Add(type.Assembly.Location);
        }
        /// <summary>
        /// A helper to add referenced assembly location
        /// </summary>
        /// <param name="assembly"></param>
        public void AddReference(Assembly assembly)
        {
            this.ReferencedAssembliesLocation.Add(assembly.Location);
        }
    }
}