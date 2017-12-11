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
        public string PePath { get; }
        public string PdbPath { get; }
        public bool IsDebug { get; set; }
        public bool IsVerbose { get; set; }
        public bool Emit { get; }
        public Stream PeStream { get; }
        public Stream PdbStream { get; }
        public string[] Sources { get; }

        public CompilerOptions2()
        {
        }

        public CompilerOptions2(Stream peStream)
        {
            this.PeStream = peStream;
            this.Emit = true;
        }
        public CompilerOptions2(Stream peStream, Stream pdbStream)
        {
            this.PeStream = peStream;
            PdbStream = pdbStream;
            this.Emit = true;
        }
        public CompilerOptions2(string pePath, string pdbPath)
        {
            PePath = pePath;
            PdbPath = pdbPath;
            this.Emit = true;
        }
        public CompilerOptions2(Stream peStream, string[] sources)
        {
            this.PeStream = peStream;
            Sources = sources;
            this.Emit = true;
        }

    }
}