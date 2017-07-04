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
    public class IpsExpTestFixture
    {

        private readonly ITestOutputHelper m_helper;

        private readonly string m_sampleXmlDocumentStoreId = "sample";
        private readonly string m_ipsDocumentStoreId = "ipsexpc";

        public IpsExpTestFixture(ITestOutputHelper helper)
        {
            m_helper = helper;
            var sample = new BinaryStore { Content = File.ReadAllBytes(@".\docs\sample.xml") };
            var ips = new BinaryStore { Content = File.ReadAllBytes(@".\docs\IPSExpC_20170606_091542_45349.xml") };
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);

            store.Setup(x => x.GetContentAsync(m_sampleXmlDocumentStoreId)).Returns(Task.FromResult(sample));
            store.Setup(x => x.GetContentAsync(m_ipsDocumentStoreId)).Returns(Task.FromResult(ips));


            ObjectBuilder.AddCacheList(store.Object);
        }

        [Fact]
        public async Task GetSampleMapping()
        {
            var xtf = new XmlTextFormatter
            {
                SampleStoreId = m_ipsDocumentStoreId,
                RootPath = "ips/MailItem/FromIPS",
                Name = "IpsExpPort",
                Namespace = "",
                XmlSchemaStoreId = null
            };
            var fields = await xtf.GetFieldMappingsAsync();
            m_helper.WriteLine("Loading sample file ");
            Assert.Equal(10, fields.Length);

            var dispatch = fields[0];
            Assert.Equal("Dispatch", dispatch.Name);
            Assert.True(dispatch.IsComplex);

            var itemId = new XmlAttributeTextFieldMapping
            {
                Name = "ItemId",
                Path = "$Parent$.ItemId",
                Type = typeof(string)
            };

            var interfaceCode = new XmlAttributeTextFieldMapping
            {
                Name = "InterfaceCode",
                Path = "$Root$.InterfaceCode",
                Type = typeof(string)
            };


            var port = new ReceivePort { Id = m_ipsDocumentStoreId, Name = xtf.Name, Entity = "IpsExpc", TextFormatter = xtf };
            port.FieldMappingCollection.AddRange(fields);
            port.FieldMappingCollection.Add(itemId);
            port.FieldMappingCollection.Add(interfaceCode);
            var ed = await port.GenerateEntityDefinitionAsync();


            var ipsEvent = ed.MemberCollection.SingleOrDefault(x => x.Name == "IPSEvent");
            Assert.NotNull(ipsEvent);
            Assert.True(ipsEvent.AllowMultiple);
            Assert.IsType<ComplexMember>(ipsEvent);
            Assert.Equal(7, ipsEvent.MemberCollection.Count);


            var date = ipsEvent.MemberCollection.OfType<SimpleMember>().SingleOrDefault(x => x.Name == "Date");
            Assert.NotNull(date);
            Assert.Equal(typeof(DateTime), date.Type);


            var codes = await port.GenerateCodeAsync();
            foreach (var @class in codes)
            {
                m_helper.WriteLine("// ====================== " + @class.Name + " ===========================");
                var code = @class.GetCode()
                    .Replace("using FileHelpers;\r\n", "");
                File.WriteAllText($@"..\..\IpsExpPort\{@class.Name}.cs", code);
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
            var port = new IpsExpPort(new PortLogger(m_helper));
            var lines = File.ReadLines(@".\docs\ipsexpe_20170415_221834_43014.xml");
            var items = port.Process(lines).ToArray();

            Assert.Equal(82, items.Length);

            var first = items[0];
            Assert.Equal("AUSYDBMYKULAAEM70095002000070", first.RecptclId);
            Assert.Equal(0.955m, first.ItemWeight);

            Assert.Equal(2, first.IPSEvent.Count);
            Assert.Equal("EJ211039802AU", first.ItemId);
            Assert.Equal("IPSExpE", first.InterfaceCode);

            m_helper.WriteLine(first.ToJson());

        }
        [Fact]
        public void IPSExpC_20170606_091542_45349()
        {
            var port = new IpsExpPort(new PortLogger(m_helper));
            var lines = File.ReadLines(@".\docs\IPSExpC_20170606_091542_45349.xml");
            var items = port.Process(lines).ToArray();

            Assert.Equal(19, items.Length);

            var first = items.Last();
            Assert.Equal("INMAACMYKULAACN70108002000194", first.RecptclId);
            Assert.Equal("P", first.Parcel.ConveyanceTypeCd);

            m_helper.WriteLine(first.ToJson());

        }
    }
}