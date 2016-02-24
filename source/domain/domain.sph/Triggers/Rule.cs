using System;
using System.Diagnostics;

namespace Bespoke.Sph.Domain
{
    public partial class Rule : DomainObject
    {
        public override string ToString()
        {
            return $"{this.Left.Name} {this.Operator} {this.Right.Name}";
        }

        public bool Execute(RuleContext context)
        {
            var left = this.Left.GetValue(context);
            var right = this.Right.GetValue(context);

            if (null == left) return false;
            if (null == right) return false;

            Debug.WriteLine("Evaluate : ({3}){0} {1} ({4}){2}", left, Operator, right, left.GetType().Name, right.GetType().Name);

            var lc = left as IComparable;
            var rc = right as IComparable;
            if (null != lc && null != rc)
            {
                switch (Operator)
                {
                    case Operator.Eq:
                        return lc.CompareTo(rc) == 0;
                    case Operator.Neq:
                        return lc.CompareTo(rc) != 0;
                    case Operator.Lt:
                        return lc.CompareTo(rc) < 0;
                    case Operator.Le:
                        return lc.CompareTo(rc) <= 0;
                    case Operator.Gt:
                        return lc.CompareTo(rc) > 0;
                    case Operator.Ge:
                        return lc.CompareTo(rc) >= 0;
                    case Operator.Substringof:
                    case Operator.StartsWith:
                    case Operator.EndsWith:
                    case Operator.NotContains:
                    case Operator.NotStartsWith:
                    case Operator.NotEndsWith:
                        break;
                }
            }

            var sl = left as string;
            var sr = right as string;
            if (null != sr && null != sl)
            {
                switch (Operator)
                {
                    case Operator.Neq:
                        return sl != sr;
                    case Operator.NotContains:
                        return !sl.ToLowerInvariant().Contains(sr.ToLowerInvariant());
                    case Operator.Substringof:
                        return sl.ToLowerInvariant().Contains(sr.ToLowerInvariant());
                    case Operator.StartsWith:
                        return sl.ToLowerInvariant().StartsWith(sr.ToLowerInvariant());
                    case Operator.NotStartsWith:
                        return !sl.ToLowerInvariant().StartsWith(sr.ToLowerInvariant());
                    case Operator.EndsWith:
                        return sl.ToLowerInvariant().EndsWith(sr.ToLowerInvariant());
                    case Operator.NotEndsWith:
                        return !sl.ToLowerInvariant().EndsWith(sr.ToLowerInvariant());
                }
            }

            return false;
        }
    }
}
