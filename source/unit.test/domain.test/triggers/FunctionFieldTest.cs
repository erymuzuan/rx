using System;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RoslynScriptEngines;
using NUnit.Framework;


namespace domain.test.triggers
{
    [TestFixture]
    class FunctionFieldTest
    {
        [SetUp]
        public void Setup()
        {

            var script = new RoslynScriptEngine();
            ObjectBuilder.AddCacheList<IScriptEngine>(script);
        }

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
            dynamic customer = this.GetCustomerInstance();
            customer.CreatedDate = DateTime.Today;

            var rule = new Rule
            {
                Left = new DocumentField { Path = "CreatedDate", Type = typeof(DateTime) },
                Operator = Operator.Eq,
                Right = new FunctionField { Script = "return DateTime.Today;" }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsTrue(result);
        }
        [Test]
        public void DocumentFieldEqFunctionExpression()
        {
            dynamic customer = this.GetCustomerInstance();
            customer.CreatedDate = DateTime.Today.AddDays(-2);

            var rule = new Rule
            {
                Left = new DocumentField { Path = "CreatedDate", Type = typeof(DateTime) },
                Operator = Operator.Eq,
                Right = new FunctionField { Script = "DateTime.Today.AddDays(-2)" }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsTrue(result);
        }

        [Test]
        public void DocumentFieldEqFunctionWithItem()
        {
            dynamic customer = this.GetCustomerInstance();
            customer.CreatedDate = DateTime.Today.AddDays(-2);
            customer.CustomerId = 1;
            customer.FullName = "Wan Fatimah";

            var rule = new Rule
            {
                Left = new DocumentField { Path = "CreatedDate", Type = typeof(DateTime) },
                Operator = Operator.Eq,
                Right = new FunctionField { Script = "Console.WriteLine(item.FullName);return item.CreatedDate;" }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsTrue(result);
        }
    }
}