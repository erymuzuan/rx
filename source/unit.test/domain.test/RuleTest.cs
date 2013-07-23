using System;
using Bespoke.SphCommercialSpaces.Domain;
using NUnit.Framework;

namespace domain.test
{
    [TestFixture]
    class RuleTest
    {
        [Test]
        public void ConstEqConst()
        {
            var building = new Building();
            var rule = new Rule
                {
                    Left = new ConstantField {Value = 500},
                    Operator = Operator.Eq,
                    Right = new ConstantField {Value = 500}
                };

            var result = rule.Execute(building);
            Assert.IsTrue(result);
        }



        [Test]
        public void GetXPathDateValueOnDocumentField()
        {
            var app = new RentalApplication{ApplicationDate = DateTime.Today};
            var field = new DocumentField { Path = "//bs:RentalApplication/@ApplicationDate",Type = typeof(DateTime)};
            var val = field.GetValue(app);
            Assert.AreEqual(DateTime.Today, val);
        }

        [Test]
        public void GetXPathValueOnDocumentField()
        {
            var building = new Building{BuildingId = 500};
            var field = new DocumentField {Path = "//bs:Building/@BuildingId", Type = typeof(int)};
            var val = field.GetValue(building);
            Assert.AreEqual(500, val);
        }




        [Test]
        public void DocumentFieldEqConst()
        {
            var building = new Building{BuildingId = 500};
            var rule = new Rule
                {
                    Left = new DocumentField {Path = "//bs:Building/@BuildingId", Type = typeof(int)},
                    Operator = Operator.Eq,
                    Right = new ConstantField {Value = 500}
                };

            var result = rule.Execute(building);
            Assert.IsTrue(result);
        }


        [Test]
        public void DocumentFieldLeConst()
        {
            var building = new Building{BuildingId = 500};
            var rule = new Rule
                {
                    Left = new DocumentField {Path = "//bs:Building/@BuildingId", Type = typeof(int)},
                    Operator = Operator.Le,
                    Right = new ConstantField {Value = 500}
                };

            var result = rule.Execute(building);
            Assert.IsTrue(result);
        }

        [Test]
        public void DateTimeDocumentFieldLtConst()
        {
            var app = new RentalApplication{ApplicationDate= new DateTime(2010,5,5)};
            var rule = new Rule
                {
                    Left = new DocumentField { Path = "//bs:RentalApplication/@ApplicationDate", Type = typeof(DateTime) },
                    Operator = Operator.Lt,
                    Right = new ConstantField {Value = new DateTime(2012,5,5)}
                };

            var result = rule.Execute(app);
            Assert.IsTrue(result);
        }
        [Test]
        public void DateTimeDocumentFieldEqConst()
        {
            var app = new RentalApplication{ApplicationDate= new DateTime(2010,5,5)};
            var rule = new Rule
                {
                    Left = new DocumentField { Path = "//bs:RentalApplication/@ApplicationDate", Type = typeof(DateTime) },
                    Operator = Operator.Eq,
                    Right = new ConstantField {Value = new DateTime(2010,5,5)}
                };

            var result = rule.Execute(app);
            Assert.IsTrue(result);
        }

        [Test]
        public void DocumentFieldLtConst()
        {
            var building = new Building{BuildingId = 300};
            var rule = new Rule
                {
                    Left = new DocumentField {Path = "//bs:Building/@BuildingId", Type = typeof(int)},
                    Operator = Operator.Lt,
                    Right = new ConstantField {Value = 400}
                };

            var result = rule.Execute(building);
            Assert.IsTrue(result);
        }
    }
}
