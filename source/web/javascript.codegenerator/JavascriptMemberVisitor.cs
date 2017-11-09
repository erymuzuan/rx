using System;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebJavascriptUtils
{
    public class JavascriptMemberVisitor : MemberVisitor<string>
    {
        private readonly string m_ns;

        public string GenerateJavascriptClass( EntityDefinition ed, string jns, string csNs, string assemblyName)
        {
            var script = new StringBuilder();
            var name = ed.TypeName.Replace("Collection", "");

            script.AppendLinf("bespoke.{0}.domain.{1} = function(optionOrWebid){{", jns, name);
            script.AppendLine(" var model = {");
            if (!string.IsNullOrWhiteSpace(assemblyName) && !string.IsNullOrWhiteSpace(csNs))
                script.AppendLine($"     $type : ko.observable(\"{csNs}.{name}, {assemblyName}\"),");


            var members = from item in ed.MemberCollection
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
            foreach (var cm in ed.MemberCollection)
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

            var classes = from m in ed.MemberCollection
                          let c = m.GenerateJavascriptClass(jns, csNs, assemblyName)
                          where !string.IsNullOrWhiteSpace(c)
                          select c;
            classes.ToList().ForEach(x => script.AppendLine(x));
            return script.ToString();
        }

        public JavascriptMemberVisitor(string ns)
        {
            m_ns = ns;
        }

        /**/
        protected override string Visit(Member p)
        {
            throw new NotImplementedException();
        }

        protected override string Visit(ComplexMember e)
        {
            return e.AllowMultiple ?
                $"     {e.Name}: ko.observableArray([])," :
                $"     {e.Name}: ko.observable(new {e.GenerateJavascriptContructor(m_ns)}()),";
        }
    }
}
