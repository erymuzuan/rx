using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize]
    [RoutePrefix("screen-activity-form")]
    public class ScreenActivityFormController : Controller
    {
        [ImportMany(FormCompilerMetadataAttribute.FORM_COMPILER_CONTRACT, typeof(FormCompiler), AllowRecomposition = true)]
        public Lazy<FormCompiler, IFormCompilerMetadata>[] Compilers { get; set; }


        [HttpGet]
        [Route("compilers")]
        public ActionResult GetAvailableCompilers()
        {
            ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.Compilers)
                throw new InvalidOperationException("Cannot instantiate FormCompilers");
            return Json(this.Compilers.Select(x => x.Metadata.Name).ToArray(), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Save()
        {
            var ef = this.GetRequestJson<ScreenActivityForm>();
            var context = new SphDataContext();

            if (string.IsNullOrWhiteSpace(ef.Id) || ef.Id == "0")
                ef.Id = ef.Route.ToIdFormat();

            using (var session = context.OpenSession())
            {
                session.Attach(ef);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", id = ef.Id });
        }

        [HttpPost]
        [Route("depublish")]
        public async Task<ActionResult> Depublish()
        {
            var context = new SphDataContext();
            var form = this.GetRequestJson<ScreenActivityForm>();

            form.IsPublished = false;
            using (var session = context.OpenSession())
            {
                session.Attach(form);
                await session.SubmitChanges("Depublish");
            }
            return Json(new { success = true, status = "OK", message = "Your form has been successfully depublished", id = form.Id });


        }

        [HttpPost]
        [Route("publish")]
        public async Task<ActionResult> Publish()
        {
            if(null == this.Compilers)
                ObjectBuilder.ComposeMefCatalog(this);

            var context = new SphDataContext();
            var form = this.GetRequestJson<ScreenActivityForm>();
            form.IsPublished = true;
            var ed = await context.LoadOneAsync<WorkflowDefinition>(e => e.Id == form.WorkflowDefinitionId);

            var buildValidation = await form.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return Json(buildValidation);
            var errors = "";
            var message = "";
            foreach (var name in form.CompilerCollection)
            {
                var name1 = name;
                var lazy = this.Compilers.SingleOrDefault(x => x.Metadata.Name == name1);
                if (null == lazy)
                    throw new InvalidOperationException("Cannot find compiler " + name);
                var compiler = lazy.Value;
                var result = await compiler.CompileAsync(form);
                errors += string.Join("\r\n", result.Errors.ToString());
                message +=string.Format("{2} to compile {0} with {1}\r\n{3}, \r\n", form.Name, name, result.Result ? "Successfully" : "Failed", errors);

            }
            using (var session = context.OpenSession())
            {
                session.Attach(form);
                await session.SubmitChanges("Publish");
            }
            return Json(new { success = true, status = "OK", errors, message, id = form.Id });

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Remove(string id)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<ScreenActivityForm>(e => e.Id == id);
            if (null == form)
                return new HttpNotFoundResult("Cannot find form to delete , Id : " + id);

            using (var session = context.OpenSession())
            {
                session.Delete(form);
                await session.SubmitChanges("Remove");
            }
            return Json(new { success = true, status = "OK", message = "Your form has been successfully deleted", id = form.Id });

        }
    }
}