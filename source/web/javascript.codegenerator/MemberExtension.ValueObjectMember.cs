using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebJavascriptUtils
{
    public static class ValueObjectMemberExtension
    {
        internal static string GenerateJavascriptInitValue(this ValueObjectMember vom, string ns)
        {
            var ctor = vom.GenerateJavascriptContructor(ns);
            if (vom.AllowMultiple)
            {
                var items = $"{vom.Name}List".ToCamelCase();
                return $@"
                    var {items} = _(optionOrWebid.{vom.Name}).map(function(v){{
                                     return new {ctor}(v);
                                }});
                    model.{vom.Name}({items});";
            }
            return $"model.{vom.Name}(new {ctor}(optionOrWebid.{vom.Name}));";
        }

        internal static string GenerateJavascriptMember(this ValueObjectMember vom, string ns)
        {
            return vom.AllowMultiple
                ? $"     {vom.Name}: ko.observableArray([]),"
                : $"     {vom.Name}: ko.observable(new {vom.GenerateJavascriptContructor(ns)}()),";
        }

        internal static string GenerateJavascriptContructor(this ValueObjectMember vom, string ns)
        {
            return $"     bespoke.{ns}.domain.{vom.ValueObjectName}";
        }


        internal static string GenerateJavascriptClass(this ValueObjectMember vom, string jns, string csNs, string assemblyName)
        {
            var script = new StringBuilder();

            script.AppendLine($"bespoke.{jns}.domain.{vom.ValueObjectName} = function(optionOrWebid){{");
            script.AppendLine(" var model = {");
            if (!string.IsNullOrWhiteSpace(assemblyName) && !string.IsNullOrWhiteSpace(csNs))
                script.AppendLine($"     $type : ko.observable(\"{csNs}.{vom.ValueObjectName}, {assemblyName}\"),");

            var members = from mb in vom.MemberCollection
                          let m = mb.GenerateJavascriptMember(jns)
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
            script.AppendLine($@"     WebId: ko.observable()");
            script.AppendLine($@" }};");

            script.AppendLine($@" 
             if (typeof optionOrWebid === ""object"") {{

                if(optionOrWebid.WebId){{
                    model.WebId(optionOrWebid.WebId);
                }}");
            foreach (var cm in vom.MemberCollection)
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

            script.AppendFormat(@"

    if (bespoke.{0}.domain.{1}Partial) {{
        return _(model).extend(new bespoke.{0}.domain.{1}Partial(model));
    }}
", jns, vom.Name.Replace("Collection", string.Empty));

            script.AppendLine(" return model;");
            script.AppendLine("};");
            var classes = from m in vom.MemberCollection
                          let c = m.GenerateJavascriptClass(jns, csNs, assemblyName)
                          where !string.IsNullOrWhiteSpace(c)
                          select c;
            classes.ToList().ForEach(x => script.AppendLine(x));
            return script.ToString();
        }



    }
}
