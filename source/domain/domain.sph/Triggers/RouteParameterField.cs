using System;

namespace Bespoke.Sph.Domain
{
    public partial class RouteParameterField : Field
    {
        public override object GetValue(RuleContext context)
        {
            return $"<<{this.Expression}>>";
        }
    }
}