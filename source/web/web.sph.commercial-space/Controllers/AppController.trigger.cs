using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {

        public ActionResult TriggerSetupHtml()
        {
            return View();
        }

        public ActionResult TriggerPathPickerJson(string id)
        {
            var type = Type.GetType(string.Format("Bespoke.SphCommercialSpaces.Domain.{0}, domain.commercialspace", id));

            var text = new List<string>();
            this.BuildFlatJsonTreeView(text, "", type);
            this.Response.ContentType = APPLICATION_JAVASCRIPT;
            return Content("[" + string.Join(",", text) + "]");
        }


        public ActionResult TriggerPathPickerHtml(string id)
        {
            var type = Type.GetType(string.Format("Bespoke.SphCommercialSpaces.Domain.{0}, domain.commercialspace", id));

            var text = new StringBuilder();
            this.BuildTreeView(text, "", type);
            return Content(text.ToString());
        }


        private void BuildTreeView(StringBuilder text, string path, Type type)
        {
            text.AppendLine("<ul>");
            foreach (var p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (p.PropertyType.Name.EndsWith("Collection"))
                {
                    text.AppendFormat("<li data-path=\"{0}\">", p.Name);
                    text.AppendFormat(" <h3>{0}</h3>", p.Name);
                    text.AppendLine("</li>");
                    text.AppendLine();

                }
                else if (p.PropertyType.Namespace == "Bespoke.SphCommercialSpaces.Domain")
                {
                    text.AppendLine("<li>");
                    text.AppendFormat("<span>{0}</span>", p.Name);
                    this.BuildTreeView(text, path + "." + p.Name, p.PropertyType);
                    text.AppendLine("</li>");
                }
                else
                {
                    var gp = path + "." + p.Name;
                    if (gp.StartsWith("."))
                        gp = gp.Substring(1, gp.Length - 1);
                    text.AppendFormat("<li class=\"k-sprite {3}\" data-path=\"{0}\" title=\"({2}){0}\">{1}</li>",
                        gp, p.Name, p.PropertyType.Name, p.PropertyType.Name.ToLowerInvariant());
                    text.AppendLine();
                }
            }
            text.AppendLine("</ul>");
        }

        private void BuildFlatJsonTreeView(IList<string> text, string path, Type type)
        {
            foreach (var p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (p.Name == "Error" && p.PropertyType == typeof(string)) continue;
                if (p.Name == "Dirty" && p.PropertyType == typeof(bool)) continue;
                if (p.Name == "Bil" && p.PropertyType == typeof(int)) continue;

                if (p.PropertyType.Namespace == "Bespoke.SphCommercialSpaces.Domain")
                {
                    this.BuildFlatJsonTreeView(text, path + "." + p.Name, p.PropertyType);
                }

                var gp = path + "." + p.Name;
                if (gp.StartsWith("."))
                    gp = gp.Substring(1, gp.Length - 1);
                var parent = path;
                if (parent.StartsWith("."))
                    parent = parent.Substring(1, parent.Length - 1) + ".";
                text.Add(string.Format("{{ \"path\":\"{0}\", \"type\":\"{1}, {2}\", \"name\":\"{3}\", \"parent\":\"{4}\"}}",
                    gp, p.PropertyType.FullName, p.PropertyType.Assembly.GetName().Name, p.Name, parent));

            }
        }

    }
}
