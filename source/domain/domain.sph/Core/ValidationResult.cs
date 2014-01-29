namespace Bespoke.Sph.Domain
{
    public class ValidationResult
    {
        public bool Success { get; set; }
        public string ReferenceNo { get; set; }
        private readonly ObjectCollection<ValidationError> m_errors = new ObjectCollection<ValidationError>();

        public ObjectCollection<ValidationError> ValidationErrors
        {
            get
            {
                return m_errors;
            }
        }
    }
}