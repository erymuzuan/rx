using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    public partial class TableDefinition
    {

        [ImportMany(typeof(ControllerAction))]
        public IEnumerable<ControllerAction> ActionCodeGenerators { get; set; }
        private string GenerateController(Adapter adapter)
        {
            if(null == this.ActionCodeGenerators)
                ObjectBuilder.ComposeMefCatalog(this);

            var header = this.GetCodeHeader();
            var code = new StringBuilder(header);

            code.AppendLinf("   [RoutePrefix(\"api/{0}/{1}\")]", this.Schema.ToLowerInvariant(), this.Name.ToLowerInvariant());
            code.AppendLinf("   public partial class {0}Controller : ApiController", this.Name);
            code.AppendLine("   {");

            var executed = new List<Type>();
            foreach (var action in this.ActionCodeGenerators)
            {
                if (executed.Contains(action.GetType())) continue;
                executed.Add(action.GetType());
                code.AppendLine(action.GenerateCode(this, adapter));
            }

            code.AppendLine("   }");// end class
            code.AppendLine("}"); // end namespace
            return code.ToString();

        }

    }
}