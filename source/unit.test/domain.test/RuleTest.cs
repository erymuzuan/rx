﻿using System;
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
                    Operator = Operator.Equal,
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
            var field = new DocumentField {Path = "//Building/@BuildingId"};
            var val = field.GetValue(building);
            Assert.AreEqual(500, val);
        }


        [Test]
        public void DocumentFieldEqConst()
        {
            var building = new Building{BuildingId = 500};
            var rule = new Rule
                {
                    Left = new DocumentField {Path = "//bs:Building/@BuildingId"},
                    Operator = Operator.Equal,
                    Right = new ConstantField {Value = "500"}
                };

            var result = rule.Execute(building);
            Assert.IsTrue(result);
        }

        [Test]
        public void DocumentFieldEqFuction()
        {
            var building = new Building{BuildingId = 500};
            var rule = new Rule
                {
                    Left = new DocumentField {Path = "//bs:Building/@CreatedDate"},
                    Operator = Operator.Equal,
                    Right = new FuctionField {Script = "DateTime.Today"}
                };

            var result = rule.Execute(building);
            Assert.IsTrue(result);
        }

        [Test]
        public void DocumentFieldLtConst()
        {
            var building = new Building{BuildingId = 500};
            var rule = new Rule
                {
                    Left = new DocumentField {Path = "//bs:Building/@BuildingId"},
                    Operator = Operator.Equal,
                    Right = new ConstantField {Value = "500"}
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
                    Left = new DocumentField {Path = "//bs:Building/@BuildingId"},
                    Operator = Operator.Equal,
                    Right = new ConstantField {Value = "500"}
                };

            var result = rule.Execute(building);
            Assert.IsTrue(result);
        }
    }
}
