using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public class CsharpCodeGenerator
    {
        private readonly XElement m_xsd;
        // ReSharper disable InconsistentNaming
        static readonly XNamespace x = "http://www.w3.org/2001/XMLSchema";
        // ReSharper restore InconsistentNaming

        public CsharpCodeGenerator(XElement xsd)
        {
            m_xsd = xsd;
        }

        public IEnumerable<Class> Generate()
        {
            var @classes = new List<Class>();

            var complexTypesElement = m_xsd.Elements(x + "complexType").ToList();
            var complexTypeClasses = complexTypesElement
                .Where(e => null != e.Attribute("name"))
                .Select(e => e.Attribute("name").Value)
                .Select(this.GetClassDefinition)
                .ToList();
            @classes.AddRange(complexTypeClasses);

            var elements = m_xsd.Elements(x + "element").ToList();
            var elementClasses = elements.Where(e => null != e.Attribute("name"))
                .Select(e => e.Attribute("name").Value)
                .Select(this.GetClassDefinition)
                .ToList();
            @classes.AddRange(elementClasses);



            return @classes;
        }

        public Class GetClassDefinition(string elementOrComplexType)
        {

            var element = m_xsd.Elements(x + "element")
                .Where(e => null != e.Attribute("name"))
                .SingleOrDefault(e => e.Attribute("name").Value == elementOrComplexType);
            var ct = m_xsd.Element(x + "complexType");
            if (null != element)
            {
                ct = element.Element(x + "complexType");
            }
            if (null == ct) throw new InvalidOperationException("cannot find complextType element in " + elementOrComplexType);

            var @class = new Class { Name = elementOrComplexType, BaseClass = "DomainObject", FileName = elementOrComplexType + ".cs" };
            var name = elementOrComplexType;
            var members = new List<Property>();
            // for extensions
            var extension = ct.Descendants(x + "extension").FirstOrDefault();
            if (null != extension)
            {
                @class.BaseClass = extension.Attribute("base").Value;
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
                             let n = at.Attribute("name").Value
                             select new Property
                             {
                                 Name = n,
                                 Type = Strings.GetType(ComplexVariable.GetClrDataType(at)),
                                 Code =  "      [XmlAttribute]\r\n"
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
                        Name = at.Attribute("name").Value,
                        Code =  string.Format("      public {1} {0} {{get;set;}}", at.Attribute("name").Value, ComplexVariable.GetClrDataType(at))
                    };
                properties.AddRange(allElements);

                var collectionElements = from at in all.Elements(x + "element")
                    where at.Attribute("name") != null
                          && at.Attribute("type") == null
                    let refElement = at.Descendants(x + "element").First()
                    let type =
                        refElement.Attribute("ref") == null
                            ? refElement.Attribute("type").Value
                            : refElement.Attribute("ref").Value
                    select new Property
                    {
                        Code = string.Format("      private readonly ObjectCollection<{1}> m_{0} = new ObjectCollection<{1}>();\r\n" +
                                                              "      public ObjectCollection<{1}> {0} {{get {{ return m_{0};}} }}", at.Attribute("name").Value, type),
                                                              Name = at.Attribute("name").Value
                    };
                properties.AddRange(collectionElements);

                var refElements = from at in all.Elements(x + "element")
                    where at.Attribute("ref") != null
                    let refa = at.Attribute("ref")
                    select new Property
                    {
                        Code = string.Format("      private  {0} m_{0} = new {0}();\r\n" +
                                             "      public {0} {0}{{get{{ return m_{0};}} set{{ m_{0} = value;}} }}",
                            refa.Value)
                    };
                properties.AddRange(refElements);

            }

            return properties;
        }
    }
}