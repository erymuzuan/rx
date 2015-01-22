using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.DateTimeIdentifiers
{
    [Export("DateTime", typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata( TypeName = "DateTime", Text = "Parse")]
    public class DateTimeParse : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            var args = "";
            if (null != arguments)
                args = string.Join(", ", arguments.Select(this.EvaluateExpressionCode));
            return string.Format("moment({0})", args);
        }
    }
}