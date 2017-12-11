using System.Reflection;

[assembly: AssemblyTitle("elasticsearch.logger")]
[assembly: AssemblyDescription("Provides an Elasticsearch 1.7.5 logging repository")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
//
//
//
//
//
//