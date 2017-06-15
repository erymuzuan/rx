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
    public class XmlTestFormatterTestFixture
    {

        private readonly ITestOutputHelper m_helper;

        private readonly string m_sampleXmlDocumentStoreId = "sample";
        private readonly string m_acceptanceXmlDocumentStoreId = "acceptance";

        public XmlTestFormatterTestFixture(ITestOutputHelper helper)
        {
            m_helper = helper;
            var sample = new BinaryStore { Content = File.ReadAllBytes(@".\docs\sample.xml") };
            var acceptance = new BinaryStore { Content = File.ReadAllBytes(@".\docs\AcceptanceData20170605144701.xml") };
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);

            store.Setup(x => x.GetContentAsync(m_sampleXmlDocumentStoreId)).Returns(Task.FromResult(sample));
            store.Setup(x => x.GetContentAsync(m_acceptanceXmlDocumentStoreId)).Returns(Task.FromResult(acceptance));


            ObjectBuilder.AddCacheList(store.Object);
        }

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

            var conNoteObject = fields[3];
            var weightField = conNoteObject.FieldMappingCollection.Single(x => x.Name == "Weight");
            weightField.IsNullable = true;

            var port = new ReceivePort { Id = m_acceptanceXmlDocumentStoreId, Name = xtf.Name, Entity = "AcceptanceData", TextFormatter = xtf };
            port.FieldMappingCollection.AddRange(fields);
            var ed = await port.GenerateEntityDefinitionAsync();

            var tellerIdMember = ed.MemberCollection.SingleOrDefault(x => x.Name == "TellerID");
            Assert.NotNull(tellerIdMember);

            var connoteObject = ed.MemberCollection.SingleOrDefault(x => x.Name == "ConnoteObject");
            Assert.NotNull(connoteObject);
            Assert.IsType<ComplexMember>(connoteObject);
            Assert.Equal(11, connoteObject.MemberCollection.Count);


            var weight = connoteObject.MemberCollection.OfType<SimpleMember>().SingleOrDefault(x => x.Name == "Weight");
            Assert.NotNull(weight);
            Assert.Equal(typeof(decimal), weight.Type);


            var codes = await port.GenerateCodeAsync();
            foreach (var @class in codes.Where(x => x.Name == "AcceptanceDataPort"))
            {
                m_helper.WriteLine("// ====================== " + @class.Name + " ===========================");
                m_helper.WriteLine(@class.GetCode());
            }

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
        [Fact]
        public void ProcessRecord()
        {
            var port = new AcceptanceDataPort(new PortLogger(m_helper));
            var lines = File.ReadLines(@".\docs\AcceptanceData20170605144701.xml");
            var acceptances = port.Process(lines).ToArray();

            Assert.Equal(2, acceptances.Length);

            var first = acceptances[0];
            Assert.Equal("4398MASNIYATI", first.TellerID);
            Assert.Equal(10141003, first.BranchCode);
            Assert.True(first.ConnoteObject.Weight.HasValue);
            Assert.Equal(2.250m, first.ConnoteObject.Weight.Value);

            m_helper.WriteLine(first.ToJson());

        }
    }

}