﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.StringIdentifiers
{
    [Export("String", typeof (IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "String", Text = "ToUpperInvariant")]
    public class StringToUpperInvariant : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            return "toUpperCase()";
        }
    }
}