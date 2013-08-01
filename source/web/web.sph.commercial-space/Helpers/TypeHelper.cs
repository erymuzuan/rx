using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bespoke.Sph.Commerspace.Web.Helpers
{
    public static class TypeHelper
    {
        public static  void BuildFlatJsonTreeView(IList<string> text, string path, Type type)
        {
            foreach (var p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (p.Name == "Error" && p.PropertyType == typeof(string)) continue;
                if (p.Name == "Dirty" && p.PropertyType == typeof(bool)) continue;
                if (p.Name == "Bil" && p.PropertyType == typeof(int)) continue;
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
                    TypeHelper.BuildFlatJsonTreeView(text, path + "." + p.Name, p.PropertyType);



            }
        }
    }
}