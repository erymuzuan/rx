using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RoslynScriptEngines;
using Xunit;
using Xunit.Abstractions;

namespace domain.test.businessrules
{
    [Trait("Category", "BusinessRule")]
    public class BusinessRuleTest
    {
        private readonly ITestOutputHelper m_outputHelper;

        public BusinessRuleTest(ITestOutputHelper outputHelper)
        {
            m_outputHelper = outputHelper;
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
        }

        [Fact]
        public void SimpleRule()
        {
            var ed = this.CreatePatientDefinition();
            dynamic patient = this.CreateInstance(ed);
            patient.FullName = "Erymuzuan";

            var br = new BusinessRule { ErrorMessage = "Nama tidak mengandungi huruf A" };
            var nameMustContainsA = new Rule
            {
                Left = new DocumentField { Type = typeof(string), Path = "FullName" },
                Operator = Operator.Substringof,
                Right = new ConstantField { Type = typeof(string), Value = "A" }
            };
            br.RuleCollection.Add(nameMustContainsA);
            ed.BusinessRuleCollection.Add(br);

            var result = patient.ValidateBusinessRule(ed.BusinessRuleCollection);
            foreach (var error in result.ValidationErrors)
            {
                m_outputHelper.WriteLine(error.ToString());
            }
            Console.WriteLine(result.ValidationErrors);
            Assert.True(result.Success);

            Assert.Equal(0, result.ValidationErrors.Count);


        }
        [Fact]
        public void SimpleRuleWithFilter()
        {
            var ed = this.CreatePatientDefinition();
            dynamic patient = this.CreateInstance(ed);
            patient.FullName = "Erymuzuan";
            patient.Gender = "Male";

            var br = new BusinessRule { ErrorMessage = "Nama tidak mengandungi huruf A" };
            var nameMustContainsA = new Rule
            {
                Left = new DocumentField { Type = typeof(string), Path = "FullName" },
                Operator = Operator.Substringof,
                Right = new ConstantField { Type = typeof(string), Value = "A" }
            };
            br.RuleCollection.Add(nameMustContainsA);
            ed.BusinessRuleCollection.Add(br);

            br.FilterCollection.Add(new Rule
            {
                Left = new DocumentField { Type = typeof(string), Path = "Gender" },
                Operator = Operator.Eq,
                Right = new ConstantField { Type = typeof(string), Value = "Male" }
            });

            ValidationResult result = patient.ValidateBusinessRule(ed.BusinessRuleCollection);
            foreach (var error in result.ValidationErrors)
            {
                Console.WriteLine(error);
            }
            Console.WriteLine(result.ValidationErrors);
            Assert.True(result.Success);

            Assert.Equal(0, result.ValidationErrors.Count);


        }

        [Fact]
        public void SimpleRuleWithFilterNotEvaluated()
        {
            var ed = this.CreatePatientDefinition();
            dynamic patient = this.CreateInstance(ed);
            patient.FullName = "Siti";
            patient.Gender = "Female";

            var br = new BusinessRule { ErrorMessage = "Nama tidak mengandungi huruf A" };
            var nameMustContainsA = new Rule
            {
                Left = new DocumentField { Type = typeof(string), Path = "FullName" },
                Operator = Operator.Substringof,
                Right = new ConstantField { Type = typeof(string), Value = "A" }
            };
            br.RuleCollection.Add(nameMustContainsA);
            ed.BusinessRuleCollection.Add(br);

            br.FilterCollection.Add(new Rule
            {
                Left = new DocumentField { Type = typeof(string), Path = "Gender" },
                Operator = Operator.Eq,
                Right = new ConstantField { Type = typeof(string), Value = "Male" }
            });

            ValidationResult result = patient.ValidateBusinessRule(ed.BusinessRuleCollection);
            foreach (var error in result.ValidationErrors)
            {
                Console.WriteLine(error);
            }
            Console.WriteLine(result.ValidationErrors);
            Assert.True(result.Success);

            Assert.Equal(0, result.ValidationErrors.Count);


        }
        [Fact]
        public void SimpleRuleWithoutFilter()
        {
            var ed = this.CreatePatientDefinition();
            dynamic patient = this.CreateInstance(ed);
            patient.FullName = "Siti";
            patient.Gender = "Female";

            var br = new BusinessRule { ErrorMessage = "Nama tidak mengandungi huruf A" };
            var nameMustContainsA = new Rule
            {
                Left = new DocumentField { Type = typeof(string), Path = "FullName" },
                Operator = Operator.Substringof,
                Right = new ConstantField { Type = typeof(string), Value = "A" }
            };
            br.RuleCollection.Add(nameMustContainsA);
            ed.BusinessRuleCollection.Add(br);

            ValidationResult result = patient.ValidateBusinessRule(ed.BusinessRuleCollection);
            Assert.False(result.Success);

            Assert.Equal(1, result.ValidationErrors.Count);


        }


