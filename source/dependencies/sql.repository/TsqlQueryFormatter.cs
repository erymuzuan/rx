using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Bespoke.SphCommercialSpaces.Domain.QueryProviders;

namespace Bespoke.Sph.SqlRepository
{
    internal class TsqlQueryFormatter : DbExpressionVisitor
    {
        StringBuilder m_sb;
        int m_indent = 2;
        int m_depth;

        internal string Format(Expression expression)
        {
            this.m_sb = new StringBuilder();
            this.Visit(expression);
            return this.m_sb.ToString();
        }

        protected enum Indentation
        {
            Same,
            Inner,
            Outer
        }

        internal int IndentationWidth
        {
            get { return this.m_indent; }
            set { this.m_indent = value; }
        }

        private void AppendNewLine(Indentation style)
        {
            m_sb.AppendLine();
            this.Indent(style);
            for (int i = 0, n = this.m_depth * this.m_indent; i < n; i++)
            {
                m_sb.Append(" ");
            }
        }

        private void Indent(Indentation style)
        {
            if (style == Indentation.Inner)
            {
                this.m_depth++;
            }
            else if (style == Indentation.Outer)
            {
                this.m_depth--;
                System.Diagnostics.Debug.Assert(this.m_depth >= 0);
            }
        }

        private string GetPropertyName(Expression expression)
        {
            string propertyName01;
            var cl02 = expression as ColumnExpression;

            if (null != cl02)
                propertyName01 = cl02.Name;
            else
            {
                //dynamic propEx01 = m.Object;
                //propertyName = propEx01.Member.Name;

                // property expression is an internal class
                var ob = string.Format("{0}", expression);
                var cutoff = ob.LastIndexOf("}", StringComparison.Ordinal) + 2;
                propertyName01 = (ob.Substring(cutoff, ob.Length - cutoff)).Replace(".", "");
            }
            return propertyName01;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            // local collection should do SELECT * FROM Table WHERE Column IN (1,2,3)
            if (m.Method.Name == "Contains" && m.Object == null && m.Arguments.Count == 2)
            {
                string propertyName01 = GetPropertyName(m.Arguments[1]);
                m_sb.AppendFormat(" [{0}] IN(", propertyName01);

                if (m.Arguments[0].Type == typeof(string[]))
                {
                    dynamic list = m.Arguments[0];
                    var flatted = ((string[])list.Value).Select(s => s.Replace("'", "''"));
                    var cccs = string.Join("','", flatted);
                    m_sb.AppendFormat("'{0}'", cccs);
                }
                if (m.Arguments[0].Type == typeof(int[]))
                {
                    dynamic list = m.Arguments[0];
                    var cccs = string.Join(",", (int[])list.Value);
                    m_sb.Append(cccs);
                }
                if (m.Arguments[0].Type == typeof(int?[]))
                {
                    dynamic list = m.Arguments[0];
                    var cccs = string.Join(",", (int?[])list.Value);
                    m_sb.Append(cccs);
                }
                m_sb.Append(")");
                return m;
            }

            var method = m.Method;
            string propertyName = GetPropertyName(m.Object);

            switch (method.Name)
            {
                case "Count":
                case "Distinct":
                    m_sb.Append("Fuck count");
                    return m;
                case "StartsWith":
                    m_sb.Append("(");
                    m_sb.AppendFormat(" [{0}] LIKE ", propertyName);
                    this.Visit(m.Arguments[0]);
                    m_sb.Append(" + '%')");
                    return m;
                case "Contains":
                    m_sb.Append("(");
                    m_sb.AppendFormat(" [{0}] LIKE '%' + ", propertyName);
                    this.Visit(m.Arguments[0]);
                    m_sb.Append(" + '%')");
                    return m;
                case "Equals":
                    m_sb.Append("(");
                    m_sb.AppendFormat(" [{0}] = '", propertyName);
                    this.Visit(m.Arguments[0]);
                    m_sb.Append("')");
                    return m;
                case "EndsWith":
                    m_sb.Append("(");
                    m_sb.AppendFormat(" [{0}] LIKE '%' + ", propertyName);
                    this.Visit(m.Arguments[0]);
                    m_sb.Append(")");
                    return m;
                default: throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));

            }
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Convert:
                    this.Visit(u.Operand);
                    break;
                case ExpressionType.Not:
                    m_sb.Append(" NOT ");
                    this.Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            m_sb.Append("(");
            this.Visit(b.Left);
            var mb = b.Left as MemberExpression;
            if (null != mb)
            {
                var inner = mb.Expression as MemberExpression;
                if (null != inner)
                    m_sb.Append(inner.Member.Name);

                m_sb.Append(mb.Member.Name);
            }

            var ce = b.Left as ColumnExpression;
            if (null != ce && ce.Type == typeof(bool))
            {
                if(!(b.Right is ConstantExpression))
                    m_sb.Append(" = 1 ");
            }

