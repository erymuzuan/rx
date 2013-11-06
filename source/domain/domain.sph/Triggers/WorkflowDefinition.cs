using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.CSharp;
using Newtonsoft.Json;

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




        private string GenerateXsdElementClass(XElement e, int level = 0)
        {
            var properties = new List<string>();

            XNamespace x = "http://www.w3.org/2001/XMLSchema";
            var code = new StringBuilder();
            var name = e.Attribute("name").Value;
            var xsd = this.GetCustomSchema();
            var ns = xsd.Attribute("targetNamespace");

            code.AppendLinf("    [XmlType(\"{0}\",  Namespace=\"{1}\")]", name, ns != null ? ns.Value : "");
            code.AppendLinf("    public partial class {0} : DomainObject", name);
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
                                  select string.Format("      public {1} {0} {{get;set;}}", at.Attribute("name").Value, this.GetClrDataType(at));
                properties.AddRange(allElements);

                var refElements = from at in all.Elements(x + "element")
                                  where at.Attribute("ref") != null
                                  select "      public " + at.Attribute("ref").Value + " " + at.Attribute("ref").Value + " {get;set;}";
                properties.AddRange(refElements);



                var refInitializers = from at in all.Elements(x + "element")
                                      where at.Attribute("ref") != null
                                      select "      this." + at.Attribute("ref").Value + " = new " + at.Attribute("ref").Value + "();";
                // contructor should be created for ref
                code.AppendLinf("       public {0}()", name);
                code.AppendLine("           {");
                refInitializers.ToList().ForEach(c => code.AppendLine(c));
                code.AppendLine("           }");

            }


            code.AppendLine(string.Join("\r\n", properties));
            code.AppendLine("   }");
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

        public string Compile(params  string[] referencedAssemblies)
        {
            var code = this.GeneratreCode();
            using (var provider = new CSharpCodeProvider())
            {
                var options = new CompilerParameters
                {
                    OutputAssembly = string.Format("workflows.{0}.{1}.dll", this.WorkflowDefinitionId, this.Version),
                    GenerateExecutable = false,
                    IncludeDebugInformation = true

                };

                options.ReferencedAssemblies.Add(typeof(Entity).Assembly.Location);
                options.ReferencedAssemblies.Add(typeof(Int32).Assembly.Location);
                options.ReferencedAssemblies.Add(typeof(INotifyPropertyChanged).Assembly.Location);
                options.ReferencedAssemblies.Add(typeof(Expression<>).Assembly.Location);
                options.ReferencedAssemblies.Add(typeof(XmlAttributeAttribute).Assembly.Location);
                foreach (var ass in referencedAssemblies)
                {
                    options.ReferencedAssemblies.Add(ass);
                }

                var result = provider.CompileAssemblyFromSource(options, code);

                var errors = new StringBuilder();
                foreach (var er in result.Errors)
                {
                    errors.AppendLine(er.ToString());
                }
                if (result.Errors.HasErrors)
                    throw new Exception("Cannot compile see error \r\n" + errors);

                return Path.GetFullPath(options.OutputAssembly);
            }
        }
        private string GeneratreCode()
        {
            var code = new StringBuilder();
            code.AppendLine("using " + typeof(Entity).Namespace + ";");
            code.AppendLine("using " + typeof(Int32).Namespace + ";");
            code.AppendLine("using " + typeof(Task<>).Namespace + ";");
            code.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            code.AppendLine("using " + typeof(XmlAttributeAttribute).Namespace + ";");

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

            // execute
            code.AppendLine("       public async override Task<ActivityExecutionResult> ExecuteAsync()");
            code.AppendLine("       {");
            code.AppendLine("               var act = this.GetCurrentActivity();");
            code.AppendLine("               if(null == act)");
            code.AppendLine("                   throw new InvalidOperationException(\"No current activity\");");
            code.AppendLine("               if(act.IsAsync)");
            code.AppendLine("               {");
            code.AppendLine("                   this.State = \"WaitingAsync\";");
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
            var customSchemaCode = elements.Select(this.GenerateXsdElementClass).ToList();
            customSchemaCode.ForEach(c => code.AppendLine(c));



            foreach (var activity in this.ActivityCollection)
            {
                code.AppendLine("   " + activity.GeneratedCustomTypeCode(this));
            }

            code.AppendLine("}");// end namespace
            Debug.WriteLine(code);
            return code.ToString();
        }



        private XElement m_customSchema;
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

        [XmlIgnore]
        [JsonIgnore]
        public string WorkflowTypeName
        {
            get
            {
                return this.Name.Replace(" ", string.Empty);
            }
        }
        [XmlIgnore]
        [JsonIgnore]
        public string CodeNamespace
        {
            get
            {
                return "Bespoke.Sph.Workflows_" + this.WorkflowDefinitionId + "_" + this.Version;
            }
        }

        public Activity GetNextActivity(string activityId)
        {
            return new DecisionActivity
            {
                WebId = Guid.NewGuid().ToString()

            };
        }
    }
}