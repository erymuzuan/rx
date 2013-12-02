using System.Text.RegularExpressions;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Helpers
{
    public class OdataSqlTranslator<T> where T : Entity
    {
        private readonly string m_column;
        private readonly string m_table;

        public OdataSqlTranslator(string column, string table)
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
            output = output.Replace(" = DateTime ", " [DateTime] ");
            return " WHERE " + output;
        }

        public string Max(string filter)
        {
            return string.Format("SELECT MAX([{0}]) FROM [Sph].[{1}] ", m_column, m_table) +
                   Translate(filter)
                ;
        }
        public string Min(string filter)
        {
            return string.Format("SELECT MIN([{0}]) FROM [Sph].[{1}] ", m_column, m_table) +
                   Translate(filter)
                ;
        }
        public string Average(string filter)
        {
            return string.Format("SELECT AVG([{0}]) FROM [Sph].[{1}] ", m_column, m_table) +
                   Translate(filter)
                ;
        }
        public string Count(string filter)
        {
            return string.Format("SELECT COUNT(*) FROM [Sph].[{0}]  ", m_table) +
                   Translate(filter)
                ;
        }

        public string Sum(string filter)
        {
            return string.Format("SELECT SUM([{0}]) FROM [Sph].[{1}]  ", m_column, m_table) +
                   this.Translate(filter)
                ;
        }

        public string Scalar(string filter)
        {
            return string.Format("SELECT [{0}] FROM [Sph].[{1}] {2} ", m_column, m_table, this.Translate(filter));
        }

        public string Distinct(string filter)
        {
            return string.Format("SELECT DISTINCT [{0}] FROM [Sph].[{1}] {2} ", m_column, m_table, this.Translate(filter));
        }

        public string Select(string filter)
        {
            var type = typeof(IRepository<T>);
            dynamic repos = ObjectBuilder.GetObject(type);

            if (string.IsNullOrEmpty(filter))
                return string.Format("SELECT [{0}Id],{1} FROM [Sph].[{0}]", m_table, repos.DataColumn);
            return string.Format("SELECT [{0}Id],{2} FROM [Sph].[{0}] {1} ", m_table, this.Translate(filter), repos.DataColumn);
        }
    }
}