using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Filters
{
    [AttributeUsage( AttributeTargets.Method)]
    public sealed class RxSourceOutputCacheAttribute : OutputCacheAttribute
    {
        public Type SourceType { get; set; }
        public RxSourceOutputCacheAttribute()
        {
            this.CacheProfile = "Long";
            this.VaryByParam = "filter;page;size";
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var fileDependencies = new List<string>();

            var sourceFolder = $"{ConfigurationManager.SphSourceDirectory}\\{SourceType.Name}";
            var files = Directory.GetFiles(sourceFolder, "*.json");
                fileDependencies.AddRange(files);
           

            if (fileDependencies.Any())
                filterContext.HttpContext.Response.AddFileDependencies(fileDependencies.ToArray());

            base.OnActionExecuting(filterContext);
        }
    }
}