        [Fact]
        public void TwoRulesOneFail()
        {

            var ed = this.CreatePatientDefinition();
            dynamic patient = this.CreateInstance(ed);

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

            
            patient.FullName = "Kelantan";
            ed.BusinessRuleCollection.Add(br);
            ed.BusinessRuleCollection.Add(br2);


            ValidationResult result = patient.ValidateBusinessRule(ed.BusinessRuleCollection);
            Assert.False(result.Success);

            Assert.Equal(1, result.ValidationErrors.Count);
            Assert.Equal(br2.ErrorMessage, result.ValidationErrors[0].Message);


        }
        [Fact]
        public void TwoRulesAllFail()
        {

            var ed = this.CreatePatientDefinition();
            dynamic patient = this.CreateInstance(ed);

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
            
            patient.FullName = "PERLIS";
            ed.BusinessRuleCollection.Add(br);
            ed.BusinessRuleCollection.Add(br2);


            ValidationResult result = patient.ValidateBusinessRule(ed.BusinessRuleCollection);
            Assert.False(result.Success);

            Assert.Equal(2, result.ValidationErrors.Count);


        }


        private dynamic CreateInstance(EntityDefinition ed, bool verbose = false)
        {
            var options = new CompilerOptions
            {
                IsVerbose = verbose,
                IsDebug = true
            };
            options.AddReference(Path.GetFullPath($@"{ConfigurationManager.WebPath}\bin\System.Web.Mvc.dll"));
            options.AddReference(Path.GetFullPath($@"{ConfigurationManager.WebPath}\bin\core.sph.dll"));
            options.AddReference(Path.GetFullPath($@"{ConfigurationManager.WebPath}\bin\Newtonsoft.Json.dll"));


            var codes = ed.GenerateCode();
            var sources = ed.SaveSources(codes);
            var result = ed.Compile(options, sources);

            result.Errors.ForEach(Console.WriteLine);

            // try to instantiate the EntityDefinition
            var assembly = Assembly.LoadFrom(result.Output);
            var edTypeName = $"{ed.CodeNamespace}.{ed.Name}";

            var edType = assembly.GetType(edTypeName);
            Assert.NotNull(edType);

            return Activator.CreateInstance(edType);
        }


        public EntityDefinition CreatePatientDefinition()
        {
            var ent = new EntityDefinition { Name = "Patient", Plural = "Patients", RecordName = "FullName", Id = "patient" };
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "FullName",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            });
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Title",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            });
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Gender",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            });
            var address = new SimpleMember { Name = "Address", TypeName = "System.Object, mscorlib" };
            address.MemberCollection.Add(new SimpleMember { Name = "Street1", IsFilterable = false, TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new SimpleMember { Name = "State", IsFilterable = true, TypeName = "System.String, mscorlib" });
            ent.MemberCollection.Add(address);


            var contacts = new SimpleMember { Name = "ContactCollection", Type = typeof(Array) };
            contacts.Add(new Dictionary<string, Type> { { "Name", typeof(string) }, { "Telephone", typeof(string) } });
            ent.MemberCollection.Add(contacts);

            return ent;

        }

    }
}
