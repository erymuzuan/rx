using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class ValueObjectVariable : Variable
    {
        private ValueObjectDefinition m_vod;

        public ValueObjectVariable()
        {

        }

        public ValueObjectVariable(ValueObjectDefinition vod)
        {
            m_vod = vod;
            this.TypeName = vod.Name;
            this.ValueObjectDefinition = vod.Name;
        }

        public override string GeneratedCtorCode(WorkflowDefinition wd)
        {
            return $"this.{Name} = new {TypeName}();";
        }

        public override async Task<string[]> GetMembersPathAsync(WorkflowDefinition wd)
        {
            var context = new SphDataContext();
            var vod = await context.LoadOneAsync<ValueObjectDefinition>(x => x.Name == this.TypeName);
            var list = vod.MemberCollection.Select(x => $"{Name}.{x.Name}").ToList();
            list.AddRange(vod.MemberCollection.Select(x => x.GetMembersPath(this.Name + ".")).SelectMany(x => x.ToArray()));

            return list.ToArray();
        }

        public override Task<IEnumerable<Class>> GenerateCustomTypesAsync(WorkflowDefinition wd)
        {
            if (null == m_vod)
            {
                var context = new SphDataContext();
                m_vod = context.LoadOneFromSources<ValueObjectDefinition>(x => x.Name == this.TypeName);
            }

            var @class = new Class { Name = TypeName, BaseClass = nameof(DomainObject), FileName = $"{TypeName}.cs", Namespace = wd.CodeNamespace };

            @class.ImportCollection.Add(typeof(DateTime).Namespace);
            @class.ImportCollection.Add(typeof(Entity).Namespace);

            var classes = new ObjectCollection<Class> { @class };


            var ctor = new StringBuilder();
            // ctor
            ctor.AppendLine($"       public {TypeName}()");
            ctor.AppendLine("       {");
            ctor.AppendLinf("           var rc = new RuleContext(this);");
            var count = 0;
            foreach (var member in m_vod.MemberCollection)
            {
                count++;
                var defaultValueCode = member.GetDefaultValueCode(count);
                if (!string.IsNullOrWhiteSpace(defaultValueCode))
                    ctor.AppendLine(defaultValueCode);
            }
            ctor.AppendLine("       }");
            @class.CtorCollection.Add(ctor.ToString());


            var properties = from m in m_vod.MemberCollection
                             let prop = m.GeneratedCode("   ")
                             select new Property { Code = prop };
            @class.PropertyCollection.ClearAndAddRange(properties);

            var childClasses = m_vod.MemberCollection
                .Select(x => x.GeneratedCustomClass(wd.CodeNamespace))
                .Where(x => null != x)
                .SelectMany(x => x.ToArray());
            @classes.AddRange(childClasses);


            return Task.FromResult(classes.AsEnumerable());
        }

        public override string GeneratedCode(WorkflowDefinition workflowDefinition)
        {
            return $"   public {TypeName} {Name} {{ get; set;}}";
        }

        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            if (this.Name.Contains(" "))
            {
                result.Result = false;
                result.Errors.Add(new BuildError(this.WebId)
                {
                    Message = $"[Variable] \"{this.Name}\" cannot contains space "
                });
            }

            return result;
        }


        public override async Task<string> GenerateCustomJavascriptAsync(WorkflowDefinition wd)
        {
            var jns = $"bespoke.{ConfigurationManager.ApplicationName}.{wd.WorkflowTypeName}.domain";
            var context = new SphDataContext();
            var vod = await context.LoadOneAsync<ValueObjectDefinition>(x => x.Name == this.TypeName);
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
    bespoke.{ConfigurationManager.ApplicationName}.{wd.WorkflowTypeName}.domain.{this.TypeName} = function(optionOrWebid){{");
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
    if({jns}.{TypeName}Partial){{    
        return _(model).extend(new {jns}.{TypeName}Partial(model));
    }}");

            script.AppendLine(" return model;");
            script.AppendLine(" }");

            return script.ToString();
        }
    }


}