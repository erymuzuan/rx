using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using AwaitExpressionSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.AwaitExpressionSyntax;

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
            var aes = (AwaitExpressionSyntax)node;
            var code = new StringBuilder();
            int index;


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
            StatementSyntax statement = null;
            foreach (var st in statements)
            {
                var any = st.DescendantNodes().OfType<AwaitExpressionSyntax>()
                    .ToList().Any(d => d == node);
                if (!any) continue;
                statement = st;
                break;
            }
            index = statements.IndexOf(statement);


            var local = statement as LocalDeclarationStatementSyntax;
            var result = string.Format("__result{0}", index);
            if (null != local)
            {
                result = local.Declaration.Variables[0].Identifier.Text;
            }

            code.AppendLine(this.GetStatementCode(model, aes.Expression));
            code.AppendLinf(".then(function({0})){{", result);

            for (int i = index + 1; i < statements.Count; i++)
            {
                var awaitStatement = statements[i].DescendantNodes().OfType<AwaitExpressionSyntax>().Any();
                if (awaitStatement)
                {
                    code.AppendLine();
                    code.AppendLine("  var __tcs = new $.Deferred();");
                }

                var rs = statements[i] as ReturnStatementSyntax;
                if (null != rs)
                {
                    code.AppendLinf("   __tcs{0}.resolve({1});", index, base.GetStatementCode(model, rs.Expression));
                    code.AppendLinf("   return __tcs.promise();", index);
                }
                else
                {
                    code.AppendLine("       " + base.GetStatementCode(model, statements[i]).TrimEnd() + ";");
                }


                if (awaitStatement) break;
                
            }


            code.AppendLinf("   __tcs{0}.resolve({1});", index, result);
            code.AppendLine("});");
            code.AppendLinf("return __tcs.promise();", index);
            code.AppendLine();
            code.AppendLine();

            return code.ToString();
        }

    }
}