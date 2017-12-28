using System;
using System.Collections.Generic;
using Bespoke.Sph.Domain;
using Microsoft.OData.UriParser;

namespace odata.queryparser
{
    public class ODataQueryNodeVisitor : QueryNodeVisitor<string>
    {
        private readonly List<Filter> m_filters = new List<Filter>();
        private Filter m_filter = new Filter();

        public List<Filter> GetFilters()
        {
            return m_filters;
        }

        public override string Visit(SingleValueFunctionCallNode nodeIn)
        {
            throw new NotImplementedException();
        }

        public override string Visit(NamedFunctionParameterNode nodeIn)
        {
            throw new NotImplementedException();
        }

        public override string Visit(ConstantNode nodeIn)
        {
            var cf = new ConstantField
            {
                Value = $"{nodeIn.Value}",
                Type = nodeIn.Value?.GetType()
            };
            m_filter.Field = cf;
            return "ConstantNode";
        }

        public override string Visit(BinaryOperatorNode nodeIn)
        {
            nodeIn.Left.Accept(this);
            nodeIn.Right.Accept(this);

            switch (nodeIn.OperatorKind)
            {
                case BinaryOperatorKind.Equal:
                    m_filter.Operator = Operator.Eq;
                    break;
                case BinaryOperatorKind.NotEqual:
                    m_filter.Operator = Operator.Neq;
                    break;
                case BinaryOperatorKind.GreaterThan:
                    m_filter.Operator = Operator.Gt;
                    break;
                case BinaryOperatorKind.GreaterThanOrEqual:
                    m_filter.Operator = Operator.Ge;
                    break;
                case BinaryOperatorKind.LessThan:
                    m_filter.Operator = Operator.Lt;
                    break;
                case BinaryOperatorKind.LessThanOrEqual:
                    m_filter.Operator = Operator.Le;
                    break;
                case BinaryOperatorKind.Has:
                    throw new NotImplementedException();
                case BinaryOperatorKind.Or:
                    //TODO: BinaryOrFilter
                    return "BinaryOperatorNode";
                case BinaryOperatorKind.And:
                    //TODO:
                    return "BinaryOperatorNode";
                case BinaryOperatorKind.Add:
                    throw new NotImplementedException();
                case BinaryOperatorKind.Subtract:
                    throw new NotImplementedException();
                case BinaryOperatorKind.Multiply:
                    throw new NotImplementedException();
                case BinaryOperatorKind.Divide:
                    throw new NotImplementedException();
                case BinaryOperatorKind.Modulo:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }

            m_filters.Add(m_filter);
            m_filter = new Filter();

            return "BinaryOperatorNode";
        }

        public override string Visit(SingleValuePropertyAccessNode nodeIn)
        {
            m_filter.Term = nodeIn.Property.Name;
            return "SingleValuePropertyAccessNode";
        }

        public override string Visit(SingleValueOpenPropertyAccessNode nodeIn)
        {
            throw new NotImplementedException();
        }

        public override string Visit(ConvertNode nodeIn)
        {
            nodeIn.Source.Accept(this);
            return "ConvertNode";
        }

        public override string Visit(AnyNode nodeIn)
        {
            throw new NotImplementedException();
        }

        public override string Visit(CollectionNavigationNode nodeIn)
        {
            throw new NotImplementedException();
        }

        public override string Visit(SingleNavigationNode nodeIn)
        {
            throw new NotImplementedException();
        }

        public override string Visit(UnaryOperatorNode nodeIn)
        {
            throw new NotImplementedException();
        }
    }
}