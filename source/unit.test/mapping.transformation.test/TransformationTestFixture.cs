using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.RoslynScriptEngines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Bespoke.Sph.Domain;

namespace mapping.transformation.test
{
    [TestClass]
    public class TransformationTestFixture
    {
        [TestInitialize]
        public void Setup()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            Console.WriteLine("Copying files to");
            File.Copy(Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, "Dev.Customer.dll"), @".\Dev.Customer.dll", true);
            File.Copy(Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, "Dev.HR_EMPLOYEES.dll"), @".\Dev.HR_EMPLOYEES.dll", true);
        }



        [TestMethod]
        public void ReadMappingFile()
        {
            var customerType = Assembly.LoadFrom(@".\Dev.Customer.dll").GetType("Bespoke.Dev_1.Domain.Customer");
            var oracleEmployeeType = Assembly.LoadFrom(@".\Dev.HR_EMPLOYEES.dll").GetType("Dev.Adapters.HR.EMPLOYEES");
            var map = new TransformDefinition
            {
                Name = "Test Survey Mapping",
                Description = "Just a description",
                InputType = customerType,
                OutputType = oracleEmployeeType
            };
            map.MapCollection.Add(new DirectMap
            {
                Source = "SurveId",
                Type = typeof(int),
                Destination = "SURVEY_ID"
            });

            Console.WriteLine(map.ToJsonString(Formatting.Indented));
        }

        [TestMethod]
        public async Task EntityCustomerToOracleEmployee()
        {
            var customerType = Assembly.LoadFrom(@".\Dev.Customer.dll").GetType("Bespoke.Dev_1.Domain.Customer");
            dynamic customer = Activator.CreateInstance(customerType);
            customer.FullName = "Erymuzuan Mustapa";
            customer.PrimaryContact = "0123889200";
            customer.LogoStoreId = "2012-05-31";
            customer.RegisteredDate = DateTime.Today;
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
                Functoid = new DateFunctoid
                {
                    SourceField = "LogoStoreId",
                    Format = "yyyy-MM-dd"

                },
                Destination = "HIRE_DATE",
                DestinationType = typeof(DateTime)
            });
            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = new FormattingFunctoid
                {
                    SourceField = "RegisteredDate",
                    Format = "Phone from date {0:yyyy-MM-dd}"

                },
                Destination = "PHONE_NUMBER",
                DestinationType = typeof(string)
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
            Assert.AreEqual(new DateTime(2012, 5, 31), output.HIRE_DATE);
            StringAssert.Contains(output.PHONE_NUMBER, "Phone from date", output.PHONE_NUMBER);
        }


        [TestMethod]
        public async Task OracleEmployeeToEntityCustomer()
        {
            var customerType = Assembly.LoadFrom(@".\Dev.Customer.dll").GetType("Bespoke.Dev_1.Domain.Customer");


            var oracleEmployeeType = Assembly.LoadFrom(@".\Dev.HR_EMPLOYEES.dll").GetType("Dev.Adapters.HR.EMPLOYEES");
            dynamic staff = Activator.CreateInstance(oracleEmployeeType);
            staff.EMAIL = "erymuzuan@hotmail.com";
            staff.FIRST_NAME = "Erymuzuan";
            staff.LAST_NAME = "Mustapa";
            staff.PHONE_NUMBER = "0123889200";
            staff.HIRE_DATE = DateTime.Parse("2012-05-31");
            staff.SALARY = 4500d;


            var td = new TransformDefinition
            {
                Name = "EmployeeToCustomerMapping",
                Description = "Just a description",
                InputType = oracleEmployeeType,
                OutputType = customerType
            };
            td.MapCollection.Add(new DirectMap
            {
                Source = "EMPLOYEE_ID",
                Type = typeof(int),
                Destination = "CustomerId"
            });
            td.MapCollection.Add(new DirectMap
            {
                Source = "EMAIL",
                Type = typeof(string),
                Destination = "Contact.Email"
            });
            var add15Days = new AddDaysFunctoid { Name = "add15Days" };
            add15Days["date"].Functoid = new SourceFunctoid { Field = "HIRE_DATE"};
            add15Days["value"].Functoid = new ConstantFunctoid{Value = 15, Type = typeof(int)};
            td.MapCollection.Add(new FunctoidMap{Functoid = add15Days, DestinationType = typeof(string), Destination = "RegisteredDate"});
         

            var sc = new StringConcateFunctoid();
            sc.ArgumentCollection.Add(new FunctoidArg { Name = "firstName", Functoid = new SourceFunctoid { Field = "FIRST_NAME" } });
            sc.ArgumentCollection.Add(new FunctoidArg { Name = "space", Functoid = new ConstantFunctoid { Value = " ", Type = typeof(string) } });
            sc.ArgumentCollection.Add(new FunctoidArg { Name = "lastName", Functoid = new SourceFunctoid { Field = "LAST_NAME" } });

            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = sc,
                Destination = "FullName"
            });

            var options = new CompilerOptions();
            var codes = td.GenerateCode();
            var sources = td.SaveSources(codes);
            var result = await td.CompileAsync(options, sources);
            if (!result.Result)
                result.Errors.ForEach(Console.WriteLine);

            Assert.IsTrue(result.Result, "Compiler fails");
            var dll = Assembly.LoadFile(result.Output);
            var mt = dll.GetType("Dev.Integrations.Transforms.EmployeeToCustomerMapping");
            dynamic map = Activator.CreateInstance(mt);


            var output = await map.TransformAsync(staff);
            Assert.IsNotNull(output);
            Assert.AreEqual(staff.EMPLOYEE_ID, output.CustomerId);
            Assert.AreEqual("erymuzuan@hotmail.com", output.Contact.Email);
            Assert.AreEqual(new DateTime(2012, 6, 15), output.RegisteredDate);
            Assert.AreEqual("Erymuzuan Mustapa", output.FullName);

        }
    }
}
