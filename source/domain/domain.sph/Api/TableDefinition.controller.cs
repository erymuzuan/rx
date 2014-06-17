using System;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    public partial class TableDefinition
    {

        private string GenerateController(Adapter adapter)
        {
            var header = this.GetCodeHeader();
            var code = new StringBuilder(header);

            GenerateListAction(adapter, code);
            code.AppendLine(GenerateInsertAction());
            code.AppendLine(GenerateUpdateAction());

            code.AppendLine(GenerateDeleteAction());
            code.AppendLine(GenerateGetAction());
            foreach (var child in this.ChildTableCollection)
            {
                //GenerateChildListAction(adapter, code, child);
            }



            code.AppendLine("   }");// end class

            code.AppendLine("}"); // end namespace
            return code.ToString();


        }


        private string GetRouteConstraint(Type type)
        {
            if (type == typeof(string)) return string.Empty;
            if (type == typeof(short)) return ":int";
            return ":" + type.ToCSharp();
        }

        private string GenerateGetAction()
        {
            var pks = this.MemberCollection.Where(m => this.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var routeConstraint = pks.Select(m => "{" + m.Name + this.GetRouteConstraint(m.Type) + "}");
            var arguments = pks.Select(m => m.Type.ToCSharp() + " " + m.Name);
            var parameters = pks.Select(m => m.Name);


            var code = new StringBuilder();

            code.AppendLinf("       [Route(\"{{{0}}}\")]", string.Join("/", routeConstraint));
            code.AppendLinf("       [HttpGet]");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Get({0})", string.Join(",", arguments));
            code.AppendLine("       {");
            code.AppendLinf(@"
            var context = new {0}Adapter();
            var item  =await context.LoadOneAsync({1});
            this.Response.ContentType = ""application/json"";
            this.Response.StatusCode = 200;
            if(null == item)
                return Content(JsonConvert.SerializeObject(new {{success = false, status = ""NotFound"", statusCode=404, url=""/api/docs/404"", message =""item not found""}}));
            return Content(JsonConvert.SerializeObject(new {{success = true, status = ""OK"", item}}));
", this.Name, string.Join(",", parameters));
            code.AppendLine("       }");

            return code.ToString();
        }

        private string GenerateDeleteAction()
        {
            var code = new StringBuilder();
            var pks = this.MemberCollection.Where(m => this.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var routeConstraint = pks.Select(m => "{" + m.Name + this.GetRouteConstraint(m.Type) + "}");
            var arguments = pks.Select(m => m.Type.ToCSharp() + " " + m.Name);
            var parameters = pks.Select(m => m.Name);
            code.AppendLinf("       [Route(\"{0}\")]", string.Join("/", routeConstraint.ToArray()));
            code.AppendLinf("       [HttpDelete]");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Remove({0})", string.Join(",", arguments));
            code.AppendLine("       {");
            code.AppendFormat(@"
            var context = new {0}Adapter();
            await context.DeleteAsync({1});
            this.Response.ContentType = ""application/json"";
            this.Response.StatusCode = 202;
            return Json(new {{success = true, status=""OK""}});", this.Name, string.Join(",", parameters));
            code.AppendLine("       }");
            return code.ToString();
        }

        private string GenerateUpdateAction()
        {
            var code = new StringBuilder();

            // update
            code.AppendLine("       [Route]");
            code.AppendLinf("       [HttpPut]");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Save([RequestBody]{0} item)", this.Name);
            code.AppendLine("       {");
            code.AppendLinf(@"
            if(null == item) throw new ArgumentNullException(""item"");
            var context = new {0}Adapter();
            await context.UpdateAsync(item);
            this.Response.ContentType = ""application/json; charset=utf-8"";
            this.Response.StatusCode = 202;
            return Json(new {{success = true, status=""OK""}});", this.Name);
            code.AppendLine("       }");

            return code.ToString();
        }

        private string GenerateInsertAction()
        {
            var code = new StringBuilder();
            // insert
            code.AppendLine("       [Route]");
            code.AppendLinf("       [HttpPost]");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Insert([RequestBody]{0} item)", this.Name);
            code.AppendLine("       {");
            code.AppendFormat(@"
            if(null == item) throw new ArgumentNullException(""item"");
            var context = new {0}Adapter();
            await context.InsertAsync(item);
            this.Response.ContentType = ""application/json; charset=utf-8"";
            this.Response.StatusCode = 202;
            return Json(new {{success = true, status=""OK"", item}});", this.Name);
            code.AppendLine("       }");

            return code.ToString();
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
            code.AppendFormat(@"
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
                var translator2 = new {0}<{1}>(null, ""{1}""){{Schema = ""{3}""}};
                var countSql = translator2.Count(filter);
                count = await context.ExecuteScalarAsync<int>(countSql);

                if (count >= lo.ItemCollection.Count())
                    nextPageToken = string.Format(
                        ""/api/{5}/{4}/?filer={{0}}&includeTotal=true&page={{1}}&size={{2}}"", filter, page + 1, size);
            }}

            string previousPageToken = string.Format(""/api/{5}/{4}/?filer={{0}}&includeTotal=true&page={{1}}&size={{2}}"", filter, page - 1, size);
            if(page == 1)
                previousPageToken = null;
            var json = new
            {{
                count,
                page,
                nextPageToken,
                previousPageToken,
                size,
                results = lo.ItemCollection.ToArray()
            }};
            var setting = new JsonSerializerSettings();
            setting.Converters.Add(new StringEnumConverter());

            this.Response.ContentType = ""application/json"";
            return Content(JsonConvert.SerializeObject(json, Formatting.Indented, setting));
            ", adapter.OdataTranslator, this.Name, this.PrimaryKey, this.Schema, this.Name.ToLowerInvariant(), this.Schema.ToLowerInvariant());


            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();
        }

        private void GenerateChildListAction(Adapter adapter, StringBuilder code, Member record, TableDefinition child)
        {

            code.AppendFormat("       [Route(\"{{id{0}}}/{1}\")]", this.GetRouteConstraint(record.Type), child.Name);
            code.AppendLinf(
                "       public async Task<System.Web.Mvc.ActionResult> List{0}(string filter = null, int page = 1, int size = 40, bool includeTotal = false)",
                child.Name);
            code.AppendLine("       {");
            code.AppendFormat(@"
           if (size > 200)
                throw new ArgumentException(""Your are not allowed to do more than 200"", ""size"");

            var orderby = this.Request.QueryString[""$orderby""];
            var translator = new {0}<{1}>(null,""{1}"" ){{Schema = ""{2}""}};
            var sql = ""SELECT * FROM {2}.{1} WHERE {4} = {4}"";
            var count = 0;

            var context = new {1}Adapter();
            var nextPageToken = string.Empty;
            var lo = await context.LoadAsync(sql, page, size);
            if (includeTotal || page > 1)
            {{
                var translator2 = new {0}<{1}>(null, ""{1}""){{Schema = ""{2}""}};
                var countSql = translator2.Count(filter);
                count = await context.ExecuteScalarAsync<int>(countSql);

                if (count >= lo.ItemCollection.Count())
                    nextPageToken = string.Format(
                        ""/api/{4}/{3}/?filer={{0}}&includeTotal=true&page={{1}}&size={{2}}"", filter, page + 1, size);
            }}

            string previousPageToken = string.Format(""/api/{4}/{3}/?filer={{0}}&includeTotal=true&page={{1}}&size={{2}}"", filter, page - 1, size);
            if(page == 1)
                previousPageToken = null;
            var json = new
            {{
                count,
                page,
                nextPageToken,
                previousPageToken,
                size,
                results = lo.ItemCollection.ToArray()
            }};
            var setting = new JsonSerializerSettings();
            setting.Converters.Add(new StringEnumConverter());

            this.Response.ContentType = ""application/json"";
            return Content(JsonConvert.SerializeObject(json, Formatting.Indented, setting));
            ", adapter.OdataTranslator, this.Name, this.Schema, this.Name.ToLowerInvariant(), this.Schema.ToLowerInvariant());


            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();
        }
    }
}