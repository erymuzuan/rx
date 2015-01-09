using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class Member : DomainObject
    {
        [XmlAttribute]
        public string FullName { get; set; }

        public override string ToString()
        {
            return string.Format("{0}->{1}:{2}", this.Name, this.FullName, this.TypeName);
        }
        [XmlIgnore]
        [JsonIgnore]
        public Type Type
        {
            get
            {
                return Type.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }

        public Property GeneratedCode(string padding = "      ")
        {
            if (null == this.Type)
                throw new InvalidOperationException(this + " doesn't have a type");
            var code = new StringBuilder();
            if (typeof(object) == this.Type)
            {
                code.AppendLinf(padding + "public {0} {1} {{get;set;}}", this.Name, this.Name);
                return new Property { Name = this.Name, TypeName = this.Name };
            }
            if (typeof(Array) == this.Type)
            {
                var col = new Property
                {
                    Name = this.Name,
                    IsReadOnly = true,
                    Initialized = true,
                    TypeName = "ObjectCollection<" + this.Name.Replace("Collection", "") + ">"
                };

                return col;
            }

            return new Property { Name = this.Name, TypeName = this.GetCsharpType(), IsNullable = this.GetNullable() == "?" };
        }

        private string GetCsharpType()
        {
            return this.Type.ToCSharp();
        }

        private string GetNullable()
        {
            if (!this.IsNullable) return string.Empty;
            if (typeof(string) == this.Type) return string.Empty;
            if (typeof(object) == this.Type) return string.Empty;
            if (typeof(Array) == this.Type) return string.Empty;
            return "?";
        }

        public Property CreateProperty()
        {
            var prop = new Property { Name = this.Name, Type = this.Type, IsNullable = this.IsNullable };
            if (this.Type == typeof(Array))
            {
                prop.IsReadOnly = true;
                prop.Initialized = true;
                prop.TypeName = "ObjectCollection<" + this.Name.Replace("Collection", "") + ">";
            }

            if (this.Type == typeof(object))
            {
                prop.TypeName = this.Name;
            }
            return prop;
        }

        public bool IsComplex { get { return (typeof(object) == this.Type || typeof(Array) == this.Type); } }
        public IEnumerable<Class> GeneratedCustomClass()
        {

            if (!this.IsComplex)
                throw new InvalidOperationException("cannot generate class for simple member type " + this.Type);


            var @class = new Class
            {
                Name = this.Name,
                BaseClass = typeof(DomainObject).Name
            };
            @class.AddNamespaceImport<DomainObject>();

            if (typeof(Array) == this.Type)
                @class.Name = this.Name.Replace("Collection", "");

            var ctor = GenerateConstructor();
            @class.CtorCollection.Add(ctor.ToString());
            @class.PropertyCollection.AddRange(this.MemberCollection.Select(x => x.CreateProperty()));

            var @classes = this.MemberCollection
                .Where(x => x.IsComplex)
                .Select(x => x.GeneratedCustomClass())
                .SelectMany(x =>
                {
                    var enumerable = x as Class[] ?? x.ToArray();
                    return enumerable;
                })
                .ToList();
            @classes.Add(@class);
            return @classes;
        }

        private StringBuilder GenerateConstructor()
        {
            var ctor = new StringBuilder();
            ctor.AppendLine("       public " + this.Name.Replace("Collection", "") + "()");
            ctor.AppendLine("       {");

            foreach (var mb in this.MemberCollection)
            {
                if (mb.Type == typeof(object))
                {
                    ctor.AppendLinf("           this.{0} = new {0}();", mb.Name);
                }
                if (null == mb.DefaultValue) continue;

                var defaultValueExpression = mb.DefaultValue.GetCSharpExpression();
                ctor.AppendLinf("           this.{0} = {1};", mb.Name, defaultValueExpression);
            }
            ctor.AppendLine("       }");
            return ctor;
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

        public Member AddMember(string name, Type type)
        {

            var child = new Member { Name = name, Type = type, WebId = Guid.NewGuid().ToString() };
            this.MemberCollection.Add(child);
            return child;
        }
    }
}