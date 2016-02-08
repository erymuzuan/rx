using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Models;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Helpers
{
    public static class TypeHelper
    {
        public static string GetTypeName(this Lazy<CustomAction, IDesignerMetadata> meta)
        {
            return meta.Value?.GetType().GetShortAssemblyQualifiedName();
        }
        public static string[] GetPropertyPath(Type type)
        {
            var list = new List<string>();
            BuildFlatJsonTreeView(list, "", type);
            var tjson = "[" + string.Join(",", list) + "]";
            var models = JsonConvert.DeserializeObject<IEnumerable<TypeModel>>(tjson)
                                    .Select(t => t.Path)
                                    .ToArray();
            return models;
        }


        public static  void BuildTreeView(StringBuilder text, string path, Type type)
        {
            text.AppendLine("<ul>");
            foreach (var p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {

                if (p.Name == "WebId" && p.PropertyType == typeof(string)) continue;
                if (p.Name == "Error" && p.PropertyType == typeof(string)) continue;
                if (p.Name == "Dirty" && p.PropertyType == typeof(bool)) continue;
                if (p.Name == "Bil" && p.PropertyType == typeof(int)) continue;
                if (p.PropertyType == typeof(char)) continue;
                if (p.PropertyType == typeof(DateTimeKind)) continue;

                if (p.PropertyType.Name.EndsWith("Collection"))
                {
                    text.AppendFormat("<li data-path=\"{0}\">", p.Name);
                    text.AppendFormat(" <h3>{0}</h3>", p.Name);
                    text.AppendLine("</li>");
                    text.AppendLine();

                }
                else if (p.PropertyType.Namespace == "Bespoke.Sph.Domain")
                {
                    text.AppendLine("<li>");
                    text.AppendFormat("<span>{0}</span>", p.Name);
                    BuildTreeView(text, path + "." + p.Name, p.PropertyType);
                    text.AppendLine("</li>");
                }
                else
                {
                    var gp = path + "." + p.Name;
                    if (gp.StartsWith("."))
                        gp = gp.Substring(1, gp.Length - 1);
                    text.AppendLine("<li>");
                    text.AppendFormat("<span class=\"k-sprite {3}\" data-path=\"{0}\" title=\"({2}){0}\"></span>{1}",
                        gp, p.Name, p.PropertyType.Name, p.PropertyType.Name.ToLowerInvariant());
                    text.AppendLine("</li>");
                    text.AppendLine();
                }
            }
            text.AppendLine("</ul>");
        }

      

        public static  void BuildFlatJsonTreeView(IList<string> text, string path, Type type)
        {
            foreach (var p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (p.Name == "Error" && p.PropertyType == typeof(string)) continue;
                if (p.Name == "Dirty" && p.PropertyType == typeof(bool)) continue;
                if (p.Name == "Bil" && p.PropertyType == typeof(int)) continue;
                if (p.Name == "Item") continue;
                if (p.PropertyType == typeof(char)) continue;
                if (p.PropertyType == typeof(DateTimeKind)) continue;

                var gp = path + "." + p.Name;
                if (gp.StartsWith("."))
                    gp = gp.Substring(1, gp.Length - 1);
                var parent = path;
                if (parent.StartsWith("."))
                    parent = parent.Substring(1, parent.Length - 1) + ".";
                text.Add(string.Format("{{ \"path\":\"{0}\", \"type\":\"{1}, {2}\", \"name\":\"{3}\", \"parent\":\"{4}\"}}",
                    gp, p.PropertyType.FullName, p.PropertyType.Assembly.GetName().Name, p.Name, parent));

                if (path.Length < 255 && !path.EndsWith(".Date"))
                    BuildFlatJsonTreeView(text, path + "." + p.Name, p.PropertyType);



            }
        }

    }
}