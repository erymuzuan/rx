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
            td.FunctoidCollection.Add(new ConstantFunctoid
            {
                Value = "2011-05-05",
                Type = typeof(string),
                WebId = "2011-05-05"
            });
            td.FunctoidCollection.Add(new ConstantFunctoid
            {
                Value = "yyyy-MM-dd",
                Type = typeof(string),
                WebId = "yyyy-MM-dd"
            });

            var df = new DateFunctoid { WebId = "df" };
            df.Initialize();
            df["format"].Functoid = "yyyy-MM-dd";
            df["value"].Functoid = "2011-05-05";

            td.FunctoidCollection.Add(df);
            td.MapCollection.Add(new DirectMap
            {
                Source = "CustomerId",
                Type = typeof(int),
                Destination = "EMPLOYEE_ID"
            });


            var genderFnctd = new ScriptFunctoid
            {
                Expression = "return item.Gender == \"Male\"? \"Lelaki\" : \"Perempuan\";",
                Name = "genderFunct",
                WebId = "genderFunct"

            };
            td.FunctoidCollection.Add(genderFnctd);

            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = genderFnctd.WebId,
                Destination = "EMAIL",
                DestinationType = typeof(string)
            });
            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = df.WebId,
                Destination = "HIRE_DATE",
                DestinationType = typeof(DateTime)
            });
            const string FORMATTING_FUNCTOID = "formattingFunctoid";
            td.FunctoidCollection.Add(new FormattingFunctoid
            {
                SourceField = "RegisteredDate",
                Format = "Phone from date {0:yyyy-MM-dd}",
                WebId = FORMATTING_FUNCTOID

            });
            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = FORMATTING_FUNCTOID,
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
            Assert.AreEqual(new DateTime(2011, 5, 5), output.HIRE_DATE);
            StringAssert.Contains(output.PHONE_NUMBER, "Phone from date", output.PHONE_NUMBER);
            Console.WriteLine(td.ToJsonString(true));
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
            var add15Days = new AddDaysFunctoid { Name = "add15Days", WebId = "add15Days" };
            add15Days.Initialize();
            var hireDate = new SourceFunctoid { Field = "HIRE_DATE", WebId = "hireDate" };
            hireDate.Initialize();
            var value15 = new ConstantFunctoid { Value = 15, Type = typeof(int), WebId = "value15" };
            value15.Initialize();
            add15Days["date"].Functoid = hireDate.WebId;
            add15Days["value"].Functoid = value15.WebId;
            td.MapCollection.Add(new FunctoidMap { Functoid = add15Days.WebId, DestinationType = typeof(string), Destination = "RegisteredDate" });


            var sc0 = new StringConcateFunctoid { WebId = "sc0" };
            var space = new ConstantFunctoid { Value = " ", Type = typeof(string), WebId = "space" };
            var bin = new ConstantFunctoid { Value = "bin", Type = typeof(string), WebId = "bin" };
            sc0.ArgumentCollection.Add(new FunctoidArg { Name = "space", Functoid = space.WebId });
            sc0.ArgumentCollection.Add(new FunctoidArg { Name = "bin", Functoid = bin.WebId });
            sc0.ArgumentCollection.Add(new FunctoidArg { Name = "space", Functoid = space.WebId });

            var sc = new StringConcateFunctoid { WebId = "sc" };
            var firstName = new SourceFunctoid { Field = "FIRST_NAME", WebId = "FIRST_NAME" };
            var lastName = new SourceFunctoid { Field = "LAST_NAME", WebId = "LAST_NAME" };

            sc.ArgumentCollection.Add(new FunctoidArg { Name = "firstName", Functoid = firstName.WebId });
            sc.ArgumentCollection.Add(new FunctoidArg { Name = "space", Functoid = sc0.WebId });
            sc.ArgumentCollection.Add(new FunctoidArg { Name = "lastName", Functoid = lastName.WebId });

            td.AddFunctoids(add15Days, hireDate, value15, sc0, space, bin, sc, firstName, lastName);

            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = sc.WebId,
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
            Assert.AreEqual("Erymuzuan bin Mustapa", output.FullName);

        }
        [TestMethod]
        public async Task ScriptFunctoidMapping()
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
                Name = "ScriptFunctoidMapping",
                Description = "Just a description",
                InputType = oracleEmployeeType,
                OutputType = customerType
            };

            var sc0 = new ScriptFunctoid
            {
                WebId = "sc0",
                Name = "sc0",
                OutputTypeName = typeof(string).GetShortAssemblyQualifiedName(),
                Expression = "Console.WriteLine(\"FirstName : {0}\",item.FIRST_NAME);\r\nreturn item.FIRST_NAME + \" bin \" + item.LAST_NAME;"
            };
            td.AddFunctoids(sc0);

            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = sc0.WebId,
                DestinationType = typeof(string),
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
            var mt = dll.GetType("Dev.Integrations.Transforms." + td.Name);
            dynamic map = Activator.CreateInstance(mt);


            var output = await map.TransformAsync(staff);
            Assert.IsNotNull(output);
            Assert.AreEqual("Erymuzuan bin Mustapa", output.FullName);

        }     [TestMethod]
        public async Task ChildItemMapping()
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
                Name = "ChildItemMapping",
                Description = "Just a description",
                InputType = oracleEmployeeType,
                OutputType = customerType
            };



            td.MapCollection.Add(new DirectMap
            {
                Source = "EMAIL",
                DestinationType = typeof(string),
                Destination = "Contact.Email"
            });

            var options = new CompilerOptions();
            var codes = td.GenerateCode();
            var sources = td.SaveSources(codes);
            var result = await td.CompileAsync(options, sources);
            if (!result.Result)
                result.Errors.ForEach(Console.WriteLine);

            Assert.IsTrue(result.Result, "Compiler fails");
            var dll = Assembly.LoadFile(result.Output);
            var mt = dll.GetType("Dev.Integrations.Transforms." + td.Name);
            dynamic map = Activator.CreateInstance(mt);


            var output = await map.TransformAsync(staff);
            Assert.IsNotNull(output);
            Assert.AreEqual("erymuzuan@hotmail.com", output.Contact.Email);

        }
    }
}
