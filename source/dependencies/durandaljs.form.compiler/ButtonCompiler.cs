using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                var cont = this.AppendCode(code, node);
                if (!cont) break;
            }
            if (button.IsAsynchronous)
                code.AppendLine("\r\n return tcs.promise();");

            var walker = new IfJsWalker(code);
            walker.Visit(root);

            return code.ToString();
        }

        private bool AppendCode(StringBuilder code, SyntaxNode statement)
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

                    var ctxWalker = ContextLoadOneAsyncSyntaxWalker.Walk(awaitExpression);


                    code.AppendFormat("{0}.{1}.", member.Expression.GetText(), ctxWalker);



                    var block = (BlockSyntax)lds.Parent;
                    var index = block.Statements.IndexOf(lds) + 1;
                    var fff = new StringBuilder();

                    if (block.Statements.Count > index)
                    {

                        for (int i = index; i < block.Statements.Count; i++)
                        {
                            this.AppendCode(fff, block.Statements[i]);
                        }
                    }

                    code.AppendFormat(@"
    done(function({0}){{
        // this  is where the subsequent statements
        {1}
        tcs.resolve({0});
    }});", variableName.Text, fff);

                    return false;


                }
                code.AppendFormat("var {0} ", variableName.Text);
                var eq1 = variableDec.ChildNodes().OfType<EqualsValueClauseSyntax>()
                    .SingleOrDefault();
                if (null != eq1)
                    code.Append(eq1.GetText());
                code.AppendLine(";");

            }
            var ifs = statement as IfStatementSyntax;
            if (null != ifs)
            {

            }

            return true;

        }

        class IfJsWalker : CSharpSyntaxWalker
        {
            private readonly StringBuilder m_code;

            public IfJsWalker(StringBuilder code)
            {
                m_code = code;
            }


            public override void VisitIfStatement(IfStatementSyntax node)
            {
                m_code.AppendLine();
                m_code.AppendLine("// VISIT IF");

                base.VisitIfStatement(node);
            }
        }

        class ContextLoadOneAsyncSyntaxWalker : CSharpSyntaxWalker
        {
            private readonly StringBuilder m_code = new StringBuilder();

            public static string Walk(SyntaxNode node)
            {
                var walker = new ContextLoadOneAsyncSyntaxWalker{m_parentNode=  node};
                walker.Visit(node);
                return walker.m_code.ToString();

            }

            private SyntaxNode m_parentNode;

            public override void VisitGenericName(GenericNameSyntax node)
            {
                var idt = node.GetFirstToken();
                var loadOneAsync = idt.RawKind == (int)SyntaxKind.IdentifierToken && idt.Text == "LoadOneAsync";
                if (!loadOneAsync)
                {
                    base.VisitGenericName(node);
                    return;
                }


                m_code.Append("loadOneAsync(");
                m_code.AppendFormat("\"{0}\",", ContextGenericTypeWalker.Walk(node));
                m_code.AppendFormat("\"{0}\"", ContextPredicateLambdaWalker.Walk(m_parentNode));

                m_code.Append(")");

                base.VisitGenericName(node);
            }

        }

        class ContextGenericTypeWalker : CSharpSyntaxWalker
        {
            public static string Walk(SyntaxNode node)
            {

                var walker = new ContextGenericTypeWalker();
                walker.Visit(node);
                return walker.m_genericName;

            }
            private string m_genericName;
            public override void VisitTypeArgumentList(TypeArgumentListSyntax node)
            {
                var identifierWalker = new GenericArgumentTypeWalker();
                identifierWalker.Visit(node);
                m_genericName = identifierWalker.Text;

                base.VisitTypeArgumentList(node);
            }
        }
        class ContextPredicateLambdaWalker : CSharpSyntaxWalker
        {
            private readonly StringBuilder m_code = new StringBuilder();
            public static string Walk(SyntaxNode node)
            {
                var walker = new ContextPredicateLambdaWalker();
                walker.Visit(node);
                return walker.m_code.ToString();

            }
            public override void VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
            {
                m_code.Append("***2*" + node);
                base.VisitSimpleLambdaExpression(node);
            }

            public override void VisitEqualsValueClause(EqualsValueClauseSyntax node)
            {
                m_code.Append("***" + node);
                base.VisitEqualsValueClause(node);
            }

            public override void VisitNameEquals(NameEqualsSyntax node)
            {
                m_code.Append("*=*=*=*");
            
                base.VisitNameEquals(node);
            }
        }

        class GenericArgumentTypeWalker : CSharpSyntaxWalker
        {
            public string Text { get; private set; }
            public override void VisitIdentifierName(IdentifierNameSyntax node)
            {
                var token = node.ChildTokens().FirstOrDefault(x => x.RawKind == (int)SyntaxKind.IdentifierToken);
                this.Text = token.Text;
                base.VisitIdentifierName(node);
            }
        }
    }
}