using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Integrations.Adapters;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace adapter.restapi.test
{
    public class HarGetExtractorTest
    {
        public ITestOutputHelper Console { get; }

        public HarGetExtractorTest(ITestOutputHelper helper)
        {
            Environment.SetEnvironmentVariable($"RX_{ConfigurationManager.ApplicationName}_HOME", @"c:\temp\rx", EnvironmentVariableTarget.Process);

            Console = helper;
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);
            store.SetupAndReturnDoc("har.setting.har");
            ObjectBuilder.AddCacheList(store.Object);

        }
        [Fact]
        public async Task GetRequestAsync()
        {
            var builder = new HarEndpointsBuilder
            {
                StoreId = "har.setting.har"
            };
            var endpoints = await builder.BuildAsync();

            Assert.Equal(1, endpoints.Count());
            Assert.Equal(5, builder.QueryStrings.Keys.Count);
            Assert.Equal(10, builder.RequestHeaders.Keys.Count);
            Assert.Equal("GET", builder.HttpMethod);
            Assert.Equal("HTTP/1.1", builder.HttpVersion);

        }


        [Fact]
        public async Task GetResponseAsync()
        {
            var builder = new HarEndpointsBuilder
            {
                StoreId = "har.setting.har"
            };
            var endpoints = await builder.BuildAsync();
            var entry = endpoints.FirstOrDefault();

            Assert.NotNull(entry);

            Assert.Equal(10, builder.ResponseHeaders.Keys.Count);
            Assert.Equal("gzip", builder.ResponseHeaders["Content-Encoding"]);
            Assert.Equal("OK", builder.ResponseStatusText);
            Assert.Equal(200, builder.ResponseStatus);
            Assert.Equal(2015, builder.ResponseBodySize);
            Assert.Equal("application/json", builder.ResponseContentType);
            Assert.Contains("29T15:49:25.4862826+08:0", builder.ResponseBodySample);
            Assert.Equal("GET", entry.HttpMethod);
            Assert.Equal("HTTP/1.1", builder.HttpVersion);
        }


        [Fact]
        public async Task GetRequestMembers()
        {
            var builder = new HarEndpointsBuilder
            {
                StoreId = "har.setting.har"
            };
            var endpoints = await builder.BuildAsync();
            var entry = endpoints.FirstOrDefault();
            Assert.NotNull(entry);
            Assert.Equal(2, entry.RequestMemberCollection.Count);
        }


        [Fact]
        public async Task BuildResponseMembers()
        {
            var builder = new HarEndpointsBuilder
            {
                StoreId = "har.setting.har"
            };
            var endpoints = await builder.BuildAsync();
            var entry = endpoints.FirstOrDefault();
            Assert.NotNull(entry);

            Assert.Equal("GetApiSystemsSetting", entry.Name);
            Assert.Equal("GET", entry.HttpMethod);

            Assert.Equal(2, entry.ResponseMemberCollection.Count);
            var body = entry.ResponseMemberCollection.Single(x => x.Name == "Body");
            Assert.Equal(8, body.MemberCollection.Count);

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


            var rx = (ComplexMember)body.MemberCollection.Single(x => x.Name == "Rx");
            Assert.False(rx.AllowMultiple);

            var version = (SimpleMember)rx.MemberCollection.Single(x => x.Name == "Version");
            Assert.Equal(false, version.AllowMultiple);
            Assert.Equal(typeof(string), version.Type);
        }
    }
}