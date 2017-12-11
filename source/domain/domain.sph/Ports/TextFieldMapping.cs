using System;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("Path = {Name}({SampleValue}), TypeName= {TypeName}")]
    public partial class TextFieldMapping : DomainObject
    {
        [JsonIgnore]
        public virtual Type Type
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
        public virtual Member GenerateMember()
        {
            var simple = new SimpleMember
            {
                Name = this.Name,
                TypeName = this.TypeName,
                AllowMultiple = false,
                IsNullable = this.IsNullable

            };
            if (!this.IsComplex) return simple;

            var complex = new ComplexMember
            {
                Name = this.Name,
                TypeName = this.TypeName,
                AllowMultiple = this.AllowMultiple
            };
            var children = from f in this.FieldMappingCollection
                           select f.GenerateMember();
            complex.MemberCollection.AddRange(children);
            return complex;
        }
        internal virtual string GenerateReadExpressionCode(string rawName = "")
        {
            if (string.IsNullOrWhiteSpace(rawName))
                rawName = $"{Name.ToCamelCase()}Raw";
            if (this.IsNullable)
                throw new InvalidOperationException("This method is meant to generate property expression for Non nullable field");
            
            if (this.Type == typeof(string)) return GenerateReadStringExpression(rawName);
            if (this.Type == typeof(DateTime)) return GenerateReadDateTimeExpression(rawName);
            if (this.Type == typeof(bool)) return GenerateReadBooleanExpression(rawName);
            if (this.Type == typeof(decimal)) return GenerateReadDecimalExpression(rawName);
            if (this.Type == typeof(int)) return GenerateReadInt32Expression(rawName);
            throw new NotSupportedException($"{TypeName} type is not supported");

        }

        internal virtual string GenerateNullableReadCode(string rawName = "")
        {
            if (!this.IsNullable)
                throw new InvalidOperationException("This method is meant to generate property getter code Nullable field");
            if (string.IsNullOrWhiteSpace(rawName))
                rawName = $"{Name.ToCamelCase()}Raw";
            if (this.Type == typeof(string))
                return GenerateReadNullableString(rawName);
            if (this.Type == typeof(DateTime))
                return GenerateReadNullableDateTime(rawName);
            if (this.Type == typeof(int))
                return GenerateReadNullableInt32(rawName);
            if (this.Type == typeof(decimal))
                return GenerateReadNullableDecimal(rawName);
            if (this.Type == typeof(bool))
                return GenerateReadNullableBoolean(rawName);
            throw new NotSupportedException($"{TypeName} type is not supported");
        }

        protected virtual string GenerateReadNullableDateTime(string rawName)
        {
            return $@"        return System.DateTime.ParseExact({rawName},{Converter.ToVerbatim()}, System.Globalization.CultureInfo.InvariantCulture);";
        }

        protected virtual string GenerateReadNullableString(string rawName)
        {
            return $@"        return {rawName};";
        }

        protected virtual string GenerateReadNullableInt32(string rawName)
        {
            // TODO : parse according to format
            return $@"        
                                        var n = 0;
                                        if(int.TryParse({rawName}, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out n)) return n;
                                        return null;";
        }

        /// <summary>
        ///  assuming Converter is the True string
        /// </summary>
        /// <returns></returns>
        protected virtual string GenerateReadNullableBoolean(string rawName)
        {
            return $@"  return {rawName} == {Converter.ToVerbatim()};";
        }

        protected virtual string GenerateReadNullableDecimal(string rawName)
        {
            //TODO : parse according to format
            return $@"        
                                        var n = 0m;
                                        if(decimal.TryParse({rawName}, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out n)) return n;
                                        return null;";
        }

        protected virtual string GenerateReadInt32Expression(string rawName)
        {
            return $"int.Parse({rawName}, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture)";
        }

        protected virtual string GenerateReadDecimalExpression(string rawName)
        {
            return $"decimal.Parse({rawName}, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture)";
        }

        protected virtual string GenerateReadBooleanExpression(string rawName)
        {
            return $"{rawName} == {Converter.ToVerbatim()}";
        }

        protected virtual string GenerateReadDateTimeExpression(string rawName)
        {
            return $"System.DateTime.ParseExact({rawName}, {Converter.ToVerbatim()}, System.Globalization.CultureInfo.InvariantCulture)";
        }
        protected virtual string GenerateReadStringExpression(string rawName)
        {
            return rawName;
        }
    }
}