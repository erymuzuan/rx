using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class AwaitExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override string[] ObjectNames
        {
            get { return new string[] { }; }
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.AwaitExpression }; }
        }

        public override bool Filter(SyntaxNode node, SemanticModel model)
        {
            return node is AwaitExpressionSyntax;
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var awaitExpression = (AwaitExpressionSyntax)node;
            var code = new StringBuilder();

            var statements = FindBodyStatements(node);
            var currentStatement = FindStatement(node);
            var index = FindStatementLine(node);
            var local = currentStatement as LocalDeclarationStatementSyntax;
            var resultIdentifier = "";
            if (null != local)
            {
                resultIdentifier = local.Declaration.Variables[0].Identifier.Text;
            }

            code.AppendLine("return " + this.GetStatementCode(model, awaitExpression.Expression));
            code.AppendFormat(".then(function({0}){{", resultIdentifier);

            var subsequentStatements = statements.SkipWhile((x, i) => i <= index);
            foreach (var statement in subsequentStatements)
            {
                code.AppendLine();
                var hasAwait = statement.DescendantNodes().OfType<AwaitExpressionSyntax>().Any();
                if (hasAwait)
                {
                    code.AppendLine("       return " + base.GetStatementCode(model, statement).TrimEnd() + ";");
                    break;
                }

                var returnStatement = statement as ReturnStatementSyntax;
                if (null != returnStatement)
                {
                    code.AppendLinf("   __tcs{0}.resolve({1});", index, base.GetStatementCode(model, returnStatement.Expression));
                    code.AppendLinf("   return __tcs.promise();", index);
                    break;
                }

                code.AppendLine("       " + base.GetStatementCode(model, statement).TrimEnd() + ";");
            }


            code.AppendLine("});");
            code.AppendLine();
            code.AppendLine();

            return code.ToString();
        }

        private StatementSyntax FindStatement(SyntaxNode node)
        {
            var statements = FindBodyStatements(node);
            StatementSyntax currentStatement = null;
            foreach (var st in statements)
            {
                var any = st.DescendantNodes().OfType<AwaitExpressionSyntax>()
                    .ToList().Any(d => d == node);
                if (!any) continue;
                currentStatement = st;
                break;
            }
            return currentStatement;
        }

        private int FindStatementLine(SyntaxNode node)
        {
            var statements = this.FindBodyStatements(node);
            var currentStatement = FindStatement(node);
            return statements.IndexOf(currentStatement);
        }
        private SyntaxList<StatementSyntax> FindBodyStatements(SyntaxNode node)
        {
            var n = node.Parent;
            MethodDeclarationSyntax mi = null;
            for (int i = 0; i < 100; i++)
            {
                mi = n as MethodDeclarationSyntax;
                if (null != mi && mi.Identifier.Text == "Evaluate")
                    break;
                n = n.Parent;
            }
            if (null == mi) throw new InvalidOperationException("Cannot find the Method Evaluate");

            var statements = mi.Body.Statements;
            return statements;
        }
    }
}