using System;

namespace Bespoke.SphCommercialSpaces.Domain
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
    public partial class Rule : DomainObject
    {
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", this.Left.Name, this.Operator, this.Right.Name);
        }

        public bool Execute(RuleContext context)
        {
            var left = this.Left.GetValue(context);
            var right = this.Right.GetValue(context);

            if (null == left) return false;
            if (null == right) return false;

            Console.WriteLine("Evaluate : ({3}){0} {1} ({4}){2}", left, Operator, right, left.GetType().Name, right.GetType().Name);

            var lc = left as IComparable;
            var rc = right as IComparable;
            if (null != lc && null != rc)
            {
                if (Operator == Operator.Eq)
                    return lc.CompareTo(rc) == 0;
                if (Operator == Operator.Lt)
                    return lc.CompareTo(rc) < 0;
                if (Operator == Operator.Le)
                    return lc.CompareTo(rc) <= 0;
                if (Operator == Operator.Gt)
                    return lc.CompareTo(rc) > 0;
                if (Operator == Operator.Ge)
                    return lc.CompareTo(rc) >= 0;

               
            }

            var sl = left as string;
            var sr = right as string;
            if (null != sr && null != sl)
            {
                if (Operator == Operator.NotContains)
                    return !sl.ToLowerInvariant().Contains(sr.ToLowerInvariant());
                if (Operator == Operator.Substringof)
                    return sl.ToLowerInvariant().Contains(sr.ToLowerInvariant());
                if (Operator == Operator.StartsWith)
                    return sl.ToLowerInvariant().StartsWith(sr.ToLowerInvariant());
                if (Operator == Operator.EndsWith)
                    return sl.ToLowerInvariant().EndsWith(sr.ToLowerInvariant());
            }

            return false;
        }
    }
}
