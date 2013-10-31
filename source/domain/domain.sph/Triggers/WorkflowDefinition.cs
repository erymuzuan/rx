using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class WorkflowDefinition : Entity
    {
        public Task<Workflow> InitiateAsync(IEnumerable<VariableValue> values = null, ScreenActivity screen = null)
        {
            var wf = new Workflow
            {
                Name = this.Name,
                WorkflowDefinitionId = this.WorkflowDefinitionId,
                State = "Active"

            };
            if (null != screen)
            {
                wf.VariableValueCollection.ClearAndAddRange(values);
            }
            return Task.FromResult(wf);
        }

        public ScreenActivity GetInititorScreen()
        {
            return this.ActivityCollection.Single(p => p.IsInitiator) as ScreenActivity;
        }


        public string GenerateJson(string elementName, XElement xsd, int level = 0)
        {
            XNamespace x = "http://www.w3.org/2001/XMLSchema";
            var elements = xsd.Elements(x + "element").ToList();
            var e = elements.SingleOrDefault(a => a.Attribute("name").Value == elementName);

            var json = this.GenerateJson(xsd,e, level);
            return json;

        }

        public string GenerateJson(XElement xsd, XElement e, int level = 0)
        {
            var tab = new string(' ', level + 1);
            var tab1 = tab;
            var properties = new List<string>();

            XNamespace x = "http://www.w3.org/2001/XMLSchema";
            var json = new StringBuilder("{");
            var name = e.Attribute("name").Value;
            json.AppendFormat(tab + "\r\n$type:\"Bespoke.Sph.Domain.Wd_{1}_{0},custom.workflow\",", name, this.WorkflowDefinitionId);

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
                                  select tab + "\r\n" + at.Attribute("ref").Value + ":" + this.GenerateJson(at.Attribute("ref").Value, xsd, level + 1);
                properties.AddRange(refElements);

            }


            json.AppendLine(string.Join(",", properties));
            json.AppendLine("}");
            return json.ToString();
        }
    }
}