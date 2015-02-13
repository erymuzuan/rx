using System;
using Bespoke.Sph.Domain;
using NUnit.Framework;
using System.Threading.Tasks;


namespace domain.test.triggers
{
    [TestFixture]
    public class AssemblyFieldTest
    {
        public const string ASSEMBLY = "assembly.test";
        [TestFixtureSetUp]
        public void Setup()
        {
            System.IO.File.Copy(@"c:\project\work\sph\source\unit.test\assembly.test\bin\Debug\assembly.test.dll",
                ConfigurationManager.WebPath + @"\bin\assembly.test.dll", true);
        }

        [Test]
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
            Assert.AreEqual("Hello erymuzuan", name);

        }
        [Test]
        public async Task GetValueStringWith2MethodArgs()
        {
            var customer = await this.GetCustomerInstanceAsync();
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
            Assert.AreEqual("Good morning erymuzuan", name);

        }

        [Test]
        public async Task GetValueStringWithEntityMethodArg()
        {
            var customer = await this.GetCustomerInstanceAsync();
            customer.FullName = "Erymuzuan";
            var context = new RuleContext(customer);

            var af = new AssemblyField
            {
                Method = "SayCustomerName",
                Location = ASSEMBLY,
                TypeName = @"assembly.test.AssemblyClassToTest"
            };
            var field = new FunctionField { Script = "return item;" };
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "Welcome", Type = typeof(string) }, Name = "greet" });
            af.MethodArgCollection.Add(new MethodArg { ValueProvider = field, Name = "customer" });

            var name = af.GetValue(context);
            Assert.AreEqual("Welcome Erymuzuan", name);

        }


        [Test]
        public async Task GetAsyncValueString()
        {
            var customer = await this.GetCustomerInstanceAsync();
            customer.FullName = "Erymuzuan";
            var context = new RuleContext(customer);

            var field = new FunctionField { Script = "return item;"};

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
            Assert.AreEqual("Welcome Erymuzuan", name);

        }
        [Test]
        public async Task TaskStringAsyncValueString()
        {
            var customer = await this.GetCustomerInstanceAsync();
            customer.FullName = "Erymuzuan";
            var context = new RuleContext(customer);

            var field = new FunctionField { Script = "return item;"};

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
            Assert.AreEqual("Welcome warning Erymuzuan", name);

        }

        [Test]
        public async Task GetNullableDateTime()
        {
            var customer = await this.GetCustomerInstanceAsync();
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
            Assert.AreEqual(new DateTime(2015, 1, 1), date);

        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetAsyncValueOverloaded()
        {
            var customer = await this.GetCustomerInstanceAsync();
            customer.FullName = "Erymuzuan";
            var context = new RuleContext(customer);

            var field = new FunctionField { Script = "return item;"};

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

            var name = af.GetValue(context);
            Assert.AreEqual("Welcome to  Masjid kampung Bukit Bunga", name);

        }

    }
}
