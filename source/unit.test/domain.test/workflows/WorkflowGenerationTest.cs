using System;
using System.CodeDom.Compiler;
using Bespoke.Sph.Domain;
using Microsoft.CSharp;
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
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "test", TypeName = "Test" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "alamat", TypeName = "Address" });


            var screen = new ScreenActivity { Title = "Pohon" };
            screen.FormDesign.FormElementCollection.Add(new TextBox { Path = "Nama", Label = "Test" });
            screen.FormDesign.FormElementCollection.Add(new TextBox { Path = "Title", Label = "Tajuk" });
            wd.ActivityCollection.Add(screen);

            var code = wd.GenerateCode();
            wd.Version = System.IO.Directory.GetFiles(".", "workflows.8.*.dll").Length + 1;
            Console.WriteLine(code);

            using (var provider = new CSharpCodeProvider())
            {
                var options = new CompilerParameters
                {
                    OutputAssembly = string.Format("workflows.{0}.{1}.dll", wd.WorkflowDefinitionId, wd.Version),
                    GenerateExecutable = false

                };
                options.ReferencedAssemblies.Add("domain.sph.dll");
                options.ReferencedAssemblies.Add("System.dll");
                options.ReferencedAssemblies.Add(@"D:\project\work\sph\packages\Microsoft.AspNet.Mvc.5.0.0\lib\net45\System.Web.Mvc.dll");

                var result = provider.CompileAssemblyFromSource(options, code);
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error);
                }
                Console.WriteLine(result.Errors);
            }
            /*
             * namespace Bespoke.Sph.Workflow_8
             * {
             *      public class Permohonan_Tanah_Wakaf
             *      {
             *          public string Title{get;set;}
             *      }
             *      
             *      public class Workflow8_PohonController : System.Web.Mvc.Controller
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