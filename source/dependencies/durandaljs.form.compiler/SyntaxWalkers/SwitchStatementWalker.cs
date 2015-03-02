using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class SwitchStatementWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SwitchStatement }; }
        }

        public override bool Filter(SyntaxNode node)
        {
            return node.Kind() == SyntaxKind.SwitchStatement;
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var sw = (SwitchStatementSyntax)node;
            var code = new StringBuilder();
            code.AppendFormat("switch({0}) {{", this.EvaluateExpressionCode(sw.Expression));
            code.AppendLine();

            foreach (var sc in sw.Sections)
            {
                var caseLabels = sc.Labels.OfType<CaseSwitchLabelSyntax>().Select(x => "case " + this.EvaluateExpressionCode(x.Value) + ":");
                code.AppendLine(string.Join("\r\n", caseLabels));
                var defaultLabels = sc.Labels.OfType<DefaultSwitchLabelSyntax>().Select(x => "default :");
                code.AppendLine(string.Join("\r\n", defaultLabels));

                var blockCode = this.Compiler.BuildAwaitStatementTree(sc.Statements.ToList(), model, true);
                code.AppendLine(blockCode);
            }
     

            code.AppendLine("}");

         



            return code.ToString();
        }
    }
}