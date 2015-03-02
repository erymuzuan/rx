using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;

namespace Bespoke.Sph.SyntaxVisualizers
{
    public static class CompilerHelper
    {
        public static string FormatCode(this string code)
        {
            var ws = new AdhocWorkspace();
            var info = ProjectInfo.Create(ProjectId.CreateNewId("formatter"),
                VersionStamp.Default, "formatter", "formatter", LanguageNames.CSharp);
            var project = ws.AddProject(info);
            var tree = CSharpSyntaxTree.ParseText(code);
            project.AddDocument("formatted", tree.GetText());

            var res = Formatter.Format(tree.GetRoot(), ws);
            return res.ToFullString();
        }
    }
}
