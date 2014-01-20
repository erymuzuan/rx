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

            code.AppendLinf("   private int m_{0}Id;",  this.Name.ToCamelCase());
            code.AppendLinf("   public int {0}Id", this.Name);
            code.AppendLine("   {");
            code.AppendLinf("       get{{ return m_{0}Id;}}", this.Name.ToCamelCase());
            code.AppendLinf("       set{{ m_{0}Id = value;}}", this.Name.ToCamelCase());
            code.AppendLine("   }");

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

            // search
            this.GenerateSearchController(code);


            code.AppendLine("}");// end namespace
            return code.ToString();
        }

        public string CodeNamespace
        {
            get { return string.Format("Bespoke.{0}_{1}.Domain", ConfigurationManager.ApplicationName, this.EntityDefinitionId); }
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

            // SAVE
            code.AppendLinf("//exec:Save");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Save()");
            code.AppendLine("       {");
            code.AppendLinf(@"
            var item = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<{0}>(this);
            var context = new Bespoke.Sph.Domain.SphDataContext();
            using(var session = context.OpenSession())
            {{
                session.Attach(item);
                await session.SubmitChanges(""save"");
            }}
            this.Response.ContentType = ""application/json; charset=utf-8"";
            return Json(new {{success = true, status=""OK"", id = item.{0}Id}});", this.Name);
            code.AppendLine("       }");

            //SCHEMAS

            code.AppendLinf("//exec:Schemas");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Schemas()");
            code.AppendLine("       {");
            code.AppendLine("           var context = new SphDataContext();");
            code.AppendLinf("           var ed = await context.LoadOneAsync<EntityDefinition>(t => t.Name == \"{0}\");", this.Name);

            code.AppendLine("           var script = await ed.GenerateCustomXsdJavascriptClassAsync();");
            code.AppendLine("           this.Response.ContentType = \"application/javascript\";");

            code.AppendLine("           return Content(script);");
            code.AppendLine("       }");

            code.AppendLine("}");

        }



    }
}
