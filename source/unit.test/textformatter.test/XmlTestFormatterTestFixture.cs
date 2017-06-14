using System;
using System.IO;
using System.Threading.Tasks;
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
                RootPath = "Data/AcceptanceData"
            };
            var fields = await xtf.GetFieldMappingsAsync();
            m_helper.WriteLine("Loading sample file");
            Assert.Equal(6, fields.Length);

            var tellerId = fields[0];
            Assert.Equal("TellerID", tellerId.Name);


            var trxObject = fields[2];

            Assert.Equal("TrxObject", trxObject.Name);
            Assert.True(trxObject.IsComplex);


        }
    }
}