            switch (b.NodeType)
            {
                case ExpressionType.And:
                    m_sb.Append(" AND ");
                    break;
                case ExpressionType.Or:
                    m_sb.Append(" OR ");
                    break;
                case ExpressionType.Equal:
                    m_sb.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    m_sb.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    m_sb.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    m_sb.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    m_sb.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    m_sb.Append(" >= ");
                    break;
                case ExpressionType.AndAlso:
                    m_sb.Append(" AND ");
                    break;
                case ExpressionType.OrElse:
                    m_sb.Append(" OR ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
            }
            this.Visit(b.Right);

            m_sb.Append(")");
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c.Value == null)
            {
                m_sb.Append("NULL");
            }
            else
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        m_sb.Append(((bool)c.Value) ? 1 : 0);
                        break;
                    case TypeCode.String:
                        m_sb.Append("'");
                        var val = c.Value as string;
                        if (!string.IsNullOrWhiteSpace(val))
                            val = val.Replace("'", "''");
                        m_sb.Append(val);
                        m_sb.Append("'");
                        break;
                    case TypeCode.DateTime:
                        m_sb.Append("'");
                        m_sb.AppendFormat("{0:s}", c.Value);
                        m_sb.Append("'");
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));
                    default:
                        m_sb.Append(c.Value);
                        break;
                }
            }
            return c;
        }

        protected override Expression VisitColumn(ColumnExpression column)
        {
            m_sb.AppendFormat("[{0}]", column.Name);
            return column;
        }

        protected override Expression VisitSelect(SelectExpression select)
        {
            /*    */
            m_sb.Append("SELECT ");
            /**/
            if (select.Columns.Count == 1) // just for single
            {
                for (int i = 0, n = select.Columns.Count; i < n; i++)
                {
                    ColumnDeclaration column = select.Columns[i];
                    if (column.Name.StartsWith("PropertyName")) continue;
                    if (i > 0)
                    {
                        m_sb.Append(", ");
                    }
                    var c = this.Visit(column.Expression) as ColumnExpression;
                    if (c == null || c.Name != select.Columns[i].Name)
                    {
                        // m_sb.Append(" AS ");
                        m_sb.Append(column.Name);
                    }
                }
            }
            else
            {
                m_sb.Append(" [Data] ");

            }


            if (select.From != null)
            {
                this.AppendNewLine(Indentation.Same);
                m_sb.Append("FROM [Sph].[");
                this.VisitSource(select.From);
                m_sb.Append("] ");
            }

            if (select.Where != null)
            {
                this.AppendNewLine(Indentation.Same);
                m_sb.Append("WHERE ");
                this.Visit(select.Where);
            }
            if (select.OrderBy != null && select.OrderBy.Count > 0)
            {
                this.AppendNewLine(Indentation.Same);
                m_sb.Append("ORDER BY ");
                for (int i = 0, n = select.OrderBy.Count; i < n; i++)
                {
                    OrderExpression exp = select.OrderBy[i];
                    if (i > 0)
                    {
                        m_sb.Append(", ");
                    }
                    this.Visit(exp.Expression);
                    if (exp.OrderType != OrderType.Ascending)
                    {
                        m_sb.Append(" DESC");
                    }
                }
            }
            return select;
        }

        protected override Expression VisitSource(Expression source)
        {
            switch ((DbExpressionType)source.NodeType)
            {
                case DbExpressionType.Table:
                    var table = (TableExpression)source;
                    m_sb.Append(table.Name);
                    break;
                case DbExpressionType.Select:
                    var select = (SelectExpression)source;
                    m_sb.Append("(");
                    this.AppendNewLine(Indentation.Inner);
                    this.Visit(select);
                    this.AppendNewLine(Indentation.Outer);
                    m_sb.Append(")");
                    m_sb.Append(" AS ");
                    m_sb.Append(select.Alias);
                    break;
                case DbExpressionType.Join:
                    this.VisitJoin((JoinExpression)source);
                    break;
                default:
                    throw new InvalidOperationException("Select source is not valid type");
            }
            return source;
        }

        protected override Expression VisitJoin(JoinExpression join)
        {
            this.VisitSource(join.Left);
            this.AppendNewLine(Indentation.Same);
            switch (join.Join)
            {
                case JoinType.CrossJoin:
                    m_sb.Append("CROSS JOIN ");
                    break;
                case JoinType.InnerJoin:
                    m_sb.Append("INNER JOIN ");
                    break;
                case JoinType.CrossApply:
                    m_sb.Append("CROSS APPLY ");
                    break;
            }
            this.VisitSource(join.Right);
            if (join.Condition != null)
            {
                this.AppendNewLine(Indentation.Inner);
                m_sb.Append("ON ");
                this.Visit(join.Condition);
                this.Indent(Indentation.Outer);
            }
            return join;
        }
    }
}
