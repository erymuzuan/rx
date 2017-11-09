using Bespoke.Sph.Domain;
using System.Text;
using System.Linq;
namespace Bespoke.Sph.WebJavascriptUtils
{
    internal static class ComplexMemberExtension
    {
        internal static string GenerateJavascriptInitValue(this ComplexMember cm, string ns)
        {
            var ctor = cm.GenerateJavascriptContructor(ns);
            if (cm.AllowMultiple)
            {
                var items = $"{cm.Name}List".ToCamelCase();
                return $@"
                    var {items} = _(optionOrWebid.{cm.Name}).map(function(v){{
                                     return new {ctor}(v);
                                }});
                    model.{cm.Name}({items});";
            }
            return $"model.{cm.Name}(new {ctor}(optionOrWebid.{cm.Name}));";
        }


        internal static string GenerateJavascriptContructor(this ComplexMember cm, string ns)
        {
            return $"bespoke.{ns}.domain.{cm.TypeName}";
        }

        internal static string GenerateJavascriptMember(this ComplexMember cm, string ns)
        {
            return cm.AllowMultiple
                ? $"     {cm.Name}: ko.observableArray([]),"
                : $"     {cm.Name}: ko.observable(new {cm.GenerateJavascriptContructor(ns)}()),";
        }
        
        internal static string GenerateJavascriptClass(this ComplexMember cxm, string jns, string csNs, string assemblyName)
        {
            var script = new StringBuilder();
            var name = cxm.TypeName.Replace("Collection", "");

            script.AppendLinf("bespoke.{0}.domain.{1} = function(optionOrWebid){{", jns, name);
            script.AppendLine(" var model = {");
            if (!string.IsNullOrWhiteSpace(assemblyName) && !string.IsNullOrWhiteSpace(csNs))
                script.AppendLine($"     $type : ko.observable(\"{csNs}.{name}, {assemblyName}\"),");


            var members = from item in cxm.MemberCollection
                          let m = item.GenerateJavascriptMember(jns)
                          where !string.IsNullOrWhiteSpace(m)
                          select m;
            members.ToList().ForEach(m => script.AppendLine(m));

            script.AppendFormat(@"
    addChildItem : function(list, type){{
                        return function(){{
                            list.push(new childType(system.guid()));
                        }}
                    }},
            
   removeChildItem : function(list, obj){{
                        return function(){{
                            list.remove(obj);
                        }}
                    }},
");
            script.AppendLine("     WebId: ko.observable()");

            script.AppendLine(" };");


            script.AppendLine(@" 
             if (typeof optionOrWebid === ""object"") {

                if(optionOrWebid.WebId){
                    model.WebId(optionOrWebid.WebId);
                }");
            foreach (var cm in cxm.MemberCollection)
            {
                var initCode = cm.GenerateJavascriptInitValue(jns);
                if (string.IsNullOrWhiteSpace(initCode)) continue;
                script.AppendLine($@"
                if(optionOrWebid.{cm.Name}){{
                    {initCode}
                }}");
            }

            script.AppendLine(@"
            }");

            script.AppendLine(@"
            if (optionOrWebid && typeof optionOrWebid === ""string"") {
                model.WebId(optionOrWebid);
            }");

            script.AppendLine($@"

    if (bespoke.{jns}.domain.{name}Partial) {{
        return _(model).extend(new bespoke.{jns}.domain.{name}Partial(model));
    }}
");

            script.AppendLine(" return model;");
            script.AppendLine("};");

            var classes = from m in cxm.MemberCollection
                          let c = m.GenerateJavascriptClass(jns, csNs, assemblyName)
                          where !string.IsNullOrWhiteSpace(c)
                          select c;
            classes.ToList().ForEach(x => script.AppendLine(x));
            return script.ToString();
        }
    }
}