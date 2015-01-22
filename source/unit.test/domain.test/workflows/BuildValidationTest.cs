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
            var wd = new WorkflowDefinition { Name = "3 Is Three" ,SchemaStoreId = Guid.NewGuid().ToString()};
            var screen = new ScreenActivity { Name = "Pohon", IsInitiator = true, WebId = Guid.NewGuid().ToString(), Performer = new Performer{IsPublic = true}};
            
           // screen.FormDesign.FormElementCollection.Add(new TextBox { Label = "Nama", Path = string.Empty });
            wd.ActivityCollection.Add(screen);


            var result = wd.ValidateBuild();
            Console.WriteLine(result.ToJsonString(Formatting.Indented));
            Assert.IsFalse(result.Result);
            Assert.AreEqual(3, result.Errors.Count);
            Assert.AreEqual("Name must be started with letter.You cannot use symbol or number as first character", result.Errors[0].Message);
            Assert.AreEqual("[ScreenActivity] : Pohon => 'Nama' does not have path", result.Errors[1].Message);

            Assert.Fail("Screeen Activity has no FormDesign");
        }

        [Test]
        public void BuildValidationMissingWebId()
        {
            var wd = new WorkflowDefinition { Name = "Test Workflow", SchemaStoreId = "123"};
            var screen = new ScreenActivity { Name = "Pohon", IsInitiator = true , Performer = new Performer{IsPublic = true}};
            //screen.FormDesign.FormElementCollection.Add(new TextBox { Label = "Nama", Path = "Nama" });
            wd.ActivityCollection.Add(screen);


            var result = wd.ValidateBuild();
            Console.WriteLine(result.ToJsonString(Formatting.Indented));
            Assert.IsFalse(result.Result);
            Assert.AreEqual(2, result.Errors.Count);
            StringAssert.Contains("Missing webid", result.Errors[0].ToString());

            Assert.Fail("Screeen Activity has no FormDesign");
        }



        [Test]
        public void BuildValidationDuplicateWebId()
        {
            var wd = new WorkflowDefinition { Name = "Test Workflow", SchemaStoreId = "123"};
            var screen = new ScreenActivity { Name = "Pohon", IsInitiator = true, WebId = "A", NextActivityWebId = "B", Performer = new Performer{IsPublic = true}};
            var screen2 = new ScreenActivity { Name = "Pohon 2", IsInitiator = false, WebId = "A", NextActivityWebId = "C" , Performer = new Performer{IsPublic = true}};
           // screen.FormDesign.FormElementCollection.Add(new TextBox { Label = "Nama", Path = "Nama" });
            wd.ActivityCollection.Add(screen);
            wd.ActivityCollection.Add(screen2);

            var result = wd.ValidateBuild();
            Console.WriteLine(result.ToJsonString());
            Assert.IsFalse(result.Result);
            Assert.AreEqual(2, result.Errors.Count);
            StringAssert.Contains("Duplicate webid", result.Errors[0].ToString());
            Assert.Fail("Screeen Activity has no FormDesign");

        }



    }
}