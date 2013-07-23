using System;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public class Rule
    {
        public Field Left { get; set; }

        public Operator Operator { get; set; }

        public Field Right { get; set; }

        public bool Execute(Entity item)
        {
            var left = this.Left.GetValue(item);
            var right = this.Right.GetValue(item);

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

            return false;
        }
    }
}
