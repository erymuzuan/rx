using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Api
{
    public class Sql2012OdataPagingTranslator : IOdataPagingProvider
    {
        public string Tranlate(string sql, int page, int size)
        {

            var skipToken = (page - 1) * size;
            var output = new StringBuilder(sql);

            output.AppendLine($"OFFSET {skipToken} ROWS");
            output.AppendLine($"FETCH NEXT {size} ROWS ONLY");

            return output.ToString();
        }

        public string SkipTop(string sql, int skip, int top)
        {
            var output = new StringBuilder(sql);

            output.AppendLine($"OFFSET {skip} ROWS");
            output.AppendLine($"FETCH NEXT {top} ROWS ONLY");

            return output.ToString();
        }
    }
}