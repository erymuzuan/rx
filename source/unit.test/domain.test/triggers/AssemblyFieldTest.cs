﻿using System;
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
            var building = new Designation { Name = "A" };
            var context = new RuleContext(building);

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
            var building = new Designation { Name = "Masjid kampung Bukit Bunga" };
            var context = new RuleContext(building);

            var af = new AssemblyField
            {
                Method = "SayDesignationName",
                Location = @"c:\project\work\sph\source\unit.test\assembly.test\bin\Debug\assembly.test.dll",
                TypeName = @"assembly.test.AssemblyClassToTest"
            };
            var masjidField = new FunctionField { Script = "return item;", ScriptEngine = new RoslynScriptEngine() };
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "Welcome to", Type = typeof(string) }, Name = "greet" });
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(Designation), ValueProvider = masjidField, Name = "masjid" });

            var name = af.GetValue(context);
            Assert.AreEqual("Welcome to Masjid kampung Bukit Bunga", name);

        }


        [Test]
        public void GetAsyncValueString()
        {
            var building = new Designation { Name = "Masjid kampung Bukit Bunga" };
            var context = new RuleContext(building);

            var masjidField = new FunctionField { Script = "return item;", ScriptEngine = new RoslynScriptEngine() };

            var af = new AssemblyField
            {
                Method = "GreetAsync",
                Location = @"c:\project\work\sph\source\unit.test\assembly.test\bin\Debug\assembly.test.dll",
                TypeName = @"assembly.test.AssemblyClassToTest",
                IsAsync = true,
                AsyncTimeout = 650
            };
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(string), ValueProvider = new ConstantField { Value = "Welcome to ", Type = typeof(string) }, Name = "greet" });
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(Designation), ValueProvider = masjidField, Name = "masjid" });

            var name = af.GetValue(context);
            Assert.AreEqual("Welcome to  Masjid kampung Bukit Bunga", name);

        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetAsyncValueOverloaded()
        {
            var building = new Designation { Name = "Masjid kampung Bukit Bunga" };
            var context = new RuleContext(building);

            var masjidField = new FunctionField { Script = "return item;", ScriptEngine = new RoslynScriptEngine() };

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
            af.MethodArgCollection.Add(new MethodArg { Type = typeof(Designation), ValueProvider = masjidField, Name = "masjid" });

            var name = af.GetValue(context);
            Assert.AreEqual("Welcome to  Masjid kampung Bukit Bunga", name);

        }

    }
}
