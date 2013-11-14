using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    public partial class WorkflowDefinition
    {
        private XElement m_customSchema;

        public string GenerateXsdElementCsharpClass(XElement e, int level = 0)
        {
            var properties = new List<string>();

            XNamespace x = "http://www.w3.org/2001/XMLSchema";
            var code = new StringBuilder();
            var name = e.Attribute("name").Value;
            var xsd = this.GetCustomSchema();
            var ns = xsd.Attribute("targetNamespace");

            code.AppendLinf("   [XmlType(\"{0}\",  Namespace=\"{1}\")]", name, ns != null ? ns.Value : "");
            code.AppendLinf("   public partial class {0} : DomainObject", name);
            code.AppendLine("   {");

            
            var ct = e.Element(x + "complexType");
            if (null == ct) return code.ToString();

            var attributes = from at in ct.Elements(x + "attribute")
                             let n = at.Attribute("name").Value
                             select "     [XmlAttribute]\r\n"
                                + string.Format("     public {1} {0} {{get;set;}}", n, this.GetClrDataType(at));
            properties.AddRange(attributes);

            var all = ct.Element(x + "all");
            if (null != all)
            {
                //Console.WriteLine(ct.Elements(x + "element").Count());
                var allElements = from at in all.Elements(x + "element")
                                  where at.Attribute("name") != null
                                  && at.Attribute("type") != null
                                  select string.Format("      public {1} {0} {{get;set;}}", at.Attribute("name").Value, this.GetClrDataType(at));
                properties.AddRange(allElements);

                var collectionElements = from at in all.Elements(x + "element")
                                         where at.Attribute("name") != null
                                         && at.Attribute("type") == null
                                         let refElement = at.Descendants(x + "element").First()
                                         select string.Format("         private readonly ObjectCollection<{1}> m_{0} = new ObjectCollection<{1}>();\r\n" +
                                                              "         public ObjectCollection<{1}> {0} {{get {{ return m_{0};}} }}", at.Attribute("name").Value, refElement.Attribute("ref").Value);
                properties.AddRange(collectionElements);

                var refElements = from at in all.Elements(x + "element")
                                  where at.Attribute("ref") != null
                                  let refa = at.Attribute("ref")
                                  select string.Format("      private  {0} m_{0} = new {0}();\r\n" +
                                    "      public {0} {0}{{get{{ return m_{0};}} set{{ m_{0} = value;}} }}",refa.Value);
                properties.AddRange(refElements);




            }


            code.AppendLine(string.Join("\r\n", properties));
            code.AppendLine("   }");
            return code.ToString();



        }

        public string GenerateXsdElementJavascript(XElement e, int level = 0)
        {
            var properties = new List<string>();

            XNamespace x = "http://www.w3.org/2001/XMLSchema";
            var code = new StringBuilder();
            var name = e.Attribute("name").Value;

            code.AppendLinf("bespoke.sph.w_{1}_{2}.{0} = function(webid){{",  name, this.WorkflowDefinitionId, this.Version);
            code.AppendLine("   var model = {");
            properties.Add(string.Format("         \"$type\" :\"{0}.{1}, workflows.{2}.{3}\"", this.CodeNamespace, name, this.WorkflowDefinitionId, this.Version));
            properties.Add("        \"WebId\": ko.observable(webid)");

            
            var ct = e.Element(x + "complexType");
            if (null == ct) return code.ToString();

            var attributes = from at in ct.Elements(x + "attribute")
                             let n = at.Attribute("name").Value
                             select string.Format("         \"{0}\" : ko.observable()", n);
            properties.AddRange(attributes);

            var all = ct.Element(x + "all");
            if (null != all)
            {
                //Console.WriteLine(ct.Elements(x + "element").Count());
                var allElements = from at in all.Elements(x + "element")
                                  where at.Attribute("name") != null
                                  && at.Attribute("type") != null
                                  select string.Format("            \"{0}\" : ko.observable()", at.Attribute("name").Value);
                properties.AddRange(allElements);

                var collectionElements = from at in all.Elements(x + "element")
                                         where at.Attribute("name") != null
                                         && at.Attribute("type") == null
                                         let refElement = at.Descendants(x + "element").First()
                                         select string.Format("         \"{0}\" : ko.observableArray()", at.Attribute("name").Value);
                properties.AddRange(collectionElements);

                var refElements = from at in all.Elements(x + "element")
                                  where at.Attribute("ref") != null
                                  let refa = at.Attribute("ref")
                                  select string.Format("            \"{0}\" : ko.observable(new bespoke.sph.w_{1}_{2}.{0}())", refa.Value, this.WorkflowDefinitionId,this.Version);
                properties.AddRange(refElements);




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

        private string GetClrDataType(XElement element)
        {

            var typeAttribute = element.Attribute("type");
            var nillableAttribute = element.Attribute("nillable");

            var xsType = typeAttribute != null ? typeAttribute.Value : "";
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

        private string GenerateCode()
        {
            var code = new StringBuilder();
            code.AppendLine("using " + typeof(Entity).Namespace + ";");
            code.AppendLine("using " + typeof(Int32).Namespace + ";");
            code.AppendLine("using " + typeof(Task<>).Namespace + ";");
            code.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            code.AppendLine("using " + typeof(XmlAttributeAttribute).Namespace + ";");
            code.AppendLine();

            code.AppendLine("namespace " + this.CodeNamespace);
            code.AppendLine("{");

            code.AppendLine("   [EntityType(typeof(Workflow))]");
            code.AppendLine("   public class " + this.WorkflowTypeName + " : " + typeof(Workflow).FullName);
            code.AppendLine("   {");

            // static ctor
            code.AppendLinf("       static {0}()", this.WorkflowTypeName);
            code.AppendLine("       {");  // register type for the XML serializer
            code.AppendLinf("           XmlSerializerService.RegisterKnownTypes(typeof({0}),typeof({1}));", typeof(Workflow).Name, this.WorkflowTypeName);

            code.AppendLine("       }");

            // contructor
            code.AppendLine("       public " + this.WorkflowTypeName + "()");
            code.AppendLine("       {");

            // default properties
            code.AppendLinf("           this.Name = \"{0}\";", this.Name);
            code.AppendLinf("           this.Version = {0};", this.Version);
            code.AppendLinf("           this.WorkflowDefinitionId = {0};", this.WorkflowDefinitionId);

            foreach (var variable in this.VariableDefinitionCollection.OfType<ComplexVariable>())
            {
                code.AppendLinf("           this.{0} = new {1}();", variable.Name, variable.TypeName);
            }
            code.AppendLine("       }");// end contructor

            // start
            code.AppendLine("       public override Task<ActivityExecutionResult> StartAsync()");
            code.AppendLine("       {");
            code.AppendLinf("           this.SerializedDefinitionStoreId = \"wd.{0}.{1}\";", this.WorkflowDefinitionId, this.Version);
            code.AppendLinf("           return this.{0}();", this.GetInititorScreen().MethodName);

            code.AppendLine("       }");
            // execute
            code.AppendLine("       public async override Task<ActivityExecutionResult> ExecuteAsync()");
            code.AppendLine("       {");
            code.AppendLine("               var act = this.GetCurrentActivity();");
            code.AppendLine("               if(null == act)");
            code.AppendLine("                   throw new InvalidOperationException(\"No current activity\");");



            code.AppendLine("               if(act.IsAsync && this.State == \"WaitingAsync\")");
            code.AppendLine("               {");
            code.AppendLine("                   return new ActivityExecutionResult{Status = ActivityExecutionStatus.WaitingAsync};");
            code.AppendLine("               }");



            code.AppendLine("               if(act.IsAsync)");
            code.AppendLine("               {");
            code.AppendLine("                   this.State = \"WaitingAsync\";");

            code.AppendLine("                   await act.InitiateAsync(this);");
            code.AppendLine("                   await this.SaveAsync(act.WebId);");
            code.AppendLine("                   return new ActivityExecutionResult{Status = ActivityExecutionStatus.WaitingAsync};");
            code.AppendLine("               }");
            code.AppendLine();
            code.AppendLine("               ActivityExecutionResult result = null;");
            code.AppendLine("               switch(act.WebId)");
            code.AppendLine("               {");

            foreach (var activity in this.ActivityCollection)
            {
                code.AppendLinf("                   case \"{0}\" : ", activity.WebId);
                code.AppendLinf("                       result = await this.{0}();", activity.MethodName);
                code.AppendLine("                       break;");
            }
            code.AppendLine("               }");// end switch

            code.AppendLine("               if(null == result)");
            code.AppendLine("                   throw new Exception(\"what ever\");");
            code.AppendLine("               if(null != result.NextActivity)");
            code.AppendLine("               {");
            code.AppendLine("                   this.CurrentActivityWebId = result.NextActivity.WebId;");
            code.AppendLine("                   await this.SaveAsync(act.WebId);");
            code.AppendLine("               }");
            code.AppendLine("                return result;");


            code.AppendLine("       }");// end Execute

            // properties for each Variables
            foreach (var variable in this.VariableDefinitionCollection)
            {
                code.AppendLine("       " + variable.GeneratedCode(this));
            }

            // activities method

            foreach (var activity in this.ActivityCollection)
            {
                code.AppendLine();
                code.AppendLine(activity.GeneratedExecutionMethodCode(this));
            }


            code.AppendLine("   }");// end class


            XNamespace x = "http://www.w3.org/2001/XMLSchema";
            var xsd = this.GetCustomSchema();

            var elements = xsd.Elements(x + "element").ToList();
            var customSchemaCode = elements.Select(this.GenerateXsdElementCsharpClass).ToList();
            customSchemaCode.ForEach(c => code.AppendLine(c));



            foreach (var activity in this.ActivityCollection)
            {
                code.AppendLine("   " + activity.GeneratedCustomTypeCode(this));
            }

            code.AppendLine("}");// end namespace
            return code.ToString();
        }

        public XElement GetCustomSchema(string id = null)
        {
            if (null != m_customSchema) return m_customSchema;

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var content = store.GetContent(id ?? this.SchemaStoreId);
            using (var stream = new MemoryStream(content.Content))
            {
                m_customSchema = XElement.Load(stream);

                return m_customSchema;
            }
        }

        public List<string> GetCustomSchemaElementNames(string name)
        {

            var xsd = this.GetCustomSchema();
            XNamespace x = "http://www.w3.org/2001/XMLSchema";
            var elements = xsd.Elements(x + "element").Select(e => e.Attribute("name").Value).ToList();
            return elements;


        }


    }
}