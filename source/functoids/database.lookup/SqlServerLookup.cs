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
    [DesignerMetadata(Name = "SqlServerLookup", FontAwesomeIcon = "database", Category = FunctoidCategory.DATABASE)]
    public class SqlServerLookup : Functoid
    {
        public override bool Initialize()
        {
            this.ArgumentCollection.Add(new FunctoidArg { Name = "connection", Type = typeof(string) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "schema", Type = typeof(string) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "table", Type = typeof(string) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "column", Type = typeof(string) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "value1", Type = typeof(string) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "value2", Type = typeof(string) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "value3", Type = typeof(string) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "value4", Type = typeof(string) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "function", Type = typeof(string) });
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
            if (string.IsNullOrWhiteSpace(this.Column))
                errors.Add(new ValidationError { PropertyName = "Column", Message = "You will need to provide a \"Column\" to SELECT from" });
            if (string.IsNullOrWhiteSpace(this.Schema))
                errors.Add(new ValidationError { PropertyName = "Schema", Message = "You will need to provide the \"Schema\" for your table" });
            if (string.IsNullOrWhiteSpace(this.Table))
                errors.Add(new ValidationError { PropertyName = "Schema", Message = "You will need to provide the \"Table\" to SELECT from" });
            if (string.IsNullOrWhiteSpace(this.Predicate) && string.IsNullOrWhiteSpace(this.Function))
                errors.Add(new ValidationError { PropertyName = "Predicate", Message = "For query without a predicate, please provide a scalar function" });
            return errors;
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

            var connection = this["connection"].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode();

            code.AppendLinf("object __result{0} = null;", this.Index);
            code.AppendLinf("var __connectionString{0} =  @{1};", this.Index, connection);

            code.AppendLinf("var __text{0} = string.Format(\"SELECT [{3}] FROM [{4}].[{5}] WHERE {1}\", {2});", this.Index, this.Predicate, "<VALUES>", this.Column, this.Schema, this.Table);
            if (!string.IsNullOrWhiteSpace(value1) && string.IsNullOrWhiteSpace(value2) && string.IsNullOrWhiteSpace(value3))
                code.Replace("<VALUES>", value1);
            if (!string.IsNullOrWhiteSpace(value1) && !string.IsNullOrWhiteSpace(value2) && string.IsNullOrWhiteSpace(value3))
                code.Replace("<VALUES>", value1 + ", " + value2);
            if (!string.IsNullOrWhiteSpace(value1) && !string.IsNullOrWhiteSpace(value2) && !string.IsNullOrWhiteSpace(value3))
                code.Replace("<VALUES>", value1 + ", " + value2 + ", " + value3);

            code.AppendLinf("using(var __conn = new {1}(__connectionString{0}))", this.Index, typeof(SqlConnection).FullName);
            code.AppendLinf("using(var __cmd = new {1}(__text{0},__conn))", this.Index, typeof(SqlCommand).FullName);
            code.AppendLine("{");
            code.AppendLine("       await __conn.OpenAsync();");
            code.AppendLinf("       __result{0} = await __cmd.ExecuteScalarAsync();", this.Index);

            code.AppendLinf("       if(__result{0} == DBNull.Value || null == __result{0}) __result{0} = {1};", this.Index, this.DefaultValue);

            code.AppendLine("}");

            return code.ToString();
        }

        public string DefaultValue { get; set; }

        public override string GetEditorView()
        {
            return database.lookup.Properties.Resources.view;
        }

        public override string GetEditorViewModel()
        {
            return database.lookup.Properties.Resources.vm;
        }


        public string Schema { get; set; }
        public string Table { get; set; }
        public string Predicate { get; set; }
        public string Column { get; set; }
        public string Function { get; set; }
    }
}
