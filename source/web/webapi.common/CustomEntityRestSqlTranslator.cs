using System.Text.RegularExpressions;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebApi
{
    public class CustomEntityRestSqlTranslator
    {
        private readonly string m_column;
        private readonly string m_table;

        public CustomEntityRestSqlTranslator(string column, string table)
        {
            m_column = column;
            m_table = table;
        }


        private string Translate(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return null;
            var input = filter
                         .Replace(" ne ", " <> ")
                         .Replace(" eq ", " = ")
                         .Replace(" gt ", " > ")
                         .Replace(" ge ", " >= ")
                         .Replace(" gte ", " > ")
                         .Replace(" lt ", " < ")
                         .Replace(" lte ", " <= ")
                         .Replace(" le ", " <= ")
                         .Replace(" in ", " IN ")
                         .Replace("(From ", "([From] ")
                         .Replace("(To ", "([To] ")
                         .Replace("DateTime'", "'");


            var output = Regex.Replace(input, @"^([\w\-]+)", m => "[" + m + "]");
            output = Regex.Replace(output, @" and ([\w\-]+)", m => " AND [" + m.ToString().Replace(" and ", string.Empty) + "]");
            output = Regex.Replace(output, @" or ([\w\-]+)", m => " OR [" + m.ToString().Replace(" or ", string.Empty) + "]");

            if (output.Contains("startswith"))
            {
                output = Regex.Replace(output,
                    @"\[?startswith\]?\((?<col>[\w\-]+),\s*'(?<val>[\w\-]+)'\) = (?<negate>true|false)",
                    m =>$"[{m.Groups["col"].Value}] {(m.Groups["negate"].Value == "true" ? "" : "NOT ")}LIKE '{m.Groups["val"].Value}%'");

                output = Regex.Replace(output,
                    @"\[?startswith\]?\((?<col>[\w\-]+),\s*'(?<val>[\w\-]+)'\)",
                    m => $"[{m.Groups["col"].Value}] LIKE '{m.Groups["val"].Value}%'");
            }

            if (output.Contains("endswith"))
            {
                output = Regex.Replace(output,
                   @"\[?endswith\]?\((?<col>[\w\-]+),\s*'(?<val>[\w\-]+)'\) = (?<negate>true|false)",
                   m => $"[{m.Groups["col"].Value}] {(m.Groups["negate"].Value == "true" ? "" : "NOT ")}LIKE '%{m.Groups["val"].Value}'");

                output = Regex.Replace(output,
                   @"\[?endswith\]?\((?<col>[\w\-]+),\s*'(?<val>[\w\-]+)'\)",
                   m => $"[{m.Groups["col"].Value}] LIKE '%{m.Groups["val"].Value}'");
            }

            if (output.Contains("substringof"))
            {
                output = Regex.Replace(output,
                    @"\[?substringof\]?\('(?<val>[\w\-]+)',\s*(?<col>[\w\-]+)\) = (?<negate>true|false)",
                    m =>$"[{m.Groups["col"].Value}] {(m.Groups["negate"].Value == "true" ? "" : "NOT ")}LIKE '%{m.Groups["val"].Value}%'");

                output = Regex.Replace(output,
                    @"\[?substringof\]?\('(?<val>[\w\-]+)',\s*(?<col>[\w\-]+)\)",
                    m => $"[{m.Groups["col"].Value}] LIKE '%{m.Groups["val"].Value}%'");

            }

            output = output.Replace(" = DateTime ", " [DateTime] ");
            return " WHERE " + output;
        }

        public string Max(string filter)
        {
            return string.Format("SELECT MAX([{0}]) FROM [{2}].[{1}] ", m_column, m_table, ConfigurationManager.ApplicationName) +
                   Translate(filter)
                ;
        }
        public string Min(string filter)
        {
            return string.Format("SELECT MIN([{0}]) FROM [{2}].[{1}] ", m_column, m_table, ConfigurationManager.ApplicationName) +
                   Translate(filter)
                ;
        }
        public string Average(string filter)
        {
            return string.Format("SELECT AVG([{0}]) FROM [{2}].[{1}] ", m_column, m_table, ConfigurationManager.ApplicationName) +
                   Translate(filter)
                ;
        }
        public string Count(string filter)
        {
            return string.Format("SELECT COUNT(*) FROM [{1}].[{0}]  ", m_table, ConfigurationManager.ApplicationName) +
                   Translate(filter)
                ;
        }

        public string Sum(string filter)
        {
            return string.Format("SELECT SUM([{0}]) FROM [{2}].[{1}]  ", m_column, m_table, ConfigurationManager.ApplicationName) +
                   this.Translate(filter)
                ;
        }

        public string Scalar(string filter)
        {
            return string.Format("SELECT [{0}] FROM [{3}].[{1}] {2} ", m_column, m_table, this.Translate(filter), ConfigurationManager.ApplicationName);
        }

        public string Distinct(string filter)
        {
            return string.Format("SELECT DISTINCT [{0}] FROM [{3}].[{1}] {2} ", m_column, m_table, this.Translate(filter), ConfigurationManager.ApplicationName);
        }

        public string Select(string filter, string orderby)
        {


            var sql = string.Format("SELECT [Id], [{1}] FROM [{2}].[{0}]", m_table, "Json", ConfigurationManager.ApplicationName);

            if (!string.IsNullOrEmpty(filter))
                sql = string.Format("SELECT [Id], [{2}] FROM [{3}].[{0}] {1} ", m_table, this.Translate(filter), "Json", ConfigurationManager.ApplicationName);

            if (!string.IsNullOrWhiteSpace(orderby))
            {
                sql += " ORDER BY " + orderby;
            }

            return sql;
        }
    }
}