using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Moq;
using Xunit;

namespace domain.test.receive.ports
{
    public static class MockExtensions
    {
        public static void SetupAndReturnDoc(this Mock<IBinaryStore> store,string id)
        {

            store.Setup(x => x.GetContentAsync(id))
                .Returns(() =>
                {
                    var doc = new BinaryStore
                    {
                        Content = File.ReadAllBytes(id),
                        Id = id,
                        Extension = ".txt",
                        FileName = id
                    };
                    return Task.FromResult(doc);
                });
        }
    }
    public class ReceivePortTest
    {
        private const string Soc = "soc.txt";
        private const string WithLabel = "with-labels.txt";

        public ReceivePortTest()
        {
            Environment.SetEnvironmentVariable($"RX_{ConfigurationManager.ApplicationName}_HOME", @"c:\temp\rx", EnvironmentVariableTarget.Process);
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);

            store.SetupAndReturnDoc(Soc);
            store.SetupAndReturnDoc(WithLabel);
            store.SetupAndReturnDoc("text-csv-with-label-children.txt");
            store.SetupAndReturnDoc("text-csv-with-escape.txt");
            ObjectBuilder.AddCacheList(store.Object);

            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
        }

        [Fact]
        public async Task GenerateFieldWithMultipeChildren()
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
            var item = new FlatFileDetailTag
            {

                RowTag = "1",
                Name = "OrderItem",
                WebId = Guid.NewGuid().ToString(),
                FieldName = "OrderItems",
                Parent = "$root"
            };
            var child = new FlatFileDetailTag
            {

                RowTag = "x",
                Name = "Child",
                WebId = Guid.NewGuid().ToString(),
                FieldName = "Children",
                Parent = "$root"
            };
            var grandChild = new FlatFileDetailTag
            {

                RowTag = "x1",
                Name = "GrandChild",
                WebId = Guid.NewGuid().ToString(),
                FieldName = "GrandChildren",
                Parent = "Child"
            };


            formatter.DetailRowCollection.AddRange(item, child, grandChild);
            var list = await formatter.GetFieldMappingsAsync();
            Assert.Equal(37, list.Length);
            Assert.Equal("OrderItems", list[0].Path);
            Assert.Equal("Children", list[1].Path);
            Assert.Equal(36, list.First().FieldMappingCollection.Count);
            Assert.Equal(1, list.First().FieldMappingCollection.Count(x => x.Path == "GrandChildren"));

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
        public async Task CsvFileWithChildren()
        {
            var formatter = new DelimitedTextFormatter
            {
                SampleStoreId = "text-csv-with-label-children.txt",
                Delimiter = ",",
                EscapeCharacter = null,
                HasTagIdentifier = true,
                RecordTag = "SO",
                Name = "SalesOrder"
            };
            var detail = new FlatFileDetailTag
            {

                RowTag = "I",
                Name = "OrderItem",
                WebId = Guid.NewGuid().ToString(),
                FieldName = "OrderItems",
                Parent = "$root"
            };


            formatter.DetailRowCollection.AddRange(detail);
            var list = await formatter.GetFieldMappingsAsync();
            Assert.Equal(1, list.Count(x => x.Path == "OrderItems"));

        }
        [Fact]
        public async Task CsvFileWithEscape()
        {
            var formatter = new DelimitedTextFormatter
            {
                SampleStoreId = "text-csv-with-escape.txt",
                Delimiter = ",",
                EscapeCharacter = "\"",
                HasTagIdentifier = true,
                RecordTag = "SO",
                Name = "SalesOrder"
            };
            var detail = new FlatFileDetailTag
            {

                RowTag = "I",
                Name = "OrderItem",
                WebId = Guid.NewGuid().ToString(),
                FieldName = "OrderItems",
                Parent = "$root"
            };


            formatter.DetailRowCollection.AddRange(detail);
            var list = await formatter.GetFieldMappingsAsync();
            Assert.Equal(1, list.Count(x => x.SampleValue == "Ahmad, sons and friends"));

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
