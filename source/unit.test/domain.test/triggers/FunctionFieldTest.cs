using System;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RoslynScriptEngines;
using NUnit.Framework;

namespace domain.test.triggers
{
    [TestFixture]
    class FunctionFieldTest
    {

        [Test]
        public void FuctionDateTimeValue()
        {
            var building = new FunctionField { Script = "DateTime.Today", ScriptEngine = new RoslynScriptEngine() };

            var result = building.GetValue(new RuleContext(new Designation()));
            Assert.AreEqual(DateTime.Today, result);
        }

        [Test]
        public void DocumentFieldEqFunction()
        {
            var script = new RoslynScriptEngine();
            var building = new Designation{ CreatedDate = DateTime.Today };
            var rule = new Rule
            {
                Left = new DocumentField { XPath = "//bs:RentalApplication/@ApplicationDate", Type = typeof(DateTime) },
                Operator = Operator.Eq,
                Right = new FunctionField { Script = "return DateTime.Today;", ScriptEngine = script }
            };

            var result = rule.Execute(new RuleContext(building));
            Assert.IsTrue(result);
        }
        [Test]
        public void DocumentFieldEqFunctionExpression()
        {
            var script = new RoslynScriptEngine();
            var building = new Designation { CreatedDate = DateTime.Today.AddDays(-2) };
            var rule = new Rule
            {
                Left = new DocumentField { XPath = "//bs:RentalApplication/@ApplicationDate", Type = typeof(DateTime) },
                Operator = Operator.Eq,
                Right = new FunctionField { Script = "DateTime.Today.AddDays(-2)", ScriptEngine = script }
            };

            var result = rule.Execute(new RuleContext(building));
            Assert.IsTrue(result);
        }

        [Test]
        public void DocumentFieldEqFunctionWithItem()
        {
            var script = new RoslynScriptEngine();
            var app = new Designation
            {
                CreatedDate = DateTime.Today.AddDays(-2),
                DesignationId = 1,
                Name = "1234"
            };
            var rule = new Rule
            {
                Left = new DocumentField { XPath = "//bs:RentalApplication/@ApplicationDate", Type = typeof(DateTime) },
                Operator = Operator.Eq,
                Right = new FunctionField { Script = "Console.WriteLine(item.RegistrationNo);return item.ApplicationDate;", ScriptEngine = script }
            };

            var result = rule.Execute(new RuleContext(app));
            Assert.IsTrue(result);
        }
    }
}