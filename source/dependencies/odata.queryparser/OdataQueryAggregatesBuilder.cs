using System;
using System.Collections.Generic;
using Bespoke.Sph.Domain;
using Microsoft.OData.UriParser;
using Microsoft.OData.UriParser.Aggregation;

namespace odata.queryparser
{
    public static class OdataQueryAggregatesBuilder
    {
        public static IEnumerable<Aggregate> TryParseNodes(IEnumerable<TransformationNode> transformationNodes)
        {
            var aggregates = new List<Aggregate>();

            foreach (var transformationNode in transformationNodes)
            {
                var node = (AggregateTransformationNode) transformationNode;

                foreach (var aggregateExpression in node.Expressions)
                {
                    var name = aggregateExpression.Alias;
                    var expression = (SingleValuePropertyAccessNode) aggregateExpression.Expression;
                    var path = expression.Property.Name;

                    switch (aggregateExpression.Method)
                    {
                        case AggregationMethod.Max:
                            aggregates.Add(new MaxAggregate(name, path));
                            break;
                        case AggregationMethod.Min:
                            aggregates.Add(new MinAggregate(name, path));
                            break;
                        case AggregationMethod.Sum:
                            aggregates.Add(new SumAggregate(name, path));
                            break;
                        case AggregationMethod.Average:
                            aggregates.Add(new AverageAggregate(name, path));
                            break;
                        case AggregationMethod.CountDistinct:
                            aggregates.Add(new CountDistinctAggregate(name, path));
                            break;
                        case AggregationMethod.VirtualPropertyCount:
                            break;
                        case AggregationMethod.Custom:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return aggregates;
        }
    }
}