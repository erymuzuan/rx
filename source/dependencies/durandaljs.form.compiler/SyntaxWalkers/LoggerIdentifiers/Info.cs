﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.LoggerIdentifiers
{
    [Export("Logger", typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "Logger", Text = "Info")]
    public class Info : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            var args = arguments.ToArray();
            return "info(" + this.EvaluateExpressionCode(args[0]) + ")";
        }
    }
}