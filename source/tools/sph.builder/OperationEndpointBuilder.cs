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

        private async Task Compile(OperationEndpoint oe)
        {
            var context = new SphDataContext();
            var ed = context.LoadOneFromSources<EntityDefinition>(x => x.Name == oe.Entity);

            var result = await oe.CompileAsync(ed);
            result.Errors.ForEach(Console.WriteLine);
            
        }

    }
}