using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Integrations.Adapters;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace adapter.restapi.test
{
    public class HarPostExtractorTest
    {
        public ITestOutputHelper Console { get; }

        public HarPostExtractorTest(ITestOutputHelper helper)
        {
            Environment.SetEnvironmentVariable($"RX_{ConfigurationManager.ApplicationName}_HOME", @"c:\temp\rx",
                EnvironmentVariableTarget.Process);

            Console = helper;
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);

            store.SetupAndReturnDoc("post.snb.har");
            ObjectBuilder.AddCacheList(store.Object);

        }
        private JObject GetHar()
        {
            var text = File.ReadAllText("post.snb.har");
            return JObject.Parse(text);
        }
        [Fact]
        public void PostRequest()
        {
            var json = GetHar();
            var entry = json.SelectToken("$.log.entries").Select(e => new RestApiOperationDefinition(e)).Last();

            Assert.Equal(0, entry.QueryStrings.Keys.Count);
            Assert.Equal(10, entry.RequestHeaders.Keys.Count);
            Assert.Equal("POST", entry.HttpMethod);
            Assert.Equal("HTTP/1.1", entry.HttpVersion);

        }


        [Fact]
        public async Task BuildRequestMembers()
        {
            var json = GetHar();
            var entry = json.SelectToken("$.log.entries").Select(e => new RestApiOperationDefinition(e)).Last();

            await entry.BuildAsync();
            
            Assert.Equal("POST", entry.HttpMethod);
            Assert.Equal("Post1", entry.Name);

            Assert.Equal(3, entry.RequestMemberCollection.Count);
            var header = entry.RequestMemberCollection.Single(x => x.Name == "Headers");
            Assert.Equal(10, header.MemberCollection.Count);
            Assert.False(header.AllowMultiple);


            var body = entry.RequestMemberCollection.Single(x => x.Name == "Body");
            Assert.Equal(7, body.MemberCollection.Count);

            var number = (SimpleMember)body.MemberCollection.Single(x => x.Name == "Number");
            Assert.Equal(typeof(string), number.Type);

        }
    }
}