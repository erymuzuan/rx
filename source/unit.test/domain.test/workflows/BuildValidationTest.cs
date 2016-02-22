using System;
using Bespoke.Sph.Domain;
using Xunit;
using Xunit.Abstractions;

namespace domain.test.workflows
{
    [Trait("Category", "Workflow")]
    public class BuildValidationTest
    {
        private readonly ITestOutputHelper m_helper;

        public BuildValidationTest(ITestOutputHelper helper)
        {
            m_helper = helper;
        }

        [Fact]
        public void BuildValidation()
        {
            var wd = new WorkflowDefinition { Name = "3 Is Three", SchemaStoreId = Guid.NewGuid().ToString() };
            var screen = new ReceiveActivity { Name = "Pohon", IsInitiator = true, WebId = Guid.NewGuid().ToString() };
            wd.ActivityCollection.Add(screen);


            var result = wd.ValidateBuild();
            m_helper.WriteLine(result.ToString());
            Assert.False(result.Result);
            Assert.Equal(4, result.Errors.Count);
            Assert.Equal("Name must be started with letter.You cannot use symbol or number as first character", result.Errors[0].Message);
            Assert.Equal("[ReceiveActivity] : Pohon => does not have Operation", result.Errors[1].Message);

        }

        [Fact]
        public void BuildValidationMissingWebId()
        {
            var wd = new WorkflowDefinition { Name = "Test Workflow", SchemaStoreId = "123" };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "StatusMessage", Type = typeof(string)});
            var screen = new ReceiveActivity { Name = "Pohon", IsInitiator = true, MessagePath = "StatusMessage" };
            wd.ActivityCollection.Add(screen);


            var result = wd.ValidateBuild();
            m_helper.WriteLine(result.ToString());
            Assert.False(result.Result);
            Assert.Equal(2, result.Errors.Count);
            Assert.Contains("Missing webid", result.Errors[0].ToString());

        }



        [Fact]
        public void BuildValidationDuplicateWebId()
        {
            var wd = new WorkflowDefinition { Name = "Test Workflow", SchemaStoreId = "123" };
            wd.VariableDefinitionCollection.Add(new SimpleVariable {Name = "SimpleMessage", Type = typeof(string)});
            var wait = new ReceiveActivity { Name = "Pohon", Operation = "SubmitPermohonan", IsInitiator = true, WebId = "A", NextActivityWebId = "B" ,MessagePath = "SimpleMessage"};
            var habis = new EndActivity { Name = "Done", WebId = "A", NextActivityWebId = "C" };
            wd.ActivityCollection.Add(wait);
            wd.ActivityCollection.Add(habis);

            var result = wd.ValidateBuild();
            m_helper.WriteLine(result.ToString());
            Assert.False(result.Result);
            Assert.Equal(1, result.Errors.Count);
            Assert.Contains("Duplicate webid", result.Errors[0].ToString());

        }



    }
}