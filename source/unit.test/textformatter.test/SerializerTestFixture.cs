using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Xunit;
using Xunit.Abstractions;

namespace textformatter.test
{
    [Trait("Serializer", "DelimitedText")]
    public class SerializerTestFixture
    {
        private readonly ITestOutputHelper m_helper;

        public SerializerTestFixture(ITestOutputHelper helper)
        {
            m_helper = helper;
        }
        [Fact]
        public Task SerializeJson()
        {
            var json = File.ReadAllText(@".\docs\vasn.json");
            var ed = json.DeserializeFromJson<EntityDefinition>();
            Assert.Equal("Vasn", ed.Name);

            return Task.FromResult(0);
        }

    }
}
