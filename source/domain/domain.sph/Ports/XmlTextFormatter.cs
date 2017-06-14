using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                var doc = XDocument.Load(stream);
                var path = this.RootPath.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                XElement root = null;
                foreach (var p in path)
                {
                    root = null == root ? doc.Element(p) : root.Element(p);
                }

                if (null == root) throw new InvalidOperationException("Cannot find root for path " + this.RootPath);
                var children = GetMappings(root);
                fields.AddRange(children);

                foreach (var e in root.Elements().Where(x => x.HasElements))
                {
                    var field = children.OfType<XmlElementTextFieldMapping>().SingleOrDefault(x => x.Name == e.Name.LocalName);
                    AddChildFields(field, e);
                }
            }

            return fields.ToArray();
        }

        private static void AddChildFields(TextFieldMapping field, XElement parent)
        {
            var fields = GetMappings(parent);
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
                TypeName =  (x.HasAttributes || x.HasElements) ? x.Name.LocalName : x.Value.TryGuessType().GetShortAssemblyQualifiedName()
            }).ToArray();

            var list = new List<TextFieldMapping>();
            list.AddRange(attributes);
            list.AddRange(elements);
            return list.ToArray();
        }

        public string RootPath { get; set; }
    }
}