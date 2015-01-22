using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs.Javascripts
{
    internal class FunctionDeclaration
    {
        public string Name { get; set; }
        public string Body { get; set; }
        private readonly ObjectCollection<string> m_argumentCollection = new ObjectCollection<string>();

        internal ObjectCollection<string> ArgumentCollection
        {
            get { return m_argumentCollection; }
        }


        public override string ToString()
        {
            var code = new StringBuilder();
            code.AppendLinf("   {0}  = function({1}){{ ", this.Name, string.Join(", ", this.ArgumentCollection));

            code.AppendLine("   " + this.Body);
            code.Append("   }");
            return code.ToString();
        }
    }
}