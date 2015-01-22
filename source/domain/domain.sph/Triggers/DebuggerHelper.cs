using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public static class DebuggerHelper
    {
        public static readonly HashSet<string> UnitTestAttributes = new HashSet<string> 
    {
        "Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute",
        "NUnit.Framework.TestFixtureAttribute",
    };
        public static bool IsRunningInUnitTest
        {
            get
            {
                foreach (var f in new StackTrace().GetFrames())
                    if (f.GetMethod().DeclaringType.GetCustomAttributes(false).Any(x => UnitTestAttributes.Contains(x.GetType().FullName)))
                        return true;
                return false;
            }
        }
        public static bool IsVerbose
        {
            get
            {
                var callStack = new StackTrace();
                var attributes = GetStackFrames(callStack).SelectMany(stackFrame => stackFrame.GetMethod()
                    .GetCustomAttributes(false))
                    .Where(x => x.GetType() == typeof(TraceAttribute));
                var da = attributes.SingleOrDefault() as TraceAttribute;
                if (null == da) return false;
                return da.Verbose;
            }
        }

        private static IEnumerable<StackFrame> GetStackFrames(StackTrace callStack)
        {
            return callStack.GetFrames() ?? new StackFrame[0];
        }
    }

    /// <summary>
    /// Use at the unit test or controller
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TraceAttribute : Attribute
    {
        public bool Verbose { get; set; }
    }
}