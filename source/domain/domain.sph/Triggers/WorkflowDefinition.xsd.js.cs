using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class WorkflowDefinition
    {
        public string GenerateXsdComplexTypeJavascript(XElement e, int level = 0)
        {
            var properties = new List<string>();

            var code = new StringBuilder();
            var name = e.Attribute("name").Value;

            code.AppendLinf("bespoke.sph.w_{1}_{2}.{0} = function(webid){{", name, this.WorkflowDefinitionId, this.Version);
            code.AppendLine("   var model = {");
            properties.Add(string.Format("         \"$type\" :\"{0}.{1}, workflows.{2}.{3}\"", this.CodeNamespace, name, this.WorkflowDefinitionId, this.Version));
            properties.Add("        \"WebId\": ko.observable(webid)");


            var members = this.GetJavascriptMembers(e);
            properties.AddRange(members);

            code.AppendLine(string.Join(",\r\n", properties));

            code.AppendLine("       };");
            code.AppendLinf(@"
    if (bespoke.sph.w_{0}_{1}.{2}Partial) {{
        return _(model).extend(new bespoke.sph.w_{0}_{1}.{2}Partial(model));
    }}       
return model;", this.WorkflowDefinitionId, this.Version, name);
            code.AppendLine("   };");
            return code.ToString();



        }

        public string GenerateXsdElementJavascript(XElement e, int level = 0, Func<string, XElement> getComplextType = null)
        {
            var properties = new List<string>();

            var code = new StringBuilder();
            var name = e.Attribute("name").Value;

            code.AppendLinf("bespoke.sph.w_{1}_{2}.{0} = function(webid){{", name, this.WorkflowDefinitionId, this.Version);
            code.AppendLine("   var model = {");
            properties.Add(string.Format("         \"$type\" :\"{0}.{1}, workflows.{2}.{3}\"", this.CodeNamespace, name, this.WorkflowDefinitionId, this.Version));
            properties.Add("        \"WebId\": ko.observable(webid)");


            var members = this.GetJavascriptMembers(e.Element(x + "complexType"));
            properties.AddRange(members);

            // for extensions
            var extension = e.Descendants(x + "extension").FirstOrDefault();
            if (null != extension)
            {
                if (null != getComplextType)
                {
                    var baseComplextType = getComplextType(extension.Attribute("base").Value);
                    var baseMembers = this.GetJavascriptMembers(baseComplextType);
                    properties.AddRange(baseMembers);
                }
                properties.AddRange(this.GetJavascriptMembers(extension));
            }

            code.AppendLine(string.Join(",\r\n", properties));

            code.AppendLine("       };");
            code.AppendLinf(@"
    if (bespoke.sph.w_{0}_{1}.{2}Partial) {{
        return _(model).extend(new bespoke.sph.w_{0}_{1}.{2}Partial(model));
    }}       
return model;", this.WorkflowDefinitionId, this.Version, name);
            code.AppendLine("   };");
            return code.ToString();



        }

        private IEnumerable<string> GetJavascriptMembers(XElement element)
        {
            var list = new List<string>();

            if (null == element) return list;

            var attributes = from at in element.Elements(x + "attribute")
                             let n = at.Attribute("name").Value
                             select string.Format("         \"{0}\" : ko.observable()", n);
            list.AddRange(attributes);

            var all = element.Element(x + "all");
            if (null != all)
            {

                var allElements = from at in all.Elements(x + "element")
                                  where at.Attribute("name") != null
                                  && at.Attribute("type") != null
                                  select string.Format("            \"{0}\" : ko.observable()", at.Attribute("name").Value);
                list.AddRange(allElements);

                var collectionElements = from at in all.Elements(x + "element")
                                         where at.Attribute("name") != null
                                         && at.Attribute("type") == null
                                         let refElement = at.Descendants(x + "element").First()
                                         select string.Format("         \"{0}\" : ko.observableArray()", at.Attribute("name").Value);
                list.AddRange(collectionElements);



                var refElements = from at in all.Elements(x + "element")
                                  where at.Attribute("ref") != null
                                  let refa = at.Attribute("ref")
                                  select string.Format("            \"{0}\" : ko.observable(new bespoke.sph.w_{1}_{2}.{0}())", refa.Value, this.WorkflowDefinitionId, this.Version);
                list.AddRange(refElements);


            }
            return list;
        }

    }
}