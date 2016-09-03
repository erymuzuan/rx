using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class DelimitedTextFormatter : TextFormatter
    {
        public override string GetRecordAttribute()
        {
            return $@"[DelimitedRecord(""{Delimiter}"")]";
        }

        public override async Task<Class> GetPortClassAsync(ReceivePort port)
        {
            var type = await base.GetPortClassAsync(port);
            type.AddNamespaceImport<FileHelpers.DelimitedRecordAttribute, List<string>>();
            type.AddNamespaceImport<Match>();

            var processCode = GenerateProcessCode(port);
            type.AddMethod(new Method { Code = processCode });

            if (!string.IsNullOrWhiteSpace(this.EscapeCharacter))
            {
                var normalize = GenerateNormalizeMethod();
                type.AddMethod(new Method { Code = normalize });
            }
            return type;
        }

        private string GenerateNormalizeMethod()
        {
            var normalize = $@"
        private string Normalize(string text, string placeHolder)
        {{
            if (string.IsNullOrWhiteSpace({EscapeCharacter.ToVerbatim()}) || !text.Contains({EscapeCharacter.ToVerbatim()}))
                return text;
            const RegexOptions OPTIONS = RegexOptions.IgnoreCase | RegexOptions.Multiline;
            var pattern = $@""(?<a>{EscapeCharacter.EscapeVerbatim()}(.*?){EscapeCharacter.EscapeVerbatim()})(?<b>\s?({Delimiter
                .EscapeVerbatim()}|$))"";
            MatchEvaluator matchEvaluator = m => (m.Groups[""a""].Value.Replace({Delimiter.ToVerbatim()}, placeHolder) + m.Groups[""b""]).Replace({EscapeCharacter
                .ToVerbatim()}, """");
            return Regex.Replace(text, pattern, matchEvaluator, OPTIONS);
        }}";
            return normalize;
        }

        private string GenerateProcessCode(ReceivePort port)
        {
            var code = new StringBuilder();
            code.Append($"public IEnumerable<{port.Entity}> Process(IEnumerable<string> lines)");
            code.AppendLine($@"
                    {{
                         var engine = new FileHelperEngine<{port.Entity}>();
                ");

            foreach (var row in this.DetailRowCollection)
            {
                var itemName = row.TypeName.ToCamelCase();
                code.AppendLine($@"var {itemName}Engine = new FileHelperEngine<{row.TypeName}>();");
            }

            var childRecordCode = new StringBuilder();
            foreach (var row in this.DetailRowCollection)
            {
                var itemName = row.TypeName.ToCamelCase();
                childRecordCode.Append($@"
                    if(line.StartsWith({row.RowTag.ToVerbatim()}))
                    {{
                        var {itemName} = {itemName}Engine.ReadString(normalized)[0];
                        record.{row.Name}.Add({itemName});
                    }}
                ");
            }

            code.AppendLine($@"
                var placeHolder = new string('x', 15);
                {port.Entity} record = null;");
            var normalized = "var normalized = line;";
            if (!string.IsNullOrWhiteSpace(this.EscapeCharacter))
                normalized = $@"
                    var normalized = line;
                    var hasEscape = line.Contains({EscapeCharacter.ToVerbatim()});
                    if (hasEscape)
                        normalized = Normalize(line, placeHolder);";
            if (this.HasTagIdentifier && this.DetailRowCollection.Count > 0)
            {
                code.AppendLine($@"
                foreach(var line in lines)
                {{
                    {normalized}
                    if(line.StartsWith({RecordTag.ToVerbatim()}) && null != record)
                         yield return record;                 
                    if(line.StartsWith({RecordTag.ToVerbatim()}))
                         record = engine.ReadString(normalized)[0];  
                    {childRecordCode}         
                }}");
            }
            else
            {
                code.AppendLine($@"
                {{
                    {normalized}
                    yield return engine.ReadString(normalized)[0];  
                }}

");
            }

            code.AppendLine("        } ");
            return code.ToString();
        }

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
                    Name = row.Name,
                    TypeName = row.TypeName,
                    IsComplex = true,
                    AllowMultiple = true
                };
                dr.FieldMappingCollection.AddRange(fields);
                if (row.Parent == "$record")
                {
                    root.Add(dr);
                    continue;
                }
                //TODO : the order of the details do matter for grand children
                var parent = root.SingleOrDefault(x => x.Name == row.Parent);
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
            var hasEscape = !string.IsNullOrWhiteSpace(this.EscapeCharacter) && line.Contains(this.EscapeCharacter);
            if (hasEscape)
                columns = Normalize(line, placeHolder).Split(new[] { this.Delimiter }, StringSplitOptions.None);
            var fields = columns.Select((col, i) => new DelimitedTextFieldMapping
            {
                Column = i,
                Name = labels[i],
                TypeName = typeof(string).GetShortAssemblyQualifiedName(),
                SampleValue = hasEscape ? col.Replace(placeHolder, this.Delimiter).Trim() : col,
                WebId = Guid.NewGuid().ToString()
            });
            return fields;
        }

        private string Normalize(string text, string placeHolder)
        {
            if (string.IsNullOrWhiteSpace(this.EscapeCharacter) || !text.Contains(this.EscapeCharacter))
                return text;
            const RegexOptions OPTIONS = RegexOptions.IgnoreCase | RegexOptions.Multiline;
            var pattern = $@"(?<a>{this.EscapeCharacter}(.*?){this.EscapeCharacter})(?<b>\s?({this.Delimiter}|$))";
            MatchEvaluator matchEvaluator = m => (m.Groups["a"].Value.Replace(this.Delimiter, placeHolder) + m.Groups["b"]).Replace(this.EscapeCharacter, "");
            return Regex.Replace(text, pattern, matchEvaluator, OPTIONS);
        }

    }
}