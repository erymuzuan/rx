namespace Bespoke.Sph.Domain
{
    public class ValidationError
    {
        public string PropertyName { get; set; }
        public string Message { get; set; }
        public string ErrorLocation { get; set; }
        public override string ToString()
        {
            return $"[{this.PropertyName}]:{this.Message}";
        }
    }
}