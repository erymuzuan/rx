using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class XmlTextFormatter : TextFormatter
    {
        public override async Task<TextFieldMapping[]> GetFieldMappingsAsync()
        {
            var fields = new List<TextFieldMapping>();
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var bin = await store.GetContentAsync(this.SampleStoreId);


            using (var stream = new MemoryStream(bin.Content))
            {
                var doc = XElement.Load(stream);
                var xn = doc.GetDefaultNamespace();
                XElement root = null;
                foreach (var path in this.RootPath.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    root = root == null ? doc.Element(xn + path) : root.Element(xn + path);
                }
                if (null == root) throw new InvalidOperationException($"Cannot query element {this.RootPath}");

                var children = GetMappings(root);
                fields.AddRange(children);

                foreach (var e in root.Elements().Where(x => x.HasElements))
                {
                    var field = children.OfType<XmlElementTextFieldMapping>().FirstOrDefault(x => x.Name == e.Name.LocalName);
                    AddChildFields(field, e);
                }
            }

            return fields.ToArray();
        }

        private static void AddChildFields(TextFieldMapping field, XElement parent)
        {
            var fields = GetMappings(parent).Where(x => field.FieldMappingCollection.All(y => y.Name != x.Name)).ToArray();
            field.FieldMappingCollection.AddRange(fields);

            foreach (var ex in parent.Elements().Where(x => x.HasElements))
            {
                var field2 = fields.OfType<XmlElementTextFieldMapping>().SingleOrDefault(x => x.Name == ex.Name.LocalName);
                AddChildFields(field2, ex);
            }
        }
        private static TextFieldMapping[] GetMappings(XElement parent)
        {
            var attributes = parent.Attributes().Where(x => !x.ToString().StartsWith("xmlns:")).Select(x => new XmlAttributeTextFieldMapping
            {
                AllowMultiple = false,
                Name = x.Name.LocalName,
                IsComplex = false,
                IsNullable = false,
                SampleValue = x.Value,
                Type = x.Value.TryGuessType()
            }).ToArray();
            var elements = parent.Elements().Select(x => new XmlElementTextFieldMapping(x)
            {
                AllowMultiple = false,
                Name = x.Name.LocalName,
                IsComplex = x.HasAttributes || x.HasElements,
                IsNullable = false,
                SampleValue = x.Value,
                TypeName = (x.HasAttributes || x.HasElements) ? x.Name.LocalName : x.Value.TryGuessType().GetShortAssemblyQualifiedName()
            }).ToList();

            // see which of the elements are array
            var groups = parent.Elements().Select(x => x.Name.LocalName).GroupBy(x => x).Where(x => x.Count() > 1);
            foreach (var g in groups)
            {
                var ef = elements.First(x => x.Name == g.Key);
                ef.AllowMultiple = true;
                var removed = elements.RemoveAll(x => x.Name == g.Key && x.AllowMultiple == false);
                System.Diagnostics.Debug.Assert(removed > 0);
            }

            var list = new List<TextFieldMapping>();
            list.AddRange(attributes);
            list.AddRange(elements);
            return list.ToArray();
        }

        public string RootPath { get; set; }


        public override string GetRecordMetadataCode()
        {
            return "//metadata";
        }

        public override async Task<Class> GetPortClassAsync(ReceivePort port)
        {
            var type = await base.GetPortClassAsync(port);
            type.AddNamespaceImport<XDocument>();

            var processCode = GenerateProcessCode(port);
            type.AddMethod(new Method { Code = processCode });

            return type;
        }


        private string GenerateProcessCode(ReceivePort port)
        {
            var code = new StringBuilder();

            var elementQueries = "";
            var paths = this.RootPath.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            for (var i = 1; i < paths.Count; i++)
            {
                elementQueries += $@".Elements(xn + ""{paths[i]}"")";
            }

            var collectionCode = new StringBuilder();
            foreach (var xe in port.FieldMappingCollection.Where(x => x.AllowMultiple).OfType<XmlElementTextFieldMapping>())
            {
                var initializer = xe.FieldMappingCollection.OfType<XmlElementTextFieldMapping>().ToString(",\r\n", x => x.Name + " = " + x.GenerateReadValueCode("ce"));
                collectionCode.AppendLine($@"foreach(var ce in e.Elements(xn + ""{xe.Name}""))");
                collectionCode.AppendLine("{");
                collectionCode.AppendLine($@"record.{xe.Name}.Add(new {xe.Name}{{
                                {initializer}
                    }});");
                collectionCode.AppendLine("}");
            }


            code.Append($"public IEnumerable<{port.Entity}> Process(IEnumerable<string> lines)");
            code.AppendLine("{");

            var elementProperties = port.FieldMappingCollection.Where(x => !x.AllowMultiple).OfType<XmlElementTextFieldMapping>().ToString(",\r\n", x => x.Name + " = " + x.GenerateReadValueCode("e"));
            var attributeProperties = port.FieldMappingCollection.OfType<XmlAttributeTextFieldMapping>().ToString(",\r\n", x => x.Name + " = " + x.GenerateReadValueCode("e"));
            var comma = port.FieldMappingCollection.OfType<XmlAttributeTextFieldMapping>().Any() ? "," : "";
            code.AppendLine($@"
            var text = string.Join(""\r\n"", lines);
            var doc = XElement.Parse(text);
            var xn =  doc.GetDefaultNamespace();

            var elements = doc{elementQueries};
            foreach (var e in elements)
            {{
                var record = new {port.Entity}
                {{
                    {elementProperties}{comma}
                    {attributeProperties}
                                       
                }};
                //TODO : AllowMultiple properties
                {collectionCode}
                yield return record;
            }}

            ");



            code.AppendLine("} ");
            return code.ToString();
        }

    }
}