using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
{
    public class ReceivePortBuilder : Builder<ReceivePort>
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

        public override async Task RestoreAsync(ReceivePort item)
        {
            await this.Compile(item);
        }

        private async Task Compile(ReceivePort item)
        {
            var result = await item.CompileAsync();
            ReportBuildStatus(result);

        }

    }
}