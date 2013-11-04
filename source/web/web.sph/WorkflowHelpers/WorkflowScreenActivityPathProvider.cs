using System.Text;
using System.Web.Hosting;

namespace Bespoke.Sph.Web.WorkflowHelpers
{
    public class WorkflowScreenActivityPathProvider : VirtualPathProvider
    {

        public override bool FileExists(string virtualPath)
        {
            var page = FindPage(virtualPath);
            if (page == null)
            {
                return base.FileExists(virtualPath);
            }
            return true;
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            dynamic page = FindPage(virtualPath);
            if (page == null)
            {
                return base.GetFile(virtualPath);
            }
            return new ScreenActivityVirtualFile(virtualPath, page.ViewData);
        }

        private object FindPage(string virtualPath)
        {
            const string razor = @"@using System.Web.Mvc.Html
@using Bespoke.Sph.Domain
@model Bespoke.Sph.Web.Controllers.WorkflowStartViewModel

@{
    ViewBag.Title =""test 1234"";
    Layout = ""~/Views/Shared/_LayoutWorkflow.cshtml"";
}
<h1> test 1234 @DateTime.Now</h1>
<div>
       @foreach (var fe in Model.Screen.FormDesign.FormElementCollection)
        {
            <div>TO DO : generate this razor for the given workflowDefinition and screen</div>
            @Html.EditorFor(f => fe)
        }
</div>
";
            if (virtualPath.Contains("workflow") && virtualPath.Contains("start") && virtualPath.EndsWith(".cshtml"))
                return new { Name = virtualPath, ViewData = Encoding.UTF8.GetBytes(razor) };
            return null;
        }
    }
}