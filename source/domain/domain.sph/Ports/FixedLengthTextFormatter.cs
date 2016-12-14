using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class FixedLengthTextFormatter : TextFormatter
    {
        public override string GetRecordAttribute()
        {
            return "[FixedLengthRecord]";
        }

        public override string GetRecordMetadataCode()
        {
            return @"[FieldHidden]private int m_lineNumber;
public int GetLineNumber(){ return m_lineNumber;}
internal void SetLineNumber(int line){ m_lineNumber = line;}";
        }

        public override async Task<Class> GetPortClassAsync(ReceivePort port)
        {
            var type = await base.GetPortClassAsync(port);
            type.AddNamespaceImport<FileHelpers.DelimitedRecordAttribute>();

            var processCode = GenerateProcessCode(port);
            type.AddMethod(new Method { Code = processCode });

            return type;
        }


        private string GenerateProcessCode(ReceivePort port)
        {
            var code = new StringBuilder();
            var hasDetailRowsWithTag = this.HasTagIdentifier && this.DetailRowCollection.Count > 0;

            code.Append($"public IEnumerable<{port.Entity}> Process(IEnumerable<string> lines)");
            code.AppendLine("{");
            code.AppendLine($@"   var engine = new FileHelperEngine<{port.Entity}>();");


            code.AppendLine(hasDetailRowsWithTag
                ? GenerateProcessCodeDetailsWithTag(port)
                : GenerateProcessCode(port.Entity));

            code.AppendLine("} ");
            return code.ToString();
        }

        private string GenerateNormalizeLineCode()
        {
            return "var normalized = line;";
        }
        private string GenerateProcessCode(string portName)
        {
            var normalized = this.GenerateNormalizeLineCode();
            return $@"
                var count = 0;
                foreach(var line in lines)
                {{
                    count++;
                    {normalized}

                    {portName} record = null;
                    try
                    {{
                        record = engine.ReadString(normalized)[0]; 
                        this.ProcessHeader(record);
                        record.SetLineNumber(count);
                        System.Diagnostics.Debug.Assert(record.ToJson() != null, ""Fail to serialize to json"");                  
                    }}
                    catch (Exception e)
                    {{
                        Logger.Log(new LogEntry(e, new []{{ $""line : {{count}}"", $""{{Uri}}"", line}}){{ Message = $""Exception reading line {{count}}""}});
                        record = null;
                    }}
                    yield return record;
                }}
";
        }
        private string GenerateProcessCodeDetailsWithTag(ReceivePort port)
        {
            var normalized = this.GenerateNormalizeLineCode();
            var code = new StringBuilder();

            var childRecordCode = new StringBuilder();
            foreach (var row in this.DetailRowCollection)
            {
                var itemName = row.TypeName.ToCamelCase();

                code.AppendLine($@"var {itemName}Engine = new FileHelperEngine<{row.TypeName}>();");
                childRecordCode.Append($@"
                    if(line.StartsWith({row.RowTag.ToVerbatim()}))
                    {{
                        try
                        {{
                            var {itemName} = {itemName}Engine.ReadString(normalized)[0];                            
                            System.Diagnostics.Debug.Assert(consignment.ToJson() != null, ""Fail to serialize to json"");
                            if(!hasError)
                                record.{row.Name}.Add({itemName});
                        }}
                        catch (Exception e)
                        {{
                            Logger.Log(new LogEntry(e, new[] {{ $""line : {{count}}"", $""{{Uri}}"", line }}) {{ Message = $""Exception reading line {{count}}"" }});
                            hasError = true;
                        }}
                    }}
                ");
            }

            code.AppendLine($@"{port.Entity} record = null;
            var count = 0;
            var hasError = false;");
            code.AppendLine($@"
                foreach(var line in lines)
                {{
                    count++;
                    {normalized}
                    if (line.StartsWith({RecordTag.ToVerbatim()}) && hasError) // starts - new line and the previous record is error
                    {{
                        record = null;
                        yield return record;
                    }}
                    if(line.StartsWith({RecordTag.ToVerbatim()}) && null != record)
                         yield return record;    

                    if (line.StartsWith({RecordTag.ToVerbatim()}))
                    {{
                        hasError = false;
                        try
                        {{
                            record = engine.ReadString(normalized)[0];
                            this.ProcessHeader(record);
                            record.SetLineNumber(count);
                            System.Diagnostics.Debug.Assert(record.ToJson() != null, ""Fail to serialize to json"");
                        }}
                        catch (Exception e)
                        {{
                            Logger.Log(new LogEntry(e, new[] {{ $""line : {{count}}"", $""{{Uri}}"", line }}) {{ Message = $""Exception reading line {{count}}"" }});
                            hasError = true;
                        }}   

                    }} 
                    {childRecordCode}         
                }}");
            code.AppendLine("yield return record;");

            return code.ToString();
        }

        public override async Task<TextFieldMapping[]> GetFieldMappingsAsync()
        {
            var lines = await GetSampleLinesAsync();
            var number = 0;
            if (this.HasLabel)
                number = 1;
            var line = lines[number];
            var start = 0;
            foreach (var m in this.FieldMappingCollection)
            {
                try
                {
                    var value = line.Substring(start, m.Length);
                    if (m.TrimMode == "Both")
                        value = value.Trim();
                    if (m.TrimMode == "Left")
                        value = value.TrimStart();
                    if (m.TrimMode == "Right")
                        value = value.TrimEnd();

                    m.SampleValue = value;
                    start += m.Length;
                }
                catch (ArgumentOutOfRangeException)
                {
                    //ignore
                }
            }

            var list = new List<TextFieldMapping>(this.FieldMappingCollection);
            return list.ToArray();
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

    }
}