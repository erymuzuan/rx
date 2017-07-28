using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.MyApp.ReceivePorts;
using Bespoke.Sph.Domain;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace textformatter.test
{
    [Trait("XML", "TextFormatter")]
    public class AcceptanceDataTestFixture
    {
        private readonly ITestOutputHelper m_helper;
        private readonly string m_sampleXmlDocumentStoreId = "sample";
        private readonly string m_acceptanceXmlDocumentStoreId = "acceptance";
        
        [Fact]
        public async Task GetSampleMapping()
        {
            var xtf = new XmlTextFormatter
            {
                SampleStoreId = m_acceptanceXmlDocumentStoreId,
                RootPath = "Data/AcceptanceData",
                Name = "AcceptanceDataPort",
                Namespace = "",
                XmlSchemaStoreId = null
            };
            var fields = await xtf.GetFieldMappingsAsync();
            m_helper.WriteLine("Loading sample file");
            Assert.Equal(6, fields.Length);

            var tellerId = fields[0];
            Assert.Equal("TellerID", tellerId.Name);

            var trxObject = fields[2];
            Assert.Equal("TrxObject", trxObject.Name);
            Assert.True(trxObject.IsComplex);

            var trxDateTime = trxObject.FieldMappingCollection.Single(x => x.Name == "TrxDateTime");
            trxDateTime.Type = typeof(DateTime);
            trxDateTime.IsNullable = false;
            trxDateTime.Converter = "d/M/yyyy h:m:ss tt";

            var attributeDateTime = trxObject.FieldMappingCollection.OfType<XmlAttributeTextFieldMapping>().Single(x => x.Name == "DateTime");
            attributeDateTime.Type = typeof(DateTime);
            attributeDateTime.IsNullable = false;
            attributeDateTime.Converter = "d/M/yyyy h:m:ss tt";



            var conNoteObject = fields[3];
            var weightField = conNoteObject.FieldMappingCollection.Single(x => x.Name == "Weight");
            weightField.IsNullable = true;


            var dateHeader = new UriFieldMapping
            {
                Name = "Date",
                Type = typeof(DateTime),
                Pattern = "AcceptanceData(?<value>[0-9]{8})[0-9]{6}.xml",
                WebId = Strings.GenerateId(),
                Converter = "yyyyMMdd"

            };

            var portOriginal = new ReceivePort { Id = m_acceptanceXmlDocumentStoreId, Name = xtf.Name, Entity = "AcceptanceData", TextFormatter = xtf };
            portOriginal.FieldMappingCollection.AddRange(fields);
            portOriginal.FieldMappingCollection.Add(dateHeader);
            var port = portOriginal.JsonClone(); // try to simulate the definition is read from json source file


            var ed = await port.GenerateEntityDefinitionAsync();

            var tellerIdMember = ed.MemberCollection.SingleOrDefault(x => x.Name == "TellerID");
            Assert.NotNull(tellerIdMember);

            var connoteObject = ed.MemberCollection.OfType<ComplexMember>().SingleOrDefault(x => x.Name == "ConnoteObject");
            Assert.NotNull(connoteObject);
            Assert.Equal(11, connoteObject.MemberCollection.Count);
            Assert.Equal(connoteObject.Name, connoteObject.TypeName);


            var weight = connoteObject.MemberCollection.OfType<SimpleMember>().SingleOrDefault(x => x.Name == "Weight");
            Assert.NotNull(weight);
            Assert.Equal(typeof(decimal), weight.Type);


            var codes = await port.GenerateCodeAsync();
            foreach (var @class in codes)
            {
                var code = @class.GetCode()
                    .Replace("using FileHelpers;\r\n", "")
                    .Replace(" [FieldHidden]\r\n", "");
                // m_helper.WriteLine("// ====================== " + @class.Name + " ===========================");
                //m_helper.WriteLine(code);
                File.WriteAllText($@"..\..\AcceptanceDataPort\{@class.Name}.cs", code);
            }


            // now compile this
            var cr = await port.CompileAsync();
            Assert.True(cr.Result, cr.Errors.ToString("\r\n"));

        }
        [Fact]
        public void ProcessRecord()
        {
            var port = new AcceptanceDataPort(new PortLogger(m_helper))
            {
                Uri = new Uri(Path.GetFullPath(@".\docs\AcceptanceData20170605144701.xml"))
            };
            var lines = File.ReadLines(port.Uri.LocalPath);
            var acceptances = port.Process(lines).ToArray();

            Assert.Equal(2, acceptances.Length);

            var first = acceptances[0];
            Assert.Equal("4398MASNIYATI", first.TellerID);
            Assert.Equal(10141003, first.BranchCode);
            Assert.True(first.ConnoteObject.Weight.HasValue);
            Assert.Equal(2.250m, first.ConnoteObject.Weight.Value);
            Assert.NotNull(first.Date);
            Assert.Equal(2017, first.Date.Value.Year);
            Assert.Equal(6, first.Date.Value.Month);
            Assert.Equal(05, first.Date.Value.Day);

            Assert.Equal(DateTime.Parse("2017-06-05T14:38:02.0000000"), first.TrxObject.TrxDateTime);

            m_helper.WriteLine(first.ToJson());

        }
        
        public AcceptanceDataTestFixture(ITestOutputHelper helper)
        {
            m_helper = helper;
            var sample = new BinaryStore { Content = File.ReadAllBytes(@".\docs\sample.xml") };
            var acceptance = new BinaryStore { Content = File.ReadAllBytes(@".\docs\AcceptanceData20170605144701.xml") };
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);

            store.Setup(x => x.GetContentAsync(m_sampleXmlDocumentStoreId)).Returns(Task.FromResult(sample));
            store.Setup(x => x.GetContentAsync(m_acceptanceXmlDocumentStoreId)).Returns(Task.FromResult(acceptance));


            ObjectBuilder.AddCacheList(store.Object);
        }
        class PortLogger : ILogger
        {
            private readonly ITestOutputHelper m_helper;

            public PortLogger(ITestOutputHelper helper)
            {
                m_helper = helper;
            }
            public Task LogAsync(LogEntry entry)
            {
                m_helper.WriteLine(entry.ToString());
                return Task.FromResult(0);
            }

            public void Log(LogEntry entry)
            {
                m_helper.WriteLine(entry.ToString());
            }
        }
    }
}