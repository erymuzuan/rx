using System;
using System.Collections.Generic;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RoslynScriptEngines;
using Xunit;


namespace domain.test.triggers
{
    [Trait("Category", "BusinessRule")]
    public class FunctionFieldTest
    {
        public FunctionFieldTest()
        {
            var script = new RoslynScriptEngine();
            ObjectBuilder.AddCacheList<IScriptEngine>(script);
        }

        public static IEnumerable<object[]> ScriptRuleContextData
        {
            get
            {
                yield return new object[] { "DateTime.Today", DateTime.Today };
                yield return new object[] { "decimal.Zero", 0m};
            }
        }

        [Theory]
        [InlineData("1 + 2 ", 3)]
        [InlineData("\"Erymuzuan\" + \" Mustapa\" ", "Erymuzuan Mustapa")]
        [MemberData("ScriptRuleContextData")]
        public void ScriptRuleContext(string script, object expected)
        {
            var building = new FunctionField { Script = script, ScriptEngine = new RoslynScriptEngine() };

            var result = building.GetValue(new RuleContext(new Designation()));
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("CreatedDate", typeof(DateTime), "return DateTime.Today;")]
        [InlineData("ChangedDate", typeof(DateTime), "DateTime.Today.AddDays(-10)")]
        [InlineData("Id", typeof(string), "\"abc\"")]
        [InlineData("CreatedDate", typeof(DateTime), "return item.CreatedDate;")]
        public void DocumentFieldEqFunction(string member, Type type, string script)
        {
            dynamic customer = this.GetCustomerInstance();
            customer.CreatedDate = DateTime.Today;
            customer.ChangedDate = DateTime.Today.AddDays(-10);
            customer.Id = "abc";

            var rule = new Rule
            {
                Left = new DocumentField { Path = member, Type = type },
                Operator = Operator.Eq,
                Right = new FunctionField { Script = script }
            };

            var result = rule.Execute(new RuleContext(customer));
            Assert.True(result);
        }


    }
}