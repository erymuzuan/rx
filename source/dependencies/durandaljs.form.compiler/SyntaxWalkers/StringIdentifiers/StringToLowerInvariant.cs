﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.StringIdentifiers
{
    [Export("String", typeof (IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "String", Text = "ToLowerInvariant")]
    public class StringToLowerInvariant : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            return "toLowerCase()";
        }
    }
}