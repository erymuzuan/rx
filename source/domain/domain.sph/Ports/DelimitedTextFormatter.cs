using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class DelimitedTextFormatter : TextFormatter
    {

        public override async Task<TextFieldMapping[]> GetFieldMappingsAsync()
        {
            var root = new List<TextFieldMapping>();
            var lines = await GetSampleLinesAsync();

            // details
            foreach (var row in this.DetailRowCollection)
            {
                var line = lines.FirstOrDefault(x => x.StartsWith(row.RowTag));
                var fields = await GetFieldsFromLineAsync(line);
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
                //TODO : the order of the details do matter, when grand children
                var parent = root.SingleOrDefault(x => x.TypeName == row.Parent);
                parent?.FieldMappingCollection.AddRange(dr);
            }


            var rootLine = lines.First(x => !this.HasTagIdentifier || (x.StartsWith(this.RecordTag)));
            var rootFields = await GetFieldsFromLineAsync(rootLine);
            root.AddRange(rootFields);

            return root.ToArray();
        }

        private string[] m_lines;
        private async Task<string[]> GetSampleLinesAsync()
        {
            if (null != m_lines) return m_lines;
            var bs = ObjectBuilder.GetObject<IBinaryStore>();
            var file = await bs.GetContentAsync(this.SampleStoreId);
            var temp = Path.GetTempFileName();
            File.WriteAllBytes(temp, file.Content);
            m_lines = File.ReadAllLines(temp);

            return m_lines;
        }

        private async Task<string[]> GetFieldLabelsAsync()
        {
            var lines = await GetSampleLinesAsync();
            var labels = lines.First().Split(new[] { this.Delimiter }, StringSplitOptions.None)
                .Select(x => x.ToPascalCase()).ToArray();
            if (!this.HasLabel)
                labels = Enumerable.Range(1, labels.Length).Select(i => $"Field_{i}").ToArray();

            return labels;
        }
        private async Task<IEnumerable<DelimitedTextFieldMapping>> GetFieldsFromLineAsync(string line)
        {
            var labels = await GetFieldLabelsAsync();
            var placeHolder = new string('x', 15);
            var columns = line.Split(new[] { this.Delimiter }, StringSplitOptions.None);
            var hasEscape = line.Contains(this.EscapeCharacter);
            if (hasEscape)
                columns = Normalize(line, placeHolder).Split(new[] { this.Delimiter }, StringSplitOptions.None);
            var fields = columns.Select((col, i) => new DelimitedTextFieldMapping
            {
                Column = i,
                Path = labels[i],
                TypeName = typeof(string).GetShortAssemblyQualifiedName(),
                SampleValue = hasEscape ? col.Replace(placeHolder, this.Delimiter).Trim() : col,
                WebId = Guid.NewGuid().ToString()
            });
            return fields;
        }

        private string Normalize(string text, string placeHolder)
        {
            if (!text.Contains(this.EscapeCharacter))
                return text;
            const RegexOptions OPTIONS = RegexOptions.IgnoreCase | RegexOptions.Multiline;
            var pattern = $@"(?<a>{this.EscapeCharacter}(.*?){this.EscapeCharacter})(?<b>\s?({this.Delimiter}|$))";
            MatchEvaluator matchEvaluator = m => (m.Groups["a"].Value.Replace(this.Delimiter, placeHolder) + m.Groups["b"]).Replace(this.EscapeCharacter, "");
            return Regex.Replace(text, pattern, matchEvaluator, OPTIONS);
        }
    }
}