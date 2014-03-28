namespace Bespoke.Sph.Domain
{
    public class JavascriptExpressionField : Field
    {
        public string Expression { get; set; }

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