using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class ValueObjectMember : Member
    {
        public override string GetDefaultValueCode(int count)
        {
            return this.AllowMultiple ? null : $"this.{Name} = new {ValueObjectName}();";
        }


        public override string GeneratedCode(string padding = "      ")
        {
            if (this.AllowMultiple)
                return padding +
                       $"public ObjectCollection<{ValueObjectName}> {Name} {{get;}} = new ObjectCollection<{ValueObjectName}>();";
            return padding + $"public {ValueObjectName} {Name} {{ get; set;}}";
        }


        public override IEnumerable<Class> GeneratedCustomClass(string codeNamespace, string[] usingNamespaces = null)
        {
            var @class = new Class { Name = this.ValueObjectName, BaseClass = nameof(DomainObject), FileName = $"{ValueObjectName}.cs", Namespace = codeNamespace };
            if (null != usingNamespaces)
            {
                @class.ImportCollection.AddRange(usingNamespaces);
            }
            else
            {
                @class.ImportCollection.Add(typeof(DateTime).Namespace);
                @class.ImportCollection.Add(typeof(Entity).Namespace);
            }
            var classes = new ObjectCollection<Class> { @class };


            var ctor = new StringBuilder();
            // ctor
            ctor.AppendLine($"       public {ValueObjectName}()");
            ctor.AppendLine("       {");
            ctor.AppendLinf("           var rc = new RuleContext(this);");
            var count = 0;
            foreach (var member in this.MemberCollection)
            {
                count++;
                var defaultValueCode = member.GetDefaultValueCode(count);
                if (!string.IsNullOrWhiteSpace(defaultValueCode))
                    ctor.AppendLine(defaultValueCode);
            }
            ctor.AppendLine("       }");
            @class.CtorCollection.Add(ctor.ToString());


            var properties = from m in this.MemberCollection
                             let prop = m.GeneratedCode("   ")
                             select new Property { Code = prop };
            @class.PropertyCollection.ClearAndAddRange(properties);

            var childClasses = this.MemberCollection
                .Select(x => x.GeneratedCustomClass(codeNamespace))
                .Where(x => null != x)
                .SelectMany(x => x.ToArray());
            @classes.AddRange(childClasses);


            return classes;
        }




        public override BuildError[] Validate()
        {
            var errors = new ObjectCollection<BuildError>();

            if (string.IsNullOrWhiteSpace(this.ValueObjectName))
                errors.Add(new BuildError(this.WebId) { Message = $"[Member] {Name} has no ValueObjectDefinition defined" });
            
            foreach (var m in this.MemberCollection)
            {
               var list = m.Validate();
                errors.AddRange(list);
            }

            return errors.ToArray();
        }

        public override string GenerateJavascriptMember(string ns)
        {
            return this.AllowMultiple
                    ? $"     {Name}: ko.observableArray([]),"
                    : $"     {Name}: ko.observable(new {this.GenerateJavascriptContructor(ns)}()),";
        }

        public override string GenerateJavascriptContructor(string ns)
        {
            return $"     bespoke.{ns}.domain.{ValueObjectName}";
        }

        public override string GenerateJavascriptInitValue(string ns)
        {
            var ctor = this.GenerateJavascriptContructor(ns);
            if (this.AllowMultiple)
            {
                var code = new StringBuilder();
                var items = $"{Name}List".ToCamelCase();
                code.AppendLine($@"var {items} = _(optionOrWebid.{Name}).map(function(v){{");
                code.AppendLine($@"                 return new {ctor}(v);");
                code.AppendLine($@"            }});");
                code.AppendLine($@"model.{Name}({items});");
                return code.ToString();
            }
            return $"model.{Name}(new {ctor}(optionOrWebid.{Name}));";
        }

        public override string GenerateJavascriptClass(string jns, string csNs, string assemblyName)
        {
            var script = new StringBuilder();

            script.AppendLine($"bespoke.{jns}.domain.{ValueObjectName} = function(optionOrWebid){{");
            script.AppendLine(" var model = {");
            script.AppendLine($"     $type : ko.observable(\"{csNs}.{ValueObjectName}, {assemblyName}\"),");

            var members = from mb in this.MemberCollection
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
            script.AppendLine("     WebId: ko.observable()");

            script.AppendLine(" };");

            script.AppendLine(@" 
             if (typeof optionOrWebid === ""object"") {");
            foreach (var cm in this.MemberCollection)
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
", jns, this.Name.Replace("Collection", string.Empty));

            script.AppendLine(" return model;");
            script.AppendLine("};");
            var classes = from m in this.MemberCollection
                          let c = m.GenerateJavascriptClass(jns, csNs, assemblyName)
                          where !string.IsNullOrWhiteSpace(c)
                          select c;
            classes.ToList().ForEach(x => script.AppendLine(x));
            return script.ToString();
        }

        public new ObjectCollection<Member> MemberCollection
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.ValueObjectName))
                    return new ObjectCollection<Member>();

                var context = new SphDataContext();
                var vod = context.LoadOne<ValueObjectDefinition>(d => d.Name == this.ValueObjectName);
                return null == vod ? new ObjectCollection<Member>() : vod.MemberCollection;
            }
        }


        public override IEnumerable<string> GetMembersPath(string root)
        {
            var list = new List<string>();
            list.AddRange(this.MemberCollection.Select(a => $"{root}{this.Name}.{a.Name}"));
            foreach (var member in this.MemberCollection)
            {
                list.AddRange(member.GetMembersPath($"{root}{this.Name}."));
            }
            return list.ToArray();
        }


    }
}