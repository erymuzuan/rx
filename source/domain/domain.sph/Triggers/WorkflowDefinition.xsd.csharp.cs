using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class WorkflowDefinition
    {
        private XElement m_customSchema;
        // ReSharper disable InconsistentNaming
        static readonly XNamespace x = "http://www.w3.org/2001/XMLSchema";
        // ReSharper restore InconsistentNaming


        public string GenerateXsdCsharpClasses()
        {
            var gen = new CsharpCodeGenerator(this.GetCustomSchema());
            return gen.Generate();
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
                case "State":
                    type = "State";
                    break;
                case "xs:anySimpleType":
                    type = "object";
                    break;
                default: throw new InvalidOperationException("Xml data type [" + xsType + "] is not supported");
            }
            if (nillable) type += "?";
            return type;
        }


        public XElement GetCustomSchema(string id = null)
        {
            if (null != m_customSchema) return m_customSchema;

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var content = store.GetContent(id ?? this.SchemaStoreId);
            if (null == content) return null;
            using (var stream = new MemoryStream(content.Content))
            {
                m_customSchema = XElement.Load(stream);
                return m_customSchema;
            }
        }

        public List<string> GetCustomSchemaElementNames(string name)
        {
            var xsd = this.GetCustomSchema();
            var elements = xsd.Elements(x + "element").Select(e => e.Attribute("name").Value).ToList();
            return elements;
        }

    }
}