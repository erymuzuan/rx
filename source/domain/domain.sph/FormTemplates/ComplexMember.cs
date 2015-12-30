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
                $"     {Name}: ko.observable(new bespoke.{ns}.domain.{Name}()),";
        }

        public override string GetDefaultValueCode(int count)
        {
            return $"           this.{Name} = new {Name}();";
        }
        
        public override string GeneratedCode(string padding = "      ")
        {
            var code = new StringBuilder();
            var className = this.Name.Replace("Collection", "");
            if (this.AllowMultiple)
                code.AppendLine(padding + $"public  ObjectCollection<{className}> {Name} {{ get; }} = new ObjectCollection<{className}>();");
            else
                code.AppendLine(padding + $"public {Name} {Name} {{ get; set;}}");

            return code.ToString();

        }
        public override IEnumerable<Class> GeneratedCustomClass(string codeNamespace, string[] usingNamespaces = null)
        {


            var @class = new Class { Name = this.Name, BaseClass = nameof(DomainObject), FileName = $"{Name}.cs", Namespace = codeNamespace };
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
            var name = this.Name.Replace("Collection", "");

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
             if (optionOrWebid && typeof optionOrWebid === ""object"") {
                for (var n in optionOrWebid) {
                    if (typeof model[n] === ""function"") {
                        model[n](optionOrWebid[n]);
                    }
                }
            }
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
    }
}