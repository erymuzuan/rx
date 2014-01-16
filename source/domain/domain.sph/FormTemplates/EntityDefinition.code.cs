using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    public partial class EntityDefinition 
    {

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

            code.AppendLine("   public class " + this.Name + " : Entity");
            code.AppendLine("   {");

            // contructor
            code.AppendLine("       public " + this.Name + "()");
            code.AppendLine("       {");

            // default properties
            code.AppendLinf("           this.Name = \"{0}\";", this.Name);

            foreach (var member in this.MemberCollection.Where(t => t.TypeName == "System.Object, mscorlib"))
            {
                code.AppendLinf("           this.{0} = new {1}();", member.Name, member.Name);
            }
            code.AppendLine("       }");// end contructor




            // properties for each members
            foreach (var member in this.MemberCollection)
            {
                code.AppendLinf("//member:{0}", member.Name);
                code.AppendLine("       " + member.GeneratedCode());
            }

        


            code.AppendLine("   }");// end class
            // classes for members
            foreach (var member in this.MemberCollection)
            {
                code.AppendLinf("//class:{0}", member.Name);
                code.AppendLine(member.GeneratedCustomClass());
            }

            //var customSchemaCode = this.GenerateXsdCsharpClasses();
            //code.AppendLine(customSchemaCode);


            // search
            this.GenerateSearchController(code);
            this.GenerateJsSchemasController(code);

            code.AppendLine("}");// end namespace
            return code.ToString();
        }

        public string CodeNamespace
        {
            get { return string.Format("Bespoke.{0}_{1}.Domain", ConfigurationManager.ApplicationName, this.EntityDefinitionId); }
        }

        private void GenerateJsSchemasController(StringBuilder code)
        {
            code.AppendLinf("public partial class {0}Controller : System.Web.Mvc.Controller", this.Name);
            code.AppendLine("{");
            code.AppendLinf("//exec:Schemas");
            // custom schema
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Schemas()");
            code.AppendLine("       {");
            code.AppendLine("           var store = ObjectBuilder.GetObject<IBinaryStore>();");
            code.AppendLinf("           var doc = await store.GetContentAsync(\"wd.{0}\");", this.Name);
            code.AppendLine(@"          WorkflowDefinition wd;
                                        using (var stream = new System.IO.MemoryStream(doc.Content))
                                        {
                                            wd = stream.DeserializeFromXml<WorkflowDefinition>();
                                        }

                                        ");
            code.AppendLinf("           var script = await wd.GenerateCustomXsdJavascriptClassAsync();", this.WebId);
            code.AppendLine("           this.Response.ContentType = \"application/javascript\";");

            code.AppendLine("           return Content(script);");
            code.AppendLine("       }");
            code.AppendLine("   }");



        }

        public Task<string> GenerateCustomXsdJavascriptClassAsync()
        {
            var jsNamespace = new string(ConfigurationManager.ApplicationName.ToCamelCase().ToArray()) + "_" + this.EntityDefinitionId;
            var script = new StringBuilder();
            script.AppendLine("var bespoke = bespoke ||{};");
            script.AppendLinf("bespoke.{0} = bespoke.{0} ||{{}};", jsNamespace);
            script.AppendLinf("bespoke.{0}.domain = bespoke.{0}.domain ||{{}};", jsNamespace);

            //var xsd = wd.GetCustomSchema();

            //var complexTypesElement = xsd.Elements(x + "complexType").ToList();
            //var complexTypeClasses = complexTypesElement.Select(wd.GenerateXsdComplexTypeJavascript).ToList();
            //complexTypeClasses.ForEach(c => script.AppendLine(c));

            //var elements = xsd.Elements(x + "element").ToList();
            //var elementClasses = elements.Select(e => wd.GenerateXsdElementJavascript(e, 0, s => complexTypesElement.Single(f => f.Attribute("name").Value == s))).ToList();
            //elementClasses.ForEach(c => script.AppendLine(c));



            return Task.FromResult(script.ToString());
        }

        private void GenerateSearchController(StringBuilder code)
        {
            code.AppendLinf("public partial class {0}Controller : System.Web.Mvc.Controller", this.Name);
            code.AppendLine("{");
            code.AppendLinf("//exec:Search");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Search()");
            code.AppendLine("       {");
            code.AppendLinf(@"
            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = string.Format(""{{0}}/{{1}}/{0}/_search"", ConfigurationManager.ElasticSearchHost, ConfigurationManager.ElasticSearchIndex );

            var client = new System.Net.Http.HttpClient();
            var response = await client.PostAsync(url, request);
            var content = response.Content as System.Net.Http.StreamContent;
            if (null == content) throw new Exception(""Cannot execute query on es "" + request);
            this.Response.ContentType = ""application/json; charset=utf-8"";
            return Content(await content.ReadAsStringAsync());", this.Plural.ToLower());
            code.AppendLine("       }");
            code.AppendLine("}");



        }



    }
}
