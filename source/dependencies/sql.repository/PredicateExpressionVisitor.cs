using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;

namespace Bespoke.Sph.SqlRepository
{
    public class PredicateExpressionVisitor<T> : DynamicExpressionVisitor where T : Entity, new()
    {
        public ILogger Logger { get; }
        private StringBuilder m_sql = new StringBuilder();

        public PredicateExpressionVisitor(ILogger logger)
        {
            Logger = logger;
        }
        public string Visit(Expression<Func<T, bool>> predicate)
        {
            Visit(predicate.Body);
            Logger.WriteDebug(predicate.ToString());
            if (m_sql.Length > 0)
                return " WHERE " + m_sql;
            return m_sql.ToString();
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);
            WriteSqlOperator(node);
            Visit(node.Right);
            return base.VisitBinary(node);
        }

        private void WriteSqlOperator(Expression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    break;
                case ExpressionType.AddChecked:
                    break;
                case ExpressionType.And:
                    break;
                case ExpressionType.AndAlso:
                    break;
                case ExpressionType.ArrayLength:
                    break;
                case ExpressionType.ArrayIndex:
                    break;
                case ExpressionType.Call:
                    break;
                case ExpressionType.Coalesce:
                    break;
                case ExpressionType.Conditional:
                    break;
                case ExpressionType.Constant:
                    break;
                case ExpressionType.Convert:
                    break;
                case ExpressionType.ConvertChecked:
                    break;
                case ExpressionType.Divide:
                    break;
                case ExpressionType.Equal:
                    m_sql.Append(" = ");
                    break;
                case ExpressionType.ExclusiveOr:
                    break;
                case ExpressionType.GreaterThan:
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    break;
                case ExpressionType.Invoke:
                    break;
                case ExpressionType.Lambda:
                    break;
                case ExpressionType.LeftShift:
                    break;
                case ExpressionType.LessThan:
                    break;
                case ExpressionType.LessThanOrEqual:
                    break;
                case ExpressionType.ListInit:
                    break;
                case ExpressionType.MemberAccess:
                    break;
                case ExpressionType.MemberInit:
                    break;
                case ExpressionType.Modulo:
                    break;
                case ExpressionType.Multiply:
                    break;
                case ExpressionType.MultiplyChecked:
                    break;
                case ExpressionType.Negate:
                    break;
                case ExpressionType.UnaryPlus:
                    break;
                case ExpressionType.NegateChecked:
                    break;
                case ExpressionType.New:
                    break;
                case ExpressionType.NewArrayInit:
                    break;
                case ExpressionType.NewArrayBounds:
                    break;
                case ExpressionType.Not:
                    break;
                case ExpressionType.NotEqual:
                    break;
                case ExpressionType.Or:
                    break;
                case ExpressionType.OrElse:
                    break;
                case ExpressionType.Parameter:
                    break;
                case ExpressionType.Power:
                    break;
                case ExpressionType.Quote:
                    break;
                case ExpressionType.RightShift:
                    break;
                case ExpressionType.Subtract:
                    break;
                case ExpressionType.SubtractChecked:
                    break;
                case ExpressionType.TypeAs:
                    break;
                case ExpressionType.TypeIs:
                    break;
                case ExpressionType.Assign:
                    break;
                case ExpressionType.Block:
                    break;
                case ExpressionType.DebugInfo:
                    break;
                case ExpressionType.Decrement:
                    break;
                case ExpressionType.Dynamic:
                    break;
                case ExpressionType.Default:
                    break;
                case ExpressionType.Extension:
                    break;
                case ExpressionType.Goto:
                    break;
                case ExpressionType.Increment:
                    break;
                case ExpressionType.Index:
                    break;
                case ExpressionType.Label:
                    break;
                case ExpressionType.RuntimeVariables:
                    break;
                case ExpressionType.Loop:
                    break;
                case ExpressionType.Switch:
                    break;
                case ExpressionType.Throw:
                    break;
                case ExpressionType.Try:
                    break;
                case ExpressionType.Unbox:
                    break;
                case ExpressionType.AddAssign:
                    break;
                case ExpressionType.AndAssign:
                    break;
                case ExpressionType.DivideAssign:
                    break;
                case ExpressionType.ExclusiveOrAssign:
                    break;
                case ExpressionType.LeftShiftAssign:
                    break;
                case ExpressionType.ModuloAssign:
                    break;
                case ExpressionType.MultiplyAssign:
                    break;
                case ExpressionType.OrAssign:
                    break;
                case ExpressionType.PowerAssign:
                    break;
                case ExpressionType.RightShiftAssign:
                    break;
                case ExpressionType.SubtractAssign:
                    break;
                case ExpressionType.AddAssignChecked:
                    break;
                case ExpressionType.MultiplyAssignChecked:
                    break;
                case ExpressionType.SubtractAssignChecked:
                    break;
                case ExpressionType.PreIncrementAssign:
                    break;
                case ExpressionType.PreDecrementAssign:
                    break;
                case ExpressionType.PostIncrementAssign:
                    break;
                case ExpressionType.PostDecrementAssign:
                    break;
                case ExpressionType.TypeEqual:
                    break;
                case ExpressionType.OnesComplement:
                    break;
                case ExpressionType.IsTrue:
                    break;
                case ExpressionType.IsFalse:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private readonly List<MemberExpression> m_visitedMemberExpressions = new List<MemberExpression>();
        protected override Expression VisitMember(MemberExpression node)
        {
            if (m_visitedMemberExpressions.Contains(node))
                return node;
            m_sql.Append($"[{node.Member.Name}]");
            m_visitedMemberExpressions.Add(node);
            return node;
        }
        private readonly List<ConstantExpression> m_visitedConstants = new List<ConstantExpression>();
        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (m_visitedConstants.Contains(node))
                return node;
            Logger.WriteDebug("Visit CONSTANT " + node.NodeType);
            switch (node.Value)
            {
                case string sv:
                    m_sql.Append($"'{sv}'");
                    break;
                case DateTime date:
                    m_sql.Append($"'{date}'");
                    break;
                default:
                    m_sql.Append($"{node.Value}");
                    break;
            }
            m_visitedConstants.Add(node);
            return node;

        }
    }
}