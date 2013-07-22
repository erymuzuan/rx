using System;
using System.Xml;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public class Rule
    {
        public Field Left { get; set; }

        public Operator Operator { get; set; }

        public Field Right { get; set; }

        public bool Execute(Entity item)
        {
            var left = this.Left.GetValue(item);
            var right = this.Right.GetValue(item);

            if (null == left) return false;
            if (null == right) return false;

            Console.WriteLine("Evaluate : ({3}){0} {1} ({4}){2}", left, Operator, right, left.GetType().Name, right.GetType().Name);

            var lc = left as IComparable;
            var rc = right as IComparable;
            if (null != lc && null != rc)
            {
                if (Operator == Operator.Equal)
                    return lc.CompareTo(rc) == 0;

                if (Operator == Operator.Lt)
                    return lc.CompareTo(rc) < 0;
                if (Operator == Operator.Le)
                    return lc.CompareTo(rc) <= 0;
                if (Operator == Operator.Gt)
                    return lc.CompareTo(rc) > 0;
                if (Operator == Operator.Ge)
                    return lc.CompareTo(rc) >= 0;
            }

            return false;
        }
    }

    public enum Operator
    {
        Equal,
        Lt,
        Le,
        Gt,
        Ge
    }

    public class FuctionField : Field
    {
        public string Script { get; set; }
        public override object GetValue(Entity item)
        {
            return this;
        }
    }

    public class ConstantField : Field
    {
        public object Value { get; set; }
        public override object GetValue(Entity item)
        {
            return this.Value;
        }
    }

    [System.Xml.Serialization.XmlInclude(typeof(DocumentField))]
    [System.Xml.Serialization.XmlInclude(typeof(FuctionField))]
    [System.Xml.Serialization.XmlInclude(typeof(ConstantField))]
    public abstract class Field : DomainObject
    {
        public virtual object GetValue(Entity item)
        {
            throw new NotImplementedException("who");
        }
    }

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
