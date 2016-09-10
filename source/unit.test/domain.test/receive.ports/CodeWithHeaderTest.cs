using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace domain.test.receive.ports
{
    public class CodeWithHeaderTest
    {
        private const string SampleStoreId = "text-sapfi-20160809-150404-12.txt";
        public ITestOutputHelper Console { get; }

        public CodeWithHeaderTest(ITestOutputHelper helper)
        {
            Environment.SetEnvironmentVariable($"RX_{ConfigurationManager.ApplicationName}_HOME", @"c:\temp\rx", EnvironmentVariableTarget.Process);

            Console = helper;
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);

            store.SetupAndReturnDoc("soc.text");
            store.SetupAndReturnDoc("with-label.txt");
            store.SetupAndReturnDoc("text-csv-with-label-children.txt");
            store.SetupAndReturnDoc(SampleStoreId);
            ObjectBuilder.AddCacheList(store.Object);

            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
        }

        [Fact]
        public async Task RecordClass()
        {
            var port = await GenerateReceivePort();
            var classes = (await port.GenerateCodeAsync()).ToArray();
            var salesOrder = classes.SingleOrDefault(x => x.Name == "SalesOrder");
            Assert.NotNull(salesOrder);
            Assert.Equal(12, salesOrder.PropertyCollection.Count);
            Console.WriteLine(salesOrder.GetCode());
            
        }

        [Fact]
        public async Task PortClass()
        {
            var port = await GenerateReceivePort();

            var classes = (await port.GenerateCodeAsync()).ToArray();
            var portType = classes.SingleOrDefault(x => x.Name == port.Name.ToPascalCase());
            Assert.NotNull(portType);
            Console.WriteLine(portType.GetCode());

        }
        [Fact]
        public async Task CompilePort()
        {
            var source = await GenerateReceivePort();
            var cr = await source.CompileAsync();
            foreach (var eror in cr.Errors)
            {
                Console.WriteLine(eror.ToString());
            }
            Assert.True(cr.Result, cr.ToString());
            Console.WriteLine(cr.Output);


            var assembly = Assembly.LoadFile(cr.Output);
            var portType = assembly.GetType($"{source.CodeNamespace}.{source.TypeName}");
            dynamic port = Activator.CreateInstance(portType);

            port.AddHeader("Name", SampleStoreId);
            port.AddHeader("LastWriteTime", $"{DateTime.Now:s}");
            port.Uri = new Uri(Path.GetFullPath(SampleStoreId));
            var lines = File.ReadLines(SampleStoreId);
            foreach (var r in port.Process(lines))
            {
                Assert.Equal(12, r.Count);
                Assert.Equal(DateTime.ParseExact("20160809-150404","yyyyMMdd-HHmmss", CultureInfo.InvariantCulture), r.Created);
                Assert.Equal(12, (int?)r.CountLong);
            }


        }

        private static async Task<ReceivePort> GenerateReceivePort()
        {
            var csv = new DelimitedTextFormatter
            {
                Name = "csv",
                SampleStoreId = SampleStoreId,
                EscapeCharacter = "\"",
                Delimiter = ",",
                HasLabel = false,
                HasTagIdentifier = true,
                RecordTag = "SO"
            };
            var port = new ReceivePort
            {
                Name = "POS Soc",
                Entity = "SalesOrder",
                Id = "pos-soc",
                Formatter = nameof(DelimitedTextFormatter),
                TextFormatter = csv
            };


            var fields = await csv.GetFieldMappingsAsync();
            port.FieldMappingCollection.ClearAndAddRange(fields);

            port.FieldMappingCollection.Add(new UriFieldMapping { Name = "CountLong", Type = typeof(int), Pattern = @"-(?<value>\d{1,4}).txt" });
            port.FieldMappingCollection.Add(new UriFieldMapping { Name = "CountLong2", IsNullable = true, Type = typeof(int), Pattern = @"-(?<value>\d{1,4}).txt" });
            port.FieldMappingCollection.Add(new HeaderFieldMapping { Name = "FileName", Type = typeof(string), Pattern = ".*", Header = "Name", SampleValue = csv.SampleStoreId });
            port.FieldMappingCollection.Add(new HeaderFieldMapping { Name = "Count", Type = typeof(int), Pattern = @"-(?<value>\d{1,4}).txt", Header = "Name" });
            port.FieldMappingCollection.Add(new HeaderFieldMapping { Name = "Created", Pattern = @"-(?<value>\d{8}-\d{6})-", Converter = "yyyyMMdd-HHmmss", Header = "Name", Type = typeof(DateTime) });
            port.FieldMappingCollection.Add(new HeaderFieldMapping { Name = "Created2",IsNullable = true, Pattern = @"-(?<value>\d{8}-\d{6})-", Converter = "yyyyMMdd-HHmmss", Header = "Name", Type = typeof(DateTime) });

            var customerNameField = fields[2];
            Assert.Equal("Ahmad, sons and friends", customerNameField.SampleValue);
            customerNameField.Name = "CustomerName";

            var orderNoField = fields[3];
            orderNoField.Name = "OrderNo";

            var shippingNoField = fields[4];
            shippingNoField.Name = "ShippingNo";

            var dateField = (DelimitedTextFieldMapping)fields[5];

            dateField.Name = "Date";
            dateField.TypeName = typeof(DateTime).GetShortAssemblyQualifiedName();
            dateField.Converter = "yyyy-MM-dd";
            
            return port;
        }

    }
}