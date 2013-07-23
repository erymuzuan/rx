using System;
using System.Xml;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public class DocumentField : Field
    {
        public DocumentField()
        {
            this.NamespacePrefix = "bs";
        }

        [System.Xml.Serialization.XmlAttribute]
        public string Path { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string NamespacePrefix { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public Type Type
        {
            get
            {
                return Type.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.AssemblyQualifiedName;
            }
        }
        [System.Xml.Serialization.XmlAttribute]
        public string TypeName { get; set; }

        public override object GetValue(Entity item)
        {
            var doc = new XmlDocument();
            var xml = item.ToXmlString();
            doc.LoadXml(xml);

            var ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace(this.NamespacePrefix, "http://www.bespoke.com.my/");

            var node = doc.SelectSingleNode(this.Path, ns);
            if (null == node) return null;
            if (Type == typeof(DateTime))
            {
                DateTime dv;
                if (DateTime.TryParse(node.Value, out dv))
                    return dv;
            }

            if (Type == typeof(int))
            {
                int dv;
                if (int.TryParse(node.Value, out dv))
                    return dv;
            }

            if (Type == typeof(double))
            {
                double dv;
                if (double.TryParse(node.Value, out dv))
                    return dv;
            }
            if (Type == typeof(float))
            {
                float dv;
                if (float.TryParse(node.Value, out dv))
                    return dv;
            }

            if (Type == typeof(decimal))
            {
                decimal dv;
                if (decimal.TryParse(node.Value, out dv))
                    return dv;
            }

            if (Type == typeof(bool))
            {
                bool dv;
                if (bool.TryParse(node.Value, out dv))
                    return dv;
            }

            if (Type == typeof(string))
            {
                return node.Value;
            }


            return null;

        }
    }
}