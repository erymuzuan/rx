using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Xunit;

namespace domain.test.receive.ports
{
    public class ReceivePortTest
    {
        public ReceivePortTest()
        {
            Environment.SetEnvironmentVariable($"RX_{ConfigurationManager.ApplicationName}_HOME", @"c:\temp\rx", EnvironmentVariableTarget.Process);
        }
        [Fact]
        public async Task Start()
        {
            await Task.Delay(100);
            var port = new ReceivePort
            {
                Name = "Test 123",
                Entity = "Customer",
                EntityId = "customer",
                Formatter = "Text",
                
            };

            port.ReceiveLocationCollection.Add(new FolderReceiveLocation
            {
                Path = @"c:\temp\rx-flat-port",
                Name = "TempFlat",
                Credential =  null
            });


            var cr = await port.CompileAsync();
            Assert.True(cr.Result);
            Console.WriteLine(cr.Output);

        }
    }
}
