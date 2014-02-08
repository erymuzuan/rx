using System;
using System.Collections.Generic;

namespace Bespoke.Sph.Domain
{
    public class ExecutedAcitivityComparer : IComparer<ExecutedActivity>
    {
        public int Compare(ExecutedActivity x, ExecutedActivity y)
        {
            var xTime = (x.Initiated ?? x.Run) ?? DateTime.MinValue;
            var yTime = (y.Initiated ?? y.Run) ?? DateTime.MinValue;
            var r = xTime.CompareTo(yTime);
            Console.WriteLine("{0}.Compare({1}) => {2}", x.Name, y.Name, r);
            Console.WriteLine("\t\t{0}.Compare({1})", xTime, yTime);
            return r;
        }
    }
}