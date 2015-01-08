using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export]
    public class CompilationUnitContainer
    {
        public SemanticModel SemanticModel { get; set; }
        public CSharpSyntaxTree SyntaxTree { get; set; }
        
    }
}