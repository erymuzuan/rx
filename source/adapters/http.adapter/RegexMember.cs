using System;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class RegexMember : Member
    {
        private string m_fieldName;
        public string Pattern { get; set; }
        public string Group { get; set; }
        public string DateFormat { get; set; }
        public string NumberFormat { get; set; }
        [JsonIgnore]
        public Type Type
        {
            get
            {
                return Strings.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }
        public string TypeName { get; set; }
        public bool IsNullable { get; set; }

        public string FieldName
        {
            get { return m_fieldName; }
            set
            {
                m_fieldName = value;
                if (string.IsNullOrWhiteSpace(this.Name) && !string.IsNullOrWhiteSpace(value))
                    this.Name = value
                        .Replace(".","_")
                        .Replace(",","_")
                        .Replace("/","_")
                        .Replace("*","_")
                        .Replace("-","_")
                        .Replace("!","_")
                        .Replace("@","_")
                        .Replace("$","_")
                        .Replace("^","_")
                        .Replace("(","_")
                        .Replace(")","_")
                        .Replace("{","_")
                        .Replace("}","_")
                        .Replace("[","_")
                        .Replace("]","_")
                        .Replace("`","_")
                        .Replace(";","_")
                        .Replace("'","_")
                        .Replace(":","_");
            }
        }

        public string GenerateParseCode(string parentPath, RegexMember member = null)
        {
            var code = new StringBuilder();
            var m = this;
            if (null != member) m = member;

            if (m.Type == typeof(string))
                code.AppendLinf("           {3}.{0} = Strings.RegexSingleValue(this.ResponseText, {1}, \"{2}\");", m.Name, m.Pattern.ToLiteral(), m.Group, parentPath);

            if (m.Type == typeof(decimal) && m.IsNullable)
                code.AppendLinf("           {3}.{0} = Strings.RegexDecimalValue(this.ResponseText, {1}, \"{2}\");", m.Name, m.Pattern.ToLiteral(), m.Group, parentPath);
            if (m.Type == typeof(decimal) && !m.IsNullable)
                code.AppendLinf("           {3}.{0} = Strings.RegexDecimalValue(this.ResponseText, {1}, \"{2}\") ?? decimal.Zero;", m.Name, m.Pattern.ToLiteral(), m.Group, parentPath);

            if (m.Type == typeof(int) && m.IsNullable)
                code.AppendLinf("           {3}.{0} = Strings.RegexInt32Value(this.ResponseText, {1}, \"{2}\");", m.Name, m.Pattern.ToLiteral(), m.Group, parentPath);
            if (m.Type == typeof(int) && !m.IsNullable)
                code.AppendLinf("           {3}.{0} = Strings.RegexInt32Value(this.ResponseText, {1}, \"{2}\") ?? 0;", m.Name, m.Pattern.ToLiteral(), m.Group, parentPath);


            if (m.Type == typeof(DateTime) && m.IsNullable)
                code.AppendLinf("           {4}.{0} = Strings.RegexDateTimeValue(this.ResponseText, {1}, \"{2}\", \"{3}\");", m.Name, m.Pattern.ToLiteral(), m.Group, m.DateFormat, parentPath);
            if (m.Type == typeof(DateTime) && !m.IsNullable)
                code.AppendLinf("           {4}.{0} = Strings.RegexDateTimeValue(this.ResponseText, {1}, \"{2}\", \"{3}\") ?? DateTime.MinValue;", m.Name, m.Pattern.ToLiteral(), m.Group, m.DateFormat, parentPath);


            var objectsChildren = from mc in m.MemberCollection.OfType<RegexMember>()
                                  where mc.Type == typeof(object)
                                  select mc.GenerateParseCode(parentPath + "." + m.Name);
            objectsChildren.ToList().ForEach(x => code.AppendLine(x));


            return code.ToString();
        }
    }
}