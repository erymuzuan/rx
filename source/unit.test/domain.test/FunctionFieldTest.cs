using System;
using Bespoke.SphCommercialSpaces.Domain;
using NUnit.Framework;
using roslyn.scriptengine;

namespace domain.test
{
    [TestFixture]
    class FunctionFieldTest
    {

        [Test]
        public void FuctionDateTimeValue()
        {
            var building = new FunctionField { Script = "DateTime.Today", ScriptEngine = new RoslynScriptEngine() };

            var result = building.GetValue(new Building());
            Assert.AreEqual(DateTime.Today, result);
        }

        [Test]
        public void DocumentFieldEqFunction()
        {
            var script = new RoslynScriptEngine();
            var building = new RentalApplication { ApplicationDate = DateTime.Today };
            var rule = new Rule
            {
                Left = new DocumentField { XPath = "//bs:RentalApplication/@ApplicationDate", Type = typeof(DateTime) },
                Operator = Operator.Eq,
                Right = new FunctionField { Script = "return DateTime.Today;", ScriptEngine = script }
            };

            var result = rule.Execute(building);
            Assert.IsTrue(result);
        }
        [Test]
        public void DocumentFieldEqFunctionExpression()
        {
            var script = new RoslynScriptEngine();
            var building = new RentalApplication { ApplicationDate = DateTime.Today.AddDays(-2) };
            var rule = new Rule
            {
                Left = new DocumentField { XPath = "//bs:RentalApplication/@ApplicationDate", Type = typeof(DateTime) },
                Operator = Operator.Eq,
                Right = new FunctionField { Script = "DateTime.Today.AddDays(-2)", ScriptEngine = script }
            };

            var result = rule.Execute(building);
            Assert.IsTrue(result);
        }

        [Test]
        public void DocumentFieldEqFunctionWithItem()
        {
            var script = new RoslynScriptEngine();
            var app = new RentalApplication
            {
                ApplicationDate = DateTime.Today.AddDays(-2),
                RentalApplicationId = 1,
                RegistrationNo = "1234"
            };
            var rule = new Rule
            {
                Left = new DocumentField { XPath = "//bs:RentalApplication/@ApplicationDate", Type = typeof(DateTime) },
                Operator = Operator.Eq,
                Right = new FunctionField { Script = "Console.WriteLine(item.RegistrationNo);return item.ApplicationDate;", ScriptEngine = script }
            };

            var result = rule.Execute(app);
            Assert.IsTrue(result);
        }
    }
}