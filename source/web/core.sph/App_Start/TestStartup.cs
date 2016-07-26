using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using Bespoke.Sph.Domain;
using Microsoft.Owin;

namespace Bespoke.Sph.Web.App_Start
{
    public class TestStartup
    {
        public void Configuration(IAppBuilder app)
        {
            Func<IOwinContext, Task> testHandler = async ctx =>
            {
                var file = AppDomain.CurrentDomain.BaseDirectory + @"\ScriptTests\index.html";
                var html = new StringBuilder(File.ReadAllText(file));

                var list = from f in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"\ScriptTests\", "test-*.js")
                           select $@" 
                    <li class="" style=""margin - top: 5px"" >
                        <a class=""btn btn-default"" href= ""?test={Path.GetFileNameWithoutExtension(f)}&"">
                        <i class=""fa fa-play pull-left""></i>
                        {Path.GetFileNameWithoutExtension(f)}
                        </a>
                    </ li >";
                var testFile = ctx.Request.Query["test"];
                var script = $@"<script src=""/ScriptTests/{testFile}.js""></script>"
                .Replace("/ScriptTests/?", "/ScriptTests/");

                html.Replace("<!-- list -->", list.ToString("\r\n"));
                if (!string.IsNullOrWhiteSpace(testFile))
                    html.Replace("<!-- test -->", script);
                ctx.Response.ContentType = "text/html";
                await ctx.Response.WriteAsync(html.ToString());
            };

            app.Run(testHandler);

        }


    }
}