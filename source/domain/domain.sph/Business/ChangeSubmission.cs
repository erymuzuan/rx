namespace Bespoke.Sph.Domain
{
    public class ChangeSubmission : DomainObject
    {
        public ObjectCollection<Entity> ChangedCollection { get; set; }
        public ObjectCollection<Entity> DeletedCollection { get; set; }
    }
}
