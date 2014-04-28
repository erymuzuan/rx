namespace Bespoke.Sph.Domain
{
    public partial class EntityChart : Entity
    {
        public override string ToString()
        {
            return string.Format("[{0}]{1}", this.EntityChartId, this.Name);
        }

        public override void SetId(int id)
        {
            this.EntityChartId = id;
        }

        public override int GetId()
        {
            return this.EntityChartId;
        }
    }
}