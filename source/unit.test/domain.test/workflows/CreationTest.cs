using System;
using System.Linq;
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
        }

        [Test]
        public void ExtractStartWorkflowVariables()
        {

            var wd = new WorkflowDefinition();
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "nama", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "pemohon", TypeName = "Applicant" });
            const string json =
                " {\"$type\":\"Bespoke.Sph.Domain.ScreenActivityViewModel,custom.workflow\",\"nama\":\"test\",\"pemohon\":{\"$type\":\"Bespoke.Sph.Domain.Wd_1_Applicant,custom.workflow\",\"Name\":\"ima\",\"Address\":{\"$type\":\"Bespoke.Sph.Domain.Wd_1_Address,custom.workflow\",\"Postcode\":\"74555\"}}}";

            dynamic obj = JsonConvert.DeserializeObject(json);
            var wf = new Workflow();
            foreach (var w in wd.VariableDefinitionCollection)
            {
                var name = w.Name;
                var value = obj[w.Name];
                var v = new VariableValue { Name = name, Value = value };
                Console.WriteLine(string.Format("{0} = {1}", name, value));
                wf.VariableValueCollection.Add(v);
            }

            Console.WriteLine(wf.VariableValueCollection.Count);
            Assert.AreEqual(2, wf.VariableValueCollection.Count);
            Assert.IsInstanceOf<VariableValue>(wf.VariableValueCollection[0]);
            Assert.AreEqual("nama", wf.VariableValueCollection[0].Name);
        }

    }
}
