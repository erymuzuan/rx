using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
{
    public class OperationEndpointBuilder : Builder<OperationEndpoint>
    {

        public override async Task RestoreAllAsync()
        {
            this.Initialize();
            var endpoints = this.GetItems();
            foreach (var item in endpoints)
            {
                await this.Compile(item);
            }
        }

        public override async Task RestoreAsync(OperationEndpoint item)
        {
            await this.Compile(item);
        }

        private async Task Compile(OperationEndpoint item)
        {
            var ed = $"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition\\{item.Entity.ToIdFormat()}.json".DeserializeFromJsonFile<EntityDefinition>();
            var result = await item.CompileAsync(ed);
            result.Errors.ForEach(Console.WriteLine);
            
        }

    }
}