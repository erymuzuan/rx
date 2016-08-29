using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class DelimitedTextFormatter : TextFormatter
    {
        public override async Task<TextFieldMapping[]> PopulateMappingsAsync()
        {
            var bs = ObjectBuilder.GetObject<IBinaryStore>();
            var file = await bs.GetContentAsync(this.SampleStoreId);
            var temp = Path.GetTempFileName();
            File.WriteAllBytes(temp, file.Content);
            var list = new List<TextFieldMapping>();

            var lines = File.ReadAllLines(temp);
            foreach (var line in lines)
            {
                if (this.HasTagIdentifier && line.StartsWith(this.RecordTag))
                {
                    var columns = line.Split(new[] { this.Delimiter }, StringSplitOptions.None);
                    var fields = columns.Select((col, i) => new DelimitedTextFieldMapping
                    {
                        Column = i,
                        Path = $"Field_{i}",
                        WebId = Guid.NewGuid().ToString()
                    });
                    list.AddRange(fields);

                    return list.ToArray();
                }
            }

            return Array.Empty<TextFieldMapping>();
        }
    }
}