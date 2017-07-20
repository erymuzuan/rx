using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.MailItems.ReceivePorts;
using Bespoke.Sph.Domain;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace textformatter.test
{
    [Trait("XML", "TextFormatter")]
    public class MailItemTestFixture
    {

        private readonly ITestOutputHelper m_helper;

        private readonly string m_sampleXmlDocumentStoreId = "sample";
        private readonly string m_ipsDocumentStoreId = "ipsexpc";

        public MailItemTestFixture(ITestOutputHelper helper)
        {
            m_helper = helper;
            var sample = new BinaryStore { Content = File.ReadAllBytes(@".\docs\sample.xml") };
            var ips = new BinaryStore { Content = File.ReadAllBytes(@".\docs\IPSExport.xml") };
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
                RootPath = "ips/MailItem",
                Name = "MailItemPort",
                Namespace = "http://upu.int/ips",
                XmlSchemaStoreId = null
            };
            var fields = await xtf.GetFieldMappingsAsync();
            m_helper.WriteLine("Loading sample file ");
            Assert.Equal(3, fields.Length);

            var ips = fields[1];
            Assert.Equal("FromIPS", ips.Name);
            Assert.True(ips.IsComplex);
            

            var edi = fields[2];
            Assert.Equal("FromEDI", edi.Name);
            Assert.True(edi.IsComplex);
            edi.IsNullable = true;

            var interfaceCode = new XmlAttributeTextFieldMapping
            {
                Name = "InterfaceCode",
                Path = "$Root$.InterfaceCode",
                Type = typeof(string)
            };


            var port = new ReceivePort { Id = m_ipsDocumentStoreId, Name = xtf.Name, Entity = "MailItem", TextFormatter = xtf };
            port.FieldMappingCollection.AddRange(fields);
            port.FieldMappingCollection.Add(interfaceCode);

            var ipsEvent = port.FieldMappingCollection.OfType<XmlElementTextFieldMapping>()
                .Single(x => x.Name == "FromIPS")
                .FieldMappingCollection.OfType<XmlElementTextFieldMapping>()
                .Single(x => x.Name == "IPSEvent");
            ipsEvent.FieldMappingCollection.Single(x => x.Name == "RetentionReasonCd").IsNullable = true;
            ipsEvent.FieldMappingCollection.Single(x => x.Name == "ConditionCd").IsNullable = true;

            //var ed = await port.JsonClone().GenerateEntityDefinitionAsync();
            //var result = await ed.ValidateBuildAsync();
            //Assert.True(result.Result);
            


            var codes = await port.Clone().GenerateCodeAsync();
            foreach (var @class in codes)
            {
                m_helper.WriteLine("// ====================== " + @class.Name + " ===========================");
                var code = @class.GetCode()
                    .Replace("using FileHelpers;\r\n", "")
                    .Replace("namespace Bespoke.MyApp.ReceivePorts", "namespace Bespoke.MailItems.ReceivePorts");
                File.WriteAllText($@"..\..\MailItem\{@class.Name}.cs", code);
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
            var port = new MailItemPort(new PortLogger(m_helper));
            var lines = File.ReadLines(@".\docs\IpsExport.xml");
            var items = port.Process(lines).ToArray();

            Assert.Equal(1, items.Length);

            var first = items[0];
            Assert.Equal("AUSYDBMYKULAAEM70095002000070", first.FromIPS.RecptclId);
            Assert.Equal(0.955m, first.FromIPS.ItemWeight);

            Assert.Equal(2, first.FromIPS.IPSEvent.Count);
            Assert.Equal("EJ211039802AU", first.ItemId);
            Assert.Equal("IPSExpE", first.InterfaceCode);

            var ipsEvent1 = first.FromIPS.IPSEvent.First();
            Assert.Equal("TN030", ipsEvent1.TNCd);
            var ipsEvent2 = first.FromIPS.IPSEvent.Last();
            Assert.Equal("TN031", ipsEvent2.TNCd);

            m_helper.WriteLine(first.ToJson());

        }
        [Fact]
        public void IpsExport()
        {
            var port = new MailItemPort(new PortLogger(m_helper));
            var lines = File.ReadLines(@".\docs\IpsExport.xml");
            var items = port.Process(lines).ToArray();

            Assert.Equal(1, items.Length);

            var last = items.Last();
            Assert.Equal("AUSYDBMYKULAAEM70095002000070", last.FromIPS.RecptclId);
            Assert.Equal("P", last.FromIPS.Parcel.ConveyanceTypeCd);

            m_helper.WriteLine(last.ToJson());

        }
    }
}