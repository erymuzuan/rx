using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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
                FormDesign = new FormDesign { Name = "Mohon" }
            };
            var form = permohonan.FormDesign;
            form.FormElementCollection.Add(new TextBox { Path = "Name", Label = "Nama" });
            form.FormElementCollection.Add(new TextBox { Path = "MyKad", Label = "IC" });
            form.FormElementCollection.Add(new TextBox { Path = "LandCategory", Label = "Kategori Tanah" });


            wd.ActivityCollection.Add(permohonan);
            //GET
            var screen = wd.GetInititorScreen();


            // POST
            var values = new List<Variable>
            {
                new Variable{Name = "Name",DefaultValue = "Ima"},
                new Variable{Name = "MyKad", DefaultValue = "999999"},
                new Variable{Name = "LandCategory", DefaultValue = "999999"}
            };
            wd.InitiateAsync(values, screen)
                .ContinueWith(_ =>
                {
                    var wf = _.Result;
                    Assert.AreEqual("Permohonan Tanah Wakaf", wf.Name);
                    Assert.AreEqual("Active", wf.State);

                    //
                    Assert.AreEqual(3, wf.VariableCollection.Count);

                    Console.WriteLine("saving the WF entity to database");
                })
                .Wait();
        }

        [Test]
        public void LoadCustomSchema()
        {
            XNamespace x = "http://www.w3.org/2001/XMLSchema";
            var xsd =
                XElement.Load(
                    new Uri(@"C:\project\work\sph\source\unit.test\domain.test\workflows\PemohonWakaf.xsd").ToString());

            XNamespace customNs = xsd.Attribute("targetNamespace").Value;
            var elements = xsd.Elements(x + "element").ToList();
            Console.WriteLine(elements.Count());
            Console.WriteLine(customNs);
            var wd = new WorkflowDefinition();
            var json = wd.GenerateJson("Applicant", xsd);
            Console.WriteLine(json);
        }
    }


}
