using System;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Now", Description = "Produce the current date time", Type = typeof(NowFunctoid), FontAwesomeIcon = "clock-o", Category = "date")]
    public class NowFunctoid : Functoid
    {
        public NowFunctoid()
        {
            this.OutputTypeName = typeof(DateTime).GetShortAssemblyQualifiedName();
        }
        public override Type GetOutputType()
        {
            return typeof(DateTime);
        }

        public override string GenerateAssignmentCode()
        {
            return "DateTime.Now";
        }
    }
}