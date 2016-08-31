using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace domain.test.receive.ports
{
    public class CodeGeneratorTest
    {
        public ITestOutputHelper Console { get; }

        public CodeGeneratorTest(ITestOutputHelper helper)
        {
            Environment.SetEnvironmentVariable($"RX_{ConfigurationManager.ApplicationName}_HOME", @"c:\temp\rx", EnvironmentVariableTarget.Process);

            Console = helper;
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);

            store.SetupAndReturnDoc("soc.text");
            store.SetupAndReturnDoc("with-label.txt");
            store.SetupAndReturnDoc("text-csv-with-label-children.txt");
            store.SetupAndReturnDoc("text-csv-with-escape.txt");
            ObjectBuilder.AddCacheList(store.Object);

            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
        }

        [Fact]
        public async Task RecordClass()
        {
            var port = await GenerateReceivePort();
            var @classes = (await port.GenerateCodeAsync()).ToArray();
            var salesOrder = @classes.SingleOrDefault(x => x.Name == "SalesOrder");
            Assert.NotNull(salesOrder);
            Assert.Equal(6, salesOrder.PropertyCollection.Count);
            Console.WriteLine(salesOrder.GetCode());



            var itemClass = @classes.SingleOrDefault(x => x.Name == "Item");
            Assert.NotNull(itemClass);
            Assert.Equal(4, itemClass.PropertyCollection.Count);
            Console.WriteLine(itemClass.GetCode());

        }

        [Fact]
        public async Task PortClass()
        {
            var port = await GenerateReceivePort();

            var @classes = (await port.GenerateCodeAsync()).ToArray();
            var portType = @classes.SingleOrDefault(x => x.Name == port.Name.ToPascalCase());
            Assert.NotNull(portType);
            Console.WriteLine(portType.GetCode());

        }
        [Fact]
        public async Task CompilePort()
        {
            var port = await GenerateReceivePort();

            var cr = await port.CompileAsync();
            foreach (var eror in cr.Errors)
            {
                Console.WriteLine(eror.ToString());
            }
            Assert.True(cr.Result, cr.ToString());
            Console.WriteLine(cr.Output);

        }

        private static async Task<ReceivePort> GenerateReceivePort()
        {
            var csv = new DelimitedTextFormatter
            {
                Name = "csv",
                SampleStoreId = "text-csv-with-escape.txt",
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

            csv.DetailRowCollection.Add(new FlatFileDetailTag
            {
                Name = "Item",
                FieldName = "Items",
                Parent = "$root",
                RowTag = "I",
                WebId = "items-row"
            });

            var fields = await csv.GetFieldMappingsAsync();
            port.FieldMappingCollection.ClearAndAddRange(fields);

            var customerNameField = fields[2];
            Assert.Equal("Ahmad, sons and friends", customerNameField.SampleValue);
            customerNameField.Path = "CustomerName";

            var orderNoField = fields[3];
            orderNoField.Path = "OrderNo";

            var shippingNoField = fields[4];
            shippingNoField.Path = "ShippingNo";

            var dateField = (DelimitedTextFieldMapping) fields[5];

            dateField.Path = "Date";
            dateField.TypeName = typeof(DateTime).GetShortAssemblyQualifiedName();
            dateField.Converter = "yyyy-MM-dd";

            fields[0].FieldMappingCollection[0].Path = "Tag";
            fields[0].FieldMappingCollection[1].Path = "Description";
            fields[0].FieldMappingCollection[2].Path = "Quantity";
            fields[0].FieldMappingCollection[2].TypeName = typeof(int).GetShortAssemblyQualifiedName();
            fields[0].FieldMappingCollection[3].Path = "Amount";
            fields[0].FieldMappingCollection[3].TypeName = typeof(decimal).GetShortAssemblyQualifiedName();
            return port;
        }

    }
}