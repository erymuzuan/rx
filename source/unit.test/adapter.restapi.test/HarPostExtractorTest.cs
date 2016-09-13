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
        public HarEndpointsBuilder GetBuilder(int position = 0)
        {
            var json = JObject.Parse(File.ReadAllText("post.snb.har"));
            var token = json.SelectToken("$.log.entries").ToArray()[position];
            return new HarEndpointsBuilder(token);
        }

        public HarPostExtractorTest(ITestOutputHelper helper)
        {
            Environment.SetEnvironmentVariable($"RX_{ConfigurationManager.ApplicationName}_HOME", @"c:\temp\rx",
                EnvironmentVariableTarget.Process);

            Console = helper;
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);

            store.SetupAndReturnDoc("post.snb.har");
            ObjectBuilder.AddCacheList(store.Object);

        }
        [Fact]
        public async Task PostRequestAsync()
        {
            var builder = GetBuilder(1);
            var entry = await builder.BuildAsync();
            Assert.NotNull(entry);
            Assert.Equal("PostApiCmdUpdateVirtualBankAccount", entry.Name);

            Assert.Equal("POST", entry.HttpMethod);
            Assert.Equal("HTTP/1.1", builder.HttpVersion);

        }


        [Fact]
        public async Task BuildRequestMembers()
        {
            var builder = GetBuilder(1);
            var entry = await builder.BuildAsync();
            Assert.NotNull(entry);

            Assert.Equal("POST", entry.HttpMethod);
            Assert.Equal("PostApiCmdUpdateVirtualBankAccount", entry.Name);

            Assert.Equal(3, entry.RequestMemberCollection.Count);
            Assert.Equal("http://eryken2.asuscomm.com:8086/api/cmd/UpdateVirtualBankAccount", entry.BaseAddress);
            Assert.Equal("POST", entry.HttpMethod);


            var header = entry.RequestMemberCollection.Single(x => x.Name == "Headers");
            Assert.Equal("Cache-Control,Pragma,Content-Type,Expires,Server,Access-Control-Allow-Origin,Access-Control-Allow-Credentials,X-AspNet-Version,X-Powered-By,Date,Content-Length",
                header.MemberCollection.ToString(",", x => x.FullName));
            Assert.Equal(11, header.MemberCollection.Count);
            Assert.False(header.AllowMultiple);


            var body = entry.RequestMemberCollection.Single(x => x.Name == "Body");
            Assert.Equal(7, body.MemberCollection.Count);

            var number = (SimpleMember)body.MemberCollection.Single(x => x.Name == "Number");
            Assert.Equal(typeof(string), number.Type);

        }
    }
}