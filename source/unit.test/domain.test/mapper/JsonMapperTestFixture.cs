using System;
using System.Threading.Tasks;
using Bespoke.Sph.RoslynScriptEngines;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Bespoke.Sph.Domain;

namespace domain.test.mapper
{
    [TestFixture]
    class JsonMapperTestFixture
    {
        [SetUp]
        public void Setup()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
        }
        
        public static EntityDefinition GetSurverEntityDefinition()
        {
            var ed = new EntityDefinition
            {
                Name = "Survey",
                RecordName = "SerialNo"

            };
            ed.MemberCollection.Add(new Member{Name = "SerialNo", Type = typeof(string)});
            ed.MemberCollection.Add(new Member{Name = "FirstName", Type = typeof(string)});
            ed.MemberCollection.Add(new Member{Name = "LastName", Type = typeof(string)});

            return ed;

        }

        public static dynamic GetInstance()
        {
            var ed = GetSurverEntityDefinition();
            var type = CustomerEntityHelper.CompileEntityDefinition(ed);
            return CustomerEntityHelper.CreateInstance(type);
        }

        [Test]
        public void ReadMappingFile()
        {
            var map = new TransformDefinition {Name = "Test Survey Mapping", Description = "Just a description"};
            map.MapCollection.Add(new DirectMap
            {
                Source = "SurveId",
                Type = typeof(int),
                Destination = "SURVEY_ID"
            });

            var sc = new StringConcateFunctoid();
            sc.ArgumentCollection.Add(new DocumentField{Path = "FirstName"});
            sc.ArgumentCollection.Add(new ConstantField{Value = " ", Type = typeof(string)});
            sc.ArgumentCollection.Add(new DocumentField{Path = "LastName"});

            map.MapCollection.Add(new FunctoidMap
            {
                Functoid = sc
            });
            Console.WriteLine(map.ToJsonString(Formatting.Indented));
        }

        [Test]
        public async  Task GetEntityMap()
        {
            var survey = GetInstance();
            survey.SerialNo = "A123";
            survey.FirstName = "Erymuzuan";
            survey.LastName = "Mustapa";
            survey.SurveyId = 500;


            var map = new TransformDefinition { Name = "Test Survey Mapping", Description = "Just a description" };
            map.MapCollection.Add(new DirectMap
            {
                Source = "SurveyId",
                Type = typeof(int),
                Destination = "SURVEY_ID"
            });

            var sc = new StringConcateFunctoid();
            sc.ArgumentCollection.Add(new DocumentField { Path = "FirstName" });
            sc.ArgumentCollection.Add(new ConstantField { Value = " ", Type = typeof(string) });
            sc.ArgumentCollection.Add(new DocumentField { Path = "LastName" });

            map.MapCollection.Add(new FunctoidMap
            {
                Functoid = sc,
                Destination = "FULL_NAME"
            });

            var text =await map.TransformAsync(survey);
            Console.WriteLine(text);
            JObject json = JObject.Parse(text);
            Assert.AreEqual(500, json.SelectToken("$.SURVEY_ID").Value<int>());


        }
    }
}
