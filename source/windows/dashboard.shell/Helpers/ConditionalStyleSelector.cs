using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;


namespace Bespoke.Station.Windows.Helpers
{
    public class ConditionalStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            object conditionValue = this.ConditionConverter.Convert(item, null, null, null);
            foreach (ConditionalStyleRule rule in this.Rules)
            {
                if (Equals(rule.Value, conditionValue))
                {
                    return rule.Style;
                }
            }


            return base.SelectStyle(item, container);
        }

        List<ConditionalStyleRule> m_rules;
        public List<ConditionalStyleRule> Rules 
        {
            get { return this.m_rules ?? (this.m_rules = new List<ConditionalStyleRule>()); }
        }

        public IValueConverter ConditionConverter { get; set; }
    }

    public class ConditionalStyleRule
    {
        public object Value { get; set; }

        public Style Style { get; set; }
    }
}
