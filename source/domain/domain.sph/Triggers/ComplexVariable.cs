using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Humanizer;

namespace Bespoke.Sph.Domain
{
    public partial class ComplexVariable : Variable
    {
        // ReSharper disable InconsistentNaming
        static readonly XNamespace x = "http://www.w3.org/2001/XMLSchema";
        // ReSharper restore InconsistentNaming
        public override string GeneratedCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            code.AppendFormat("public {0} {1} {{ get;set; }}", this.TypeName, this.Name);
            return code.ToString();
        }

        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            if (string.IsNullOrWhiteSpace(this.TypeName))
            {
                result.Result = false;
                result.Errors.Add(new BuildError(this.WebId, string.Format("[Variable] \"{0}\" does not have a valid type", this.Name)));
            }

            return result;
        }
        private readonly ObjectCollection<Member> m_auxilliaryTypeDefinitionCollection = new ObjectCollection<Member>();

        public ObjectCollection<Member> AuxilliaryTypeDefinitionCollection
        {
            get { return m_auxilliaryTypeDefinitionCollection; }
        }
        public override Member CreateMember(WorkflowDefinition wd)
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var content = store.GetContent(wd.SchemaStoreId);
            if (null == content) return null;
            XElement xsd;
            using (var stream = new MemoryStream(content.Content))
            {
                xsd = XElement.Load(stream);
            }

            var element = xsd.Elements(x + "element")
                   .Where(e => null != e.Attribute("name"))
                   .SingleOrDefault(e => e.Attribute("name").Value == this.TypeName);
            var auxs = xsd.Elements(x + "element")
                   .Where(e => null != e.Attribute("name"))
                   .Select(e => this.GetXsdmembers(e.Element(x + "complexType")))
                   .SelectMany(v => v.ToArray());
            this.AuxilliaryTypeDefinitionCollection.ClearAndAddRange(auxs.ToArray());

            var root = new Member { Name = this.Name };
            if (null == element) return root;

            var children = this.GetXsdmembers(element.Element(x + "complexType"));
            root.MemberCollection.AddRange(children);
            return root;
        }


        private IEnumerable<Member> GetXsdmembers(XElement element)
        {
            var list = new List<Member>();
            if (null == element) return list;

            var attributes = from at in element.Elements(x + "attribute")
                             select GetMemberFromAttributeElement(at);
            list.AddRange(attributes);

            var all = element.Element(x + "all");
            if (null != all)
            {

                var allElements = from at in all.Elements(x + "element")
                                  where at.Attribute("name") != null
                                  && at.Attribute("type") != null
                                  select GetMemberFromAttributeElement(at);
                list.AddRange(allElements);

                var collectionElements = from at in all.Elements(x + "element")
                                         where at.Attribute("name") != null
                                         && at.Attribute("type") == null
                                         let refElement = at.Descendants(x + "element").First()
                                         select GetMemberFromAttributeElement(at);
                list.AddRange(collectionElements);



                var refElements = from at in all.Elements(x + "element")
                                  where at.Attribute("ref") != null
                                  let refa = at.Attribute("ref")
                                  select GetMemberFromRefElement(at);
                list.AddRange(refElements);


            }
            return list;
        }


        private static Member GetMemberFromRefElement(XElement element)
        {
            var member = new Member { Name = element.Attribute("ref").Value };
            return member;
        }

        static Member GetMemberFromAttributeElement(XElement element)
        {

            var typeAttribute = element.Attribute("type");
            var nillableAttribute = element.Attribute("nillable");

            var xsType = typeAttribute != null ? typeAttribute.Value : "";
            var nillable = nillableAttribute != null && bool.Parse(nillableAttribute.Value);
            var member = new Member { Name = element.Attribute("name").Value, IsNullable = nillable };

            switch (xsType)
            {
                case "xs:string":
                    member.Type = typeof(string);
                    break;
                case "xs:date":
                case "xs:dateTime":
                    member.Type = typeof(DateTime);
                    break;
                case "xs:int":
                    member.Type = typeof(In);
                    break;
                case "xs:long":
                    member.Type = typeof(long);
                    break;
                case "xs:boolean":
                    member.Type = typeof(bool);
                    break;
                case "xs:float":
                    member.Type = typeof(float);
                    break;
                case "xs:double":
                    member.Type = typeof(double);
                    break;
                case "xs:decimal":
                    member.Type = typeof(decimal);
                    break;
                case "xs:anySimpleType":
                    member.Type = typeof(object);
                    break;
            }
            return member;
        }

        
        public static string GetClrDataType(XElement element)
        {
            var typeAttribute = element.Attribute("type");
            var nillableAttribute = element.Attribute("nillable");

            var xsType = typeAttribute != null ? typeAttribute.Value : "";
            var nillable = nillableAttribute != null && bool.Parse(nillableAttribute.Value);

            string type;
            switch (xsType)
            {
                case "xs:string":
                    type = "string";
                    break;
                case "xs:date":
                case "xs:dateTime":
                    type = "DateTime";
                    break;
                case "xs:int":
                    type = "int";
                    break;
                case "xs:long":
                    type = "long";
                    break;
                case "xs:boolean":
                    type = "bool";
                    break;
                case "xs:float":
                    type = "float";
                    break;
                case "xs:double":
                    type = "double";
                    break;
                case "xs:decimal":
                    type = "decimal";
                    break;
                case "xs:anySimpleType":
                    type = "object";
                    break;
                default:
                    type = xsType;
                    break;
            }
            if (nillable) type += "?";
            return type;
        }

    }
}