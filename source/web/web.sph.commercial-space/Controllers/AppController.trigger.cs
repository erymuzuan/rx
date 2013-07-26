﻿using System;
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

        public ActionResult TriggerPathPickerHtml(string id)
        {
            var type = Type.GetType(string.Format("Bespoke.SphCommercialSpaces.Domain.{0}, domain.commercialspace, Version=1.0.2.1006, Culture=neutral", id));

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

        public ActionResult TriggerPathPickerJs()
        {
            return View();
        }

    }

    public class TriggerPathPickerHtmlViewModel
    {
        public Type Type { get; set; }
        public string Path { get; set; }
    }
}
