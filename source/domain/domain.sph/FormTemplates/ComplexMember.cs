using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class ComplexMember : Member
    {
        public override string ToString()
        {
            return $"[ComplexMember]{this.Name}; AllowMultiple = {AllowMultiple}, Members = {MemberCollection.Count}";
        }

        public override string GenerateJavascriptMember(string ns)
        {
            return this.AllowMultiple ?
                $"     {Name}: ko.observableArray([])," :
                $"     {Name}: ko.observable(new {this.GenerateJavascriptContructor(ns)}()),";
        }

        public override string GenerateJavascriptContructor(string ns)
        {
            return $"bespoke.{ns}.domain.{TypeName}";
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

        public override string GetDefaultValueCode(int count)
        {
            if (this.AllowMultiple) return null;
            return $"           this.{Name} = new {TypeName}();";
        }

        public override string GeneratedCode(string padding = "      ")
        {
            var code = new StringBuilder();
            if (this.AllowMultiple)
                code.AppendLine(padding + $"public  ObjectCollection<{TypeName}> {Name} {{ get; }} = new ObjectCollection<{TypeName}>();");
            else
                code.AppendLine(padding + $"public {TypeName} {Name} {{ get; set;}}");

            return code.ToString();

        }
        public override IEnumerable<Class> GeneratedCustomClass(string codeNamespace, string[] usingNamespaces = null)
        {


            var @class = new Class { Name = this.TypeName, BaseClass = nameof(DomainObject), FileName = $"{TypeName}.cs", Namespace = codeNamespace };
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



            var ctor = new StringBuilder($"public {@class.Name}()");
            ctor.AppendLine("{");
            ctor.AppendLine("  var rc = new RuleContext(this);");
            var count = 0;
            foreach (var member in this.MemberCollection)
            {
                count++;
                var defaultValueCode = member.GetDefaultValueCode(count);
                if (!string.IsNullOrWhiteSpace(defaultValueCode))
                    ctor.AppendLine(defaultValueCode);
            }
            ctor.AppendLine("}");
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

            return @classes;
        }
        public override string GenerateJavascriptClass(string jns, string csNs, string assemblyName)
        {
            var script = new StringBuilder();
            var name = this.TypeName.Replace("Collection", "");

            script.AppendLinf("bespoke.{0}.domain.{1} = function(optionOrWebid){{", jns, name);
            script.AppendLine(" var model = {");
            script.AppendLinf("     $type : ko.observable(\"{0}.{1}, {2}\"),", csNs, name,
                assemblyName);


            var members = from item in this.MemberCollection
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

            script.AppendLine($@"

    if (bespoke.{jns}.domain.{name}Partial) {{
        return _(model).extend(new bespoke.{jns}.domain.{name}Partial(model));
    }}
");

            script.AppendLine(" return model;");
            script.AppendLine("};");

            var classes = from m in this.MemberCollection
                          let c = m.GenerateJavascriptClass(jns, csNs, assemblyName)
                          where !string.IsNullOrWhiteSpace(c)
                          select c;
            classes.ToList().ForEach(x => script.AppendLine(x));
            return script.ToString();
        }

        public void Add(Dictionary<string, Type> dictionary)
        {
            foreach (var member in dictionary.Keys)
            {
                this.MemberCollection.Add(new SimpleMember { Name = member, Type = dictionary[member] });
            }
        }
    }
}