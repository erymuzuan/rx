using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.FormCompilers.DurandalJs.Javascripts
{
    internal class ReturnStatement
    {
        private readonly Dictionary<string, string> m_valueCollection = new Dictionary<string, string>();

        public Dictionary<string,string> ValueCollection
        {
            get { return m_valueCollection; }
        }

        public override string ToString()
        {
            var code = new StringBuilder();
            code.AppendLine("   var vm = { ");

            var values = this.ValueCollection.Keys.Select(x => string.Format("      {0} : {1}", x, this.ValueCollection[x]));
            code.AppendLine(string.Join(",\r\n", values));
            code.AppendLine("   }");
            code.AppendLine("   return vm;");
            return code.ToString();
        }
    }
}