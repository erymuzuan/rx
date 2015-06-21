namespace Bespoke.Sph.Domain
{
    public class BuildValidationResult
    {
        public bool Result { get; set; }
        public string Uri { get; set; }
        public ObjectCollection<BuildError> Errors { get; } = new ObjectCollection<BuildError>();
        public ObjectCollection<BuildError> Warnings { get; } = new ObjectCollection<BuildError>();

        // prop
    }
}