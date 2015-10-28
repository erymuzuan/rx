using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.Domain
{

    public static class CompilerHelper
    {
        public static void AddReference(this CompilerParameters parameters, params Type[] types)
        {
            foreach (var type in types)
            {
                parameters.ReferencedAssemblies.Add(type.Assembly.Location);
            }
        }
        public static MetadataReference CreateMetadataReference(this Type type)
        {
            return MetadataReference.CreateFromFile(type.Assembly.Location);
        }

        public static MetadataReference CreateMetadataReference<T>(this object type)
        {
            return MetadataReference.CreateFromFile((typeof(T)).Assembly.Location);
        }

        public static CSharpCompilation AddReference<T>(this CSharpCompilation type)
        {
            return type.AddReferences(MetadataReference.CreateFromFile((typeof(T)).Assembly.Location));
        }
        public static Project AddMetadataReference<T>(this Project type)
        {
            return type.AddMetadataReference(MetadataReference.CreateFromFile((typeof(T)).Assembly.Location));
        }

        public static Project AddMetadataReference(this Project project, Type type)
        {
            return project.AddMetadataReference(MetadataReference.CreateFromFile(type.Assembly.Location));
        }

        public static Project AddMetadataReference(this Project project, string location)
        {
            return project.AddMetadataReference(MetadataReference.CreateFromFile(location));
        }
        public static ProjectInfo AddMetadataReference<T>(this ProjectInfo info)
        {
            var list = info.MetadataReferences.ToList();
            list.Add(MetadataReference.CreateFromFile((typeof(T)).Assembly.Location));
            return info.WithMetadataReferences(list.ToArray());
        }


        public static ProjectInfo AddMetadataReference(this ProjectInfo info, Type type)
        {
            var list = info.MetadataReferences.ToList();
            list.Add(MetadataReference.CreateFromFile(type.Assembly.Location));
            return info.WithMetadataReferences(list.ToArray());
        }

        public static ProjectInfo AddMetadataReference(this ProjectInfo info, params string[] locations)
        {
            var list = info.MetadataReferences.ToList();
            var references = locations.Select(x => MetadataReference.CreateFromFile(x));
            list.AddRange(references);
            return info.WithMetadataReferences(list.ToArray());
        }

        public static string FormatCode(this StringBuilder code)
        {
            var ws = new AdhocWorkspace();
            var info = ProjectInfo.Create(ProjectId.CreateNewId("formatter"),
                VersionStamp.Default, "formatter", "formatter", LanguageNames.CSharp);
            var project = ws.AddProject(info);
            var tree = CSharpSyntaxTree.ParseText(code.ToString());
            project.AddDocument("formatted", tree.GetText());
            
            var res = Microsoft.CodeAnalysis.Formatting.Formatter.Format(tree.GetRoot(), ws);
            return res.ToFullString();
        }
    }
}
