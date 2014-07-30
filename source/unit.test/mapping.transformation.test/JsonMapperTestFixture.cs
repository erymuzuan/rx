using System;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.RoslynScriptEngines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Bespoke.Sph.Domain;

namespace mapping.transformation.test
{
    [TestClass]
    public class JsonMapperTestFixture
    {
        [TestInitialize]
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


        [TestMethod]
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

        [TestMethod]
        public async Task GetEntityMap()
        {
            var customerType = Assembly.LoadFrom(@".\Dev.Customer.dll").GetType("Bespoke.Dev_1.Domain.Customer");
            dynamic customer = Activator.CreateInstance(customerType);
            customer.FullName = "Erymuzuan Mustapa";
            customer.PrimaryContact = "0123889200";
            customer.RegisteredDate = new DateTime(2000, 1, 1);
            customer.Age = 45;
            customer.CustomerId = 50;

            var oracleEmployeeType = Assembly.LoadFrom(@".\Dev.HR_EMPLOYEES.dll").GetType("Dev.Adapters.HR.EMPLOYEES");


            var td = new TransformDefinition
            {
                Name = "EmployeeMapping",
                Description = "Just a description",
                InputType = customerType,
                OutputType = oracleEmployeeType
            };
            td.MapCollection.Add(new DirectMap
            {
                Source = "CustomerId",
                Type = typeof(int),
                Destination = "EMPLOYEE_ID"
            });

            var sc = new StringConcateFunctoid();
            sc.ArgumentCollection.Add(new DocumentField { Path = "FullName" });
            sc.ArgumentCollection.Add(new ConstantField { Value = " ", Type = typeof(string) });
            sc.ArgumentCollection.Add(new DocumentField { Path = "Age" });

            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = sc,
                Destination = "FIRST_NAME"
            });
            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = new ScriptFunctoid
                {
                    Expression = "return item.Gender == \"Male\"? \"Lelaki\" : \"Perempuan\";",
                    Name = "genderFunct"
                    
                },
                Destination = "EMAIL",
                DestinationType = typeof(string)
            });
            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = new ScriptFunctoid
                {
                    Expression = "DateTime.Today",
                    Name = "hireDateFunc"
                    
                },
                Destination = "HIRE_DATE",
                DestinationType = typeof(DateTime)
            });
            var options = new CompilerOptions();
            var codes = td.GenerateCode();
            var sources = td.SaveSources(codes);
            var result = await td.CompileAsync(options, sources);
            if (!result.Result)
                result.Errors.ForEach(Console.WriteLine);
            Assert.IsTrue(result.Result, "Compiler fails");
            var dll = Assembly.LoadFile(result.Output);
            var mt = dll.GetType("Dev.Integrations.Transforms.EmployeeMapping");
            dynamic map = Activator.CreateInstance(mt);


            var output = await map.TransformAsync(customer);
            Assert.IsNotNull(output);
            Assert.AreEqual(customer.CustomerId, output.EMPLOYEE_ID);
            Assert.AreEqual(DateTime.Today, output.HIRE_DATE);



        }
    }
}
