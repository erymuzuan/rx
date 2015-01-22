using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public class XsdJavascript
    {
        // ReSharper disable InconsistentNaming
        static readonly XNamespace x = "http://www.w3.org/2001/XMLSchema";
        // ReSharper restore InconsistentNaming

        private readonly WorkflowDefinition m_wd;

        public XsdJavascript(WorkflowDefinition wd)
        {
            m_wd = wd;
        }
        // from ComplexVariable
        public string GetJsonIntance(ComplexVariable cv)
        {

            var schema = m_wd.GetCustomSchema();
            var xsd = new XsdMetadata(schema);
            var members = xsd.GetMembersPath(cv.TypeName);

            var json = new StringBuilder();
            json.AppendFormat("\"{0}\":", cv.Name);
            json.AppendLine("{");

            var childs = members.Select(v => string.Format("\"{0}\":null", v));
            json.AppendLine(string.Join(",\r\n", childs));

            // TODO : split the child aggregate into it's own object

            json.AppendLine("}");

            return json.ToString();
        }

        public string GenerateXsdComplexTypeJavascript(XElement e, int level = 0)
        {
            var properties = new List<string>();

            var code = new StringBuilder();
            var name = e.Attribute("name").Value;

            code.AppendLinf("bespoke.sph.wf.{0}.{1} = function(webid){{", m_wd.WorkflowTypeName, name);
            code.AppendLine("   var model = {");
            properties.Add(string.Format("         \"$type\" :\"{0}.{1}, workflows.{2}.{3}\"", m_wd.CodeNamespace, name, m_wd.Id, m_wd.Version));
            properties.Add("        \"WebId\": ko.observable(webid)");


            var members = this.GetJavascriptMembers(e);
            properties.AddRange(members);

            code.AppendLine(string.Join(",\r\n", properties));

            code.AppendLine("       };");
            code.AppendLinf(@"
    if (bespoke.sph.wf.{0}.{1}Partial) {{
        return _(model).extend(new bespoke.sph.wf.{0}.{1}Partial(model));
    }}       
return model;", m_wd.WorkflowTypeName, name);
            code.AppendLine("   };");
            return code.ToString();



        }

        public string GenerateXsdElementJavascript(XElement e, int level = 0, Func<string, XElement> getComplextType = null)
        {
            var properties = new List<string>();

            var code = new StringBuilder();
            var name = e.Attribute("name").Value;

            code.AppendLinf("bespoke.sph.wf.{0}.{1} = function(webid){{", m_wd.WorkflowTypeName, name);
            code.AppendLine("   var model = {");
            properties.Add(string.Format("         \"$type\" :\"{0}.{1}, workflows.{2}.{3}\"", m_wd.CodeNamespace, name, m_wd.Id, m_wd.Version));
            properties.Add("         \"WebId\": ko.observable(webid)");


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
    if (bespoke.sph.wf.{0}.{1}Partial) {{
        return _(model).extend(new bespoke.sph.wf.{0}.{1}Partial(model));
    }}       
    return model;", m_wd.WorkflowTypeName, name);
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
                                  select string.Format("            \"{0}\" : ko.observable(new bespoke.sph.w_{1}_{2}.{0}())", refa.Value, m_wd.Id, m_wd.Version);
                list.AddRange(refElements);


            }
            return list;
        }
    }
}