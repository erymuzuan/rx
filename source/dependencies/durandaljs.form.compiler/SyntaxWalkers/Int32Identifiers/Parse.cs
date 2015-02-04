using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.Int32Identifiers
{
    [Export(typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "Int32", Text = "Parse")]
    public class Parse : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            var arg = arguments.FirstOrDefault();
            if (null == arg)
                throw new Exception("Parse must have at least 1 arg");
            return string.Format("parseInt({0})", this.EvaluateExpressionCode(arg));
        }
    }
}