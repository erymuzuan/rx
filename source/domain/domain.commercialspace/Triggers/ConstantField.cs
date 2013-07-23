namespace Bespoke.SphCommercialSpaces.Domain
{
    public class ConstantField : Field
    {
        public object Value { get; set; }
        public override object GetValue(Entity item)
        {
            return this.Value;
        }
    }
}