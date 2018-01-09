using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.RoslynScriptEngines;
using Xunit;
using Xunit.Abstractions;

namespace domain.test.businessrules
{
    [Trait("Category", "BusinessRule")]
    public class BusinessRuleTest
    {
        public ITestOutputHelper Console { get; }

        public BusinessRuleTest(ITestOutputHelper console)
        {
            Console = console;
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));

            var git = new MockSourceRepository();
            ObjectBuilder.AddCacheList<ISourceRepository>(git);
            ObjectBuilder.AddCacheList<ICvsProvider>(new MockGit());
        }

        [Fact]
        public async Task SimpleRule()
        {
            var ed = this.CreatePatientDefinition();
            dynamic patient = await this.CreateInstanceAsync(ed);
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
                Console.WriteLine(error.ToString());
            }
            Console.WriteLine(result.ValidationErrors.ToString("\r\n"));
            Assert.True(result.Success);

            Assert.Equal(0, result.ValidationErrors.Count);


        }
        [Fact]
        public async Task SimpleRuleWithFilter()
        {
            var ed = this.CreatePatientDefinition();
            dynamic patient = await this.CreateInstanceAsync(ed);
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
                Console.WriteLine(error.ToString());
            }
            Console.WriteLine(result.ValidationErrors.ToString(", "));
            Assert.True(result.Success);

            Assert.Empty(result.ValidationErrors);


        }

        [Fact]
        public async Task SimpleRuleWithFilterNotEvaluated()
        {
            var ed = this.CreatePatientDefinition();
            dynamic patient = await this.CreateInstanceAsync(ed);
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
                Console.WriteLine(error.ToString());
            }
            Console.WriteLine(result.ValidationErrors.ToString(", "));
            Assert.True(result.Success);

            Assert.Empty(result.ValidationErrors);


        }
        [Fact]
        public async Task SimpleRuleWithoutFilter()
        {
            var ed = this.CreatePatientDefinition();
            dynamic patient = await this.CreateInstanceAsync(ed);
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

            Assert.Single(result.ValidationErrors);


        }


        [Fact]
        public async Task TwoRulesOneFail()
        {

            var ed = this.CreatePatientDefinition();
            dynamic patient = await this.CreateInstanceAsync(ed);

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

            Assert.Single(result.ValidationErrors);
            Assert.Equal(br2.ErrorMessage, result.ValidationErrors[0].Message);


        }
        [Fact]
        public async Task TwoRulesAllFail()
        {

            var ed = this.CreatePatientDefinition();
            dynamic patient = await this.CreateInstanceAsync(ed);

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


        private async Task<dynamic> CreateInstanceAsync(EntityDefinition ed)
        {
            var compiler = new Bespoke.Sph.Csharp.CompilersServices.EntityDefinitionCompiler
            {
                BuildDiagnostics = Array.Empty<IBuildDiagnostics>()
            };
            var tempFileName = Path.GetTempFileName();
            var peStream = tempFileName + ".dll";
            var pdbStream = tempFileName + ".pdb";
            var result = await compiler.BuildAsync(ed, x => new CompilerOptions2(peStream, pdbStream));
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.ToString());
            }
            Assert.True(result.Result);
            // try to instantiate the EntityDefinition
            var assembly = Assembly.LoadFrom(peStream);
            var edTypeName = $"{ed.CodeNamespace}.{ed.Name}";

            var edType = assembly.GetType(edTypeName);
            Assert.NotNull(edType);

            return Activator.CreateInstance(edType);
        }


        public EntityDefinition CreatePatientDefinition()
        {
            var ent = new EntityDefinition { Name = "Patient", Plural = "Patients", RecordName = "FullName", Id = "patient" };
            ent.AddSimpleMember("FullName", true);
            ent.AddSimpleMember("Title", true);
            ent.AddSimpleMember("Gender", true);

            var address = ent.AddMember<ComplexMember>("Address");
            address.TypeName = "Address";
            address.AddMember<string>("Street1");
            address.AddMember<string>("State", true);


            var contacts = ent.AddMember<ComplexMember>("Contacts", true);
            contacts.TypeName = "Contact";
            contacts.Add(new Dictionary<string, Type> { { "Name", typeof(string) }, { "Telephone", typeof(string) } });

            return ent;

        }

    }
}