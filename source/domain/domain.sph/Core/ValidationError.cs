namespace Bespoke.Sph.Domain
{
    public class ValidationError
    {
        public string PropertyName { get; set; }
        public string Message { get; set; }
        public string ErrorLocation { get; set; }
    }

    public class ValidationResult
    {
        public bool Success { get; set; }
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