namespace Bespoke.Sph.Domain
{
    [PersistenceOption(IsSource = true)]
    public partial class EntityChart : Entity
    {
        public override string ToString()
        {
            return $"[{this.Id}]{this.Name}";
        }

    
    }
}