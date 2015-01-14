using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs.Javascripts
{
    public class Class
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        private readonly Dictionary<string, string> m_dependeciesCollection = new Dictionary<string, string>();
        private readonly ObjectCollection<Field> m_fieldCollection = new ObjectCollection<Field>();
        private readonly ObjectCollection<Function> m_functionCollection = new ObjectCollection<Function>();

        public ObjectCollection<Function> FunctionCollection
        {
            get { return m_functionCollection; }
        }
        public ObjectCollection<Field> FieldCollection
        {
            get { return m_fieldCollection; }
        }
        public Dictionary<string, string> DependeciesCollection
        {
            get { return m_dependeciesCollection; }
        }

        public override string ToString()
        {
            var code = new StringBuilder();
            var keys = this.DependeciesCollection.Keys.Select(x => string.Format("\"{0}\"", x));
            var values = this.DependeciesCollection.Values.Select(x => string.Format("\"{0}\"", x));

            code.AppendLinf("define([{0}],", string.Join(", ", keys));
            code.AppendLinf("   function({0}){{", string.Join(", ", values));

            code.AppendLine("var ");
            code.Append(string.Join(",\r\n", this.FieldCollection.Select(x => x.ToString())));
            code.Append(";");

            code.AppendLine("var ");
            code.Append(string.Join(",\r\n", this.FunctionCollection.Select(x => x.ToString())));
            code.Append(";");


            code.AppendLine("}");
            code.AppendLine(");");
            
            return code.ToString();
        }
    }
}