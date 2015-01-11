using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public class CustomObjectModel
    {
        public CSharpSyntaxTree SyntaxTree { get; set; }
        public bool IncludeAsParameter { get; set; }
        public string ClassName { get; set; }
        public string IdentifierText { get; set; }
    }
}