using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using FileHelpers;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace domain.test.receive.ports
{
    public class ReceivePortTest
    {
        private const string Soc = "soc.txt";
        private const string WithLabel = "with-labels.txt";

         private readonly ITestOutputHelper m_outputHelper;
        public ReceivePortTest(ITestOutputHelper outputHelper)
        {
            Environment.SetEnvironmentVariable($"RX_{ConfigurationManager.ApplicationName}_HOME", @"c:\temp\rx", EnvironmentVariableTarget.Process);
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);

            store.SetupAndReturnDoc(Soc);
            store.SetupAndReturnDoc(WithLabel);
            store.SetupAndReturnDoc("text-csv-with-label-children.txt");
            store.SetupAndReturnDoc("text-csv-with-escape.txt");
            ObjectBuilder.AddCacheList(store.Object);

            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());

            m_outputHelper = outputHelper;
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
        public void FileHelperCsvEscape()
        {
            const string FILE = @"SO,""Ahmad, sons and friends"",123456,P009,2016-05-06
I,""Samsung Note 7"",2,""3,999.99""
I,""Samsung Note 7"",2,""3,299.99""
I,LG G5, 1, 3099";
            var lines = FILE.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            // ReSharper disable InconsistentNaming
            const string delimiter = ",";
            const string escape = "\"";
            const string placeHolder = "<comma>";
            // ReSharper restore InconsistentNaming

            const RegexOptions OPTIONS = RegexOptions.IgnoreCase | RegexOptions.Multiline;
            var pattern = $@"(?<a>{escape}(.*?){escape})(?<b>\s?({delimiter}|$))";
            MatchEvaluator matchEvaluator = m => (m.Groups["a"].Value.Replace(delimiter, placeHolder) + m.Groups["b"]).Replace(escape, "");
            var so = Regex.Replace(lines.First(), pattern, matchEvaluator, OPTIONS);
            Assert.Equal("SO,Ahmad<comma> sons and friends,123456,P009,2016-05-06", so);

            var line2 = Regex.Replace(lines[1], pattern, matchEvaluator, OPTIONS);
            Assert.Equal($@"I,Samsung Note 7,2,3{placeHolder}999.99", line2);

            var line3 = Regex.Replace(lines[2], pattern,matchEvaluator,OPTIONS);
            Assert.Equal($@"I,Samsung Note 7,2,3{placeHolder}299.99", line3);

            var engine = new FileHelperEngine<CsvSalesOrder>();
            var records = engine.ReadString(so);
            Assert.Equal("Ahmad, sons and friends", records[0].Name);

            var ie = new FileHelperEngine<CsvOrderItem>();
            var item1 = ie.ReadString(line2);
            Assert.Equal(3999.99m, item1[0].Amount);
            var item2 = ie.ReadString(line3);
            Assert.Equal(3299.99m, item2[0].Amount);
            m_outputHelper.WriteLine(item1[0].ToJson());

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
