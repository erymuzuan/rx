using System.Text;

namespace __NAMESPACE__
{
    public class MySqlPagingTranslator 
    {
        public string Translate(string sql, int page, int size)
        {
            var skipToken = (page - 1) * size;
            var output = new StringBuilder(sql);

            output.AppendLine();
            output.AppendFormat("OFFSET {0}, {1}", skipToken, size);

            return output.ToString();
        }
    }
}