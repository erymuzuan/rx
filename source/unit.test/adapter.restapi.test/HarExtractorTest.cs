using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Moq;
using Newtonsoft.Json.Linq;
using restapi.adapter;
using Xunit;
using Xunit.Abstractions;

namespace adapter.restapi.test
{
    public class HarExtractorTest
    {
        public ITestOutputHelper Console { get; }

        public HarExtractorTest(ITestOutputHelper helper)
        {
            Environment.SetEnvironmentVariable($"RX_{ConfigurationManager.ApplicationName}_HOME", @"c:\temp\rx", EnvironmentVariableTarget.Process);

            Console = helper;
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);

            store.SetupAndReturnDoc("har.setting.har");
            ObjectBuilder.AddCacheList(store.Object);

        }
        [Fact]
        public void GetRequest()
        {
            var json = JObject.Parse(File.ReadAllText("har.setting.har"));
            var entry = json.SelectToken("$.log.entries").Select(e => new RestApiOperationDefinition(e)).First();
            
            Assert.Equal(5, entry.QueryStrings.Keys.Count);
            Assert.Equal(10, entry.RequestHeaders.Keys.Count);
            Assert.Equal("GET", entry.HttpMethod);
            Assert.Equal("HTTP/1.1", entry.HttpVersion);
            
        }

        [Fact]
        public void GetResponse()
        {
            var json = JObject.Parse(File.ReadAllText("har.setting.har"));
            var entry = json.SelectToken("$.log.entries").Select(e => new RestApiOperationDefinition(e)).First();
            
            Assert.Equal(10, entry.ResponseHeaders.Keys.Count);
            Assert.Equal("gzip", entry.ResponseHeaders["Content-Encoding"]);
            Assert.Equal("OK", entry.ResponseStatusText);
            Assert.Equal(200, entry.ResponseStatus);
            Assert.Equal(2015, entry.ResponseBodySize);
            Assert.Equal("application/json", entry.ResponseContentType);
            Assert.Contains("29T15:49:25.4862826+08:0", entry.ResponseBodySample);
            Assert.Equal("GET", entry.HttpMethod);
            Assert.Equal("HTTP/1.1", entry.HttpVersion);
        }


        [Fact]
        public async Task GetRequestMembers()
        {
            var json = JObject.Parse(File.ReadAllText("har.setting.har"));
            var entry = json.SelectToken("$.log.entries").Select(e => new RestApiOperationDefinition(e)).First();

            await entry.BuildAsync();
            Assert.Equal(15, entry.RequestMemberCollection.Count);
        }
        [Fact]
        public async Task BuildResponseMembers()
        {
            var json = JObject.Parse(File.ReadAllText("har.setting.har"));
            var entry = json.SelectToken("$.log.entries").Select(e => new RestApiOperationDefinition(e)).First();

            await entry.BuildAsync();
            Assert.Equal(2, entry.ResponseMemberCollection.Count);
            var body = entry.ResponseMemberCollection.Single(x => x.Name == "Body");
            Assert.Equal(7, body.MemberCollection.Count);

            var row = (SimpleMember)body.MemberCollection.Single(x => x.Name == "Size");
            Assert.Equal(typeof(int), row.Type);

            var results = (ComplexMember)body.MemberCollection.Single(x => x.Name == "Results");
            Assert.Equal(true, results.AllowMultiple);
            Assert.Equal("ResultsItem", results.TypeName);
            Assert.Equal(10, results.MemberCollection.Count);

            var userName = (SimpleMember)results.MemberCollection.Single(x => x.Name == "UserName");
            Assert.Equal(false, userName.AllowMultiple);
            Assert.Equal(typeof(string), userName.Type);

            var changedDate = (SimpleMember)results.MemberCollection.Single(x => x.Name == "ChangedDate");
            Assert.Equal(false, changedDate.AllowMultiple);
            Assert.Equal(typeof(DateTime), changedDate.Type);
        }
    }
}