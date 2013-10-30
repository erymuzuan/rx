using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
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
                    Assert.AreEqual(3, wf.CustomFieldValueCollection.Count);

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

            var json = this.GenerateJson("Applicant");
            Console.WriteLine(json);
        }

        private string GenerateJson(string elementName, int level = 0)
        {
            XNamespace x = "http://www.w3.org/2001/XMLSchema";
            var xsd =
                XElement.Load(
                    new Uri(@"C:\project\work\sph\source\unit.test\domain.test\workflows\PemohonWakaf.xsd").ToString());

            var elements = xsd.Elements(x + "element").ToList();
            var e = elements.SingleOrDefault(a => a.Attribute("name").Value == elementName);

            var json = this.GenerateJson(e, level);

            return json;

        }

        private string GenerateJson(XElement e, int level = 0)
        {
            var tab = new string(' ', level + 1);
            var tab1 = tab;
            var properties = new List<string>();

            XNamespace x = "http://www.w3.org/2001/XMLSchema";
            var json = new StringBuilder("{");
            var name = e.Attribute("name").Value;
            json.AppendFormat(tab + "\r\n$type:\"{0}\",", name);

            var ct = e.Element(x + "complexType");
            if (null == ct) return json.ToString();

            var attributes = from at in ct.Elements(x + "attribute")
                             let n = at.Attribute("name").Value
                             select string.Format("{0}\r\n{1} :null", tab1, n);
            properties.AddRange(attributes);

            var all = ct.Element(x + "all");
            if (null != all)
            {
                //Console.WriteLine(ct.Elements(x + "element").Count());
                var allElements = from at in all.Elements(x + "element")
                                  where at.Attribute("name") != null
                                  select string.Format(tab1 + "\r\n{0} :null", at.Attribute("name").Value);
                properties.AddRange(allElements);

                var refElements = from at in all.Elements(x + "element")
                                  where at.Attribute("ref") != null
                                  select tab + "\r\n" + at.Attribute("ref").Value + ":" + this.GenerateJson(at.Attribute("ref").Value, level + 1);
                properties.AddRange(refElements);

            }


            json.AppendLine(string.Join(",", properties));
            json.AppendLine("}");
            return json.ToString();
        }
    }


}
