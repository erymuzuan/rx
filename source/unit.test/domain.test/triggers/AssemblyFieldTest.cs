using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace domain.test.triggers
{
    [TestFixture]
    public class AssemblyFieldTest
    {
        [Test]
        public void GetValueStringWithOneParameter()
        {
            var building = new Building { Name = "A" };
            var context = new RuleContext(building);

            var af = new AssemblyField
            {
                Method = "SayHello",
                Location = @"c:\project\work\sph\source\unit.test\assembly.test\bin\Debug\assembly.test.dll",
                TypeName = @"assembly.test.AssemblyClassToTest"
            };
            af.ParameterCollection.Add(new Parameter { Type = typeof(string), Value = "erymuzuan", Name = "name"});

            var name = af.GetValue(context);
            Assert.AreEqual("Hello erymuzuan", name);

        }
        [Test]
        public void GetValueStringWith2Parameters()
        {
            var building = new Building { Name = "A" };
            var context = new RuleContext(building);

            var af = new AssemblyField
            {
                Method = "Greet",
                Location = @"c:\project\work\sph\source\unit.test\assembly.test\bin\Debug\assembly.test.dll",
                TypeName = @"assembly.test.AssemblyClassToTest"
            };
            af.ParameterCollection.Add(new Parameter { Type = typeof(string), Value = "erymuzuan", Name = "name" });
            af.ParameterCollection.Add(new Parameter { Type = typeof(string), Value = "Good morning", Name = "greet" });

            var name = af.GetValue(context);
            Assert.AreEqual("Good morning erymuzuan", name);

        }

        [Test]
        public void GetValueStringWithEntityParameter()
        {
            var building = new Building { Name = "Masjid kampung Bukit Bunga" };
            var context = new RuleContext(building);

            var af = new AssemblyField
            {
                Method = "SayBuildingName",
                Location = @"c:\project\work\sph\source\unit.test\assembly.test\bin\Debug\assembly.test.dll",
                TypeName = @"assembly.test.AssemblyClassToTest"
            };
            af.ParameterCollection.Add(new Parameter { Type = typeof(string), Value = "Welcome to", Name = "greet" });
            af.ParameterCollection.Add(new Parameter { Type = typeof(Building), Value = building, Name = "masjid" });

            var name = af.GetValue(context);
            Assert.AreEqual("Welcome to Masjid kampung Bukit Bunga", name);

        } 
        
        [Test]
        public void GetAsyncValueString()
        {
            var building = new Building { Name = "Masjid kampung Bukit Bunga" };
            var context = new RuleContext(building);

            var af = new AssemblyField
            {
                Method = "GreetAsync",
                Location = @"c:\project\work\sph\source\unit.test\assembly.test\bin\Debug\assembly.test.dll",
                TypeName = @"assembly.test.AssemblyClassToTest",
                IsAsync = true,
                AsyncTimeout = 650
            };
            af.ParameterCollection.Add(new Parameter { Type = typeof(string), Value = "Welcome to", Name = "greet" });
            af.ParameterCollection.Add(new Parameter { Type = typeof(Building), Value = building, Name = "masjid" });

            var name = af.GetValue(context);
            Assert.AreEqual("Welcome to Masjid kampung Bukit Bunga", name);

        }

    }
}
