namespace Bespoke.Sph.Domain
{
    public class RuleContext
    {
        public RuleContext(Entity item)
        {
            this.Item = item;
        }
        public RuleContext(DomainObject @object)
        {
            this.Object = @object;
        }
        public Entity Item { get; set; }
        public DomainObject Object{ get; set; }
        public AuditTrail Log { get; set; }
    }
}