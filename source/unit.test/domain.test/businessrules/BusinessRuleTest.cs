using NUnit.Framework;

namespace domain.test.businessrules
{
    [TestFixture]
    public class BusinessRuleTest
    {
        [Test]
        public void SimpleRule()
        {

            Assert.Fail();
            //ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            //var space = new Designation();
            //var template = new SpaceTemplate();
            //var br = new BusinessRule {ErrorMessage = "Nama tidak mengandungi huruf A"};
            //var nameMustContainsA = new Rule
            //{
            //    Left = new DocumentField { Type = typeof(string), Path = "TemplateName" },
            //    Operator = Operator.Substringof,
            //    Right = new ConstantField { Type = typeof(string),Value = "A"}
            //};
            //br.RuleCollection.Add(nameMustContainsA);

            //space.TemplateName = "Temp A";
            //space.State = "Selangor";
            //template.BusinessRuleCollection.Add(br);
            //Console.WriteLine(space);



            //var result = space.ValidateBusinessRule(template.BusinessRuleCollection);
            //Assert.IsTrue(result.Success);

            //Assert.AreEqual(0, result.ValidationErrors.Count);


        }
        [Test]
        public void TwoRulesAOneFail()
        {

            Assert.Fail();
            //    ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            //    var space = new Space();
            //    var template = new SpaceTemplate();

            //    var br = new BusinessRule {ErrorMessage = "Nama tidak mengandungi huruf A"};
            //    var nameMustContainsA = new Rule
            //    {
            //        Left = new DocumentField { Type = typeof(string), Path = "State" },
            //        Operator = Operator.Substringof,
            //        Right = new ConstantField { Type = typeof(string),Value = "A"}
            //    };
            //    br.RuleCollection.Add(nameMustContainsA);


            //    var br2 = new BusinessRule {ErrorMessage = "Nama tidak mengandungi huruf B"};
            //    var nameMustContainsB = new Rule
            //    {
            //        Left = new DocumentField { Type = typeof(string), Path = "State" },
            //        Operator = Operator.Substringof,
            //        Right = new ConstantField { Type = typeof(string),Value = "B"}
            //    };
            //    br2.RuleCollection.Add(nameMustContainsB);




            //    space.TemplateName = "Temp C";
            //    space.State = "Kelantan";
            //    template.BusinessRuleCollection.Add(br);
            //    template.BusinessRuleCollection.Add(br2);


            //    var result = space.ValidateBusinessRule(template.BusinessRuleCollection);
            //    Assert.IsFalse(result.Success);

            //    Assert.AreEqual(1, result.ValidationErrors.Count);
            //    Assert.AreEqual(br2.ErrorMessage, result.ValidationErrors[0].Message);


        }
        [Test]
        public void TwoRulesAllFail()
        {

            Assert.Fail();
            //ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            //var space = new Space();
            //var template = new SpaceTemplate();

            //var br = new BusinessRule {ErrorMessage = "Nama tidak mengandungi huruf A"};
            //var nameMustContainsA = new Rule
            //{
            //    Left = new DocumentField { Type = typeof(string), Path = "State" },
            //    Operator = Operator.Substringof,
            //    Right = new ConstantField { Type = typeof(string),Value = "A"}
            //};
            //br.RuleCollection.Add(nameMustContainsA);


            //var br2 = new BusinessRule {ErrorMessage = "Nama tidak mengandungi huruf B"};
            //var nameMustContainsB = new Rule
            //{
            //    Left = new DocumentField { Type = typeof(string), Path = "State" },
            //    Operator = Operator.Substringof,
            //    Right = new ConstantField { Type = typeof(string),Value = "B"}
            //};
            //br2.RuleCollection.Add(nameMustContainsB);




            //space.TemplateName = "Temp C";
            //space.State = "PERLIS";
            //template.BusinessRuleCollection.Add(br);
            //template.BusinessRuleCollection.Add(br2);


            //var result = space.ValidateBusinessRule(template.BusinessRuleCollection);
            //Assert.IsFalse(result.Success);

            //Assert.AreEqual(2, result.ValidationErrors.Count);


        }


    }
}
