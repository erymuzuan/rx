namespace Bespoke.Sph.Domain
{
    [StoreAsSource]
    public partial class EntityChart : Entity
    {
        public override string ToString()
        {
            return $"[{this.Id}]{this.Name}";
        }

    
    }
}