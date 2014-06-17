using System;

namespace Bespoke.Sph.Domain.Api
{
    public abstract class ControllerAction
    {
        protected string GetRouteConstraint(Type type)
        {
            if (type == typeof(string)) return string.Empty;
            if (type == typeof(short)) return ":int";
            return ":" + type.ToCSharp();
        }

     
        public abstract string GenerateCode(TableDefinition table, Adapter adapter);
    }
}
