using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.Domain
{

    public static class CompilerHelper
    {
        public static MetadataReference CreateMetadataReference(this Type type)
        {
            return MetadataReference.CreateFromAssembly(type.Assembly);
        }

        public static MetadataReference CreateMetadataReference<T>(this object type)
        {
            return MetadataReference.CreateFromAssembly((typeof(T)).Assembly);
        }


        public static CSharpCompilation AddReference<T>(this CSharpCompilation type)
        {
            return type.AddReferences(MetadataReference.CreateFromAssembly((typeof(T)).Assembly));
        }
        public static Project AddMetadataReference<T>(this Project type)
        {
            return type.AddMetadataReference(MetadataReference.CreateFromAssembly((typeof(T)).Assembly));
        }

        public static Project AddMetadataReference(this Project project, Type type)
        {
            return project.AddMetadataReference(MetadataReference.CreateFromAssembly(type.Assembly));
        }

        public static Project AddMetadataReference(this Project project, string location)
        {
            return project.AddMetadataReference(MetadataReference.CreateFromFile(location));
        }
        public static ProjectInfo AddMetadataReference<T>(this ProjectInfo info)
        {
            var list = info.MetadataReferences.ToList();
            list.Add(MetadataReference.CreateFromAssembly((typeof(T)).Assembly));
            return info.WithMetadataReferences(list.ToArray());
        }


        public static ProjectInfo AddMetadataReference(this ProjectInfo info, Type type)
        {
            var list = info.MetadataReferences.ToList();
            list.Add(MetadataReference.CreateFromAssembly(type.Assembly));
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
            var ws = new CustomWorkspace();
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
