using System;
using Newtonsoft.Json;

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
        public abstract string Name { get; }
        public bool IsEnabled { get; set; }
        public virtual string ActionName => string.Empty;
        public virtual bool IsAsync => false;
        public virtual string Route => null;
        public virtual HypermediaLink[] GetHypermediaLinks(Adapter adapter, TableDefinition table)
        {
            return new HypermediaLink[] { };
        }
        [JsonIgnore]
        public virtual Type ReturnType => typeof(object);
        public string ReturnTypeName => this.ReturnType.GetShortAssemblyQualifiedName();
        public abstract string GenerateCode(TableDefinition table, Adapter adapter);


        public virtual string GetDesignerHtmlView()
        {
            return
                $@"       
            <form>
                <div class=""form-group"">
                    <label class=""control-label"">Enable &nbsp;
                        <input type=""checkbox"" data-bind=""checked: IsEnabled""/>
                    </label>
                </div>
            </form>
          ";
        }
        public virtual string GetPartialDesignerJsCode()
        {
            return null;
        }

    }
}
