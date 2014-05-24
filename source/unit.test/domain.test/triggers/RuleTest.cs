using System;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RoslynScriptEngines;
using NUnit.Framework;

namespace domain.test.triggers
{
    [TestFixture]
    class RuleTest
    {
        [SetUp]
        public void Init()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
        }
        [Test]
        public void EndWith()
        {
            var customer = this.GetCustomerInstance();
            var rule = new Rule
                {
                    Left = new ConstantField { Value = "This e-mail is not a new bill; it is meant to help you to identify", Type = typeof(string) },
                    Operator = Operator.Substringof,
                    Right = new ConstantField { Value = "IDENTIFY", Type = typeof(string) }
                };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsTrue(result);
        }
        [Test]
        public void StartsWith()
        {
            var customer = this.GetCustomerInstance();
            var rule = new Rule
                {
                    Left = new ConstantField { Value = "This e-mail is not a new bill; it is meant to help you to identify the", Type = typeof(string) },
                    Operator = Operator.Substringof,
                    Right = new ConstantField { Value = "this", Type = typeof(string) }
                };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsTrue(result);
        }

        [Test]
        public void Contains()
        {
            var custoemr = this.GetCustomerInstance();
            var rule = new Rule
                {
                    Left = new ConstantField { Value = "This e-mail is not a new bill; it is meant to help you to identify the", Type = typeof(string) },
                    Operator = Operator.Substringof,
                    Right = new ConstantField { Value = "Mail", Type = typeof(string) }
                };

            var result = rule.Execute(new RuleContext(custoemr));
            Assert.IsTrue(result);
        }

        [Test]
        public void ConstEqConstString()
        {
            var customer = this.GetCustomerInstance();
            var rule = new Rule
                {
                    Left = new ConstantField { Value = "erymuzuan", Type = typeof(string) },
                    Operator = Operator.Eq,
                    Right = new ConstantField { Value = "Erymuzuan", Type = typeof(string) }
                };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsFalse(result);
        }

        [Test]
        public void ConstEqConstString2()
        {
            var customer = this.GetCustomerInstance();
            var rule = new Rule
                {
                    Left = new ConstantField { Value = "erymuzuan", Type = typeof(string) },
                    Operator = Operator.Eq,
                    Right = new ConstantField { Value = "erymuzuan", Type = typeof(string) }
                };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsTrue(result);
        }
        [Test]
        public void ConstNeqConstString()
        {
            var building = this.GetCustomerInstance();
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
            var customer = this.GetCustomerInstance();
            var rule = new Rule
                {
                    Left = new ConstantField { Value = 500, Type = typeof(int) },
                    Operator = Operator.Neq,
                    Right = new ConstantField { Value = 501, Type = typeof(int) }
                };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsTrue(result);
        }

        [Test]
        public void ConstEqConst()
        {
            var customer = this.GetCustomerInstance();
            var rule = new Rule
                {
                    Left = new ConstantField { Value = 500, Type = typeof(int) },
                    Operator = Operator.Eq,
                    Right = new ConstantField { Value = 500, Type = typeof(int) }
                };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsTrue(result);
        }

        [Test]
        public void ConstString()
        {
            var customer = this.GetCustomerInstance();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "Mohd Ali", Type = typeof(string) },
                Operator = Operator.Substringof,
                Right = new ConstantField { Value = "Ali", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsTrue(result);
        }

        [Test]
        public void NotContainsString()
        {
            var customer = this.GetCustomerInstance();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "Mohd Ali", Type = typeof(string) },
                Operator = Operator.NotContains,
                Right = new ConstantField { Value = "Sam", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsTrue(result);
        }
        [Test]
        public void NotContainsStringFalse()
        {
            var customer = this.GetCustomerInstance();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "Mohd Ali", Type = typeof(string) },
                Operator = Operator.NotContains,
                Right = new ConstantField { Value = "Ali", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsFalse(result);
        }

        [Test]
        public void NotStartsWith()
        {
            var customer = this.GetCustomerInstance();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "Mohd Ali", Type = typeof(string) },
                Operator = Operator.NotStartsWith,
                Right = new ConstantField { Value = "Ali", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsTrue(result);
        }

        [Test]
        public void NotEndsWith()
        {
            var customer = this.GetCustomerInstance();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "Mohd Ali", Type = typeof(string) },
                Operator = Operator.NotEndsWith,
                Right = new ConstantField { Value = "M", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsTrue(result);
        }




        [Test]
        public void ExecuteLinqValueOnDocumentField()
        {
            var script = new RoslynScriptEngine();
            var customer = this.GetCustomerInstance();
            customer.Revenue = 100m * 10;
            var field = new FunctionField { Script = "item.Revenue", ScriptEngine = script };
            var val = field.GetValue(new RuleContext(customer));
            Assert.AreEqual(1000m, val);
        }





        [Test]
        public void DocumentFieldEqConst()
        {
            var customer = this.GetCustomerInstance();
            customer.Rating = 6;
            customer.CustomerId = 15;

            var doc = new DocumentField { Path = "Rating", Type = typeof(int) };
            var rule = new Rule
                {
                    Left = doc,
                    Operator = Operator.Eq,
                    Right = new ConstantField { Value = 6 }
                };
            Assert.AreEqual(6, doc.GetValue(new RuleContext(customer)));

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsTrue(result);
        }


        [Test]
        public void DocumentFieldLeConst()
        {
            var customer = this.GetCustomerInstance();
            customer.CustomerId = 50;
            var rule = new Rule
                {
                    Left = new DocumentField { Path = "CustomerId", Type = typeof(int) },
                    Operator = Operator.Le,
                    Right = new ConstantField { Value = 500 }
                };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsTrue(result);
        }

        [Test]
        public void DateTimeDocumentFieldLtConst()
        {
            var customer = this.GetCustomerInstance();
            customer.CreatedDate = new DateTime(2010, 5, 5);
            var rule = new Rule
                {
                    Left = new DocumentField { Path = "CreatedDate", Type = typeof(DateTime) },
                    Operator = Operator.Lt,
                    Right = new ConstantField { Value = new DateTime(2012, 5, 5) }
                };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsTrue(result);
        }
        [Test]
        public void DateTimeDocumentFieldEqConst()
        {
            var customer = this.GetCustomerInstance();
            customer. CreatedDate = new DateTime(2010, 5, 5) ;
            var rule = new Rule
                {
                    Left = new DocumentField { Path = "CreatedDate", Type = typeof(DateTime) },
                    Operator = Operator.Eq,
                    Right = new ConstantField { Value = new DateTime(2010, 5, 5) }
                };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsTrue(result);
        }

        [Test]
        public void DocumentFieldLtConst()
        {
            var customer = this.GetCustomerInstance();
            customer.CustomerId = 300 ;
            var rule = new Rule
                {
                    Left = new DocumentField { Path = "CustomerId", Type = typeof(int) },
                    Operator = Operator.Lt,
                    Right = new ConstantField { Value = 400 }
                };

            var result = rule.Execute(new RuleContext(customer));
            Assert.IsTrue(result);
        }
    }
}
