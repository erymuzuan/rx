using System;

namespace Bespoke.Sph.Domain.Api
{
    public abstract class ControllerAction
    {
        protected string GetRouteConstraint(Member member)
        {
            var sm = member as SimpleMember;
            if (null == sm) return string.Empty;
            var type = sm.Type;
            if (type == typeof(string)) return string.Empty;
            if (type == typeof(short)) return ":int";
            return ":" + type.ToCSharp();
        }

        public virtual string ActionName => string.Empty;
        public virtual bool IsAsync => false;
        public virtual string Route => null;
        public virtual Type ReturnType => typeof(string);
        public abstract string GenerateCode(TableDefinition table, Adapter adapter);

       

    }
}
