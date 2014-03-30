namespace Bespoke.Sph.Domain
{
    public partial class JavascriptExpressionField : Field
    {

        public override object GetValue(RuleContext context)
        {
            return this;
        }

        public override string ToString()
        {
            return this.Expression;
        }
    }
}