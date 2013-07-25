using System;
using System.IO;
using Bespoke.SphCommercialSpaces.Domain;
using NUnit.Framework;
using Newtonsoft.Json;
using roslyn.scriptengine;

namespace domain.test.triggers
{
    [TestFixture]
    class JsonSerializationTest
    {

        [Test]
        public void TrrigerWithAction()
        {
            var json = this.GetTriggerWithActionJson();
            Console.WriteLine(json);
            var trigger = JsonConvert.DeserializeObject<Trigger>(json);
            Assert.AreEqual(2, trigger.ActionCollection.Count);
        }
        [Test]
        public void TrrigerWithRule()
        {
            var json = this.GetTriggerWithRuleJson();
            Console.WriteLine(json);
            var trigger = JsonConvert.DeserializeObject<Trigger>(json);
            Assert.AreEqual(1, trigger.RuleCollection.Count);
        }

        public string GetTriggerWithRuleJson()
        {
            var rule = new Rule
                {
                    Left = new DocumentField
                    {
                        Path = "test",
                        Name = "test"
                    },
                    Operator = Operator.Eq,
                    Right = new ConstantField { Value = 500 }
                };
            var t = new Trigger { Name = "test action" };
            t.RuleCollection.Add(rule);
            return JsonConvert.SerializeObject(t, Formatting.Indented , new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });
        }

        public string GetTriggerWithActionJson()
        {
            var email = new EmailAction { To = "ima@bespoke.com.my", Cc = "izzati@bespoke.com.my", SubjectTemplate = "Test", BodyTemplate = "test" };
            var setter = new SetterAction { Note = "Watever" };
            setter.SetterActionChildCollection.Add(new SetterActionChild
            {
                Path = "Name",
                Field = new FunctionField
                    {
                        Name = "Whate ver",
                        Script = "LotNo",
                        ScriptEngine = new RoslynScriptEngine()
                    }
            });
            var t = new Trigger { Name = "test action" };
            t.ActionCollection.Add(email);
            t.ActionCollection.Add(setter);
            return JsonConvert.SerializeObject(t, Formatting.Indented,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });
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
