using System;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RoslynScriptEngines;
using NUnit.Framework;

namespace domain.test.triggers
{
    [TestFixture]
    public class AssemblyFieldTest
    {
        [Test]
        public void GetValueStringWithOneMethodArg()
        {
            var building = new Designation { Name = "A" };
            var context = new RuleContext(building);

            var af = new AssemblyField
            {
                Method = "SayHello",
                Location = @"c:\project\work\sph\source\unit.test\assembly.test\bin\Debug\assembly.test.dll",
                TypeName = @"assembly.test.AssemblyClassToTest"
            };
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "erymuzuan", Type = typeof(string) }, Name = "name" });

            var name = af.GetValue(context);
            Assert.AreEqual("Hello erymuzuan", name);

        }
        [Test]
        public void GetValueStringWith2MethodArgs()
        {
            var customer = this.GetCustomerInstance();
            var context = new RuleContext(customer);

            var af = new AssemblyField
            {
                Method = "Greet",
                Location = @"c:\project\work\sph\source\unit.test\assembly.test\bin\Debug\assembly.test.dll",
                TypeName = @"assembly.test.AssemblyClassToTest"
            };
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "erymuzuan", Type = typeof(string) }, Name = "name" });
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "Good morning", Type = typeof(string) }, Name = "greet" });

            var name = af.GetValue(context);
            Assert.AreEqual("Good morning erymuzuan", name);

        }

        [Test]
        public void GetValueStringWithEntityMethodArg()
        {
            var customer = this.GetCustomerInstance();
            customer.FullName = "Erymuzuan";
            var context = new RuleContext(customer);

            var af = new AssemblyField
            {
                Method = "SayCustomerName",
                Location = @"c:\project\work\sph\source\unit.test\assembly.test\bin\Debug\assembly.test.dll",
                TypeName = @"assembly.test.AssemblyClassToTest"
            };
            var field = new FunctionField { Script = "return item;", ScriptEngine = new RoslynScriptEngine() };
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "Welcome", Type = typeof(string) }, Name = "greet" });
            af.MethodArgCollection.Add(new MethodArg {  ValueProvider = field, Name = "customer" });

            var name = af.GetValue(context);
            Assert.AreEqual("Welcome Erymuzuan", name);

        }


        [Test]
        public void GetAsyncValueString()
        {
            var customer = this.GetCustomerInstance();
            customer.FullName = "Erymuzuan";
            var context = new RuleContext(customer);

            var field = new FunctionField { Script = "return item;", ScriptEngine = new RoslynScriptEngine() };

            var af = new AssemblyField
            {
                Method = "GreetAsync",
                Location = @"c:\project\work\sph\source\unit.test\assembly.test\bin\Debug\assembly.test.dll",
                TypeName = @"assembly.test.AssemblyClassToTest",
                IsAsync = true,
                AsyncTimeout = 650
            };
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "Welcome", Type = typeof(string) }, Name = "greet" });
            af.MethodArgCollection.Add(new MethodArg { Type = customer.GetType(), ValueProvider = field, Name = "customer" });

            var name = af.GetValue(context);
            Assert.AreEqual("Welcome Erymuzuan", name);

        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetAsyncValueOverloaded()
        {
            var customer = this.GetCustomerInstance();
            customer.FullName = "Erymuzuan";
            var context = new RuleContext(customer);

            var field = new FunctionField { Script = "return item;", ScriptEngine = new RoslynScriptEngine() };

            var af = new AssemblyField
            {
                Method = "GreetAsync",
                Location = @"c:\project\work\sph\source\unit.test\assembly.test\bin\Debug\assembly.test.dll",
                TypeName = @"assembly.test.AssemblyClassToTest",
                IsAsync = true,
                AsyncTimeout = 650
            };
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "Welcome to ", Type = typeof(string) }, Name = "greet" });
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "Wwhat... ", Type = typeof(string) }, Name = "warning" });
            af.MethodArgCollection.Add(new MethodArg { Type = customer.GetType(), ValueProvider = field, Name = "customer" });

            var name = af.GetValue(context);
            Assert.AreEqual("Welcome to  Masjid kampung Bukit Bunga", name);

        }

    }
}
