using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.OdataQueryCompilers
{
    class CustomObjectModel
    {
        public CSharpSyntaxTree SyntaxTree { get; set; }
        public bool IncludeAsParameter { get; set; }
        public string ClassName { get; set; }
        public string IdentifierText { get; set; }
    }
}