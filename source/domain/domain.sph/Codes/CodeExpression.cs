using System.Text;

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

        public bool HasAsyncAwait => this.HasAsyncAwaitExpression();
    }
}