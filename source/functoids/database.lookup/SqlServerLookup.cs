using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Sql lookup", FontAwesomeIcon = "database", Category = FunctoidCategory.DATABASE)]
    public class SqlServerLookup : Functoid
    {
        public override bool Initialize()
        {
            this.ArgumentCollection.Add(new FunctoidArg { Name = "connection", Type = typeof(string) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "value1", Type = typeof(string), IsOptional = true });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "value2", Type = typeof(string), IsOptional = true });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "value3", Type = typeof(string), IsOptional = true });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "value4", Type = typeof(string), IsOptional = true });
            return base.Initialize();

        }

        public override async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errors = (await base.ValidateAsync()).ToList();
            var conn = this["connection"].GetFunctoid(this.TransformDefinition);
            if (null == conn)
                errors.Add(new ValidationError { Message = "We need the connection", PropertyName = "Connection" });

            if (string.IsNullOrWhiteSpace(this.DefaultValue))
                errors.Add(new ValidationError { PropertyName = "DefaultValue", Message = "You will need to provide a default value for non nullable destination" });
            if (string.IsNullOrWhiteSpace(this.SqlText))
                errors.Add(new ValidationError { PropertyName = "SqlText", Message = "You will need to provide a \"SqlText\"" });
            return errors;
        }

        public override string GenerateAssignmentCode()
        {
            return string.Format("({1})__result{0}", this.Index, Strings.GetType(this.OutputTypeName).ToCSharp());
        }

        public override string GenerateStatementCode()
        {
            var code = new StringBuilder();

            Func<string, string> evalValue = key =>
            {
                var assignmentCode = string.Empty;
                if (null != this[key].GetFunctoid(this.TransformDefinition))
                    assignmentCode = this[key].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode();
                return assignmentCode;
            };

            var value1 = evalValue("value1");
            var value2 = evalValue("value2");
            var value3 = evalValue("value3");
            var value4 = evalValue("value4");

            var connection = this["connection"].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode();

            code.AppendLine($"object __result{Index} = null;");
            code.AppendLine($"var __connectionString{Index} =  @{connection};");

            code.AppendLine($"const string __text{Index} = {SqlText.ToVerbatim()};");


            code.AppendLine($"using(var __conn{Index} = new System.Data.SqlClient.SqlConnection(__connectionString{Index}))");
            code.AppendLine($"using(var __cmd{Index} = new System.Data.SqlClient.SqlCommand(__text{Index},__conn{Index}))");
            code.AppendLine("{");
            if (this.SqlText.Contains("@value1"))
                code.AppendLine($"   __cmd{Index}.Parameters.AddWithValue(\"@value1\",{value1}.ToDbNull());");
            if (this.SqlText.Contains("@value2"))
                code.AppendLine($"   __cmd{Index}.Parameters.AddWithValue(\"@value2\",{value2}.ToDbNull());");
            if (this.SqlText.Contains("@value3"))
                code.AppendLine($"   __cmd{Index}.Parameters.AddWithValue(\"@value3\",{value3}.ToDbNull());");
            if (this.SqlText.Contains("@value4"))
                code.AppendLine($"   __cmd{Index}.Parameters.AddWithValue(\"@value4\",{value4}.ToDbNull());");


            code.AppendLine($"       await __conn{Index}.OpenAsync();");
            code.AppendLine($"       __result{Index} = await __cmd{Index}.ExecuteScalarAsync();");

            var defaultValue = this.DefaultValue;
            if (this.OutputTypeName == "System.String, mscorlib")
                defaultValue = $"\"{this.DefaultValue}\"";

            code.AppendLine($"       if(__result{Index} == DBNull.Value || null == __result{Index}) __result{Index} = {defaultValue};");

            code.AppendLine("}");

            return code.ToString();
        }

        public string DefaultValue { get; set; }
        public string SqlText { get; set; }

        public override string GetEditorView()
        {
            return database.lookup.Properties.Resources.view;
        }

        public override string GetEditorViewModel()
        {
            return database.lookup.Properties.Resources.vm;
        }
    }
}
