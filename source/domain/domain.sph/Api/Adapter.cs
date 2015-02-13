using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.Api
{
    public abstract partial class Adapter : CustomProject
    {
       
        public override Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var vr = new ObjectCollection<ValidationError>();
            if (string.IsNullOrWhiteSpace(this.Name))
                vr.Add("Name", "Name cannot be empty");

            return Task.FromResult(vr.AsEnumerable());
        }



        public virtual Task OpenAsync(bool verbose = false)
        {
            return Task.FromResult(0);
        }

        protected abstract Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces);
        protected abstract Task<Tuple<string, string>> GenerateOdataTranslatorSourceCodeAsync();
        protected abstract Task<Tuple<string, string>> GeneratePagingSourceCodeAsync();
        protected abstract Task<TableDefinition> GetSchemaDefinitionAsync(string table);

        public virtual void SaveAssets()
        {

        }


    }
}
