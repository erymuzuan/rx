using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "MySQL table", FontAwesomeIcon = "list-ol", Category = FunctoidCategory.DATABASE)]
    public class MySqlTable : Functoid
    {
        public override bool Initialize()
        {
            this.OutputTypeName = typeof(List<IDictionary<string, object>>).ToCSharp();
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
            
            if (string.IsNullOrWhiteSpace(this.SqlText))
                errors.Add(new ValidationError { PropertyName = "SqlText", Message = "You will need to provide a \"SqlText\"" });
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

            code.AppendLine($"var __result{Index} = new List<IDictionary<string,object>>();");
            code.AppendLine($"var __connectionString{Index} =  @{connection};");

            code.AppendLine($"const string __text{Index} = \"{SqlText}\";");

            code.AppendLine($"using(var __conn = new MySql.Data.MySqlClient.MySqlConnection(__connectionString{Index}))");
            code.AppendLine($"using(var __cmd = new MySql.Data.MySqlClient.MySqlCommand(__text{Index},__conn))");
            code.AppendLine("{");
            if (this.SqlText.Contains("@value1"))
                code.AppendLine($"   __cmd.Parameters.AddWithValue(\"@value1\",{value1});");
            if (this.SqlText.Contains("@value2"))
                code.AppendLine($"   __cmd.Parameters.AddWithValue(\"@value2\",{value2});");
            if (this.SqlText.Contains("@value3"))
                code.AppendLine($"   __cmd.Parameters.AddWithValue(\"@value3\",{value3});");
            if (this.SqlText.Contains("@value4"))
                code.AppendLine($"   __cmd.Parameters.AddWithValue(\"@value4\",{value4});");


            code.AppendLine("       await __conn.OpenAsync();");
            code.AppendLine($"       using(var __reader{Index} =await __cmd.ExecuteReaderAsync()) ");
            code.AppendLine("       {");
            code.AppendLine($@"           		
        while (await reader.ReadAsync())
		{{
			var row = new Dictionary<string, object>();			
			for (int i = 0; i < reader.FieldCount; i++)
			{{
				var val = reader.GetValue(i);
				if (val is MySqlDateTime)
				{{
					val = ((MySqlDateTime)val).Value;
				}}
				row.Add(reader.GetName(i), val);
			}}
			__result{Index}.Add(row);
		}}");

            code.AppendLine("       }");
            

            code.AppendLine("}");

            return code.ToString();
        }
        public string SqlText { get; set; }

        public override string GetEditorView()
        {
            return Functoids.Properties.Resources.tableview;
        }

        public override string GetEditorViewModel()
        {
            return Functoids.Properties.Resources.tablevm;
        }
    }
}