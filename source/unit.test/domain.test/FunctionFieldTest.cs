using System;
using Bespoke.SphCommercialSpaces.Domain;
using NUnit.Framework;

namespace domain.test
{
    [TestFixture]
    class FunctionFieldTest
    {

        [Test]
        public void FuctionDateTimeValue()
        {
            var building = new FuctionField { Script = "DateTime.Today" };

            var result = building.GetValue(new Building());
            Assert.AreEqual(DateTime.Today, result);
        }

        [Test]
        public void DocumentFieldEqFuction()
        {
            var building = new Building { BuildingId = 500 };
            var rule = new Rule
            {
                Left = new DocumentField { Path = "//bs:Building/@CreatedDate", Type = typeof(DateTime) },
                Operator = Operator.Equal,
                Right = new FuctionField { Script = "DateTime.Today" }
            };

            var result = rule.Execute(building);
            Assert.IsTrue(result);
        }
    }
}