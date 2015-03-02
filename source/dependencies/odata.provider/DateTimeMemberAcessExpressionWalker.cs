using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace Bespoke.Sph.OdataQueryCompilers
{
    [Export(typeof(OdataSyntaxWalker))]
    class DateTimeMemberAcessExpressionWalker : OdataSyntaxWalker
    {
        public const string DATE_TIME = "DateTime";

        protected override bool Filter(IMethodSymbol method)
        {
            return method.ContainingType.Name == DATE_TIME &&
                method.ContainingNamespace.Name == SYSTEM &&
                method.ContainingAssembly.Name == MSCORLIB;
        }

        protected override bool Filter(IFieldSymbol field)
        {
            return field.ContainingType.Name == DATE_TIME &&
                field.ContainingNamespace.Name == SYSTEM &&
                field.ContainingAssembly.Name == MSCORLIB;
        }

        protected override bool Filter(IPropertySymbol prop)
        {
            if (null != prop.ContainingType && prop.ContainingType.Name == DATE_TIME)
            {
                return prop.ContainingNamespace.Name == SYSTEM &&
                    prop.ContainingAssembly.Name == MSCORLIB;
            }
            // if the property type is DateTime, then return false
            // we just interested the current type, property belong to DateTime
            return false;
        }

        protected override bool Filter(INamedTypeSymbol nts)
        {
            return nts.ToString() == typeof(DateTime).FullName;
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.InvocationExpression }; }
        }

        protected override string InferredTypeName
        {
            get { return DATE_TIME; }
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            if (node.Kind() == SyntaxKind.ObjectCreationExpression)
            {
                var creation = (ObjectCreationExpressionSyntax)node;
                var args = creation.ArgumentList.Arguments.Select(x => this.EvaluateExpressionCode(x.Expression)).ToArray();
                var date = new DateTime(int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]));
                if (args.Length == 6)
                    date = new DateTime(int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]),
                                        int.Parse(args[3]), int.Parse(args[4]), int.Parse(args[5]));
                return "DateTime'" + date.ToString("s") + "'";

            }
            return base.Walk(node, model);
        }
    }
}