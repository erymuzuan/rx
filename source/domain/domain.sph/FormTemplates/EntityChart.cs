namespace Bespoke.Sph.Domain
{
    public partial class EntityChart : Entity
    {
        public override string ToString()
        {
            return string.Format("[{0}]{1}", this.EntityChartId, this.Name);
        }

    
    }
}