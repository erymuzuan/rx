using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public class CsharpCodeGenerator
    {
        private readonly XElement m_xsd;
        private readonly string m_codeNamespace;
        // ReSharper disable InconsistentNaming
        static readonly XNamespace x = "http://www.w3.org/2001/XMLSchema";
        // ReSharper restore InconsistentNaming

        public CsharpCodeGenerator(XElement xsd, string codeNamespace)
        {
            m_xsd = xsd;
            m_codeNamespace = codeNamespace;
        }

        public IEnumerable<Class> Generate()
        {
            var @classes = new List<Class>();

            var complexTypesElement = m_xsd.Elements(x + "complexType").ToList();
            var complexTypeClasses = complexTypesElement
                .Where(e => null != e.Attribute("name"))
                .Select(e => e.Attribute("name")?.Value)
                .Select(this.GetClassDefinition)
                .ToList();
            @classes.AddRange(complexTypeClasses);

            var elements = m_xsd.Elements(x + "element").ToList();
            var elementClasses = elements.Where(e => null != e.Attribute("name"))
                .Select(e => e.Attribute("name")?.Value)
                .Select(this.GetClassDefinition)
                .ToList();
            @classes.AddRange(elementClasses);

            return @classes;
        }

        public Class GetClassDefinition(string elementOrComplexType)
        {

            var element = m_xsd.Elements(x + "element")
                .Where(e => null != e.Attribute("name"))
                .SingleOrDefault(e => e.Attribute("name")?.Value == elementOrComplexType);
            var ct = m_xsd.Element(x + "complexType");
            if (null != element)
            {
                ct = element.Element(x + "complexType");
            }
            if (null == ct) throw new InvalidOperationException("cannot find complextType element in " + elementOrComplexType);

            var @class = new Class
            {
                Name = elementOrComplexType,
                BaseClass = "DomainObject",
                Namespace = m_codeNamespace,
                FileName = elementOrComplexType + ".cs"
            };
            @class.AddNamespaceImport(typeof(DateTime));
            @class.AddNamespaceImport(typeof(DomainObject));
            @class.AddNamespaceImport(typeof(XmlTypeAttribute));

            var name = elementOrComplexType;
            var members = new List<Property>();
            // for extensions
            var extension = ct.Descendants(x + "extension").FirstOrDefault();
            if (null != extension)
            {
                @class.BaseClass = extension.Attribute("base")?.Value;
                members.AddRange(this.GetMembers(extension));
            }
            var ns = m_xsd.Attribute("targetNamespace");

            @class.AttributeCollection.Add($"   [XmlType(\"{name}\",  Namespace=\"{ns?.Value ?? ""}\")]");


            members.AddRange(this.GetMembers(ct));

            @class.PropertyCollection.AddRange(members);

            return @class;
        }

        private IEnumerable<Property> GetMembers(XElement ct)
        {
            var properties = new List<Property>();
            if (null == ct) return properties;

            var attributes = from at in ct.Elements(x + "attribute")
                             let n = at.Attribute("name")?.Value
                             select new Property
                             {
                                 Name = n,
                                 Type = Strings.GetType(ComplexVariable.GetClrDataType(at)),
                                 Code = "      [XmlAttribute]\r\n"
                                         + string.Format("      public {1} {0} {{get;set;}}", n, ComplexVariable.GetClrDataType(at))
                             };

            properties.AddRange(attributes);

            var all = ct.Element(x + "all");
            if (null != all)
            {
                var allElements = from at in all.Elements(x + "element")
                                  where at.Attribute("name") != null
                                        && at.Attribute("type") != null
                                  select new Property
                                  {
                                      Name = at.Attribute("name")?.Value,
                                      Code = string.Format("      public {1} {0} {{get;set;}}", at.Attribute("name")?.Value, ComplexVariable.GetClrDataType(at))
                                  };
                properties.AddRange(allElements);

                var collectionElements = from at in all.Elements(x + "element")
                                         where at.Attribute("name") != null
                                               && at.Attribute("type") == null
                                         let refElement = at.Descendants(x + "element").First()
                                         let type =
                                             refElement.Attribute("ref") == null
                                                 ? refElement.Attribute("type")?.Value
                                                 : refElement.Attribute("ref")?.Value
                                         select new Property
                                         {
                                             Code =
                                                 $"      private readonly ObjectCollection<{type}> m_{at.Attribute("name")?.Value} = new ObjectCollection<{type}>();\r\n" +
                                                 $"      public ObjectCollection<{type}> {at.Attribute("name")?.Value} {{get {{ return m_{at.Attribute("name")?.Value};}} }}",
                                             Name = at.Attribute("name")?.Value
                                         };
                properties.AddRange(collectionElements);

                var refElements = from at in all.Elements(x + "element")
                                  where at.Attribute("ref") != null
                                  let refa = at.Attribute("ref")
                                  select new Property
                                  {
                                      Code = $"      private  {refa.Value} m_{refa.Value} = new {refa.Value}();\r\n" +
                                             $"      public {refa.Value} {refa.Value}{{get{{ return m_{refa.Value};}} set{{ m_{refa.Value} = value;}} }}"
                                  };
                properties.AddRange(refElements);

            }

            return properties;
        }
    }
}