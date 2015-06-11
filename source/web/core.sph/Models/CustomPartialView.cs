using Bespoke.Sph.Domain;
using static System.IO.File;

namespace Bespoke.Sph.Web.Models
{
    public static class CustomFormHelpers
    {

        public static string GetHtmlDiff(this JsRoute view, string folder)
        {
            var name = view.ModuleId.Replace("viewmodels/", "");
            var mineHtml = $"{ConfigurationManager.WebPath}\\SphApp\\views\\{name}.html";
            var theirHtml = $"{folder}\\SphApp\\views\\{name}.html";
            if (!Exists(mineHtml) && Exists(theirHtml))
                return "added";
            if (Exists(mineHtml) && Exists(theirHtml) && ReadAllText(mineHtml) != ReadAllText(theirHtml))
                return "changed";
            if (Exists(mineHtml) && !Exists(theirHtml))
                return "deleted";

            return null;
        }

        public static string GetJsDiff(this JsRoute view, string folder)
        {
            var name = view.ModuleId.Replace("viewmodels/", "");
            var mineJs = $"{ConfigurationManager.WebPath}\\SphApp\\viewmodels\\{name}.js";
            var theirJs = $"{folder}\\SphApp\\viewmodels\\{name}.js";
            if (!Exists(mineJs) && Exists(theirJs))
                return "added";
            if (Exists(mineJs) && !Exists(theirJs))
                return "deleted";

            if (Exists(mineJs) && Exists(theirJs) && ReadAllText(mineJs) != ReadAllText(theirJs))
                return "changed";


            return null;
        }

        public static string GetHtmlDiff(this CustomPartialView view, string folder)
        {
            var mineHtml = $"{ConfigurationManager.WebPath}\\SphApp\\views\\{view.Name}.html";
            var theirHtml = $"{folder}\\SphApp\\views\\{view.Name}.html";
            if (!Exists(mineHtml) && Exists(theirHtml))
                return "added";
            if (Exists(mineHtml) && Exists(theirHtml) && ReadAllText(mineHtml) != ReadAllText(theirHtml))
                return "changed";
            if (Exists(mineHtml) && !Exists(theirHtml))
                return "deleted";

            return view.HtmlDiff;
        }

        public static string GetJsDiff(this CustomPartialView view, string folder)
        {
            var mineJs = $"{ConfigurationManager.WebPath}\\SphApp\\viewmodels\\{view.Name}.js";
            var theirJs = $"{folder}\\SphApp\\viewmodels\\{view.Name}.js";
            if (!Exists(mineJs) && Exists(theirJs))
                return "added";
            if (Exists(mineJs) && !Exists(theirJs))
                return "deleted";

            if (Exists(mineJs) && Exists(theirJs) && ReadAllText(mineJs) != ReadAllText(theirJs))
                return "changed";


            return view.JsDiff;
        }

        public static string GetHtmlDiff(this CustomDialog dlg, string folder)
        {
            var mineHtml = $"{ConfigurationManager.WebPath}\\SphApp\\views\\{dlg.Name}.html";
            var theirHtml = $"{folder}\\SphApp\\views\\{dlg.Name}.html";
            if (!Exists(mineHtml) && Exists(theirHtml))
                return "added";
            if (Exists(mineHtml) && Exists(theirHtml) && ReadAllText(mineHtml) != ReadAllText(theirHtml))
                return "changed";
            if (Exists(mineHtml) && !Exists(theirHtml))
                return "deleted";

            return dlg.HtmlDiff;
        }

        public static string GetJsDiff(this CustomDialog dlg, string folder)
        {
            var mineJs = $"{ConfigurationManager.WebPath}\\SphApp\\viewmodels\\{dlg.Name}.js";
            var theirJs = $"{folder}\\SphApp\\viewmodels\\{dlg.Name}.js";
            if (!Exists(mineJs) && Exists(theirJs))
                return "added";
            if (Exists(mineJs) && !Exists(theirJs))
                return "deleted";

            if (Exists(mineJs) && Exists(theirJs) && ReadAllText(mineJs) != ReadAllText(theirJs))
                return "changed";


            return dlg.JsDiff;
        }
    }
    public class CustomDialog
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string JsDiff { get; set; }
        public string HtmlDiff { get; set; }


    }
    public class CustomScript
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string JsDiff { get; set; }

        public string GetDiff(string folder)
        {
            var mine = $"{ConfigurationManager.WebPath}\\SphApp\\services\\{this.Name}.js";
            var theirs = $"{folder}\\SphApp\\services\\{this.Name}.js";
            if (!Exists(mine) && Exists(theirs))
                return "added";
            if (Exists(mine) && !Exists(theirs))
                return "deleted";
            if (ReadAllText(mine) != ReadAllText(theirs))
                return "changed";

            return null;
        }
    }
    public class CustomPartialView
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public bool UseViewModel { get; set; }
        public string JsDiff { get; set; }
        public string HtmlDiff { get; set; }
    }
}