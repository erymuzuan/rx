namespace Bespoke.Sph.Domain
{
    public class BuildValidationResult
    {
        public bool Result { get; set; }
        private readonly ObjectCollection<BuildError> m_errors = new ObjectCollection<BuildError>();

        public ObjectCollection<BuildError> Errors
        {
            get { return m_errors; }
        }
        // prop
    }
}