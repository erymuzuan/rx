﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class IfStatementWalker : CustomObjectSyntaxWalker
    {
        protected override string[] ObjectNames
        {
            get { return new string[] { }; }
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.IfStatement }; }
        }

        public override bool Filter(SyntaxNode node, SemanticModel model)
        {
            return node is IfStatementSyntax;
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var iss = (IfStatementSyntax)node;
            var code = new StringBuilder();
            code.AppendLine("if( " + iss.Condition + "){");


            var block = iss.Statement as BlockSyntax;
            if (null != block)
            {
                foreach (var st in block.Statements)
                {
                    code.AppendLine(this.GetStatementCode(model, st));
                }
            }
            else
            {
                code.AppendLine(this.GetStatementCode(model, iss.Statement));
            }


            code.AppendLine("}");
            return code.ToString();
        }
    }
}