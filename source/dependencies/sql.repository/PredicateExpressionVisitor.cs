using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;

namespace Bespoke.Sph.SqlRepository
{
    public class PredicateExpressionVisitor<T> : DynamicExpressionVisitor where T : Entity, new()
    {
        public ILogger Logger { get; }
        private readonly Dictionary<Expression, string> m_sql = new Dictionary<Expression, string>();

        public PredicateExpressionVisitor(ILogger logger)
        {
            Logger = logger;
        }
        public string Visit(Expression<Func<T, bool>> predicate)
        {
            Visit(predicate.Body);
            Logger.WriteDebug(predicate.ToString());
            if (m_sql.Count > 0)
                return " WHERE " + m_sql.Values.ToString(" ");
            return string.Empty;
        }

        private int m_count;
        public override Expression Visit(Expression node)
        {
            Logger.WriteDebug($"{++m_count,3} : {node.NodeType}({node.GetType().Name})");
            return base.Visit(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            m_sql.AddOrReplace(node, "");
            var bv = base.VisitUnary(node);
            if (node.NodeType == ExpressionType.Not)
            {
                if (m_sql.ContainsKey(node.Operand))
                    m_sql[node.Operand] = m_sql[node.Operand].Replace(" = 1", " = 0");
            }
            return bv;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);
            WriteSqlOperator(node);
            Visit(node.Right);
            return base.VisitBinary(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Type == typeof(bool))
            {
                m_sql.AddIfNotExist(node, $"[{node.Member.Name}] = 1");
                return node;
            }
            m_sql.AddIfNotExist(node, $"[{node.Member.Name}]");
            return node;
        }
        protected override Expression VisitConstant(ConstantExpression node)
        {
            switch (node.Value)
            {
                case string sv:
                    m_sql.AddIfNotExist(node, $"'{sv}'");
                    break;
                case DateTime date:
                    m_sql.AddIfNotExist(node, $"'{date:O}'");
                    break;
                case bool bv:
                    m_sql.AddIfNotExist(node, bv ? "1" : "0");
                    break;
                default:
                    m_sql.AddIfNotExist(node, $"{node.Value}");
                    break;
            }
            return node;

        }

        private void WriteSqlOperator(Expression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    m_sql.AddIfNotExist(node, " + ");
                    break;
                case ExpressionType.AddChecked:
                    break;
                case ExpressionType.And:
                    break;
                case ExpressionType.AndAlso:
                    m_sql.AddIfNotExist(node, " AND ");
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
                    m_sql.AddIfNotExist(node, " / ");
                    break;
                case ExpressionType.Equal:
                    m_sql.AddIfNotExist(node, " = ");
                    break;
                case ExpressionType.ExclusiveOr:
                    break;
                case ExpressionType.GreaterThan:
                    m_sql.AddIfNotExist(node, " > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    m_sql.AddIfNotExist(node, " >= ");
                    break;
                case ExpressionType.Invoke:
                    break;
                case ExpressionType.Lambda:
                    break;
                case ExpressionType.LeftShift:
                    break;
                case ExpressionType.LessThan:
                    m_sql.AddIfNotExist(node, " < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    m_sql.AddIfNotExist(node, " <= ");
                    break;
                case ExpressionType.ListInit:
                    break;
                case ExpressionType.MemberAccess:
                    break;
                case ExpressionType.MemberInit:
                    break;
                case ExpressionType.Modulo:
                    m_sql.AddIfNotExist(node, " % ");
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
                    m_sql.AddIfNotExist(node, " NOT ");
                    break;
                case ExpressionType.NotEqual:
                    m_sql.AddIfNotExist(node, " <> ");
                    break;
                case ExpressionType.Or:
                    break;
                case ExpressionType.OrElse:
                    m_sql.AddIfNotExist(node, " OR ");
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
                    m_sql.AddIfNotExist(node, " = 1");
                    break;
                case ExpressionType.IsFalse:
                    m_sql.AddIfNotExist(node, " = 0");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}