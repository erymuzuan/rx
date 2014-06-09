using System;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.RoslynScriptEngines;
using Newtonsoft.Json;
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

        public static dynamic GetOracleInstance()
        {
            var folder = ConfigurationManager.WorkflowCompilerOutputPath;
            var ed = Assembly.LoadFile(folder + @"\Dev.EMPLOYEES.dll");
            var type = ed.GetType("EMPLOYEES");
            return Activator.CreateInstance(type);
        }

        public static EntityDefinition GetSourceEntityDefinition()
        {
            var ed = new EntityDefinition
            {
                Name = "Employee",
                RecordName = "EmployeeId"

            };
            ed.MemberCollection.Add(new Member { Name = "FirstName", Type = typeof(string) });
            ed.MemberCollection.Add(new Member { Name = "LastName", Type = typeof(string) });
            ed.MemberCollection.Add(new Member { Name = "Phone", Type = typeof(string) });
            ed.MemberCollection.Add(new Member { Name = "Email", Type = typeof(string) });
            ed.MemberCollection.Add(new Member { Name = "HireDate", Type = typeof(DateTime), IsNullable = true });

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
            var emp = GetSourceInstance();
            emp.FirstName = "Erymuzuan";
            emp.LastName = "Mustapa";
            emp.Phone = "0123889200";
            emp.HireDate = new DateTime(2000, 1, 1);
            emp.EmployeeId = 500;


            var td = new TransformDefinition { Name = "EmployeeMapping", Description = "Just a description" };
            td.MapCollection.Add(new DirectMap
            {
                Source = "EmplyeeId",
                Type = typeof(int),
                Destination = "EMPLOYEE_ID"
            });

            var sc = new StringConcateFunctoid();
            sc.ArgumentCollection.Add(new DocumentField { Path = "FirstName" });
            sc.ArgumentCollection.Add(new ConstantField { Value = " ", Type = typeof(string) });
            sc.ArgumentCollection.Add(new DocumentField { Path = "LastName" });

            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = sc,
                Destination = "FIRST_NAME"
            });
            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = new ScriptFunctoid
                {
                    Expression = "return item.Gender == \"Male\"? \"Lelaki\" : \"Perempuan\";"
                },
                Destination = "EMAIL"
            });
            var options = new CompilerOptions();
            var codes = td.GenerateCode();
            var sources = td.SaveSources(codes);
            var result = await td.CompileAsync(options, sources);
            if(!result.Result)
                result.Errors.ForEach(Console.WriteLine);
            Assert.IsTrue(result.Result, "Compiler fails");
            var dll = Assembly.LoadFile(result.Output);
            var mt = dll.GetType("Dev.Integrations.Transforms.EmployeeMapping");
            dynamic map = Activator.CreateInstance(mt);


            var output = await map.TransformAsync(emp);
            Assert.IsNotNull(output);
            Assert.AreEqual(output.FIRST_NAME, emp.FirstName);



        }
    }
}
