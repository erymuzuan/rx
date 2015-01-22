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
            get { return Type.GetType(this.TypeName); }
            set { this.TypeName = value.GetShortAssemblyQualifiedName(); }
        }

        [XmlIgnore]
        [JsonIgnore]
        public string InferredType
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.TypeName) && this.MemberCollection.Any() && !this.AllowMultiple)
                {
                    return this.Name;
                }

                if (string.IsNullOrWhiteSpace(this.TypeName) && this.MemberCollection.Any() && this.AllowMultiple)
                {
                    return "ObjectCollection`1" + this.Name.Singularize();
                }

                return null;
            }

        }

        public Property CreateProperty()
        {
            var prop = new Property { Name = this.Name, IsNullable = this.IsNullable };
            if (this.AllowMultiple)
            {
                prop.IsReadOnly = true;
                prop.Initialized = true;
            }

            if (!string.IsNullOrWhiteSpace(this.InferredType))
            {
                prop.TypeName = this.AllowMultiple ? "ObjectCollection<" + this.Name.Singularize() + ">" : this.Name;
            }
            else
            {
                prop.Type = this.AllowMultiple ? typeof(ObjectCollection<>).MakeGenericType(this.Type) : this.Type;
            }
            return prop;
        }

        public static readonly Type[] NativeTypes =
        {
            typeof(string), typeof(long),typeof(short), typeof(int), typeof(double), typeof(bool), typeof(decimal), typeof(DateTime), typeof(char),
            typeof(long?),typeof(short?),  typeof(int?), typeof(double?), typeof(bool?), typeof(decimal?), typeof(DateTime?), typeof(char?)
        };

        public bool IsComplex
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.InferredType)) return true;
                return !NativeTypes.Contains(this.Type);
            }
        }

        public bool AllowMultiple { get; set; }

        public IEnumerable<Class> GeneratedCustomClass()
        {
            if (!this.IsComplex)
                throw new InvalidOperationException("cannot generate class for simple member type " + this.Type);

            var @class = new Class
            {
                Name = this.AllowMultiple ? this.Name.Singularize() : this.Name,
                BaseClass = typeof(DomainObject).Name
            };
            @class.AddNamespaceImport<DomainObject>();
            @class.AddNamespaceImport<DateTime>();

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
                var type = mb.IsComplex ? mb.InferredType : mb.Name;
                if (!mb.AllowMultiple && mb.IsComplex)
                {
                    ctor.AppendLinf("           this.{0} = new {0}();", mb.Name, type);
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

        public Member AddMember(string name, Type type)
        {

            var child = new Member { Name = name, Type = type, WebId = Guid.NewGuid().ToString() };
            this.MemberCollection.Add(child);
            return child;
        }
    }
}