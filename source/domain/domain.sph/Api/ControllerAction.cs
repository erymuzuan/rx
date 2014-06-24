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

        public abstract string ActionName { get; }

        public virtual bool IsAsync
        {
            get { return false; }
        }
        public virtual string Route
        {
            get { return null; }
        }
        public virtual Type ReturnType
        {
            get { return typeof(string); }
        }

        public abstract string GenerateCode(TableDefinition table, Adapter adapter);
    }
}
