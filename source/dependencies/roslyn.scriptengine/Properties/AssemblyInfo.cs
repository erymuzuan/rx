using System.Reflection;

[assembly: AssemblyTitle("roslyn.scriptengine")]
[assembly: AssemblyDescription("Roslyn implementation for IScriptEngine")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
//
//