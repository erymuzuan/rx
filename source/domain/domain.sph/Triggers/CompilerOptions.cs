using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [Obsolete("Moving to Roslyn compilers")]
    public class CompilerOptions
    {
        public bool IsDebug { get; set; }
        public bool IsVerbose { get; set; }
        public string SourceCodeDirectory { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public ObjectCollection<string> ReferencedAssembliesLocation { get; } = new ObjectCollection<string>();

        public ObjectCollection<string> EmbeddedResourceCollection { get; } = new ObjectCollection<string>();

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
        public void AddReference<T>()
        {
            this.ReferencedAssembliesLocation.Add(typeof(T).Assembly.Location);
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

    public class CompilerOptions2
    {
        public bool IsDebug { get; set; }
        public bool IsVerbose { get; set; }
        public bool Emit { get; }
        public Stream Stream { get; }
        public string[] Sources { get; }

        public CompilerOptions2()
        {
        }

        public CompilerOptions2(Stream stream)
        {
            this.Stream = stream;
            this.Emit = true;
        }
        public CompilerOptions2(Stream stream, string[] sources)
        {
            this.Stream = stream;
            Sources = sources;
            this.Emit = true;
        }

    }
}