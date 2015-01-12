using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace scheduler.report.delivery
{
    class Program
    {
        static void Main(string[] args)
        {
            var id = args[0];
            var prog = new Program();
            prog.StartAsync(id).Wait();
        }

        private async Task StartAsync(string id)
        {
            var context = new SphDataContext();
            var del = await context.LoadOneAsync<ReportDelivery>(d => d.Id == id);
            var rdl = await context.LoadOneAsync<ReportDefinition>(d => d.Id == del.ReportDefinitionId);
            foreach (var p in rdl.DataSource.ParameterCollection)
            {
                Console.WriteLine(p);
                // TODO : fill in the parameters value
            }

            // TODO : call the web server to generate the report, capture the response and email it to the recipients or archive them is the system(BinaryStore)
            Console.WriteLine(rdl);
        }
    }
}
