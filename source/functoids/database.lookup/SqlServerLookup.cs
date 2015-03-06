using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

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

        public override MetadataReference[] GetMetadataReferences()
        {
            return new[]
            {
                this.CreateMetadataReference<SqlCommand>(),
                MetadataReference.CreateFromAssembly(Assembly.Load("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")),
            };
        }

        public override string GenerateAssignmentCode()
        {
            return string.Format("({1})__result{0}", this.Index, Type.GetType(this.OutputTypeName).ToCSharp());
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

            code.AppendLinf("object __result{0} = null;", this.Index);
            code.AppendLinf("var __connectionString{0} =  @{1};", this.Index, connection);

            code.AppendLinf("const string __text{0} = \"{1}\";", this.Index, this.SqlText);


            code.AppendLinf("using(var __conn = new {1}(__connectionString{0}))", this.Index, typeof(SqlConnection).FullName);
            code.AppendLinf("using(var __cmd = new {1}(__text{0},__conn))", this.Index, typeof(SqlCommand).FullName);
            code.AppendLine("{");
            if (this.SqlText.Contains("@value1"))
                code.AppendLine("   __cmd.Parameters.AddWithValue(\"@value1\"," + value1 + ");");
            if (this.SqlText.Contains("@value2"))
                code.AppendLine("   __cmd.Parameters.AddWithValue(\"@value2\"," + value2 + ");");
            if (this.SqlText.Contains("@value3"))
                code.AppendLine("   __cmd.Parameters.AddWithValue(\"@value3\"," + value3 + ");");
            if (this.SqlText.Contains("@value4"))
                code.AppendLine("   __cmd.Parameters.AddWithValue(\"@value4\"," + value4 + ");");


            code.AppendLine("       await __conn.OpenAsync();");
            code.AppendLinf("       __result{0} = await __cmd.ExecuteScalarAsync();", this.Index);

            var defaultValue = this.DefaultValue;
            if (this.OutputTypeName == "System.String, mscorlib")
                defaultValue = string.Format("\"{0}\"", this.DefaultValue);

            code.AppendLinf("       if(__result{0} == DBNull.Value || null == __result{0}) __result{0} = {1};", this.Index, defaultValue);

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
