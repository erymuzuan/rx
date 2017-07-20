namespace Bespoke.Sph.Domain
{
    public class ValidationResult
    {
        public bool Success { get; set; }
        public string ReferenceNo { get; set; }
        public ObjectCollection<ValidationError> ValidationErrors { get; } = new ObjectCollection<ValidationError>();
    }
}