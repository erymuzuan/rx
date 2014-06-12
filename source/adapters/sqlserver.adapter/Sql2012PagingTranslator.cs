using System.Text;

namespace __NAMESPACE__
{
    public class SqlPagingTranslator 
    {
        public string Tranlate(string sql, int page, int size)
        {
            var skipToken = (page - 1) * size;
            var output = new StringBuilder(sql);

            output.AppendLine();
            output.AppendFormat("OFFSET {0} ROWS", skipToken);
            output.AppendLine();
            output.AppendFormat("FETCH NEXT {0} ROWS ONLY", size);

            return output.ToString();
        }
    }
}