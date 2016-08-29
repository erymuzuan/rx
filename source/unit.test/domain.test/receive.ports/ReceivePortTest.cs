using System;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Moq;
using Xunit;

namespace domain.test.receive.ports
{
    public class ReceivePortTest
    {
        private readonly string m_sampleStore = Strings.GenerateId();

        public ReceivePortTest()
        {
            Environment.SetEnvironmentVariable($"RX_{ConfigurationManager.ApplicationName}_HOME", @"c:\temp\rx", EnvironmentVariableTarget.Process);
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);
            var doc = new BinaryStore
            {
                Content = File.ReadAllBytes(@"C:\project\work\entt.ipos\docs\sample_ipos_soc\20151104105551_0_ipos_soc_0805_20151104-105236_1246_36.txt.log"),
                Id = m_sampleStore,
                Extension = ".txt",
                FileName = @"20151104105551_0_ipos_soc_0805_20151104-105236_1246_36.txt.log"
            };
            store.Setup(x => x.GetContentAsync(m_sampleStore))
                .Returns(Task.FromResult(doc));
            ObjectBuilder.AddCacheList(store.Object);

            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
        }

        [Fact]
        public async Task GenerateFieldAsync()
        {
            var formatter = new DelimitedTextFormatter
            {
                SampleStoreId = m_sampleStore,
                Delimiter = "\t",
                EscapeCharacter = null,
                HasTagIdentifier = true,
                RecordTag = "9",
                Name = "SalesOrder"
            };
            var detail = new FlatFileDetailTag
            {

                RowTag = "1",
                Name = "OrderItem",
                WebId = Guid.NewGuid().ToString(),
                FieldName = "OrderItems",
                Parent = "$root"
            };
           
                 
            formatter.DetailRowCollection.AddRange(detail);
            var list = await formatter.GetFieldMappingsAsync();
            Assert.Equal(36, list.Length);

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
                Credential = null
            });


            var cr = await port.CompileAsync();
            Assert.True(cr.Result);
            Console.WriteLine(cr.Output);

        }
    }
}
