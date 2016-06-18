using System;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Domain.Api
{
    public abstract class ControllerAction
    {
        protected string GetRouteConstraint(Column column)
        {
            var type = column.ClrType;
            if (type == typeof(string)) return string.Empty;
            if (type == typeof(short)) return ":int";
            if (type == typeof(byte)) return ":int";
            if (type == typeof(Guid)) return ":guid";
            return ":" + type.ToCSharp();
        }

        public virtual string ActionName => string.Empty;
        public virtual bool IsAsync => false;
        public virtual string Route => null;
        public virtual HypermediaLink[] GetHypermediaLinks(Adapter adapter, TableDefinition table)
        {
            return new HypermediaLink[] {};
        }

        public virtual Type ReturnType => typeof(string);
        public abstract string GenerateCode(TableDefinition table, Adapter adapter);


    }
}
