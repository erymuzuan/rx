using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public Type Type
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

        public virtual string GeneratedCustomClass()
        {
            var code = new StringBuilder();
            if (typeof(object) == this.Type)
            {
                code.AppendLinf("   public class {0}: DomainObject", this.Name);
            }
            if (typeof(Array) == this.Type)
            {
                code.AppendLinf("   public class {0}: DomainObject", this.Name.Replace("Collection", ""));
            }

            if (typeof(object) == this.Type || typeof(Array) == this.Type)
            {

                code.AppendLine("   {");
                // ctor
                code.AppendLine("       public " + this.Name.Replace("Collection", "") + "()");
                code.AppendLine("       {");
                //code.AppendLinf("           var rc = new RuleContext(this);");
                //var count = 0;
                foreach (var member in this.MemberCollection)
                {
                    if (member.Type == typeof(object))
                    {
                        code.AppendLinf("           this.{0} = new {0}();", member.Name);
                    }
                    /*
                    if (null == member.DefaultValue) continue;
                    count++;
                    code.AppendLine();
                    code.AppendLinf("           var mj{1} = \"{0}\";", member.DefaultValue.ToJsonString().Replace("\"", "\\\""), count);
                    code.AppendLinf("           var field{0} = mj{0}.DeserializeFromJson<{1}>();", count, member.DefaultValue.GetType().Name);
                    code.AppendLinf("           var val{0} = field{0}.GetValue(rc);", count);
                    code.AppendLinf("           this.{0} = ({1})val{2};", member.Name, member.Type.FullName, count);
                
                     * 
                     */
                }
                code.AppendLine("       }");


                foreach (var member in this.MemberCollection)
                {
                    code.AppendLine(member.GeneratedCode());

                }
                code.AppendLine("   }");
                foreach (var member in this.MemberCollection)
                {
                    code.AppendLine(member.GeneratedCustomClass());
                }
            }
            return code.FormatCode();
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

        public string GenerateJavascriptClass(string jsNamespace, string codeNamespace, string assemblyName)
        {
            var script = new StringBuilder();
            var name = this.Name.Replace("Collection", "");

            script.AppendLinf("bespoke.{0}.domain.{1} = function(optionOrWebid){{", jsNamespace, name);
            script.AppendLine(" var model = {");
            script.AppendLinf("     $type : ko.observable(\"{0}.{1}, {2}\"),", codeNamespace, name,
                assemblyName);
            foreach (var item in this.MemberCollection)
            {
                if (item.Type == typeof(Array))
                    script.AppendLinf("     {0}: ko.observableArray([]),", item.Name);
                else if (item.Type == typeof(object))
                    script.AppendLinf("     {0}: ko.observable(new bespoke.{1}.domain.{0}()),", item.Name, jsNamespace);
                else
                    script.AppendLinf("     {0}: ko.observable(),", item.Name);
            }
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
", jsNamespace, this.Name.Replace("Collection", string.Empty));

            script.AppendLine(" return model;");
            script.AppendLine("};");

            foreach (var item in this.MemberCollection.Where(m => m.Type == typeof(object) || m.Type == typeof(Array)))
            {
                var code = item.GenerateJavascriptClass(jsNamespace, codeNamespace, assemblyName);
                script.AppendLine(code);
            }
            return script.ToString();
        }
    }
}