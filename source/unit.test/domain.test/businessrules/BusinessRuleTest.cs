using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RoslynScriptEngines;
using NUnit.Framework;

namespace domain.test.businessrules
{
    [TestFixture]
    public class BusinessRuleTest
    {
        [SetUp]
        public void Setup()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
        }

        [Test]
        public void SimpleRule()
        {
            var customerDefinition = this.CreateCustomerDefinition();
            dynamic customer = this.CreateInstance(customerDefinition);
            customer.FullName = "Erymuzuan";

            var br = new BusinessRule { ErrorMessage = "Nama tidak mengandungi huruf A" };
            var nameMustContainsA = new Rule
            {
                Left = new DocumentField { Type = typeof(string), Path = "FullName" },
                Operator = Operator.Substringof,
                Right = new ConstantField { Type = typeof(string), Value = "A" }
            };
            br.RuleCollection.Add(nameMustContainsA);
            customerDefinition.BusinessRuleCollection.Add(br);

            ValidationResult result = customer.ValidateBusinessRule(customerDefinition.BusinessRuleCollection);
            foreach (var error in result.ValidationErrors)
            {
                Console.WriteLine(error);
            }
            Console.WriteLine(result.ValidationErrors);
            Assert.IsTrue(result.Success);

            Assert.AreEqual(0, result.ValidationErrors.Count);


        }
        [Test]
        public void TwoRulesAOneFail()
        {

            var customerDefinition = this.CreateCustomerDefinition();
            dynamic customer = this.CreateInstance(customerDefinition);

            var br = new BusinessRule { ErrorMessage = "Nama tidak mengandungi huruf A" };
            var nameMustContainsA = new Rule
            {
                Left = new DocumentField { Type = typeof(string), Path = "FullName" },
                Operator = Operator.Substringof,
                Right = new ConstantField { Type = typeof(string), Value = "A" }
            };
            br.RuleCollection.Add(nameMustContainsA);


            var br2 = new BusinessRule { ErrorMessage = "Nama tidak mengandungi huruf B" };
            var nameMustContainsB = new Rule
            {
                Left = new DocumentField { Type = typeof(string), Path = "FullName" },
                Operator = Operator.Substringof,
                Right = new ConstantField { Type = typeof(string), Value = "B" }
            };
            br2.RuleCollection.Add(nameMustContainsB);
            

            customer.Title= "Temp C";
            customer.FullName = "Kelantan";
            customerDefinition.BusinessRuleCollection.Add(br);
            customerDefinition.BusinessRuleCollection.Add(br2);


            ValidationResult result = customer.ValidateBusinessRule(customerDefinition.BusinessRuleCollection);
            Assert.IsFalse(result.Success);

            Assert.AreEqual(1, result.ValidationErrors.Count);
            Assert.AreEqual(br2.ErrorMessage, result.ValidationErrors[0].Message);


        }
        [Test]
        public void TwoRulesAllFail()
        {

            var customerDefinition = this.CreateCustomerDefinition();
            dynamic customer = this.CreateInstance(customerDefinition);

            var br = new BusinessRule { ErrorMessage = "Nama tidak mengandungi huruf A" };
            var nameMustContainsA = new Rule
            {
                Left = new DocumentField { Type = typeof(string), Path = "FullName" },
                Operator = Operator.Substringof,
                Right = new ConstantField { Type = typeof(string), Value = "A" }
            };
            br.RuleCollection.Add(nameMustContainsA);


            var br2 = new BusinessRule { ErrorMessage = "Nama tidak mengandungi huruf B" };
            var nameMustContainsB = new Rule
            {
                Left = new DocumentField { Type = typeof(string), Path = "FullName" },
                Operator = Operator.Substringof,
                Right = new ConstantField { Type = typeof(string), Value = "B" }
            };
            br2.RuleCollection.Add(nameMustContainsB);


            customer.Title = "Temp C";
            customer.FullName = "PERLIS";
            customerDefinition.BusinessRuleCollection.Add(br);
            customerDefinition.BusinessRuleCollection.Add(br2);


            ValidationResult result = customer.ValidateBusinessRule(customerDefinition.BusinessRuleCollection);
            Assert.IsFalse(result.Success);

            Assert.AreEqual(2, result.ValidationErrors.Count);


        }


        private dynamic CreateInstance(EntityDefinition ed, bool verbose = false)
        {
            var options = new CompilerOptions
            {
                IsVerbose = verbose,
                IsDebug = true
            };
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\core.sph.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll")));


            var result = ed.Compile(options);
            result.Errors.ForEach(Console.WriteLine);

            // try to instantiate the EntityDefinition
            var assembly = Assembly.LoadFrom(result.Output);
            var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, ed.EntityDefinitionId, ed.Name);

            var edType = assembly.GetType(edTypeName);
            Assert.IsNotNull(edType, edTypeName + " is null");

            return Activator.CreateInstance(edType);
        }


        public EntityDefinition CreateCustomerDefinition()
        {
            var ent = new EntityDefinition { Name = "Customer", Plural = "Customers" };
            ent.MemberCollection.Add(new Member
            {
                Name = "FullName",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            }); ent.MemberCollection.Add(new Member
            {
                Name = "Title",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            });
            var address = new Member { Name = "Address", TypeName = "System.Object, mscorlib" };
            address.MemberCollection.Add(new Member { Name = "Street1", IsFilterable = false, TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new Member { Name = "State", IsFilterable = true, TypeName = "System.String, mscorlib" });
            ent.MemberCollection.Add(address);


            var contacts = new Member { Name = "ContactCollection", Type = typeof(Array) };
            contacts.Add(new Dictionary<string, Type> { { "Name", typeof(string) }, { "Telephone", typeof(string) } });
            ent.MemberCollection.Add(contacts);

            return ent;

        }

    }
}
