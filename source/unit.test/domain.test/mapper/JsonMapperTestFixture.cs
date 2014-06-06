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

        public static EntityDefinition GetOracleEntityDefinition()
        {
            var ed = new EntityDefinition
            {
                Name = "SURVEY",
                RecordName = "SERIAL_NUM"

            };
            ed.MemberCollection.Add(new Member { Name = "SERIAL_NUM", Type = typeof(string) });
            ed.MemberCollection.Add(new Member { Name = "FULL_NAME", Type = typeof(string) });
            ed.MemberCollection.Add(new Member { Name = "APPROVED", Type = typeof(bool) });
            ed.MemberCollection.Add(new Member { Name = "RATING", Type = typeof(int) });
            ed.MemberCollection.Add(new Member { Name = "SUBMITTED", Type = typeof(DateTime), IsNullable = true });
            ed.MemberCollection.Add(new Member { Name = "SEX", Type = typeof(string) });
            ed.MemberCollection.Add(new Member { Name = "ADDRESS_STREET", Type = typeof(string) });
            ed.MemberCollection.Add(new Member { Name = "ADDRESS_POSTCODE", Type = typeof(string) });

            var products = new Member { Name = "PRODUCTCollection", Type = typeof(Array) };
            products.MemberCollection.Add(new Member { Name = "SURVEY_ID", Type = typeof(int) });
            products.MemberCollection.Add(new Member { Name = "CODE", Type = typeof(string) });
            products.MemberCollection.Add(new Member { Name = "UNIT", Type = typeof(int) });
            products.MemberCollection.Add(new Member { Name = "PRICE", Type = typeof(decimal) });
            ed.MemberCollection.Add(products);

            return ed;

        }

        public static dynamic GetOracleInstance()
        {
            var ed = GetOracleEntityDefinition();
            var type = CustomerEntityHelper.CompileEntityDefinition(ed);
            return CustomerEntityHelper.CreateInstance(type);
        }

        public static EntityDefinition GetSourceEntityDefinition()
        {
            var ed = new EntityDefinition
            {
                Name = "Survey",
                RecordName = "SerialNo"

            };
            ed.MemberCollection.Add(new Member { Name = "SerialNo", Type = typeof(string) });
            ed.MemberCollection.Add(new Member { Name = "FirstName", Type = typeof(string) });
            ed.MemberCollection.Add(new Member { Name = "LastName", Type = typeof(string) });
            ed.MemberCollection.Add(new Member { Name = "IsApproved", Type = typeof(bool) });
            ed.MemberCollection.Add(new Member { Name = "Rating", Type = typeof(int) });
            ed.MemberCollection.Add(new Member { Name = "DateSubmitted", Type = typeof(DateTime), IsNullable = true });
            ed.MemberCollection.Add(new Member { Name = "Gender", Type = typeof(string) });
            var address = new Member { Name = "Address", Type = typeof(object) };
            address.MemberCollection.Add(new Member { Name = "Street", Type = typeof(string) });
            address.MemberCollection.Add(new Member { Name = "Postcode", Type = typeof(string) });
            ed.MemberCollection.Add(address);
            var products = new Member { Name = "ProductCollection", Type = typeof(Array) };
            products.MemberCollection.Add(new Member { Name = "Code", Type = typeof(string) });
            products.MemberCollection.Add(new Member { Name = "Unit", Type = typeof(int) });
            products.MemberCollection.Add(new Member { Name = "Price", Type = typeof(decimal) });
            ed.MemberCollection.Add(products);

            return ed;

        }

        public static dynamic GetSourceInstance()
        {
            var ed = GetSourceEntityDefinition();
            var type = CustomerEntityHelper.CompileEntityDefinition(ed);
            return CustomerEntityHelper.CreateInstance(type);
        }

        [Test]
        public void ReadMappingFile()
        {
            var map = new TransformDefinition { Name = "Test Survey Mapping", Description = "Just a description" };
            map.MapCollection.Add(new DirectMap
            {
                Source = "SurveId",
                Type = typeof(int),
                Destination = "SURVEY_ID"
            });

            var sc = new StringConcateFunctoid();
            sc.ArgumentCollection.Add(new DocumentField { Path = "FirstName" });
            sc.ArgumentCollection.Add(new ConstantField { Value = " ", Type = typeof(string) });
            sc.ArgumentCollection.Add(new DocumentField { Path = "LastName" });

            map.MapCollection.Add(new FunctoidMap
            {
                Functoid = sc
            });
            Console.WriteLine(map.ToJsonString(Formatting.Indented));
        }

        [Test]
        public async Task GetEntityMap()
        {
            var survey = GetSourceInstance();
            survey.SerialNo = "A123";
            survey.FirstName = "Erymuzuan";
            survey.LastName = "Mustapa";
            survey.Gender = "Male";
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
            map.MapCollection.Add(new FunctoidMap
            {
                Functoid = new ScriptFunctoid
                {
                    Expression = "return item.Gender == \"Male\"? \"Lelaki\" : \"Perempuan\";"
                },
                Destination = "SEX"
            });

            var text = await map.TransformAsync(survey);
            Console.WriteLine(text);
            JObject json = JObject.Parse(text);
            Assert.AreEqual(500, json.SelectToken("$.SURVEY_ID").Value<int>());


        }
    }
}
