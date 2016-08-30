using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class DelimitedTextFormatter : TextFormatter
    {
        public override async Task<TextFieldMapping[]> GetFieldMappingsAsync()
        {
            var bs = ObjectBuilder.GetObject<IBinaryStore>();
            var file = await bs.GetContentAsync(this.SampleStoreId);
            var temp = Path.GetTempFileName();
            File.WriteAllBytes(temp, file.Content);
            var root = new List<TextFieldMapping>();

            var lines = File.ReadAllLines(temp);

            // details
            foreach (var row in this.DetailRowCollection)
            {
                var line = lines.FirstOrDefault(x => x.StartsWith(row.RowTag));
                if (string.IsNullOrEmpty(line)) continue;

                var columns = line.Split(new[] { this.Delimiter }, StringSplitOptions.None);
                var fields = columns.Select((col, i) => new DelimitedTextFieldMapping
                {
                    Column = i,
                    Path = $"DetailField_{i + 1}",
                    TypeName = typeof(string).GetShortAssemblyQualifiedName(),
                    SampleValue = col,
                    WebId = Guid.NewGuid().ToString()
                });
                var dr = new DelimitedTextFieldMapping
                {
                    Path = row.FieldName,
                    TypeName = row.Name,
                    IsComplex = true,
                    AllowMultiple = true
                };
                dr.FieldMappingCollection.AddRange(fields);
                if (row.Parent == "$root")
                {
                    root.Add(dr);
                    continue;
                }

                var parent = root.SingleOrDefault(x => x.Path == row.Parent);
                parent?.FieldMappingCollection.AddRange(dr);
            }

            var rootLine = lines.First(x => !this.HasTagIdentifier || (x.StartsWith(this.RecordTag)));
            var rootColumns = rootLine.Split(new[] { this.Delimiter }, StringSplitOptions.None);
            var rootFields = rootColumns.Select((col, i) => new DelimitedTextFieldMapping
            {
                Column = i,
                Path = $"Field_{i + 1}",
                TypeName = typeof(string).GetShortAssemblyQualifiedName(),
                SampleValue = col,
                WebId = Guid.NewGuid().ToString()
            });
            root.AddRange(rootFields);




            return root.ToArray();
        }
    }
}