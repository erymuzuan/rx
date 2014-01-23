namespace Bespoke.Sph.Domain
{
    public class RuleContext
    {
        public RuleContext(Entity item)
        {
            this.Item = item;
        }
        public Entity Item { get; set; }
        public AuditTrail Log { get; set; }
    }
}