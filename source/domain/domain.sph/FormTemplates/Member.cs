using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class Member : DomainObject
    {
        public string FullName { get; set; }
        public string PropertyAttribute { get; set; }


        public virtual string GetDefaultValueCode(int count)
        {
            var member = this;
            if (member.Type == typeof(object))
            {
                return $"           this.{Name} = new {Name}();";
            }
            if (null == member.DefaultValue) return null;


            var json = member.DefaultValue.ToJsonString().Replace("\"", "\\\"");
            var typeName = member.DefaultValue.GetType().Name;

            var code = new StringBuilder();
            code.AppendLine($"           var mj{count} = \"{json}\";");
            code.AppendLine($"           var field{count} = mj{count}.DeserializeFromJson<{typeName}>();");
            code.AppendLine($"           var val{count} = field{count}.GetValue(rc);");
            code.AppendLine($"           this.{member.Name} = ({member.Type.FullName})val{count};");

            return code.ToString();

        }
        public override string ToString()
        {
            return $"{this.Name}->{this.FullName}:{this.TypeName}";
        }
        [JsonIgnore]
        public virtual Type Type
        {
            get
            {
                return Strings.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }

        public virtual string GenerateJavascriptMember(string ns)
        {
            var item = this;
            if (item.Type == typeof(Array) || this.AllowMultiple)
                return $"     {Name}: ko.observableArray([]),";

            return item.Type == typeof(object) ?
                $"     {Name}: ko.observable(new bespoke.{ns}.domain.{Name}())," :
                $"     {Name}: ko.observable(),";
        }

        public virtual string GeneratedCode(string padding = "      ")
        {
            if (null == this.Type)
                throw new InvalidOperationException(this + " doesn't have a type");
            var code = new StringBuilder();
            if (typeof(object) == this.Type)
            {
                var className = this.Name.Replace("Collection", "");
                if (this.AllowMultiple)
                    code.AppendLine(padding + $"public  ObjectCollection<{className}> {Name} {{ get; }} = new ObjectCollection<{className}>();");
                else
                    code.AppendLine(padding + $"public {Name} {Name} {{ get; set;}}");

                return code.ToString();
            }
            if (typeof(Array) == this.Type)
            {
                var className = this.Name.Replace("Collection", "");
                code.AppendLine(padding + $"public ObjectCollection<{className}> {Name} {{ get; }} = new ObjectCollection<{className}>();");
                return code.ToString();
            }

            if (!string.IsNullOrWhiteSpace(PropertyAttribute))
                code.AppendLine(padding + PropertyAttribute);


            if (this.AllowMultiple)
                code.AppendLine(padding + $"public  ObjectCollection<{this.GetCsharpType()}> {Name} {{ get; }} = new ObjectCollection<{this.GetCsharpType()}>();");
            else
                code.AppendLine(padding + $"public {this.GetCsharpType()}{this.GetNullable()} {Name} {{ get; set; }}");
            return code.ToString();
        }

        protected string GetCsharpType()
        {
            return this.Type.ToCSharp();
        }

        protected string GetNullable()
        {
            if (!this.IsNullable) return string.Empty;
            if (typeof(string) == this.Type) return string.Empty;
            if (typeof(object) == this.Type) return string.Empty;
            if (typeof(Array) == this.Type) return string.Empty;
            return "?";
        }
        

        public virtual IEnumerable<Class> GeneratedCustomClass(string codeNamespace, string[] usingNamespaces = null)
        {
            var complex = this.Type == typeof(object) || this.Type == typeof(Array);
            if (!complex) return new Class[] {};

            var @class = new Class { Name = this.Name, BaseClass = nameof(DomainObject), FileName = $"{Name}.cs" , Namespace = codeNamespace};
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

            if (typeof(Array) == this.Type)
            {
                var name = this.Name.Replace("Collection", "");
                @class.Name = name;
                @class.FileName = $"{name}.cs";
            }

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
                select new Property {Code = prop};
            @class.PropertyCollection.ClearAndAddRange(properties);

            var childClasses = this.MemberCollection
                .Select(x => x.GeneratedCustomClass(codeNamespace))
                .Where(x => null != x)
                .SelectMany(x => x.ToArray());
            @classes.AddRange(childClasses);

            return @classes;
        }


        public void Add(Dictionary<string, Type> members)
        {
            foreach (var k in members.Keys)
            {
                this.MemberCollection.Add(new Member { Name = k, Type = members[k] });
            }
        }

        public new Member this[string index]
        {
            get { return this.MemberCollection.Single(m => m.Name == index); }
        }

        public IEnumerable<string> GetMembersPath(string root)
        {
            var list = new List<string>();
            list.AddRange(this.MemberCollection.Select(a => root + this.Name.Replace("Collection", "") + "." + a.Name));
            foreach (var member in this.MemberCollection)
            {
                list.AddRange(member.GetMembersPath(root + this.Name.Replace("Collection", "") + "."));
            }
            return list.ToArray();
        }

        public virtual string GenerateJavascriptClass(string jns, string csNs, string assemblyName)
        {
            var complex = this.Type == typeof(object) || this.Type == typeof(Array);
            if (!complex) return null;


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