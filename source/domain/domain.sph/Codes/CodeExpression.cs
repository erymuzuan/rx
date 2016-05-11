using System.Text;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain.Codes
{
    public class CodeExpression
    {
        private readonly string m_expression;

        public static CodeExpression Load(string expression)
        {
            return new CodeExpression(expression);
        }
        public static CodeExpression Load(StringBuilder expression)
        {
            return new CodeExpression(expression.ToString());
        }

        public CodeExpression(string expression)
        {
            m_expression = expression;
        }

        public override string ToString()
        {
            return m_expression;
        }
        [JsonIgnore]
        public bool HasAsyncAwait => this.HasAsyncAwaitExpression();
        [JsonIgnore]
        public bool IsEmpty => string.IsNullOrWhiteSpace(m_expression);
    }
}