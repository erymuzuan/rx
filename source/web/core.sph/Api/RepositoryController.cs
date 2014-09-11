using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Api
{
    [Authorize]
    public class RepositoryController : Controller
    {
        private readonly string m_connectionString;

        public RepositoryController()
        {
            m_connectionString = ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;
        }


        public async Task<ActionResult> Delete(int id, string type)
        {
            if (id == 0 || string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("test", "type");


            if (id == 0) throw new ArgumentException("id is zero", "id");
            var assembly = Assembly.GetAssembly(typeof(DomainObject));
            var domainType = assembly.GetType(type);
            var sql = string.Format("DELETE FROM [Sph].[{0}] WHERE [{0}Id] = {1}", domainType.Name, id);
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                var rows = await cmd.ExecuteNonQueryAsync();
                return Content(rows > 0 ? "true" : "false");
            }

        }


        [ValidateInput(false)]
        public async Task<ActionResult> Save()
        {
            var xml = XElement.Load(this.Request.InputStream);
            var changes = xml.Deserialize<ChangeSubmission>();

            var persistent = ObjectBuilder.GetObject<IPersistence>();
            var so = await persistent.SubmitChanges(changes.ChangedCollection, changes.DeletedCollection, null);

            var webids = changes.ChangedCollection.Select(e => e.WebId).ToArray();

            var list = from d in webids
                       let id = so.Get(d)
                       where !string.IsNullOrWhiteSpace(id)
                       select new
                                  {
                                      __webid = d,
                                      __id = id
                                  };
            dynamic result = new { __count = so.RowsAffected, __results = list.ToArray() };
            return Json(result);


        }

    }
}
