using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain.Api
{
    public partial class TableDefinition
    {
        private string GenerateController()
        {
            var header = this.GetCodeHeader();
            var code = new StringBuilder(header);

            code.AppendLinf("   public partial class {0}Controller : System.Web.Mvc.Controller", this.Name);
            code.AppendLine("   {");
            code.AppendLinf("       //exec:Search");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Search()");
            code.AppendLine("       {");
            code.AppendFormat(@"
            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = ""{1}/{0}/_search"";

            using(var client = new System.Net.Http.HttpClient())
            {{
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.PostAsync(url, request);
                var content = response.Content as System.Net.Http.StreamContent;
                if (null == content) throw new Exception(""Cannot execute query on es "" + request);
                this.Response.ContentType = ""application/json; charset=utf-8"";
                return Content(await content.ReadAsStringAsync());
            }}
            ", this.Name.ToLower(), ConfigurationManager.ApplicationName.ToLower());
            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();

            // SAVE
            code.AppendLinf("       //exec:Save");
            code.AppendLinf("       [HttpPut]");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Insert([RequestBody]{0} item)", this.Name);
            code.AppendLine("       {");
            code.AppendLinf(@"
            if(null == item) throw new ArgumentNullException(""item"");
            var context = new {1}Adapter();
            await context.InsertAsync(item);
            this.Response.ContentType = ""application/json; charset=utf-8"";
            this.Response.StatusCode = 202;
            return Json(new {{success = true, status=""OK"", id = item.{0}}});", this.RecordName,this.Name);
            code.AppendLine("       }");


            // SAVE
            code.AppendLinf("       //exec:Save");
            code.AppendLinf("       [HttpPost]");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Save([RequestBody]{0} item)", this.Name);
            code.AppendLine("       {");
            code.AppendLinf(@"
            if(null == item) throw new ArgumentNullException(""item"");
            var context = new {1}Adapter();
            await context.UpdateAsync(item);
            this.Response.ContentType = ""application/json; charset=utf-8"";
            this.Response.StatusCode = 202;
            return Json(new {{success = true, status=""OK"", id = item.{0}}});", this.RecordName,this.Name);
            code.AppendLine("       }");


            // REMOVE
            var record = this.MemberCollection.Single(m => m.Name == this.RecordName);
            code.AppendLinf("       //exec:Remove");
            code.AppendLinf("       [HttpDelete]");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Remove({0} id)", record.Type.ToCSharp());
            code.AppendLine("       {");
            code.AppendLinf(@"
            // TODO
            var context = new {0}Adapter();
            await context.DeleteAsync(id);
            this.Response.ContentType = ""application/json; charset=utf-8"";
            this.Response.StatusCode = 202;
            return Json(new {{success = true, status=""OK"", id = id}});", this.Name);
            code.AppendLine("       }");

         

   

            code.AppendLine("   }");// end class

            code.AppendLine("}"); // end namespace
            return code.ToString();


        }


        private string GetCodeHeader()
        {

            var header = new StringBuilder();
            header.AppendLine("using " + typeof(Entity).Namespace + ";");
            header.AppendLine("using " + typeof(Int32).Namespace + ";");
            header.AppendLine("using " + typeof(Task<>).Namespace + ";");
            header.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            header.AppendLine("using " + typeof(XmlAttributeAttribute).Namespace + ";");
            header.AppendLine("using System.Web.Mvc;");
            header.AppendLine("using Bespoke.Sph.Web.Helpers;");
            header.AppendLine();

            header.AppendLine("namespace " + this.CodeNamespace);
            header.AppendLine("{");
            return header.ToString();

        }

        public Dictionary<string, string> GenerateCode()
        {
            var header = this.GetCodeHeader();
            var code = new StringBuilder(header);

            code.AppendLine("   public class " + this.Name + "");
            code.AppendLine("   {");





            code.AppendFormat(@"     
        public override string ToString()
        {{
            return ""{0}:"" + {1};
        }}", this.Name, this.RecordName);


            // properties for each members
            foreach (var member in this.MemberCollection)
            {
                code.AppendLinf("       //member:{0}", member.Name);
                code.AppendLine(member.GeneratedCode());
            }


            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace

            var sourceCodes = new Dictionary<string, string> { { this.Name + ".cs", code.ToString() } };

            // classes for members
            foreach (var member in this.MemberCollection.Where(m => m.Type == typeof(object) || m.Type == typeof(Array)))
            {
                var mc = header + member.GeneratedCustomClass() + "\r\n}";
                sourceCodes.Add(member.Name + ".cs", mc);
            }

            var controller = this.GenerateController();
            sourceCodes.Add(this.Name + "Controller.cs", controller);


            return sourceCodes;
        }

        public string Name { get; set; }
        public string CodeNamespace { get; set; }


        public string WebId { get; set; }
    }
}