using System;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using NUnit.Framework;

namespace domain.test.workflows
{
    public class BuildValidationTest
    {
        [Test]
        public void BuildValidation()
        {
            var wd = new WorkflowDefinition { Name = "3 Is Three", SchemaStoreId = Guid.NewGuid().ToString() };
            var screen = new ScreenActivity { Name = "Pohon", IsInitiator = true, NextActivityWebId = "end", WebId = Strings.GenerateId(), Performer = new Performer { IsPublic = true } };
            var end = new EndActivity { WebId = "end" };
            wd.ActivityCollection.Add(screen);
            wd.ActivityCollection.Add(end);


            var result = wd.ValidateBuild();
            Console.WriteLine(result.ToJsonString(Formatting.Indented));
            Assert.IsFalse(result.Result);
            Assert.AreEqual(2, result.Errors.Count);
            Assert.AreEqual("Name must be started with letter.You cannot use symbol or number as first character", result.Errors[0].Message);
            Assert.AreEqual("[ScreenActivity] : Pohon => does not have a Form defined", result.Errors[1].Message);

        }

        [Test]
        public void BuildValidationMissingWebId()
        {
            var wd = new WorkflowDefinition { Name = "Test Workflow", SchemaStoreId = "123" };
            var end = new EndActivity { WebId = "end" };
            var screen = new ScreenActivity { Name = "Pohon", NextActivityWebId = end.WebId, IsInitiator = true, Performer = new Performer { IsPublic = true } };
            wd.ActivityCollection.Add(screen);
            wd.ActivityCollection.Add(end);


            var result = wd.ValidateBuild();
            Console.WriteLine(result.ToJsonString(Formatting.Indented));
            Assert.IsFalse(result.Result);
            Assert.AreEqual(2, result.Errors.Count);
            StringAssert.Contains("Missing webid", result.Errors[0].ToString());

        }



        [Test]
        public void BuildValidationDuplicateWebId()
        {
            var wd = new WorkflowDefinition { Name = "Test Workflow", SchemaStoreId = "123" };
            var end = new EndActivity { WebId = "end" };
            var screen = new ScreenActivity { Name = "Pohon", FormId = "x", IsInitiator = true, WebId = "A", NextActivityWebId = "B", Performer = new Performer { IsPublic = true } };
            var screen2 = new ScreenActivity { Name = "Pohon 2", FormId = "x", IsInitiator = false, WebId = "A", NextActivityWebId = end.WebId, Performer = new Performer { IsPublic = true } };

            wd.ActivityCollection.Add(screen);
            wd.ActivityCollection.Add(screen2);
            wd.ActivityCollection.Add(end);

            var result = wd.ValidateBuild();
            Console.WriteLine(result.ToJsonString(true));
            Assert.IsFalse(result.Result);
            Assert.AreEqual(2, result.Errors.Count);
            StringAssert.Contains("Duplicate webid", result.Errors[0].ToString());

        }



    }
}