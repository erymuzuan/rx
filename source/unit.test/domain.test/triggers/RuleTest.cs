using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RoslynScriptEngines;
using Xunit;

namespace domain.test.triggers
{
    [Trait("Category", "BusinessRule")]
    public class RuleTest
    {
        public RuleTest()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
        }

        [Trait("Field", "Constant")]
        [Fact]
        public async Task EndWith()
        {
            var customer = await this.GetCustomerInstanceAsync();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "This e-mail is not a new bill; it is meant to help you to identify", Type = typeof(string) },
                Operator = Operator.Substringof,
                Right = new ConstantField { Value = "IDENTIFY", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.True(result);
        }

        [Fact]
        [Trait("Field", "Constant")]
        public async Task StartsWith()
        {
            var customer =await this.GetCustomerInstanceAsync();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "This e-mail is not a new bill; it is meant to help you to identify the", Type = typeof(string) },
                Operator = Operator.Substringof,
                Right = new ConstantField { Value = "this", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.True(result);
        }
        [Fact]
        [Trait("Field", "Constant")]
        public async Task IsNull()
        {
            var customer =await this.GetCustomerInstanceAsync();
            var rule = new Rule
            {
                Left = new DocumentField { Name = "AccountNo", Path = "AccountNo", Type = typeof(string) },
                Operator = Operator.IsNull
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.True(result);
        }

        [Fact]
        public async Task Contains()
        {
            var custoemr =await this.GetCustomerInstanceAsync();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "This e-mail is not a new bill; it is meant to help you to identify the", Type = typeof(string) },
                Operator = Operator.Substringof,
                Right = new ConstantField { Value = "Mail", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(custoemr));
            Assert.True(result);
        }

        [Fact]
        public async Task ConstEqConstString()
        {
            var customer =await this.GetCustomerInstanceAsync();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "erymuzuan", Type = typeof(string) },
                Operator = Operator.Eq,
                Right = new ConstantField { Value = "Erymuzuan", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.False(result);
        }

