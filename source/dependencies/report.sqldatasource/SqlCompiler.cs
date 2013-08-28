using System;
using System.Linq;
using System.Text;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.SqlReportDataSource
{
    public class SqlCompiler
    {
        private readonly ReportDefinition m_rdl;

        public SqlCompiler(ReportDefinition rdl)
        {
            m_rdl = rdl;
        }

        public string Compile(DataSource dataSource)
        {
            var sql = new StringBuilder("SELECT ");
            var useDefaultColumn = true;
            // for normal aggregate
            foreach (var c in dataSource.EntityFieldCollection.Where(t => !string.IsNullOrWhiteSpace(t.Aggregate)))
            {
                if (c.Aggregate == "GROUP") continue;
                sql.AppendFormat("{0}([{1}]) AS {1}_{0}", c.Aggregate, c.Name);

                useDefaultColumn = false;
            }
            if (useDefaultColumn)
            {
                sql.AppendFormat("[{0}Id], [Data] ", dataSource.EntityName);
            }
            else
            {
                foreach (var c in dataSource.EntityFieldCollection.Where(t => t.Aggregate == "GROUP"))
                {
                    sql.AppendFormat(", [{0}]", c.Name);
                }
            }

            sql.AppendFormat(" FROM [Sph].[{0}] ", dataSource.EntityName);

            if (dataSource.ReportFilterCollection.Any())
                sql.Append("WHERE ");

            var first = true;
            foreach (var filter in dataSource.ReportFilterCollection)
            {
                var value = this.GetFilterValue(filter);
                var op = this.GetFilterOperator(filter);
                if (!first)
                    sql.AppendLine(" AND");
                if (op.Contains("{0}"))
                {
                    sql.AppendFormat("[{0}] ", filter.FieldName);
                    sql.AppendFormat(op, value);
                }
                else
                    sql.AppendFormat("[{0}] {1} {2}", filter.FieldName, op, value);
                // State Eq @State

                first = false;
            }

            // for GROUP
            var groupColumns = dataSource.EntityFieldCollection.Where(t => t.Aggregate == "GROUP").Select(t => "[" + t.Name + "]").ToArray();
            if (groupColumns.Any())
            {
                sql.Append(" GROUP BY ");
                sql.Append(string.Join(", ", groupColumns));
            }

            // for ORDER
            var orderColumns =
                dataSource.EntityFieldCollection.Where(t => !string.IsNullOrWhiteSpace(t.Order))
                    .OrderBy(t => t.OrderPosition);
            if (orderColumns.Any())
            {
                sql.Append(" ORDER BY ");
                var cols = orderColumns.Select(t => "[" + t.Name + "] " + t.Order).ToArray();
                sql.Append(string.Join(", ", cols));

            }



            return sql.ToString();
        }

        public string GetFilterOperator(ReportFilter filter)
        {
            if (null == filter) throw new ArgumentNullException("filter");
            switch (filter.Operator)
            {
                case "Eq": return "=";
                case "Ge": return ">=";
                case "Gt": return ">";
                case "Le": return "<=";
                case "Lt": return "<";
                case "Substringof": return "LIKE '%' + {0} + '%'";
                case "StartsWith": return "LIKE {0} + '%''";
                case "EndsWith": return "LIKE '%' + {0}";
                case "Contains": return "Contains({0})";
            }
            throw new Exception("Whoaaaa");
        }

        public string GetFilterValue(ReportFilter filter)
        {
            if (null == filter) throw new ArgumentNullException("filter");
            if (null == filter.Type) throw new ArgumentException("You must specify the type", "filter");

            object val = filter.Value;
            if (null == filter) throw new ArgumentNullException("filter");
            if (filter.Value.StartsWith("@"))
                return filter.Value;
            if (filter.Value.StartsWith("="))
            {
                var script = filter.Value.Substring(1, filter.Value.Length - 1);
                var engine = ObjectBuilder.GetObject<IScriptEngine>();
                val = engine.Evaluate(script, m_rdl);

            }
            if (null == val) return "NULL";

            if (filter.Type == typeof(DateTime))
                return string.Format("'{0:s}'", val);
            if (filter.Type == typeof(bool))
            {
                if (val is string)
                    return bool.Parse(val as string) ? "1" : "0";
                return (bool)val ? "1" : "0";
            }
            if (filter.Type == typeof(int))
            {
                return string.Format("{0}",val);
            }
            if (filter.Type == typeof(string))
                return string.Format("'{0}'", val);
            // for string
            return string.Format("'{0}'", filter.Value);
        }
    }
}