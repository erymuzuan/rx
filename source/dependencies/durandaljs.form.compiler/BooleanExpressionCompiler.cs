using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(BooleanExpressionCompiler))]
    public partial class BooleanExpressionCompiler
    {
        public SnippetCompilerResult Compile(string expression, EntityDefinition entity)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return new SnippetCompilerResult { Code = "true" };

            var file = new StringBuilder();
            file.AppendLine("using System;");
            file.AppendLine("using System.Linq;");
            file.AppendLine();
            file.AppendLine("namespace Bespoke." + ConfigurationManager.ApplicationName + "_" + entity.Id + ".Domain");
            file.AppendLine("{");
            file.AppendLine("   public class BooleanExpression");
            file.AppendLine("   {");
            file.AppendLinf("       public bool Evaluate({0} item, ConfigurationManager config)  ", entity.Name);
            file.AppendLine("       {");
            file.AppendLinf("           return {0};", expression);
            file.AppendLine("       }");
            file.AppendLine("   }");
            file.AppendLine("}");

            var trees = new ObjectCollection<CSharpSyntaxTree>();

            var tree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(file.ToString());
            var root = (CompilationUnitSyntax)tree.GetRoot();
            trees.Add(tree);


            var codes = from c in entity.GenerateCode()
                        where !c.Key.EndsWith("Controller")
                        where !c.Key.EndsWith("Controller.cs")
                        let x = c.Value.Replace("using Bespoke.Sph.Web.Helpers;", string.Empty)
                        .Replace("using System.Web.Mvc;", string.Empty)
                        .Replace("using System.Linq;", string.Empty)
                        .Replace("using System.Threading.Tasks;", string.Empty)
                        select (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(x);
            trees.AddRange(codes.ToArray());
            trees.Add(this.ConfigObjectModel(entity));

            var compilation = CSharpCompilation.Create("eval")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReference<object>()
                .AddReference<XmlAttributeAttribute>()
                .AddReference<EntityDefinition>()
                .AddReference<EnumerableQuery>()
                .AddSyntaxTrees(trees);


            var diagnostics = compilation.GetDiagnostics();

            var result = new SnippetCompilerResult { Success = true };
            result.DiagnosticCollection.AddRange(diagnostics.Where(x => x.Id != "CS8019"));
            result.Success = result.DiagnosticCollection.Count == 0;
            result.DiagnosticCollection.ForEach(Console.WriteLine);
            if (!result.Success)
                return result;


            var statement = root.DescendantNodes().OfType<ReturnStatementSyntax>()
                .Single()
                .Expression;

            result.Code = CompileExpression(statement);
            return result;
        }


        private string CompileExpression(SyntaxNode statement)
        {
            if (statement is LiteralExpressionSyntax)
            {
                switch (statement.CSharpKind())
                {
                    case SyntaxKind.TrueLiteralExpression:
                        return "true";
                    case SyntaxKind.FalseLiteralExpression:
                        return "false";
                    default: throw new NotSupportedException("\"" + statement.GetText() + "\" expression is not supported");
                }
            }

            var parenthesiz = statement as ParenthesizedExpressionSyntax;
            if (null != parenthesiz)
            {
                return "(" + this.CompileExpression(parenthesiz.Expression) + ")";
            }
            var bes = statement as BinaryExpressionSyntax;
            if (bes != null)
            {
                switch (bes.RawKind)
                {
                    case (int)SyntaxKind.LogicalAndExpression:
                        return this.CompileExpression(bes.Left)
                               + " && "
                               + this.CompileExpression(bes.Right);
                    case (int)SyntaxKind.LogicalOrExpression:
                        return this.CompileExpression(bes.Left)
                               + " || "
                               + this.CompileExpression(bes.Right);
                    case (int)SyntaxKind.LogicalNotExpression:
                        throw new Exception("Not implemented for LogicalNotExpression :" + statement.GetText());
                }
            }

            if (statement is PrefixUnaryExpressionSyntax)
            {
                return UnaryExpressionWalker.Walk(statement);
            }


            var code = BinaryExpressionWalker.Walk(statement);
            if (string.IsNullOrWhiteSpace(code))
                code = ConfigMemberAcessExpressionWalker.Walk(statement);
            if (string.IsNullOrWhiteSpace(code))
                code = ItemMemberAccessExpressionWalker.Walk(statement);
            if (string.IsNullOrWhiteSpace(code) || code == "!".Trim())
                code += MethodInvocationExpressionWalker.Walk(statement);

            return code;
        }

        class UnaryExpressionWalker : CSharpSyntaxWalker
        {
            private readonly StringBuilder m_code = new StringBuilder();
            internal static string Walk(SyntaxNode node)
            {
                var walker = new UnaryExpressionWalker();
                walker.Visit(node);
                return walker.m_code.ToString();
            }

            public override void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
            {
                var ot = node.OperatorToken.RawKind;
                if (ot == (int)SyntaxKind.ExclamationToken)
                    m_code.Append("!");

                m_code.Append(ItemMemberAccessExpressionWalker.Walk(node.Operand));
                m_code.Append(MethodInvocationExpressionWalker.Walk(node.Operand));
                base.VisitPrefixUnaryExpression(node);
            }


        }




    }


}