        [Fact]
        public async Task ConstEqConstString2()
        {
            var customer = await this.GetCustomerInstanceAsync();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "erymuzuan", Type = typeof(string) },
                Operator = Operator.Eq,
                Right = new ConstantField { Value = "erymuzuan", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.True(result);
        }
        [Fact]
        public async Task ConstNeqConstString()
        {
            var building = await this.GetCustomerInstanceAsync();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "erymuzuan", Type = typeof(string) },
                Operator = Operator.Neq,
                Right = new ConstantField { Value = "Erymuzuan", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(building));
            Assert.True(result);
        }
        [Fact]
        public async Task ConstNeqConstInteger()
        {
            var customer = await this.GetCustomerInstanceAsync();
            var rule = new Rule
            {
                Left = new ConstantField { Value = 500, Type = typeof(int) },
                Operator = Operator.Neq,
                Right = new ConstantField { Value = 501, Type = typeof(int) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.True(result);
        }

        [Fact]
        public async Task ConstEqConst()
        {
            var customer = await this.GetCustomerInstanceAsync();
            var rule = new Rule
            {
                Left = new ConstantField { Value = 500, Type = typeof(int) },
                Operator = Operator.Eq,
                Right = new ConstantField { Value = 500, Type = typeof(int) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.True(result);
        }

        [Fact]
        public async Task ConstString()
        {
            var customer = await this.GetCustomerInstanceAsync();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "Mohd Ali", Type = typeof(string) },
                Operator = Operator.Substringof,
                Right = new ConstantField { Value = "Ali", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.True(result);
        }

        [Fact]
        public async Task NotContainsString()
        {
            var customer = await this.GetCustomerInstanceAsync();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "Mohd Ali", Type = typeof(string) },
                Operator = Operator.NotContains,
                Right = new ConstantField { Value = "Sam", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.True(result);
        }
        [Fact]
        public async Task NotContainsStringFalse()
        {
            var customer = await this.GetCustomerInstanceAsync();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "Mohd Ali", Type = typeof(string) },
                Operator = Operator.NotContains,
                Right = new ConstantField { Value = "Ali", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.False(result);
        }

        [Fact]
        public async Task NotStartsWith()
        {
            var customer = await this.GetCustomerInstanceAsync();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "Mohd Ali", Type = typeof(string) },
                Operator = Operator.NotStartsWith,
                Right = new ConstantField { Value = "Ali", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.True(result);
        }

        [Fact]
        public async Task NotEndsWith()
        {
            var customer = await this.GetCustomerInstanceAsync();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "Mohd Ali", Type = typeof(string) },
                Operator = Operator.NotEndsWith,
                Right = new ConstantField { Value = "M", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.True(result);
        }
        [Fact]
        public async Task NotEndsWithFalse()
        {
            var customer = await this.GetCustomerInstanceAsync();
            var rule = new Rule
            {
                Left = new ConstantField { Value = "EY123456MY", Type = typeof(string) },
                Operator = Operator.NotEndsWith,
                Right = new ConstantField { Value = "MY", Type = typeof(string) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.False(result);
        }




        [Fact]
        public async Task ExecuteLinqValueOnDocumentField()
        {
            var script = new RoslynScriptEngine();
            var customer = await this.GetCustomerInstanceAsync();
            customer.Revenue = 100m * 10;
            var field = new FunctionField { Script = "item.Revenue", ScriptEngine = script };
            var val = field.GetValue(new RuleContext(customer));
            Assert.Equal(1000m, val);
        }





        [Fact]
        public async Task DocumentFieldEqConst()
        {
            var customer = await this.GetCustomerInstanceAsync();
            customer.Rating = 6;
            customer.Id = "15";

            var doc = new DocumentField { Path = "Rating", Type = typeof(int) };
            var rule = new Rule
            {
                Left = doc,
                Operator = Operator.Eq,
                Right = new ConstantField { Value = 6 }
            };
            Assert.Equal(6, doc.GetValue(new RuleContext(customer)));

            var result = rule.Execute(new RuleContext(customer));
            Assert.True(result);
        }


        [Fact]
        public async Task DocumentFieldLeConst()
        {
            var customer = await this.GetCustomerInstanceAsync();
            customer.Id = "50";
            customer.Age = 50;
            var rule = new Rule
            {
                Left = new DocumentField { Path = "Age", Type = typeof(int) },
                Operator = Operator.Le,
                Right = new ConstantField { Value = 500 }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.True(result);
        }

        [Fact]
        public async Task DateTimeDocumentFieldLtConst()
        {
            var customer = await this.GetCustomerInstanceAsync();
            customer.CreatedDate = new DateTime(2010, 5, 5);
            var rule = new Rule
            {
                Left = new DocumentField { Path = "CreatedDate", Type = typeof(DateTime) },
                Operator = Operator.Lt,
                Right = new ConstantField { Value = new DateTime(2012, 5, 5) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.True(result);
        }
        [Fact]
        public async Task DateTimeDocumentFieldEqConst()
        {
            var customer = await this.GetCustomerInstanceAsync();
            customer.CreatedDate = new DateTime(2010, 5, 5);
            var rule = new Rule
            {
                Left = new DocumentField { Path = "CreatedDate", Type = typeof(DateTime) },
                Operator = Operator.Eq,
                Right = new ConstantField { Value = new DateTime(2010, 5, 5) }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.True(result);
        }

        [Fact]
        public async Task DocumentFieldLtConst()
        {
            var customer = await this.GetCustomerInstanceAsync();
            customer.Age = 300;
            var rule = new Rule
            {
                Left = new DocumentField { Path = "Age", Type = typeof(int) },
                Operator = Operator.Lt,
                Right = new ConstantField { Value = 400 }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.True(result);
        }
    }
}
