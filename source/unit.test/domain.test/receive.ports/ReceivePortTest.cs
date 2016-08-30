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
        private const string Soc = "soc.txt";
        private const string WithLabel = "with-labels.txt";
        public ReceivePortTest()
        {
            Environment.SetEnvironmentVariable($"RX_{ConfigurationManager.ApplicationName}_HOME", @"c:\temp\rx", EnvironmentVariableTarget.Process);
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);

            store.Setup(x => x.GetContentAsync(Soc))
                .Returns(() =>
                {
                    var doc = new BinaryStore
                    {
                        Content = File.ReadAllBytes(Soc),
                        Id = Soc,
                        Extension = ".txt",
                        FileName = Soc
                    };
                    return Task.FromResult(doc);
                });
            store.Setup(x => x.GetContentAsync(WithLabel))
                .Returns(() =>
                {
                    var doc = new BinaryStore
                    {
                        Content = File.ReadAllBytes(WithLabel),
                        Id = WithLabel,
                        Extension = ".txt",
                        FileName = WithLabel
                    };
                    return Task.FromResult(doc);
                });
            ObjectBuilder.AddCacheList(store.Object);

            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
        }

        [Fact]
        public async Task GenerateWithLabel()
        {
            var formatter = new DelimitedTextFormatter
            {
                SampleStoreId = WithLabel,
                Delimiter = "\t",
                EscapeCharacter = null,
                HasTagIdentifier = true,
                RecordTag = "9",
                Name = "SalesOrder",
                HasLabel = true
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
            Assert.Equal("OrderItems", list[0].Path);
            Assert.Equal("OrderType", list[2].Path);

        }
        [Fact]
        public async Task GenerateFieldAsync()
        {
            var formatter = new DelimitedTextFormatter
            {
                SampleStoreId = Soc,
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
            var patient = new EntityDefinition { Name = "Patient", Id = "patient", Plural = "Patients" };
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
                CredentialUserName = null
            });
            port.InitTest(patient);


            var cr = await port.CompileAsync();
            Assert.True(cr.Result);
            Console.WriteLine(cr.Output);

        }
    }
}
