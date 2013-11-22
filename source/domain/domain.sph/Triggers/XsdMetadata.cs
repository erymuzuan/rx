using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Bespoke.Sph.Domain
{
    public class XsdMetadata
    {
        private readonly XElement m_xsd;
        // ReSharper disable InconsistentNaming
        static readonly XNamespace x = "http://www.w3.org/2001/XMLSchema";
        // ReSharper restore InconsistentNaming
        public XsdMetadata(XElement xsd)
        {
            m_xsd = xsd;
        }

        public string[] GetMembersPath(string elementOrComplexType)
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

            return GetMembers(ct).ToArray();
        }


        private IEnumerable<string> GetMembers(XElement ct)
        {
            var properties = new List<string>();
            if (null == ct) return properties;

            var attributes = from at in ct.Elements(x + "attribute")
                             let n = at.Attribute("name").Value
                             select n;
            properties.AddRange(attributes);

            var all = ct.Element(x + "all");
            if (null != all)
            {
                var allElements = from at in all.Elements(x + "element")
                                  where at.Attribute("name") != null
                                        && at.Attribute("type") != null
                                  select at.Attribute("name").Value;
                properties.AddRange(allElements);

                var collectionElements = from at in all.Elements(x + "element")
                                         where at.Attribute("name") != null
                                               && at.Attribute("type") == null
                                         let refElement = at.Descendants(x + "element").First()
                                         let type = refElement.Attribute("ref") == null ? refElement.Attribute("type").Value : refElement.Attribute("ref").Value
                                         select at.Attribute("name").Value;
                properties.AddRange(collectionElements);

                // TODO, get ref elements children
                var refElements = (from at in all.Elements(x + "element")
                                   where at.Attribute("ref") != null
                                   let refa = at.Attribute("ref")
                                   select refa.Value).ToArray();
                var refElementsChildren = refElements.Select(s => this.GetMembersPath(s).Select(u => s + "." + u)).SelectMany(s => s);
                properties.AddRange(refElements);
                properties.AddRange(refElementsChildren);

            }

            return properties;
        }
    }
}