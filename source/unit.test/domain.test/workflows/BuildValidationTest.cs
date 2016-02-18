using System;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Xunit;

namespace domain.test.workflows
{
    public class BuildValidationTest
    {
        [Fact]
        public void BuildValidation()
        {
            var wd = new WorkflowDefinition { Name = "3 Is Three" ,SchemaStoreId = Guid.NewGuid().ToString()};
            var screen = new ReceiveActivity { Name = "Pohon", IsInitiator = true, WebId = Guid.NewGuid().ToString()};
            wd.ActivityCollection.Add(screen);


            var result = wd.ValidateBuild();
            Console.WriteLine(result.ToJsonString(Formatting.Indented));
            Assert.False(result.Result);
            Assert.Equal(3, result.Errors.Count);
            Assert.Equal("Name must be started with letter.You cannot use symbol or number as first character", result.Errors[0].Message);
            Assert.Equal("[ScreenActivity] : Pohon => 'Nama' does not have path", result.Errors[1].Message);

        }

        [Fact]
        public void BuildValidationMissingWebId()
        {
            var wd = new WorkflowDefinition { Name = "Test Workflow", SchemaStoreId = "123"};
            var screen = new ReceiveActivity { Name = "Pohon", IsInitiator = true };
            wd.ActivityCollection.Add(screen);


            var result = wd.ValidateBuild();
            Console.WriteLine(result.ToJsonString(Formatting.Indented));
            Assert.False(result.Result);
            Assert.Equal(2, result.Errors.Count);
            Assert.Contains("Missing webid", result.Errors[0].ToString());

        }



        [Fact]
        public void BuildValidationDuplicateWebId()
        {
            var wd = new WorkflowDefinition { Name = "Test Workflow", SchemaStoreId = "123"};
            var screen = new ReceiveActivity { Name = "Pohon", IsInitiator = true, WebId = "A", NextActivityWebId = "B"};
            var screen2 = new ReceiveActivity { Name = "Pohon 2", IsInitiator = false, WebId = "A", NextActivityWebId = "C" };
            wd.ActivityCollection.Add(screen);
            wd.ActivityCollection.Add(screen2);

            var result = wd.ValidateBuild();
            Console.WriteLine(result.ToJsonString());
            Assert.False(result.Result);
            Assert.Equal(2, result.Errors.Count);
            Assert.Contains("Duplicate webid", result.Errors[0].ToString());

        }



    }
}