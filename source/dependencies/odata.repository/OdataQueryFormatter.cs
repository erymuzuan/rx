using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Bespoke.SphCommercialSpaces.Domain.QueryProviders;

namespace Bespoke.Sph.OdataRepository
{
    /// <summary>
    /// OdataQueryFormatter is a visitor that converts an bound expression tree into SQL query text
    /// </summary>
    internal class OdataQueryFormatter : DbExpressionVisitor
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
                var column = GetPropertyName(m.Arguments[1]);

                if (m.Arguments[0].Type == typeof(string[]))
                {
                    dynamic list = m.Arguments[0];
                    var flatted = ((string[])list.Value).Select(s => s.Replace("'", "''"))
                        .Select(c => string.Format("({0} eq '{1}')", column, c));
                    var cccs = string.Join(" OR ", flatted);
                    m_sb.AppendFormat("{0}", cccs);
                }
                if (m.Arguments[0].Type == typeof(int[]))
                {
                    dynamic list = m.Arguments[0];
                    var arrays = ((int[])list.Value).Select(c => string.Format("({0} eq {1})", column, c));
                    var cccs = string.Join(" OR ", arrays);
                    m_sb.Append(cccs);
                }
                if (m.Arguments[0].Type == typeof(int?[]))
                {
                    throw new NotSupportedException("Not yet implemented for int?[]");
                }
                return m;
            }

            var method = m.Method;
            string propertyName = null;
            if (method.Name == "IsEqual")
            {
                var col = m.Arguments[0] as ColumnExpression;
                if (null == col)
                {
                    Console.WriteLine(m.Arguments[0]);
                }
                if (null != col)
                    propertyName = col.Name;

                if (string.IsNullOrWhiteSpace(propertyName))
                    throw new Exception("IsEqual is used with enum only, not Nullable enum");
            }
            else
            {

                var column = m.Object as ColumnExpression;
                if (null != column)
                    propertyName = column.Name;
                else
                {
                    var ob = string.Format("{0}", m.Object);
                    var cutoff = ob.LastIndexOf("}") + 2;
                    propertyName = (ob.Substring(cutoff, ob.Length - cutoff));
                    propertyName = propertyName.Replace(".", string.Empty);
                }
            }

            switch (method.Name)
            {
                case "StartsWith":
                    m_sb.AppendFormat("startswith({0},", propertyName);
                    this.Visit(m.Arguments[0]);
                    m_sb.Append(") eq true");
                    return m;
                case "Contains":
                    m_sb.Append("substringof(");
                    this.Visit(m.Arguments[0]);
                    m_sb.AppendFormat(" ,{0}) eq true", propertyName);
                    return m;
                case "EndsWith":
                    m_sb.AppendFormat("endswith({0},", propertyName);
                    this.Visit(m.Arguments[0]);
                    m_sb.Append(") eq true");
                    return m;
                case "Equals": // for enum
                    m_sb.AppendFormat("{0} eq '", propertyName);
                    this.Visit(m.Arguments[0]);
                    m_sb.Append("'");
                    return m;
                case "IsEqual": // for enum
                    m_sb.AppendFormat("{0} eq '", propertyName);
                    this.Visit(m.Arguments[1]);
                    m_sb.Append("'");
                    return m;
                default: throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));

            }

        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    m_sb.Append(" NOT ");
                    this.Visit(u.Operand);
                    break;
                case ExpressionType.Convert: // for enum
                    if (!m_sb.ToString().EndsWith("$filter="))
                        m_sb.Append(" eq ");
                    this.Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            this.Visit(b.Left);
            var mb = b.Left as MemberExpression;
            if (null != mb)
            {
                var inner = mb.Expression as MemberExpression;
                if (null != inner)
                    m_sb.Append(inner.Member.Name);

                m_sb.Append(mb.Member.Name);
            }
            switch (b.NodeType)
            {
                case ExpressionType.And:
                    m_sb.Append(" and ");
                    break;
                case ExpressionType.Or:
                    m_sb.Append(" or ");
                    break;
                case ExpressionType.Equal:
                    if (!m_sb.ToString().EndsWith(" eq "))
                        m_sb.Append(" eq ");
                    break;
                case ExpressionType.NotEqual:
                    m_sb.Append(" ne ");
                    break;
                case ExpressionType.LessThan:
                    m_sb.Append(" lt ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    m_sb.Append(" le ");
                    break;
                case ExpressionType.GreaterThan:
                    m_sb.Append(" gt ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    m_sb.Append(" ge ");
                    break;
                case ExpressionType.AndAlso:
                    m_sb.Append(" and ");
                    break;
                case ExpressionType.OrElse:
                    m_sb.Append(" or ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
            }
            this.Visit(b.Right);
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c.Value == null)
            {
                m_sb.Append("null");
                return c;
            }

            switch (Type.GetTypeCode(c.Value.GetType()))
            {
                case TypeCode.Boolean:
                    m_sb.Append(((bool)c.Value) ? "true" : "false");
                    break;
                case TypeCode.String:
                    m_sb.Append("'");
                    m_sb.Append(System.Web.HttpUtility.UrlEncode(c.Value as string));
                    m_sb.Append("'");
                    break;
                case TypeCode.DateTime:
                    m_sb.Append("DateTime'");
                    m_sb.AppendFormat("{0:s}", c.Value);
                    m_sb.Append("'");
                    break;
                case TypeCode.Object:
                    throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));
                default:
                    m_sb.Append(c.Value);
                    break;
            }

            return c;
        }

        protected override Expression VisitColumn(ColumnExpression column)
        {
            m_sb.Append(column.Name);
            return column;
        }

        protected override Expression VisitSelect(SelectExpression select)
        {
            if (select.Where != null)
            {
                m_sb.Append("$filter=");
                this.Visit(select.Where);
            }
            if (select.OrderBy != null && select.OrderBy.Count > 0)
            {
                m_sb.Append("&$orderby=");
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
                        m_sb.Append(" desc");
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
                    m_sb.Append(" AS ");
                    m_sb.Append(table.Alias);
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
