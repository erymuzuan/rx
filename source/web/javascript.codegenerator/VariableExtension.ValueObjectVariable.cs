using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebJavascriptUtils
{
    public static partial class VariableExtension
    {

        public static async Task<string> GenerateCustomJavascriptAsync(this ValueObjectVariable vov, WorkflowDefinition wd)
        {
            var jns = $"bespoke.{ConfigurationManager.ApplicationName}.{wd.WorkflowTypeName}.domain";
            var context = new SphDataContext();
            var vod = await context.LoadOneAsync<ValueObjectDefinition>(x => x.Name == vov.TypeName);
            var script = new StringBuilder();
            script.AppendLine($@"
var bespoke = bespoke ||{{}};
bespoke.{ConfigurationManager.ApplicationName} = bespoke.{ConfigurationManager.ApplicationName} ||{{}};
bespoke.{ConfigurationManager.ApplicationName}.{wd.WorkflowTypeName} = bespoke.{ConfigurationManager.ApplicationName}.{wd.WorkflowTypeName} ||{{}};
{jns} = {jns} ||{{}};
");

            foreach (var member in vod.MemberCollection)
            {
                var @class = member.GenerateJavascriptClass($"{ConfigurationManager.ApplicationName}.{wd.WorkflowTypeName}", "", "");
                if (!string.IsNullOrWhiteSpace(@class))
                    script.AppendLine(@class);
            }

            script.AppendLine($@"
    bespoke.{ConfigurationManager.ApplicationName}.{wd.WorkflowTypeName}.domain.{vov.TypeName} = function(optionOrWebid){{");
            script.AppendLine(@" 
    var system = require('durandal/system'),
        model = {");
            var members = from mb in vod.MemberCollection
                          let memberDeclare = mb.GenerateJavascriptMember(jns)
                          where !string.IsNullOrWhiteSpace(memberDeclare)
                          select memberDeclare;
            members.ToList().ForEach(m => script.AppendLine(m));
            script.AppendLine(@"   
   addChildItem : function(list, type){
                        return function(){
                            list.push(new type(system.guid()));
                        }
                    },
            
   removeChildItem : function(list, obj){
                        return function(){
                            list.remove(obj);
                        }
                    }");
            script.AppendLine(" };");

            script.AppendLine(" if(typeof optionOrWebid === \"object\"){");

            var initCodes = from mb in vod.MemberCollection
                            let init = mb.GenerateJavascriptInitValue(jns)
                            where !string.IsNullOrWhiteSpace(init)
                            select $@"    
        if(optionOrWebid.{mb.Name}){{
            {init}
        }}";
            initCodes.ToList().ForEach(m => script.AppendLine(m));
            script.AppendLine(" }");


            script.AppendLine($@"   
    if({jns}.{vov.TypeName}Partial){{    
        return _(model).extend(new {jns}.{vov.TypeName}Partial(model));
    }}");

            script.AppendLine(" return model;");
            script.AppendLine(" }");

            return script.ToString();
        }
    }
}
