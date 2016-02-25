using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain.Api
{
    public partial class TableDefinition
    {

        [ImportMany(typeof(ControllerAction))]
        [JsonIgnore]
        public ControllerAction[] ActionCodeGenerators { get; set; }

        private Class GenerateController(Adapter adapter)
        {
            if (null == this.ActionCodeGenerators)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.ActionCodeGenerators)
                throw new Exception($"Cannot compose MEF for {nameof(TableDefinition)}");

            var code = new Class { Name = $"{Name}Controller", BaseClass = "ApiController", Namespace = CodeNamespace };
            code.AttributeCollection.Add($"   [RoutePrefix(\"api/{Schema.ToLowerInvariant()}/{Name.ToLowerInvariant()}\")]");
            code.ImportCollection.AddRange(ImportDirectives);

            var executed = new List<Type>();
            foreach (var generator in this.ActionCodeGenerators)
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