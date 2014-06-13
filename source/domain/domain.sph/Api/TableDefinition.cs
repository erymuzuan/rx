using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain.Api
{
    public partial class TableDefinition
    {
        private string GenerateController(Adapter adapter)
        {
            var record = this.MemberCollection.Single(m => m.Name == this.RecordName);
            var header = this.GetCodeHeader();
            var code = new StringBuilder(header);

            GenerateListAction(adapter, code);
            GenerateInsertAction(code);
            GenerateUpdateAction(code);
            GenerateDeleteAction(code, record);
            GenerateGetAction(code, record);

            code.AppendLine("   }");// end class

            code.AppendLine("}"); // end namespace
            return code.ToString();


        }

        private void GenerateGetAction(StringBuilder code, Member record)
        {
            // delete
            code.AppendLinf("       [Route(\"{{id:{0}}}\")]", record.Type.ToCSharp());
            code.AppendLinf("       [HttpGet]");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Get({0} id)", record.Type.ToCSharp());
            code.AppendLine("       {");
            code.AppendLinf(@"
            var context = new {0}Adapter();
            var item  =await context.LoadOneAsync(id);
            this.Response.ContentType = ""application/json"";
            this.Response.StatusCode = 200;
            if(null == item)
                return Content(JsonConvert.SerializeObject(new {{success = false, status = ""NotFound"", statusCode=404, url=""/api/docs/404"", message =""item not found""}}));
            return Content(JsonConvert.SerializeObject(new {{success = true, status = ""OK"", item}}));
", this.Name);
            code.AppendLine("       }");
        }

        private void GenerateDeleteAction(StringBuilder code, Member record)
        {
            // delete
            code.AppendLine("       [Route(\"{id:int}\")]");
            code.AppendLinf("       [HttpDelete]");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Remove({0} id)", record.Type.ToCSharp());
            code.AppendLine("       {");
            code.AppendLinf(@"
            var context = new {0}Adapter();
            await context.DeleteAsync(id);
            this.Response.ContentType = ""application/json"";
            this.Response.StatusCode = 202;
            return Json(new {{success = true, status=""OK"", id = id}});", this.Name);
            code.AppendLine("       }");
        }

        private void GenerateUpdateAction(StringBuilder code)
        {
            // update
            code.AppendLine("       [Route]");
            code.AppendLinf("       [HttpPut]");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Save([RequestBody]{0} item)", this.Name);
            code.AppendLine("       {");
            code.AppendLinf(@"
            if(null == item) throw new ArgumentNullException(""item"");
            var context = new {1}Adapter();
            await context.UpdateAsync(item);
            this.Response.ContentType = ""application/json; charset=utf-8"";
            this.Response.StatusCode = 202;
            return Json(new {{success = true, status=""OK"", id = item.{0}}});", this.RecordName, this.Name);
            code.AppendLine("       }");
        }

        private void GenerateInsertAction(StringBuilder code)
        {
            // insert
            code.AppendLine("       [Route]");
            code.AppendLinf("       [HttpPost]");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Insert([RequestBody]{0} item)", this.Name);
            code.AppendLine("       {");
            code.AppendLinf(@"
            if(null == item) throw new ArgumentNullException(""item"");
            var context = new {1}Adapter();
            await context.InsertAsync(item);
            this.Response.ContentType = ""application/json; charset=utf-8"";
            this.Response.StatusCode = 202;
            return Json(new {{success = true, status=""OK"", id = item.{0}}});", this.RecordName, this.Name);
            code.AppendLine("       }");
        }

        private void GenerateListAction(Adapter adapter, StringBuilder code)
        {
            code.AppendLinf("   [RoutePrefix(\"api/{0}/{1}\")]", this.Schema.ToLowerInvariant(), this.Name.ToLowerInvariant());
            code.AppendLinf("   public partial class {0}Controller : System.Web.Mvc.Controller", this.Name);
            code.AppendLine("   {");
            code.AppendLine("       [Route]");
            code.AppendLinf(
                "       public async Task<System.Web.Mvc.ActionResult> Index(string filter = null, int page = 1, int size = 40, bool includeTotal = false)");
            code.AppendLine("       {");
            code.AppendLinf(@"
           if (size > 200)
                throw new ArgumentException(""Your are not allowed to do more than 200"", ""size"");

            var orderby = this.Request.QueryString[""$orderby""];
            var translator = new {0}<{1}>(null,""{1}"" ){{Schema = ""{3}""}};
            var sql = translator.Select(string.IsNullOrWhiteSpace(filter) ? ""{2} gt 0"" : filter, orderby);
            var count = 0;

            var context = new {1}Adapter();
            var nextPageToken = string.Empty;
            var lo = await context.LoadAsync(sql, page, size);
            if (includeTotal || page > 1)
            {{
                var translator2 = new {0}<{1}>(""{0}Id"", ""{1}""){{Schema = ""{3}""}};
                var countSql = translator2.Count(filter);
                count = await context.ExecuteScalarAsync<int>(countSql);

                if (count >= lo.ItemCollection.Count())
                    nextPageToken = string.Format(
                        ""/api/{3}/{1}/?filer={{0}}&includeTotal=true&page={{1}}&size={{2}}"", filter, page + 1, size);
            }}

            string previousPageToken = DateTime.Now.ToShortTimeString();
            var json = new
            {{
                count,
                page,
                nextPageToken,
                previousPageToken,
                size,
                results = lo.ItemCollection.ToArray()
            }};
            var setting = new JsonSerializerSettings{{}};

            this.Response.ContentType = ""application/json"";
            return Content(JsonConvert.SerializeObject(json, Formatting.Indented, setting));
            ", adapter.OdataTranslator, this.Name, this.RecordName, this.Schema);


            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();
        }

        private string GetCodeHeader()
        {

            var header = new StringBuilder();
            header.AppendLine("using " + typeof(Entity).Namespace + ";");
            header.AppendLine("using " + typeof(Int32).Namespace + ";");
            header.AppendLine("using " + typeof(Task<>).Namespace + ";");
            header.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            header.AppendLine("using " + typeof(JsonConvert).Namespace + ";");
            header.AppendLine("using " + typeof(XmlAttributeAttribute).Namespace + ";");
            header.AppendLine("using System.Web.Mvc;");
            header.AppendLine("using Bespoke.Sph.Web.Helpers;");
            header.AppendLine();

            header.AppendLine("namespace " + this.CodeNamespace);
            header.AppendLine("{");
            return header.ToString();

        }

        public Dictionary<string, string> GenerateCode(Adapter adapter)
        {
            var header = this.GetCodeHeader();
            var code = new StringBuilder(header);

            code.AppendLine("   public class " + this.Name + ":DomainObject");
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

            var controller = this.GenerateController(adapter);
            sourceCodes.Add(this.Name + "Controller.cs", controller);


            return sourceCodes;
        }

        public string Name { get; set; }
        public string CodeNamespace { get; set; }

        public string WebId { get; set; }
    }
}