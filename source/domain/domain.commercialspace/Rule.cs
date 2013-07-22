using System;
using System.Xml;
using System.Xml.Linq;

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

            Console.WriteLine("Evaluate : {0} {1} {2}", left, Operator, right);
            if (Operator == Operator.Equal)
                return left.Equals(right);
            if (Operator == Operator.Lt)
                return left.Equals(right);

            return false;
        }
    }

    public enum Operator
    {
        Equal,
        Lt
    }

    public class FuctionField : Field
    {
        public string Script { get; set; }
        public override object GetValue(Entity item)
        {
            return this;
        }
    }

    public class ConstantField :Field
    {
        public object Value { get; set; }
        public override object GetValue(Entity item)
        {
            return this.Value;
        }
    }

    public abstract class Field
    {
        public virtual object GetValue(Entity item)
        {
            throw new NotImplementedException("wjo");
        }
    }

    public class DocumentField : Field
    {
        public string Path { get; set; }
        public Type Type { get; set; }

        public override object GetValue(Entity item)
        {
            var doc = new XmlDocument();
            var xml = item.ToXmlString();
            doc.LoadXml(xml);

            var ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("bs", "http://www.bespoke.com.my/");

            var node = doc.SelectSingleNode(this.Path,ns);
            if (null == node) return null;
            if(Type ==typeof(DateTime))
            {
                DateTime dv;
                if (DateTime.TryParse(node.Value, out dv))
                    return dv;
            }
            return null;

        }
    }
}
