﻿using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
{
    public class ReceiveLocationBuilder : Builder<ReceiveLocation>
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

        public override async Task RestoreAsync(ReceiveLocation item)
        {
            await this.Compile(item);
        }

        private async Task Compile(ReceiveLocation item)
        {
            var portSource = $"{ConfigurationManager.SphSourceDirectory}\\ReceivePort\\{item.ReceivePort.ToIdFormat()}.json";
            var port = portSource.DeserializeFromJsonFile<ReceivePort>();
            var result = await item.CompileAsync(port);
            result.Errors.ForEach(Console.WriteLine);
            
        }

    }
}