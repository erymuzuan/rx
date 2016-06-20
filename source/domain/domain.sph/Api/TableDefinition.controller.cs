using System;
using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain.Api
{
    public partial class TableDefinition
    {
        private Class GenerateController(Adapter adapter)
        {

            var code = new Class { Name = $"{ClrName}Controller", BaseClass = "BaseApiController", Namespace = CodeNamespace };
            code.AttributeCollection.Add($"   [RoutePrefix(\"{adapter.RoutePrefix}/{Name.ToIdFormat()}\")]");
            code.ImportCollection.AddRange(ImportDirectives);
            code.ImportCollection.AddRange("Polly", "Polly.Utilities", "Polly.Retry");

            var executed = new List<Type>();
            foreach (var generator in this.ControllerActionCollection.Where(x => x.IsEnabled))
            {
                if (executed.Contains(generator.GetType())) continue;
                executed.Add(generator.GetType());

                var action = generator.GenerateCode(this, adapter);
                if (!string.IsNullOrWhiteSpace(action))
                    code.AddMethod(generator.GenerateCode(this, adapter));
            }

            return code;

        }

    }
}