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

            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", WorkflowDefinitionId = 8};
            wd.VariableDefinitionCollection.Add(new SimpleVariable{Name = "Title", Type = typeof(string)});


            var screen = new ScreenActivity {Title = "Pohon"};
            screen.FormDesign.FormElementCollection.Add(new TextBox{Path = "Nama",Label = "Test"});

            
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