using System;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RoslynScriptEngines;
using NUnit.Framework;

namespace domain.test.businessrules
{
    [TestFixture]
    public  class BusinessRuleTest
    {
        [Test]
        public void SimpleRule()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            var space = new Space();
            var template = new SpaceTemplate();
            var br = new BusinessRule();
            br.ErrorMessage = "Nama tidak mengandungi huruf A";
            var nameMustContainsA = new Rule
            {
                Left = new DocumentField { Type = typeof(string), Path = "TemplateName" },
                Operator = Operator.Substringof,
                Right = new ConstantField { Type = typeof(string),Value = "A"}
            };
            br.RuleCollection.Add(nameMustContainsA);

            space.TemplateName = "Temp A";
            space.State = "Selangor";
            template.BusinessRuleCollection.Add(br);
            Console.WriteLine(space);
            var valid = space.ValidateBusinessRule(template.BusinessRuleCollection);
            Console.WriteLine(valid);
            Assert.AreEqual(valid, "rule berjaya");

        }


    }
}
