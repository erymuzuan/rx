using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Bespoke.Sph.Domain.QueryProviders
{
    /// <summary>
    /// Extended node types for custom expressions
    /// </summary>
    public enum DbExpressionType
    {
        Table = 1000, // make sure these don't overlap with ExpressionType
        Column,
        Select,
        Projection,
        Join
    }

    public static class DbExpressionExtensions
    {
        public static bool IsDbExpression(this ExpressionType et)
        {
            return ((int) et) >= 1000;
        }
    }

    /// <summary>
    /// A custom expression node that represents a table reference in a SQL query
    /// </summary>
    public class TableExpression : Expression
    {
        private readonly Type m_type;
        private readonly string m_alias;
        private readonly string m_name;

        public TableExpression(Type type, string alias, string name)
        {
            m_type = type;
            m_alias = alias;
            m_name = name;
        }

        public override Type Type
        {
            get { return m_type; }
        }

        public override ExpressionType NodeType
        {
            get { return (ExpressionType)DbExpressionType.Table; }
        }

        public string Alias
        {
            get { return m_alias; }
        }

        public string Name
        {
            get { return m_name; }
        }
    }

    /// <summary>
    /// A custom expression node that represents a reference to a column in a SQL query
    /// </summary>
    public class ColumnExpression : Expression
    {
        private readonly Type m_type;
        private readonly string m_alias;
        private readonly string m_name;
        private readonly int m_ordinal;

        public ColumnExpression(Type type, string alias, string name, int ordinal)
        {
            m_type = type;
            m_alias = alias;
            this.m_name = name;
            this.m_ordinal = ordinal;
        }
        public override Type Type
        {
            get { return m_type; }
        }

        public override ExpressionType NodeType
        {
            get { return (ExpressionType)DbExpressionType.Column; }
        }


        public string Alias
        {
            get { return m_alias; }
        }

        public string Name
        {
            get { return m_name; }
        }

        public int Ordinal
        {
            get { return m_ordinal; }
        }
    }

    /// <summary>
    /// A declaration of a column in a SQL SELECT expression
    /// </summary>
    public class ColumnDeclaration
    {
        private readonly Expression m_expression;
        private readonly string m_name;

        public ColumnDeclaration(string name, Expression expression)
        {
            this.m_name = name;
            this.m_expression = expression;
        }

        public string Name
        {
            get { return m_name; }
        }

        public Expression Expression
        {
            get { return m_expression; }
        }
    }

    /// <summary>
    /// An SQL OrderBy order type 
    /// </summary>
    public enum OrderType
    {
        Ascending,
        Descending
    }

    /// <summary>
    /// A pairing of an expression and an order type for use in a SQL Order By clause
    /// </summary>
    public class OrderExpression
    {
        private readonly Expression m_expression;
        private readonly OrderType m_orderType;

        public OrderExpression(OrderType orderType, Expression expression)
        {
            this.m_orderType = orderType;
            this.m_expression = expression;
        }

        public OrderType OrderType
        {
            get { return m_orderType; }
        }

        public Expression Expression
        {
            get { return m_expression; }
        }
    }

    /// <summary>
    /// A custom expression node used to represent a SQL SELECT expression
    /// </summary>
    public class SelectExpression : Expression
    {
        private readonly Type m_type;
        private readonly string m_alias;
        private readonly ReadOnlyCollection<ColumnDeclaration> m_columns;
        private readonly Expression m_from;
        private readonly ReadOnlyCollection<OrderExpression> m_orderBy;
        private readonly Expression m_where;
        //private readonly Expression m_count;

        public SelectExpression(
            Type type, string alias, IEnumerable<ColumnDeclaration> columns,
            Expression from, Expression where, IEnumerable<OrderExpression> orderBy)
        {
            m_type = type;
            this.m_alias = alias;
            this.m_columns = columns as ReadOnlyCollection<ColumnDeclaration> ??
                           new List<ColumnDeclaration>(columns).AsReadOnly();
            this.m_from = from;
            this.m_where = where;
            this.m_orderBy = orderBy as ReadOnlyCollection<OrderExpression>;
            if (this.m_orderBy == null && orderBy != null)
            {
                this.m_orderBy = new List<OrderExpression>(orderBy).AsReadOnly();
            }
        }
        public override Type Type
        {
            get { return m_type; }
        }

        public override ExpressionType NodeType
        {
            get { return (ExpressionType)DbExpressionType.Select; }
        }

        public SelectExpression(
            Type type, string alias, IEnumerable<ColumnDeclaration> columns,
            Expression @from, Expression @where)
            : this(type, alias, columns, @from, @where, null)
        {
        }

        public string Alias
        {
            get { return m_alias; }
        }

        public ReadOnlyCollection<ColumnDeclaration> Columns
        {
            get { return m_columns; }
        }

        public Expression From
        {
            get { return m_from; }
        }

        public Expression Where
        {
            get { return m_where; }
        }


        public ReadOnlyCollection<OrderExpression> OrderBy
        {
            get { return m_orderBy; }
        }
    }

    /// <summary>
    /// A kind of SQL join
    /// </summary>
    public enum JoinType
    {
        CrossJoin,
        InnerJoin,
        CrossApply,
    }

    /// <summary>
    /// A custom expression node representing a SQL join clause
    /// </summary>
    public class JoinExpression : Expression
    {
        private readonly Expression m_condition;
        private readonly Type m_type;
        private readonly JoinType m_joinType;
        private readonly Expression m_left;
        private readonly Expression m_right;

        public JoinExpression(Type type, JoinType joinType, Expression left, Expression right, Expression condition)
        {
            m_type = type;
            this.m_joinType = joinType;
            this.m_left = left;
            this.m_right = right;
            this.m_condition = condition;
        }
        public override Type Type
        {
            get { return m_type; }
        }

        public override ExpressionType NodeType
        {
            get { return (ExpressionType)DbExpressionType.Join; }
        }


        public JoinType Join
        {
            get { return m_joinType; }
        }

        public Expression Left
        {
            get { return m_left; }
        }

        public Expression Right
        {
            get { return m_right; }
        }

        public new Expression Condition
        {
            get { return m_condition; }
        }
    }

    /// <summary>
    /// A custom expression representing the construction of one or more result objects from a 
    /// SQL select expression
    /// </summary>
    public class ProjectionExpression : Expression
    {
        private readonly Expression m_projector;
        private readonly SelectExpression m_source;

        public ProjectionExpression(SelectExpression source, Expression projector)
        {
            this.m_source = source;
            this.m_projector = projector;
        }
         public override Type Type
        {
            get { return m_source.Type; }
        }

        public override ExpressionType NodeType
        {
            get { return (ExpressionType)DbExpressionType.Projection; }
        }

        public SelectExpression Source
        {
            get { return m_source; }
        }

        public Expression Projector
        {
            get { return m_projector; }
        }
    }

    /// <summary>
    /// An extended expression visitor including custom DbExpression nodes
    /// </summary>
    public class DbExpressionVisitor : ExpressionVisitor
    {
        protected override Expression Visit(Expression exp)
        {
            if (exp == null)
            {
                return null;
            }
            switch ((DbExpressionType) exp.NodeType)
            {
                case DbExpressionType.Table:
                    return VisitTable((TableExpression) exp);
                case DbExpressionType.Column:
                    return VisitColumn((ColumnExpression) exp);
                case DbExpressionType.Select:
                    return VisitSelect((SelectExpression) exp);
                case DbExpressionType.Join:
                    return VisitJoin((JoinExpression) exp);
                case DbExpressionType.Projection:
                    return VisitProjection((ProjectionExpression) exp);
                default:
                    return base.Visit(exp);
            }
        }

        protected virtual Expression VisitTable(TableExpression table)
        {
            return table;
        }

        protected virtual Expression VisitColumn(ColumnExpression column)
        {
            return column;
        }

        protected virtual Expression VisitSelect(SelectExpression select)
        {
            Expression from = VisitSource(select.From);
            Expression where = Visit(select.Where);
            ReadOnlyCollection<ColumnDeclaration> columns = VisitColumnDeclarations(select.Columns);
            ReadOnlyCollection<OrderExpression> orderBy = VisitOrderBy(select.OrderBy);
            if (from != select.From || where != select.Where || columns != select.Columns || orderBy != select.OrderBy)
            {
                return new SelectExpression(select.Type, select.Alias, columns, from, where, orderBy);
            }
            return select;
        }

        protected virtual Expression VisitJoin(JoinExpression join)
        {
            Expression left = VisitSource(join.Left);
            Expression right = VisitSource(join.Right);
            Expression condition = Visit(join.Condition);
            if (left != join.Left || right != join.Right || condition != join.Condition)
            {
                return new JoinExpression(join.Type, join.Join, left, right, condition);
            }
            return join;
        }

        protected virtual Expression VisitSource(Expression source)
        {
            return Visit(source);
        }

        protected virtual Expression VisitProjection(ProjectionExpression proj)
        {
            var source = (SelectExpression) Visit(proj.Source);
            Expression projector = Visit(proj.Projector);
            if (source != proj.Source || projector != proj.Projector)
            {
                return new ProjectionExpression(source, projector);
            }
            return proj;
        }

        protected ReadOnlyCollection<ColumnDeclaration> VisitColumnDeclarations(
            ReadOnlyCollection<ColumnDeclaration> columns)
        {
            List<ColumnDeclaration> alternate = null;
            for (int i = 0, n = columns.Count; i < n; i++)
            {
                ColumnDeclaration column = columns[i];
                Expression e = Visit(column.Expression);
                if (alternate == null && e != column.Expression)
                {
                    alternate = columns.Take(i).ToList();
                }
                if (alternate != null)
                {
                    alternate.Add(new ColumnDeclaration(column.Name, e));
                }
            }
            if (alternate != null)
            {
                return alternate.AsReadOnly();
            }
            return columns;
        }

        protected ReadOnlyCollection<OrderExpression> VisitOrderBy(ReadOnlyCollection<OrderExpression> expressions)
        {
            if (expressions != null)
            {
                List<OrderExpression> alternate = null;
                for (int i = 0, n = expressions.Count; i < n; i++)
                {
                    OrderExpression expr = expressions[i];
                    Expression e = Visit(expr.Expression);
                    if (alternate == null && e != expr.Expression)
                    {
                        alternate = expressions.Take(i).ToList();
                    }
                    if (alternate != null)
                    {
                        alternate.Add(new OrderExpression(expr.OrderType, e));
                    }
                }
                if (alternate != null)
                {
                    return alternate.AsReadOnly();
                }
            }
            return expressions;
        }
    }
}