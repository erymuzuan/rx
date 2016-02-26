using System;

namespace Bespoke.Sph.Domain
{
    public partial class ThrowActivity : Activity
    {
        public override string GenerateExecMethodBody(WorkflowDefinition wd)
        {
            throw new Exception("You can use ExpressionActivity to throw an Exception");
        }

    }
}