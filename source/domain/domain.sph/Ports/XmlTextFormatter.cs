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
                var attributes = root.Attributes().Where(x => !x.ToString().StartsWith("xmlns:")).Select(x => new TextFieldMapping
                {
                    AllowMultiple = false,
                    Name = x.Name.LocalName,
                    IsComplex = false,
                    IsNullable = false,
                    SampleValue = x.Value,
                    Type = typeof(string)
                }).ToArray();

                var elements = root.Elements().Select(x => new TextFieldMapping
                {
                    AllowMultiple = false,
                    Name = x.Name.LocalName,
                    IsComplex = x.HasAttributes || x.HasElements,
                    IsNullable = false,
                    SampleValue = x.Value,
                    Type = (x.HasAttributes || x.HasElements) ? typeof(object) : typeof(string)
                });
                fields.AddRange(attributes);
                fields.AddRange(elements);

            }



            return fields.ToArray();
        }

        public string RootPath { get; set; }
    }
}