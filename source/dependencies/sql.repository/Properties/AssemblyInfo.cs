using System.Reflection;

[assembly: AssemblyTitle("sql.repository")]
[assembly: AssemblyDescription("SQL Server implementation for IRepository")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif