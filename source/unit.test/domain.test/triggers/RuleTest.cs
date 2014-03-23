using System;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RoslynScriptEngines;
using NUnit.Framework;

namespace domain.test.triggers
{
    [TestFixture]
    class RuleTest
    {
        [Test]
        public void EndWith()
        {
            var building = new Designation();
            var rule = new Rule
                {
                    Left = new ConstantField { Value = "This e-mail is not a new bill; it is meant to help you to identify", Type = typeof(string) },
                    Operator = Operator.Substringof,
                    Right = new ConstantField { Value = "IDENTIFY", Type = typeof(string) }
                };

            var result = rule.Execute(new RuleContext(building));
            Assert.IsTrue(result);
        }
        [Test]
        public void StartsWith()
        {
            var building = new Designation();
            var rule = new Rule
                {
                    Left = new ConstantField { Value = "This e-mail is not a new bill; it is meant to help you to identify the", Type = typeof(string) },
                    Operator = Operator.Substringof,
                    Right = new ConstantField { Value = "this", Type = typeof(string) }
                };

            var result = rule.Execute(new RuleContext(building));
            Assert.IsTrue(result);
        }

        [Test]
        public void Contains()
        {
            var building = new Designation();
            var rule = new Rule
                {
                    Left = new ConstantField { Value = "This e-mail is not a new bill; it is meant to help you to identify the", Type = typeof(string) },
                    Operator = Operator.Substringof,
                    Right = new ConstantField { Value = "Mail", Type = typeof(string) }
                };

            var result = rule.Execute(new RuleContext(building));
            Assert.IsTrue(result);
        }

        [Test]
        public void ConstEqConstString()
        {
            var building = new Designation();
            var rule = new Rule
                {
                    Left = new ConstantField { Value = "erymuzuan", Type = typeof(string) },
                    Operator = Operator.Eq,
                    Right = new ConstantField { Value = "Erymuzuan", Type = typeof(string) }
                };

            var result = rule.Execute(new RuleContext(building));
            Assert.IsFalse(result);
        }

        [Test]
        public void ConstEqConstString2()
        {
            var building = new Designation();
            var rule = new Rule
                {
                    Left = new ConstantField { Value = "erymuzuan", Type = typeof(string) },
                    Operator = Operator.Eq,
                    Right = new ConstantField { Value = "erymuzuan", Type = typeof(string) }
                };

            var result = rule.Execute(new RuleContext(building));
            Assert.IsTrue(result);
        }
        [Test]
        public void ConstNeqConstString()
        {
            var building = new Designation();
            var rule = new Rule
                {
                    Left = new ConstantField { Value = "erymuzuan", Type = typeof(string) },
                    Operator = Operator.Neq,
                    Right = new ConstantField { Value = "Erymuzuan", Type = typeof(string) }
                };

            var result = rule.Execute(new RuleContext(building));
            Assert.IsTrue(result);
        }
        [Test]
        public void ConstNeqConstInteger()
        {
            var building = new Designation();
            var rule = new Rule
                {
                    Left = new ConstantField { Value = 500, Type = typeof(int) },
                    Operator = Operator.Neq,
                    Right = new ConstantField { Value = 501, Type = typeof(int) }
                };

            var result = rule.Execute(new RuleContext(building));
            Assert.IsTrue(result);
        }

        [Test]
        public void ConstEqConst()
        {
            var building = new Designation();
            var rule = new Rule
                {
                    Left = new ConstantField { Value = 500, Type = typeof(int) },
                    Operator = Operator.Eq,
                    Right = new ConstantField { Value = 500, Type = typeof(int) }
                };

            var result = rule.Execute(new RuleContext(building));
            Assert.IsTrue(result);
        }

        [Test]
        public void ConstString()
        {
            var building = new Designation();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "Mohd Ali", Type = typeof(string) },
                Operator = Operator.Substringof,
                Right = new ConstantField { Value = "Ali", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(building));
            Assert.IsTrue(result);
        }



