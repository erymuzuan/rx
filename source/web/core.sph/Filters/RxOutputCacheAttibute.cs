using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class RxOutputCacheAttibute : OutputCacheAttribute
    {
        public Type[] DirectoryDependencies { get; set; }
        public string[] FileDependencies { get; set; }
        public string FilePattern { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.IsDebuggingEnabled)
            {
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                filterContext.HttpContext.Response.Cache.SetNoStore();

                return;
            }
            var fileDependencies = new List<string>();
            if (null != this.DirectoryDependencies)
            {
                var files = from dir in this.DirectoryDependencies
                            select Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\{dir.Name}", "*.json");
                fileDependencies.AddRange(files.SelectMany(x => x.ToArray()));

            }
            if (null != this.FileDependencies)
            {
                fileDependencies.AddRange(this.FileDependencies);
            }

            if (fileDependencies.Any())
                filterContext.HttpContext.Response.AddFileDependencies(fileDependencies.ToArray());

            base.OnActionExecuting(filterContext);
        }
    }
}