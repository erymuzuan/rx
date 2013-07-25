using System;
using System.IO;
using Bespoke.SphCommercialSpaces.Domain;
using NUnit.Framework;
using Newtonsoft.Json;

namespace domain.test.triggers
{
    [TestFixture]
    class JsonSerializationTest
    {
        [Test]
        public void ParseWithAction()
        {
            var email = new EmailAction();
            var setter = new SetterAction();
            var t = new Trigger{Name = "test action"};
            t.ActionCollection.Add(email.ToString());
            t.ActionCollection.Add(setter.ToString());
            var json = JsonConvert.SerializeObject(t);
            Console.WriteLine(json);
            
        }
        [Test]
        public void Parse()
        {

            var json = File.ReadAllText("../../triggers/trigger.json");

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
