using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using InvocationExpressionSyntax = Microsoft.CodeAnalysis.VisualBasic.Syntax.InvocationExpressionSyntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.FORM_ELEMENT_COMPILER_CONTRACT, typeof(FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.DURANDAL_JS, Type = typeof(Button))]
    public class ButtonCompiler : DurandalJsElementCompiler<Button>
    {
        public string Compile(Button button)
        {
            if (string.IsNullOrWhiteSpace(button.Command))
                return null;
            var file = new StringBuilder("public ");
            file.Append(button.IsAsynchronous ? " async Task " : " void ");
            file.Append(" ExecuteOperation(SphDataContext context, Logger logger, State item)");
            file.AppendLine("{");
            file.AppendLine(button.Command);
            file.AppendLine("}");

            var tree = CSharpSyntaxTree.ParseText(file.ToString());
            var root = (CompilationUnitSyntax)tree.GetRoot();


            var methodBlock = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Single()
                .DescendantNodes()
                .OfType<BlockSyntax>()
                .Single();

            var code = new StringBuilder();
            if (button.IsAsynchronous)
                code.AppendLine("var tcs = new $.Deferred();");
            foreach (var node in methodBlock.Statements)
            {
                this.AppendCode(code, node);
            }
            if (button.IsAsynchronous)
                code.AppendLine(" return tcs.promise();");


            return code.ToString();
        }

        private void AppendCode(StringBuilder code, SyntaxNode statement)
        {
            var lds = statement as LocalDeclarationStatementSyntax;
            if (null != lds)
            {
                var vd = lds.ChildNodes().OfType<VariableDeclarationSyntax>().Single();
                var identifierName = vd.ChildNodes().OfType<IdentifierNameSyntax>().Single();
                var variableDec = vd.ChildNodes().OfType<VariableDeclaratorSyntax>().Single();


                var variableName = variableDec.ChildTokens().Single(x => x.IsKind(SyntaxKind.IdentifierToken));


                var eqValue = variableDec.ChildNodes().OfType<EqualsValueClauseSyntax>().Single();
                var awaitExpression = eqValue.ChildNodes().OfType<AwaitExpressionSyntax>().SingleOrDefault();
                if (null != awaitExpression)
                {
                    var inv = awaitExpression.Expression;

                    var member = inv.ChildNodes().OfType<MemberAccessExpressionSyntax>().First();
                    //code.Insert()
                    code.AppendFormat("{0}.{1}({2}).", member.Expression.GetText(), "AsyncMethodName", "argumentList");

                    code.AppendFormat(@"
    done(function({0}){{
        
        tcs.resolve({0});
    }});", variableName.Text);


                }
                else
                {
                    code.Append("var " + variableName.Text);
                }


            }
            var ifs = statement as IfStatementSyntax;
            if (null != ifs)
            {

            }
            code.AppendLine();

        }
    }
}