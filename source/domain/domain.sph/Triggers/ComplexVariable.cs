using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class ComplexVariable : Variable
    {
        public override string GeneratedCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            code.AppendFormat("public {0} {1} {{get;set;}}", this.TypeName, this.Name);
            return code.ToString();
        }

        public override string GeneratedCtorCode(WorkflowDefinition wd)
        {
            return $"           this.{Name} = new {TypeName}();";
        }

        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            if (string.IsNullOrWhiteSpace(this.TypeName))
            {
                result.Result = false;
                result.Errors.Add(new BuildError(this.WebId, $"[Variable] \"{this.Name}\" does not have a valid type"));
            }

            return result;
        }


        private string GenerateXsdComplexTypeJavascript(WorkflowDefinition wd, XElement e, int level = 0)
        {
            var properties = new List<string>();

            var code = new StringBuilder();
            var name = e.Attribute("name").Value;

            code.AppendLinf("bespoke.sph.wf.{0}.{1} = function(webid){{", wd.WorkflowTypeName, name);
            code.AppendLine("   var model = {");
            properties.Add($"         \"$type\" :\"{wd.CodeNamespace}.{name}, workflows.{wd.Id}.{wd.Version}\"");
            properties.Add("        \"WebId\": ko.observable(webid)");


            var members = this.GetJavascriptMembers(wd, e);
            properties.AddRange(members);

            code.AppendLine(string.Join(",\r\n", properties));

            code.AppendLine("       };");
            code.AppendLinf(@"
    if (bespoke.sph.wf.{0}.{1}Partial) {{
        return _(model).extend(new bespoke.sph.wf.{0}.{1}Partial(model));
    }}       
return model;", wd.WorkflowTypeName, name);
            code.AppendLine("   };");
            return code.ToString();



        }

        private string GenerateXsdElementJavascript(WorkflowDefinition wd, XElement e, int level = 0, Func<string, XElement> getComplextType = null)
        {
            var properties = new List<string>();

            var code = new StringBuilder();
            var name = e.Attribute("name").Value;

            code.AppendLinf("bespoke.sph.wf.{0}.{1} = function(webid){{", wd.WorkflowTypeName, name);
            code.AppendLine("   var model = {");
            properties.Add($"         \"$type\" :\"{wd.CodeNamespace}.{name}, workflows.{wd.Id}.{wd.Version}\"");
            properties.Add("         \"WebId\": ko.observable(webid)");


            var members = this.GetJavascriptMembers(wd, e.Element(x + "complexType"));
            properties.AddRange(members);

            // for extensions
            var extension = e.Descendants(x + "extension").FirstOrDefault();
            if (null != extension)
            {
                if (null != getComplextType)
                {
                    var baseComplextType = getComplextType(extension.Attribute("base").Value);
                    var baseMembers = this.GetJavascriptMembers(wd, baseComplextType);
                    properties.AddRange(baseMembers);
                }
                properties.AddRange(this.GetJavascriptMembers(wd, extension));
            }

            code.AppendLine(string.Join(",\r\n", properties));

            code.AppendLine("       };");
            code.AppendLinf(@"
    if (bespoke.sph.wf.{0}.{1}Partial) {{
        return _(model).extend(new bespoke.sph.wf.{0}.{1}Partial(model));
    }}       
    return model;", wd.WorkflowTypeName, name);
            code.AppendLine("   };");
            return code.ToString();



        }

        private IEnumerable<string> GetJavascriptMembers(WorkflowDefinition wd, XElement element)
        {
            var list = new List<string>();

            if (null == element) return list;

            var attributes = from at in element.Elements(x + "attribute")
                             let n = at.Attribute("name").Value
                             select $"         \"{n}\" : ko.observable()";
            list.AddRange(attributes);

            var all = element.Element(x + "all");
            if (null != all)
            {

                var allElements = from at in all.Elements(x + "element")
                                  where at.Attribute("name") != null
                                  && at.Attribute("type") != null
                                  select $"            \"{at.Attribute("name").Value}\" : ko.observable()";
                list.AddRange(allElements);

                var collectionElements = from at in all.Elements(x + "element")
                                         where at.Attribute("name") != null
                                         && at.Attribute("type") == null
                                         let refElement = at.Descendants(x + "element").First()
                                         select $"         \"{at.Attribute("name").Value}\" : ko.observableArray()";
                list.AddRange(collectionElements);



                var refElements = from at in all.Elements(x + "element")
                                  where at.Attribute("ref") != null
                                  let refa = at.Attribute("ref")
                                  select string.Format("            \"{0}\" : ko.observable(new bespoke.sph.w_{1}_{2}.{0}())", refa.Value, wd.Id, wd.Version);
                list.AddRange(refElements);


            }
            return list;
        }


        public override Task<string> GenerateCustomJavascriptAsync(WorkflowDefinition wd)
        {
            var script = new StringBuilder();
            script.AppendLine("var bespoke = bespoke || {};");
            script.AppendLine("bespoke.sph = bespoke.sph || {};");
            script.AppendLine("bespoke.sph.wf = bespoke.sph.wf || {};");
            script.AppendLine($"bespoke.sph.wf.{wd.WorkflowTypeName} = bespoke.sph.wf.v{wd.Version} || {{}};");

            var xsd = wd.GetCustomSchema();

            var complexTypesElement = xsd.Elements(x + "complexType").ToList();
            var complexTypeClasses = complexTypesElement.Select(e => this.GenerateXsdComplexTypeJavascript(wd, e)).ToList();
            complexTypeClasses.ForEach(c => script.AppendLine(c));

            var elements = xsd.Elements(x + "element").ToList();
            var elementClasses = elements.Select(e => this.GenerateXsdElementJavascript(wd, e, 0, s => complexTypesElement.Single(f => f.Attribute("name").Value == s))).ToList();
            elementClasses.ForEach(c => script.AppendLine(c));



            return Task.FromResult(script.ToString());
        }

        public override Task<IEnumerable<Class>> GenerateCustomTypesAsync(WorkflowDefinition wd)
        {
            var gen = new CsharpCodeGenerator(wd.GetCustomSchema(), wd.CodeNamespace);
            var classes = gen.Generate();
            return Task.FromResult(classes);
        }

        // ReSharper disable InconsistentNaming
        private static readonly XNamespace x = "http://www.w3.org/2001/XMLSchema";
        // ReSharper restore InconsistentNaming



        public static string GetClrDataType(XElement element)
        {

            var typeAttribute = element.Attribute("type");
            var nillableAttribute = element.Attribute("nillable");

            var xsType = typeAttribute?.Value ?? "";
            var nillable = nillableAttribute != null && bool.Parse(nillableAttribute.Value);

            string type;
            switch (xsType)
            {
                case "xs:string":
                    type = "string";
                    break;
                case "xs:date":
                case "xs:dateTime":
                    type = "DateTime";
                    break;
                case "xs:int":
                    type = "int";
                    break;
                case "xs:long":
                    type = "long";
                    break;
                case "xs:boolean":
                    type = "bool";
                    break;
                case "xs:float":
                    type = "float";
                    break;
                case "xs:double":
                    type = "double";
                    break;
                case "xs:decimal":
                    type = "decimal";
                    break;
                case "State":
                    type = "State";
                    break;
                case "xs:anySimpleType":
                    type = "object";
                    break;
                default:
                    type = xsType;
                    break;
            }
            if (nillable) type += "?";
            return type;
        }



        private List<string> GetCustomSchemaElementNames(WorkflowDefinition wd, string name)
        {
            var xsd = wd.GetCustomSchema();
            var elements = xsd.Elements(x + "element").Select(e => e.Attribute("name").Value).ToList();
            return elements;
        }
    }
}