        [Test]
        public void GetXPathDateValueOnDocumentField()
        {
            var app = new Designation { CreatedDate = DateTime.Today };
            var field = new DocumentField { XPath = "//bs:Designation/@ApplicationDate", Type = typeof(DateTime) };
            var val = field.GetValue(new RuleContext(app));
            Assert.AreEqual(DateTime.Today, val);
        }

        [Test]
        public void ExecuteLinqValueOnDocumentField()
        {
            Assert.Fail();
            var script = new RoslynScriptEngine();
            var v = new Designation { DesignationId = 45 };
            v.RoleCollection.Add("50");
            v.RoleCollection.Add("60");

            var field = new FunctionField { Script = "item.DepositPaymentCollection.Sum(d => d.Amount)", ScriptEngine = script };
            var val = field.GetValue(new RuleContext(v));
            Assert.AreEqual(100m, val);
        }
        [Test]
        public void GetPathValueOnDocumentField()
        {
            Assert.Fail();
            //ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            //var v = new Deposit { DepositId = 45 };
            //v.DepositPaymentCollection.Add(new DepositPayment { Amount = 50m });
            //v.DepositPaymentCollection.Add(new DepositPayment { Amount = 50m });

            //var field = new DocumentField { Path = "DepositPaid", Type = typeof(decimal) };
            //var val = field.GetValue(new RuleContext(v));
            //Assert.AreEqual(100m, val);
        }

        [Test]
        public void GetXPathValueOnDocumentField()
        {
            var building = new Designation { DesignationId = 500 };
            var field = new DocumentField { XPath = "//bs:Designation/@DesignationId", Type = typeof(int) };
            var val = field.GetValue(new RuleContext(building));
            Assert.AreEqual(500, val);
        }




        [Test]
        public void DocumentFieldEqConst()
        {
            var building = new Designation { DesignationId = 500 };
            var rule = new Rule
                {
                    Left = new DocumentField { XPath = "//bs:Designation/@DesignationId", Type = typeof(int) },
                    Operator = Operator.Eq,
                    Right = new ConstantField { Value = 500 }
                };

            var result = rule.Execute(new RuleContext(building));
            Assert.IsTrue(result);
        }


        [Test]
        public void DocumentFieldLeConst()
        {
            var building = new Designation { DesignationId = 500 };
            var rule = new Rule
                {
                    Left = new DocumentField { XPath = "//bs:Designation/@DesignationId", Type = typeof(int) },
                    Operator = Operator.Le,
                    Right = new ConstantField { Value = 500 }
                };

            var result = rule.Execute(new RuleContext(building));
            Assert.IsTrue(result);
        }

        [Test]
        public void DateTimeDocumentFieldLtConst()
        {
            Assert.Fail();
            var app = new Designation { CreatedDate = new DateTime(2010, 5, 5) };
            var rule = new Rule
                {
                    Left = new DocumentField { XPath = "//bs:Designation/@ApplicationDate", Type = typeof(DateTime) },
                    Operator = Operator.Lt,
                    Right = new ConstantField { Value = new DateTime(2012, 5, 5) }
                };

            var result = rule.Execute(new RuleContext(app));
            Assert.IsTrue(result);
        }
        [Test]
        public void DateTimeDocumentFieldEqConst()
        {
            var app = new Designation { CreatedDate = new DateTime(2010, 5, 5) };
            var rule = new Rule
                {
                    Left = new DocumentField { XPath = "//bs:Designation/@ApplicationDate", Type = typeof(DateTime) },
                    Operator = Operator.Eq,
                    Right = new ConstantField { Value = new DateTime(2010, 5, 5) }
                };

            var result = rule.Execute(new RuleContext(app));
            Assert.IsTrue(result);
        }

        [Test]
        public void DocumentFieldLtConst()
        {
            var building = new Designation { DesignationId = 300 };
            var rule = new Rule
                {
                    Left = new DocumentField { XPath = "//bs:Designation/@DesignationId", Type = typeof(int) },
                    Operator = Operator.Lt,
                    Right = new ConstantField { Value = 400 }
                };

            var result = rule.Execute(new RuleContext(building));
            Assert.IsTrue(result);
        }
    }
}
