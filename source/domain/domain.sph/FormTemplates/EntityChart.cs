namespace Bespoke.Sph.Domain
{
    public partial class EntityChart : Entity
    {
        public override string ToString()
        {
            return $"[{this.Id}]{this.Name}";
        }

    
    }
}