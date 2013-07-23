namespace Bespoke.SphCommercialSpaces.Domain
{
    partial class Trigger : Entity
    {

        private readonly ObjectCollection<Rule> m_ruleCollection = new ObjectCollection<Rule>();
        private readonly ObjectCollection<string> m_actionCollection = new ObjectCollection<string>();

        public ObjectCollection<string> ActionCollection
        {
            get { return m_actionCollection; }
        }
        public ObjectCollection<Rule> RuleCollection
        {
            get { return m_ruleCollection; }
        }
    }
}
