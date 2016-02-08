using System;
using System.IO;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RoslynScriptEngines;
using Xunit;

namespace domain.test.triggers
{
    public class AssemblyFieldTest
    {
        public const string ASSEMBLY = "assembly.test";
        public AssemblyFieldTest()
        {      var source = $@"{ConfigurationManager.Home}\..\source\unit.test\assembly.test\bin\Debug\assembly.test.dll";
           
            var destFileName = $"{ConfigurationManager.WebPath}\\bin\\assembly.test.dll";
            if (!File.Exists(destFileName))
            {
               File.Copy(source,destFileName, false);
            }
        }

        [Fact]
        public void GetValueStringWithOneMethodArg()
        {
            var building = new Designation { Name = "A" };
            var context = new RuleContext(building);

            var af = new AssemblyField
            {
                Method = "SayHello",
                Location = ASSEMBLY,
                TypeName = @"assembly.test.AssemblyClassToTest"
            };
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "erymuzuan", Type = typeof(string) }, Name = "name" });

            var name = af.GetValue(context);
            Assert.Equal("Hello erymuzuan", name);

        }
        [Fact]
        public void GetValueStringWith2MethodArgs()
        {
            var customer = this.GetCustomerInstance();
            var context = new RuleContext(customer);

            var af = new AssemblyField
            {
                Method = "Greet",
                Location = ASSEMBLY,
                TypeName = @"assembly.test.AssemblyClassToTest"
            };
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "erymuzuan", Type = typeof(string) }, Name = "name" });
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "Good morning", Type = typeof(string) }, Name = "greet" });

            var name = af.GetValue(context);
            Assert.Equal("Good morning erymuzuan", name);

        }

        [Fact]
        public void GetValueStringWithEntityMethodArg()
        {
            var customer = this.GetCustomerInstance();
            customer.FullName = "Erymuzuan";
            var context = new RuleContext(customer);

            var af = new AssemblyField
            {
                Method = "SayCustomerName",
                Location = ASSEMBLY,
                TypeName = @"assembly.test.AssemblyClassToTest"
            };
            var field = new FunctionField { Script = "return item;", ScriptEngine = new RoslynScriptEngine() };
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "Welcome", Type = typeof(string) }, Name = "greet" });
            af.MethodArgCollection.Add(new MethodArg { ValueProvider = field, Name = "customer" });

            var name = af.GetValue(context);
            Assert.Equal("Welcome Erymuzuan", name);

        }


        [Fact]
        public void GetAsyncValueString()
        {
            var customer = this.GetCustomerInstance();
            customer.FullName = "Erymuzuan";
            var context = new RuleContext(customer);

            var field = new FunctionField { Script = "return item;", ScriptEngine = new RoslynScriptEngine() };

            var af = new AssemblyField
            {
                Method = "GreetAsync",
                Location = ASSEMBLY,
                TypeName = @"assembly.test.AssemblyClassToTest",
                IsAsync = true,
                AsyncTimeout = 650
            };
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "Welcome", Type = typeof(string) }, Name = "greet" });
            af.MethodArgCollection.Add(new MethodArg { Type = customer.GetType(), ValueProvider = field, Name = "customer" });

            var name = af.GetValue(context);
            Assert.Equal("Welcome Erymuzuan", name);

        }
        [Fact]
        public void TaskStringAsyncValueString()
        {
            var customer = this.GetCustomerInstance();
            customer.FullName = "Erymuzuan";
            var context = new RuleContext(customer);

            var field = new FunctionField { Script = "return item;", ScriptEngine = new RoslynScriptEngine() };

            var af = new AssemblyField
            {
                Method = "SayHelloAsync",
                Location = ASSEMBLY,
                TypeName = @"assembly.test.AssemblyClassToTest",
                IsAsync = true,
                AsyncTimeout = 650
            };
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(bool), ValueProvider = new ConstantField { Value = "false", Type = typeof(bool) }, Name = "warning" });
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "Welcome", Type = typeof(string) }, Name = "greet" });
            af.MethodArgCollection.Add(new MethodArg { Type = customer.GetType(), ValueProvider = field, Name = "customer" });

            var name = af.GetValue(context);
            Assert.Equal("Welcome warning Erymuzuan", name);

        }

        [Fact]
        public void GetNullableDateTime()
        {
            var customer = this.GetCustomerInstance();
            customer.FullName = "Erymuzuan";
            var context = new RuleContext(customer);


            var af = new AssemblyField
            {
                Method = "GetNullableDateTime",
                Location = ASSEMBLY,
                TypeName = @"assembly.test.AssemblyClassToTest",
                IsAsync = true,
                AsyncTimeout = 650
            };
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(DateTime), ValueProvider = new ConstantField { Value = "2015-01-01", Type = typeof(DateTime) }, Name = "date" });

            var date = af.GetValue(context);
            Assert.Equal(new DateTime(2015, 1, 1), date);

        }

        [Fact]
        public void GetAsyncValueOverloaded()
        {
            var customer = this.GetCustomerInstance();
            customer.FullName = "Erymuzuan";
            var context = new RuleContext(customer);

            var field = new FunctionField { Script = "return item;", ScriptEngine = new RoslynScriptEngine() };

            var af = new AssemblyField
            {
                Method = "GreetAsync",
                Location = ASSEMBLY,
                TypeName = @"assembly.test.AssemblyClassToTest",
                IsAsync = true,
                AsyncTimeout = 650
            };
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "Welcome to ", Type = typeof(string) }, Name = "greet" });
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "Wwhat... ", Type = typeof(string) }, Name = "warning" });
            af.MethodArgCollection.Add(new MethodArg { Type = customer.GetType(), ValueProvider = field, Name = "customer" });

            var ioe = Assert.Throws<InvalidOperationException>(() =>
            {
                var name = af.GetValue(context);
                Assert.Equal("Welcome to  Masjid kampung Bukit Bunga", name);
            });
            Assert.Equal("This is not yet possible to have overloaded method with same parameters length", ioe.Message);

        }

    }
}
