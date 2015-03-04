// Copyright (c) Microsoft Open Technologies, Inc.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using CSharpExtensions = Microsoft.CodeAnalysis.CSharp.CSharpExtensions;

namespace Roslyn.SyntaxVisualizer.Control
{
    public static class SyntaxKindHelper
    {
        // Helpers that return the language-sepcific (C# / VB) SyntaxKind of a language-agnostic
        // SyntaxNode / SyntaxToken / SyntaxTrivia.

        public static string GetKind(this SyntaxNodeOrToken nodeOrToken)
        {
            var kind = nodeOrToken.IsNode ? nodeOrToken.AsNode().GetKind() : nodeOrToken.AsToken().GetKind();
            return kind;
        }

        public static string GetKind(this SyntaxNode node)
        {
            var kind = node.Language == LanguageNames.CSharp ? ((Microsoft.CodeAnalysis.CSharp.SyntaxKind)node.RawKind).ToString() : node.Kind().ToString();
            return kind;
        }

        public static string GetKind(this SyntaxToken token)
        {
            var kind = token.Language == LanguageNames.CSharp ? CSharpExtensions.Kind(token).ToString() : token.Kind().ToString();
            return kind;
        }

        public static string GetKind(this SyntaxTrivia trivia)
        {
            var kind = trivia.Language == LanguageNames.CSharp ? CSharpExtensions.Kind(trivia).ToString() : trivia.Kind().ToString();
            return kind;
        }
    }
}