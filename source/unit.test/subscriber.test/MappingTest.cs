using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace subscriber.test
{
    public class MappingTest
    {
        private readonly ITestOutputHelper m_outputHelper;

        public MappingTest(ITestOutputHelper outputHelper)
        {
            m_outputHelper = outputHelper;
        }

        [Fact]
        public void GenerateColumn()
        {
            var ent = new EntityDefinition { Name = "Customer", Plural = "Customers" };
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Name",
                TypeName = "System.String, mscorlib",
                IsFilterable = true,
                IsAnalyzed = true,
                Boost = 5
            });
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Title",
                TypeName = "System.String, mscorlib",
                IsFilterable = true,
                IsAnalyzed = false
            });
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "RegisteredDate",
                TypeName = "System.DateTime, mscorlib",
                IsFilterable = true
            });
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Age",
                TypeName = "System.Int32, mscorlib",
                IsFilterable = true
            });
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Salary",
                TypeName = "System.Decimal, mscorlib",
                IsFilterable = true,
                IsExcludeInAll = true
            });
            var address = new SimpleMember { Name = "Address", TypeName = "System.Object, mscorlib" };
            address.MemberCollection.Add(new SimpleMember { Name = "Street1", IsFilterable = false, TypeName = "System.String, mscorlib", IsAnalyzed = true });
            address.MemberCollection.Add(new SimpleMember { Name = "Street2", IsNotIndexed = true, TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new SimpleMember { Name = "Postcode", IsFilterable = true, TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new SimpleMember { Name = "State", IsFilterable = true, TypeName = "System.String, mscorlib" });
            ent.MemberCollection.Add(address);


            var locality = new SimpleMember { Name = "Locality", Type = typeof(object) };
            locality.Add(new Dictionary<string, Type> { { "Mode", typeof(string) } });
            address.MemberCollection.Add(locality);

            var map = ent.GetElasticsearchMapping();
            m_outputHelper.WriteLine(map);
            Assert.Contains("\"type\":", map);

        }


        [Fact]
        public void MapCustomer()
        {
            var customerDefinition = File.ReadAllText(Path.Combine(ConfigurationManager.SphSourceDirectory, "EntityDefinition/Customer.json"));
            var ed = customerDefinition.DeserializeFromJson<EntityDefinition>();

            var map = ed.GetElasticsearchMapping();
            m_outputHelper.WriteLine(map);
            Assert.Contains("\"type\":", map);

        }
        [Fact]
        public async Task CompareCustomerMapping()
        {
            var customerDefinition = File.ReadAllText(Path.Combine(ConfigurationManager.SphSourceDirectory, "EntityDefinition/Customer.json"));
            var ed = customerDefinition.DeserializeFromJson<EntityDefinition>();
            var map = ed.GetElasticsearchMapping();

            string url = $"{ConfigurationManager.ApplicationName.ToLowerInvariant()}/_mapping/customer";

            var content = new StringContent(map);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);

                var response = await client.PutAsync(url, content);
                m_outputHelper.WriteLine($"Response status : {response.StatusCode}");
                while (response.StatusCode != HttpStatusCode.OK)
                {
                    m_outputHelper.WriteLine(response.StatusCode.ToString() + ".");
                    response = await client.PutAsync(ConfigurationManager.ApplicationName.ToLowerInvariant(), new StringContent(""));

                }
                var existingMapping = (await client.GetStringAsync(url))
                            .Replace(".0", string.Empty)
                            .Replace(@"""norms"":{""enabled"":false},", string.Empty)
                            .Replace("\"format\":\"dateOptionalTime\",", string.Empty)
                            .Replace(",\"format\":\"dateOptionalTime\"", string.Empty)
                            .Replace("\"ignore_malformed\":false,", string.Empty)
                            .Replace(",\"ignore_malformed\":false", string.Empty)
                            .Replace("\"index_options\":\"docs\",", string.Empty)
                            .Replace(",\"index_options\":\"docs\"", string.Empty);


                var mapJson = map
                .Replace("\"boost\":1,", string.Empty)
                .Replace(",\"boost\":1", string.Empty)
                .Replace("\"index\":\"analyzed\",", string.Empty)
                .Replace(",\"index\":\"analyzed\"", string.Empty);

                dynamic serverMapping = JsonConvert.DeserializeObject(existingMapping);
                dynamic generatedMapping = JsonConvert.DeserializeObject(mapJson);
                dynamic sm = serverMapping.devv1.mappings.customer.properties;
                dynamic gm = generatedMapping.customer.properties;

                var count = 0;
                foreach (var g in gm.Children())
                {
                    m_outputHelper.WriteLine("[{0}]:\t" + g.Name, count++);
                    var g0 = g as JProperty;
                    if (null == g0) continue;
                    var s0 = ((JObject)sm).Children().OfType<JProperty>().SingleOrDefault(h => h.Name == g0.Name);
                    //m_outputHelper.WriteLine(c0);
                    if (g0.ToString().Contains("\"type\": \"object\""))
                    {
                        m_outputHelper.WriteLine(@".." + g0.Children()[0]);
                        continue;
                    }
                    dynamic g1 = JObject.Parse(g0.ToString().Replace("\"" + g.Name + "\":", ""));
                    dynamic s1 = JObject.Parse(s0?.ToString().Replace("\"" + g.Name + "\":", ""));
                    Assert.Equal(g1.type, s1.type);
                    if (g1.ToString() != s1.ToString())
                    {
                        m_outputHelper.WriteLine("Generated : " + g1);
                        m_outputHelper.WriteLine("Server : " + s1);
                    }

                    var g1Index = "analyzed";
                    var s1Index = "analyzed";

                    if (g1.index is JValue)
                    {
                        g1Index = g1.index.ToString();
                    }
                    if (s1.index is JValue)
                    {
                        s1Index = s1.index.ToString();
                    }


                    if (s1.type == "boolean")
                    {
                        s1Index = "not_analyzed";
                    }

                    Assert.Equal(g1Index, s1Index);
                    if (s1.type == "string")
                        Assert.Equal(g1.include_in_all ?? false, s1.include_in_all ?? false);
                }
                Assert.Equal(sm.Count, gm.Count);
                Assert.Equal(sm.CreatedBy, gm.CreatedBy);
            }

            //

        }
    }
}
