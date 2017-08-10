using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Mangements
{
    class QueueCleaner
    {
        public static readonly string[] QueueNames = { "ed_es_mapping_gen", "" };

        public async Task StartAsync()
        {
            await Task.Delay(500);
            foreach (var queue in QueueNames)
            {
                
            }
        }
    }
}
