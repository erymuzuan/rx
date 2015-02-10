﻿using System;
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
                Name = "__Test Survey Mapping",
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

        private dynamic CreateDynamicCustomer()
        {
            var customerType = Assembly.LoadFrom(@".\Dev.Customer.dll").GetType("Bespoke.Dev_1.Domain.Customer");
            dynamic customer = Activator.CreateInstance(customerType);
            return customer;
        }
        private Type GetCustomerType()
        {
            return Assembly.LoadFrom(@".\Dev.Customer.dll").GetType("Bespoke.Dev_1.Domain.Customer");
        }

        [TestMethod]
        public async Task EntityCustomerToOracleEmployee()
        {
            var customer = this.CreateDynamicCustomer();
            customer.FullName = "Erymuzuan Mustapa";
            customer.PrimaryContact = "0123889200";
            customer.LogoStoreId = "2012-05-31";
            customer.RegisteredDate = DateTime.Today;
            customer.Age = 45;
            customer.CustomerId = 50;

            var oracleEmployeeType = Assembly.LoadFrom(@".\Dev.HR_EMPLOYEES.dll").GetType("Dev.Adapters.HR.EMPLOYEES");


            var td = new TransformDefinition
            {
                Name = "__EmployeeMapping",
                Description = "Just a description",
                InputType = this.GetCustomerType(),
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

            var df = new ParseDateTimeFunctoid { WebId = "df" };
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
                WebId = "genderFunct",
                OutputType = typeof(string)

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
            var ff = new FormattingFunctoid
            {
                Format = "Phone from date {0:yyyy-MM-dd}",
                WebId = FORMATTING_FUNCTOID
            };
            ff.Initialize();
            var sfff = new SourceFunctoid { Field = "CreatedDate", WebId = Guid.NewGuid().ToString() };
            ff["value"].Functoid = sfff.WebId;
            td.FunctoidCollection.Add(sfff);
            td.FunctoidCollection.Add(ff);
            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = FORMATTING_FUNCTOID,
                Destination = "PHONE_NUMBER",
                DestinationType = typeof(string)
            });

            var options = new CompilerOptions();
            var result = await td.CompileAsync(options);
            if (!result.Result)
                result.Errors.ForEach(Console.WriteLine);
            Assert.IsTrue(result.Result, "Compiler fails");
            var dll = Assembly.LoadFile(result.Output);
            var mt = dll.GetType("Dev.Integrations.Transforms." + td.Name);
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
                Name = "__OracleEmployeeToEntityCustomer",
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
            sc0.ArgumentCollection.Add(new FunctoidArg { Name = "1", Functoid = space.WebId, });
            sc0.ArgumentCollection.Add(new FunctoidArg { Name = "2", Functoid = bin.WebId });
            sc0.ArgumentCollection.Add(new FunctoidArg { Name = "3", Functoid = space.WebId });

            var sc = new StringConcateFunctoid { WebId = "sc" };
            var firstName = new SourceFunctoid { Field = "FIRST_NAME", WebId = "FIRST_NAME" };
            var lastName = new SourceFunctoid { Field = "LAST_NAME", WebId = "LAST_NAME" };

            sc.ArgumentCollection.Add(new FunctoidArg { Name = "1", Functoid = firstName.WebId });
            sc.ArgumentCollection.Add(new FunctoidArg { Name = "2", Functoid = sc0.WebId });
            sc.ArgumentCollection.Add(new FunctoidArg { Name = "3", Functoid = lastName.WebId });

            td.AddFunctoids(add15Days, hireDate, value15, sc0, space, bin, sc, firstName, lastName);

            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = sc.WebId,
                Destination = "FullName"
            });

            var options = new CompilerOptions();
            var result = await td.CompileAsync(options);
            if (!result.Result)
                result.Errors.ForEach(Console.WriteLine);

            Assert.IsTrue(result.Result, "Compiler fails");
            var dll = Assembly.LoadFile(result.Output);
            var mt = dll.GetType("Dev.Integrations.Transforms." + td.Name);
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
                Name = "__ScriptFunctoidMapping",
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
            var result = await td.CompileAsync(options);
            if (!result.Result)
                result.Errors.ForEach(Console.WriteLine);

            Assert.IsTrue(result.Result, "Compiler fails");
            var dll = Assembly.LoadFile(result.Output);
            var mt = dll.GetType("Dev.Integrations.Transforms." + td.Name);
            dynamic map = Activator.CreateInstance(mt);


            var output = await map.TransformAsync(staff);
            Assert.IsNotNull(output);
            Assert.AreEqual("Erymuzuan bin Mustapa", output.FullName);

        }

        [TestMethod]
        public async Task FormatObjectMapping()
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
                Name = "__FormatObjectMapping",
                Description = "Just a description",
                InputType = oracleEmployeeType,
                OutputType = customerType
            };

            var formatRinggit = new FormattingFunctoid
            {
                WebId = "formatRinggit",
                Name = "formatRinggit",
                OutputTypeName = typeof(string).GetShortAssemblyQualifiedName(),
                Format = "RM {0:F2}"
            };
            formatRinggit.Initialize();
            td.AddFunctoids(formatRinggit);

            var salaryFunctoid = new SourceFunctoid
            {
                Name = "SALARY",
                Field = "SALARY",
                WebId = "SALARY"
            };
            formatRinggit["value"].Functoid = salaryFunctoid.WebId;
            td.AddFunctoids(salaryFunctoid);


            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = formatRinggit.WebId,
                DestinationType = typeof(string),
                Destination = "FullName"
            });

            var options = new CompilerOptions();
            var result = await td.CompileAsync(options);
            if (!result.Result)
                result.Errors.ForEach(Console.WriteLine);

            Assert.IsTrue(result.Result, "Compiler fails");
            var dll = Assembly.LoadFile(result.Output);
            var mt = dll.GetType("Dev.Integrations.Transforms." + td.Name);
            dynamic map = Activator.CreateInstance(mt);


            var output = await map.TransformAsync(staff);
            Assert.IsNotNull(output);
            Assert.AreEqual("RM 4500.00", output.FullName);

        }
        [TestMethod]
        public async Task ParseDateMapping()
        {
            var customerType = Assembly.LoadFrom(@".\Dev.Customer.dll").GetType("Bespoke.Dev_1.Domain.Customer");
            var oracleEmployeeType = Assembly.LoadFrom(@".\Dev.HR_EMPLOYEES.dll").GetType("Dev.Adapters.HR.EMPLOYEES");

            dynamic staff = Activator.CreateInstance(oracleEmployeeType);
            staff.EMAIL = "erymuzuan@hotmail.com";
            staff.FIRST_NAME = "Erymuzuan";
            staff.LAST_NAME = "Mustapa";
            staff.PHONE_NUMBER = "2014-05-01";
            staff.HIRE_DATE = DateTime.Parse("2012-05-31");
            staff.SALARY = 4500d;

            var td = new TransformDefinition
            {
                Name = "__ParseDateMapping",
                Description = "Just a description",
                InputType = oracleEmployeeType,
                OutputType = customerType
            };

            var parseDate = new ParseDateTimeFunctoid
            {
                WebId = "parseDate",
                Name = "parseDate",
                OutputTypeName = typeof(string).GetShortAssemblyQualifiedName()
            };
            parseDate.Initialize();
            td.AddFunctoids(parseDate);

            var phoneNumber = new SourceFunctoid
            {
                Name = "PHONE_NUMBER",
                Field = "PHONE_NUMBER",
                WebId = "PHONE_NUMBER"
            };
            td.AddFunctoids(phoneNumber);
            parseDate["value"].Functoid = phoneNumber.WebId;

            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = parseDate.WebId,
                DestinationType = typeof(DateTime),
                Destination = "RegisteredDate"
            });

            var options = new CompilerOptions();
            var result = await td.CompileAsync(options);
            if (!result.Result)
                result.Errors.ForEach(Console.WriteLine);

            Assert.IsTrue(result.Result, "Compiler fails");
            var dll = Assembly.LoadFile(result.Output);
            var mt = dll.GetType("Dev.Integrations.Transforms." + td.Name);
            dynamic map = Activator.CreateInstance(mt);


            var output = await map.TransformAsync(staff);
            Assert.IsNotNull(output);
            Assert.AreEqual(new DateTime(2014, 5, 1), output.RegisteredDate);

        }

        [TestMethod]
        public async Task ParseDecimalMapping()
        {
            var customerType = Assembly.LoadFrom(@".\Dev.Customer.dll").GetType("Bespoke.Dev_1.Domain.Customer");
            var oracleEmployeeType = Assembly.LoadFrom(@".\Dev.HR_EMPLOYEES.dll").GetType("Dev.Adapters.HR.EMPLOYEES");

            dynamic staff = Activator.CreateInstance(oracleEmployeeType);
            staff.EMAIL = "erymuzuan@hotmail.com";
            staff.FIRST_NAME = "Erymuzuan";
            staff.LAST_NAME = "Mustapa";
            staff.PHONE_NUMBER = "4589";
            staff.HIRE_DATE = DateTime.Parse("2012-05-31");
            staff.SALARY = 4500d;

            var td = new TransformDefinition
            {
                Name = "__ParseDecimalMapping",
                Description = "Just a description",
                InputType = oracleEmployeeType,
                OutputType = customerType
            };

            var parseDecimal = new ParseDecimalFunctoid
            {
                WebId = "parseDecimal",
                Name = "parseDecimal",
                OutputTypeName = typeof(string).GetShortAssemblyQualifiedName()
            };
            parseDecimal.Initialize();
            td.AddFunctoids(parseDecimal);

            var phoneNumber = new SourceFunctoid
            {
                Name = "PHONE_NUMBER",
                Field = "PHONE_NUMBER",
                WebId = "PHONE_NUMBER"
            };
            td.AddFunctoids(phoneNumber);
            parseDecimal["value"].Functoid = phoneNumber.WebId;

            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = parseDecimal.WebId,
                DestinationType = typeof(DateTime),
                Destination = "Revenue"
            });

            var options = new CompilerOptions();
            var result = await td.CompileAsync(options);
            if (!result.Result)
                result.Errors.ForEach(Console.WriteLine);

            Assert.IsTrue(result.Result, "Compiler fails");
            var dll = Assembly.LoadFile(result.Output);
            var mt = dll.GetType("Dev.Integrations.Transforms." + td.Name);
            dynamic map = Activator.CreateInstance(mt);


            var output = await map.TransformAsync(staff);
            Assert.IsNotNull(output);
            Assert.AreEqual(4589m, output.Revenue);

        }

        [TestMethod]
        public async Task ParseDoubleMapping()
        {
            var customerType = Assembly.LoadFrom(@".\Dev.Customer.dll").GetType("Bespoke.Dev_1.Domain.Customer");
            var oracleEmployeeType = Assembly.LoadFrom(@".\Dev.HR_EMPLOYEES.dll").GetType("Dev.Adapters.HR.EMPLOYEES");

            dynamic staff = Activator.CreateInstance(oracleEmployeeType);
            staff.EMAIL = "erymuzuan@hotmail.com";
            staff.FIRST_NAME = "Erymuzuan";
            staff.LAST_NAME = "Mustapa";
            staff.PHONE_NUMBER = "4589";
            staff.HIRE_DATE = DateTime.Parse("2012-05-31");
            staff.SALARY = 4500d;

            var td = new TransformDefinition
            {
                Name = "__ParseDoubleMapping",
                Description = "Just a description",
                InputType = oracleEmployeeType,
                OutputType = customerType
            };



            var parseDouble = new ParseDoubleFunctoid
            {
                WebId = "parseDouble",
                Name = "parseDeparseDoublecimal",
                OutputTypeName = typeof(string).GetShortAssemblyQualifiedName()
            };
            parseDouble.Initialize();
            td.AddFunctoids(parseDouble);
            var ff = new FormattingFunctoid
            {
                WebId = "ff",
                Name = "ff"
            };
            ff.Initialize();
            td.AddFunctoids(ff);
            ff["value"].Functoid = parseDouble.WebId;
            ff.Format = "Nama : {0}";

            var phoneNumber = new SourceFunctoid
            {
                Name = "PHONE_NUMBER",
                Field = "PHONE_NUMBER",
                WebId = "PHONE_NUMBER"
            };
            td.AddFunctoids(phoneNumber);
            parseDouble["value"].Functoid = phoneNumber.WebId;

            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = ff.WebId,
                DestinationType = typeof(DateTime),
                Destination = "FullName"
            });

            var options = new CompilerOptions();
            var result = await td.CompileAsync(options);
            if (!result.Result)
                result.Errors.ForEach(Console.WriteLine);

            Assert.IsTrue(result.Result, "Compiler fails");
            var dll = Assembly.LoadFile(result.Output);
            var mt = dll.GetType("Dev.Integrations.Transforms." + td.Name);
            dynamic map = Activator.CreateInstance(mt);


            var output = await map.TransformAsync(staff);
            Assert.IsNotNull(output);
            Assert.AreEqual("Nama : 4589", output.FullName);

        }

        [TestMethod]
        public async Task ParseInt32Mapping()
        {
            var customerType = Assembly.LoadFrom(@".\Dev.Customer.dll").GetType("Bespoke.Dev_1.Domain.Customer");
            var oracleEmployeeType = Assembly.LoadFrom(@".\Dev.HR_EMPLOYEES.dll").GetType("Dev.Adapters.HR.EMPLOYEES");

            dynamic staff = Activator.CreateInstance(oracleEmployeeType);
            staff.EMAIL = "erymuzuan@hotmail.com";
            staff.FIRST_NAME = "Erymuzuan";
            staff.LAST_NAME = "Mustapa";
            staff.PHONE_NUMBER = "4589";
            staff.HIRE_DATE = DateTime.Parse("2012-05-31");
            staff.SALARY = 4500d;

            var td = new TransformDefinition
            {
                Name = "__ParseDecimalMapping",
                Description = "Just a description",
                InputType = oracleEmployeeType,
                OutputType = customerType
            };

            var parseInt32 = new ParseInt32Functoid
            {
                WebId = "parseInt32",
                Name = "parseInt32",
                OutputTypeName = typeof(string).GetShortAssemblyQualifiedName()
            };
            parseInt32.Initialize();
            td.AddFunctoids(parseInt32);

            var phoneNumber = new SourceFunctoid
            {
                Name = "PHONE_NUMBER",
                Field = "PHONE_NUMBER",
                WebId = "PHONE_NUMBER"
            };
            td.AddFunctoids(phoneNumber);
            parseInt32["value"].Functoid = phoneNumber.WebId;

            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = parseInt32.WebId,
                DestinationType = typeof(DateTime),
                Destination = "CustomerId"
            });

            var options = new CompilerOptions();
            var result = await td.CompileAsync(options);
            if (!result.Result)
                result.Errors.ForEach(Console.WriteLine);

            Assert.IsTrue(result.Result, "Compiler fails");
            var dll = Assembly.LoadFile(result.Output);
            var mt = dll.GetType("Dev.Integrations.Transforms." + td.Name);
            dynamic map = Activator.CreateInstance(mt);


            var output = await map.TransformAsync(staff);
            Assert.IsNotNull(output);
            Assert.AreEqual(4589, output.CustomerId);

        }

        [TestMethod]
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
                Name = "__ChildItemMapping",
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
            var result = await td.CompileAsync(options);
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
