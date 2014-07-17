using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class HttpAdapter : Adapter
    {
        protected override Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces)
        {
            throw new NotImplementedException();
        }

        protected override Task<Tuple<string, string>> GenerateOdataTranslatorSourceCodeAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task<Tuple<string, string>> GeneratePagingSourceCodeAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task<TableDefinition> GetSchemaDefinitionAsync(string table)
        {
            throw new NotImplementedException();
        }

        public override string OdataTranslator
        {
            get { throw new NotImplementedException(); }
        }

        public string Har { get; set; }
    }
}
