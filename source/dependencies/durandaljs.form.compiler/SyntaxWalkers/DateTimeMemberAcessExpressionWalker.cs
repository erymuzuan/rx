using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class DateTimeMemberAcessExpressionWalker : CustomObjectSyntaxWalker
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

    }
}