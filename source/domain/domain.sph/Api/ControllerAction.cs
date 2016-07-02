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

        public virtual bool Applicable(TableDefinition table)
        {
            return true;
        }
        public ErrorRetry ErrorRetry { get; set; } = new ErrorRetry { Wait = 500, Algorithm = WaitAlgorithm.Linear, Attempt = 3 };
        public abstract string Name { get; }
        public bool IsEnabled { get; set; }
        public abstract string[] GetActionNames(TableDefinition table, Adapter adapter);
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
            var name = this.GetType().Name.ToIdFormat();
            return
                $@"       
         <form data-bind=""with:ErrorRetry"">
                    <div class=""form-group"">
                        <label for=""retry-count-{name}"" class=""control-label"">Attempt</label>
                        <input type=""number"" max=""50"" data-bind=""value: Attempt, tooltip:'Enable retry for your database call, the min value is 2, in an Exception is thrown, after the number of retry count you set, the execution will stop and exception is propagated to the call stack'"" min=""2""
                               placeholder=""Set the number if retries if the invocation throws any exception""
                               class=""form-control"" id=""retry-count-{name}"">
                    </div>
                    <div class=""form-group"">
                        <label for=""retry-interval-{name}"" class=""control-label"">Wait</label>
                        <input type=""number"" step=""10"" max=""50000"" data-bind=""value: Wait, tooltip:'The time in ms, the code will wait before attempting the next retry. The default is 500ms'""
                               placeholder=""The interval between retries in ms""
                               class=""form-control"" id=""retry-interval-{name}"">
                    </div>
                    <div class=""form-group"">
                        <label for=""retry-wait-{name}"" class=""control-label"">Algorithm</label>
                        <select data-bind=""value: Algorithm, tooltip:'Connstant - set to your wait interval value, Liner = interval * n, Exponential = interval * (2^n), n is the retry attempt'""
                                class=""form-control"" id=""retry-wait-{name}"">
                            <option value=""Constant"">Constant</option>
                            <option value=""Linear"">Linear</option>
                            <option value=""Exponential"">Exponential</option>
                        </select>
                    </div>
                </form>";
        }
        public virtual string GetPartialDesignerJsCode()
        {
            return null;
        }

    }
}
