using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Dictionary value extractor", FontAwesomeIcon = "copy", Category = FunctoidCategory.DATABASE)]
    public class DictionaryValueExtractor : Functoid
    {
        public override bool Initialize()
        {
            this.ArgumentCollection.Add(new FunctoidArg { Name = "key", Type = typeof(string) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "source", Type = typeof(string) });
            return base.Initialize();

        }

        public override async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errors = (await base.ValidateAsync()).ToList();
            var conn = this["connection"].GetFunctoid(this.TransformDefinition);
            if (null == conn)
                errors.Add(new ValidationError { Message = "We need the connection", PropertyName = "Connection" });

            if (string.IsNullOrWhiteSpace(this.Key))
                errors.Add(new ValidationError { PropertyName = nameof(Key), Message = "You will need to provide a \"Key\"" });
            return errors;
        }

        public override string GenerateAssignmentCode()
        {
            var typeName = Strings.GetType(this.OutputTypeName).ToCSharp();
            return $"({typeName})__result{this.Index}";
        }

        public override string GenerateStatementCode()
        {
            var code = new StringBuilder();
            
            var source = this["source"].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode();

            code.AppendLine($"var __source{Index} =  {source};");
            code.AppendLine($"var __row{Index} =  ({source}) as IDictionary<string,object>;");

            var defaultValue = this.DefaultValue;
            if (this.OutputTypeName == typeof(string).GetShortAssemblyQualifiedName())
                defaultValue = $"\"{this.DefaultValue}\"";

            code.AppendLine($"var __result{Index} = __row{Index} == null ? {defaultValue} : __row{Index}[\"{Key}\"]");

            return code.ToString();
        }
        public string Key { get; set; }
        public string DefaultValue { get; set; }

        public override string GetEditorView()
        {
            return Functoids.Properties.Resources.ValueExtractorView;
        }

        public override string GetEditorViewModel()
        {
            return Functoids.Properties.Resources.ValueExtractorViewModel;
        }
    }
}