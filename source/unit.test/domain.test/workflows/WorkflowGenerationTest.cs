using System;
using System.Reflection;
using System.Reflection.Emit;
using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class WorkflowGenerationTest
    {
        [Test]
        public void Generate()
        {

            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", WorkflowDefinitionId = 8, SchemaStoreId = "cd6a8751-ceed-4805-a200-02a193b651e0" };
            wd.VariableDefinitionCollection.Add(new SimpleVariable{Name = "Title", Type = typeof(string)});
            wd.VariableDefinitionCollection.Add(new ComplexVariable{Name = "alamat", TypeName = "Address"});
            wd.VariableDefinitionCollection.Add(new ComplexVariable{Name = "test", TypeName = "Test"});

            var screen = new ScreenActivity {Title = "Pohon"};
            screen.FormDesign.FormElementCollection.Add(new TextBox{Path = "Nama",Label = "Test"});
            screen.FormDesign.FormElementCollection.Add(new TextBox{Path = "Title",Label = "Tajuk"});

            wd.ActivityCollection.Add(screen);
            Console.WriteLine(wd.GenerateCode());

            /*
             * namespace Bespoke.Sph.Workflow_8
             * {
             *      public class Permohonan_Tanah_Wakaf
             *      {
             *          public string Title{get;set;}
             *      }
             *      
             *      public class PohonController : System.Web.Mvc.Controller
             *      {
             *          public ActionResult Index()
             *          {
             *              var html = "<input/>";   
             *              return Content(html);
             *          }
             *      }
             *      
             * }
             * 
             **/

        }
    }
}