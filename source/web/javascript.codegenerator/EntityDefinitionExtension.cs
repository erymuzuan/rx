using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebJavascriptUtils
{
    public static class EntityDefinitionExtension
    {

        public static Task<string> GenerateCustomXsdJavascriptClassAsync(this EntityDefinition ed)
        {
            var jsNamespace = $"{ConfigurationManager.ApplicationName}_{ed.Name.ToCamelCase()}";
            var assemblyName = $"{ConfigurationManager.ApplicationName}.{ed.Name}";
            var script = new StringBuilder();
            script.AppendLine("var bespoke = bespoke ||{};");
            script.AppendLine($"bespoke.{jsNamespace} = bespoke.{jsNamespace} ||{{}};");
            script.AppendLine($"bespoke.{jsNamespace}.domain = bespoke.{jsNamespace}.domain ||{{}};");

            script.AppendLine($"bespoke.{jsNamespace}.domain.{ed.Name} = function(optionOrWebid){{");
            script.AppendLine(" var system = require('services/system'),");
            script.AppendLine(" model = {");
            script.AppendLine($@"     $type : ko.observable(""{ed.CodeNamespace}.{ed.Name}, {assemblyName}""),");
            script.AppendLine(@"     Id : ko.observable(""0""),");

            var members = from item in ed.MemberCollection
                          let m = item.GenerateJavascriptMember(jsNamespace)
                          where !string.IsNullOrWhiteSpace(m)
                          select m;
            members.ToList().ForEach(m => script.AppendLine(m));

            script.AppendFormat(@"
    addChildItem : function(list, type){{
                        return function(){{                          
                            list.push(new type(system.guid()));
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

                if(optionOrWebid.Id){
                    model.Id(optionOrWebid.Id);
                }
                if(optionOrWebid.WebId){
                    model.WebId(optionOrWebid.WebId);
                }");


            foreach (var cm in ed.MemberCollection)
            {
                var initCode = cm.GenerateJavascriptInitValue(jsNamespace);
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
                }}", jsNamespace, ed.Name);

            script.AppendLine(" return model;");
            script.AppendLine("};");

            var classes = from m in ed.MemberCollection
                          let c = m.GenerateJavascriptClass(jsNamespace, ed.CodeNamespace, assemblyName)
                          where !string.IsNullOrWhiteSpace(c)
                          select c;
            classes.ToList().ForEach(x => script.AppendLine(x));


            return Task.FromResult(script.ToString());
        }


    }
}