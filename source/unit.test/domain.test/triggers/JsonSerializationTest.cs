using System;
using System.IO;
using Bespoke.SphCommercialSpaces.Domain;
using NUnit.Framework;

namespace domain.test.triggers
{
    [TestFixture]
    class JsonSerializationTest
    {
        [Test]
        public void Parse()
        {

            var json = File.ReadAllText("../../triggers/trigger.json");
            Console.WriteLine(typeof(DocumentField).AssemblyQualifiedName);

            var trigger = Trigger.ParseJson(json);
            Assert.AreEqual("test 0002", trigger.Name);

            Assert.IsInstanceOf<DocumentField>(trigger.RuleCollection[0].Left);

            var cf = (ConstantField)trigger.RuleCollection[1].Right;
            Assert.IsInstanceOf<int>(cf.Value);

            var cf2 = (ConstantField)trigger.RuleCollection[2].Right;
            Assert.IsInstanceOf<DateTime>(cf2.Value);

            var cf3 = (ConstantField)trigger.RuleCollection[3].Right;
            Assert.IsInstanceOf<Decimal>(cf3.Value);


        }
    }
}
