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
            var building = new FuctionField { Script = "DateTime.Today", ScriptEngine = new RoslynScriptEngine()};

            var result = building.GetValue(new Building());
            Assert.AreEqual(DateTime.Today, result);
        }

        [Test]
        public void DocumentFieldEqFuction()
        {
            var script = new RoslynScriptEngine();
            var building = new RentalApplication{ ApplicationDate= DateTime.Today};
            var rule = new Rule
            {
                Left = new DocumentField { Path = "//bs:RentalApplication/@ApplicationDate", Type = typeof(DateTime) },
                Operator = Operator.Equal,
                Right = new FuctionField { Script = "return DateTime.Today;", ScriptEngine = script }
            };

            var result = rule.Execute(building);
            Assert.IsTrue(result);
        }
        [Test]
        public void DocumentFieldEqFuctionExpression()
        {
            var script = new RoslynScriptEngine();
            var building = new RentalApplication{ ApplicationDate= DateTime.Today.AddDays(-2)};
            var rule = new Rule
            {
                Left = new DocumentField { Path = "//bs:RentalApplication/@ApplicationDate", Type = typeof(DateTime) },
                Operator = Operator.Equal,
                Right = new FuctionField { Script = "DateTime.Today.AddDays(-2)", ScriptEngine = script }
            };

            var result = rule.Execute(building);
            Assert.IsTrue(result);
        }

        [Test]
        public void DocumentFieldEqFuctionWithItem()
        {
            var script = new RoslynScriptEngine();
            var building = new RentalApplication{ ApplicationDate= DateTime.Today.AddDays(-2),RentalApplicationId = 1, RegistrationNo = "1234"};
            var rule = new Rule
            {
                Left = new DocumentField { Path = "//bs:RentalApplication/@ApplicationDate", Type = typeof(DateTime) },
                Operator = Operator.Equal,
                Right = new FuctionField { Script = "Console.WriteLine(item);return item.ApplicationDate;", ScriptEngine = script }
            };

            var result = rule.Execute(building);
            Assert.IsTrue(result);
        }
    }
}