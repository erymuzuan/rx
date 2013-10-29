using System;
using System.Collections.Generic;
using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class CreationTest
    {
        [Test]
        public void InitiateNewWorkflow()
        {
            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf" };
            wd.InitiateAsync()
                .ContinueWith(_ =>
                {
                    var wf = _.Result;
                    Assert.AreEqual("Permohonan Tanah Wakaf", wf.Name);
                    Console.WriteLine(wf);
                })
                .Wait();
        }

        [Test]
        public void InitiateWakafApplication()
        {
            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf" };
            var permohonan = new ScreenActivity
            {
                Title = "Permohonan mewakaf tanah",
                IsInitiator = true,
                FormDesign = new FormDesign {Name = "Mohon"}
            };
            var form = permohonan.FormDesign;
            form.FormElementCollection.Add(new TextBox{Path = "Name",Label = "Nama"});
            form.FormElementCollection.Add(new TextBox{Path = "MyKad",Label = "IC"});
            form.FormElementCollection.Add(new TextBox{Path = "LandCategory",Label = "Kategori Tanah"});


            wd.ActivityCollection.Add(permohonan);
            //GET
            var screen = wd.GetInititorScreen();


            // POST
            var values = new List<CustomFieldValue>
            {
                new CustomFieldValue{Name = "Name",Value = "Ima"},
                new CustomFieldValue{Name = "MyKad", Value = "999999"},
                new CustomFieldValue{Name = "LandCategory", Value = "999999"}
            };
            wd.InitiateAsync(values, screen)
                .ContinueWith(_ =>
                {
                    var wf = _.Result;
                    Assert.AreEqual("Permohonan Tanah Wakaf", wf.Name);
                    Assert.AreEqual("Active", wf.State);

                    //
                    Assert.AreEqual(3,wf.CustomFieldValueCollection.Count);

                    Console.WriteLine("saving the WF entity to database");
                })
                .Wait();
        }
    }
}
