using System;
using System.Collections.Generic;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;

namespace durandaljs.compiler.test
{
    static class HtmlCompileHelper
    {

        public static string CompileHtml(this string expression, bool verbose = false)
        {
            var patient = CreatePatientDefinition();

            var compiler = new ExpressionCompiler();
            var result = compiler.CompileAsync<bool>(expression, patient).Result;
            return result.Code;
        }

        public static EntityDefinition CreatePatientDefinition()
        {
            var patient = new EntityDefinition
            {
                Id = "patient",
                Name = "Patient",
                WebId = "patient-webid",
                Plural = "Patients",
                RecordName = "Mrn"
            };
            patient.MemberCollection.Add(new Member { Name = "Name", Type = typeof(string), IsNullable = true });
            patient.MemberCollection.Add(new Member { Name = "Mrn", Type = typeof(string), IsNullable = false });
            patient.MemberCollection.Add(new Member { Name = "Tag", Type = typeof(string), IsNullable = false });
            patient.MemberCollection.Add(new Member { Name = "MyKad", Type = typeof(string), IsNullable = true });
            patient.MemberCollection.Add(new Member { Name = "Age", Type = typeof(int), IsNullable = true });
            patient.MemberCollection.Add(new Member { Name = "Dob", Type = typeof(DateTime), IsNullable = true });
            patient.MemberCollection.Add(new Member { Name = "RegisteredDate", Type = typeof(DateTime), IsNullable = false });
            patient.MemberCollection.Add(new Member { Name = "IsMarried", Type = typeof(bool), IsNullable = false });

            var address = new Member { Name = "Address", Type = typeof(object) };
            address.Add(new Dictionary<string, Type>
            {
                {"CreatedDate", typeof (DateTime)},
                {"Street", typeof (string)},
                {"Street2", typeof (string)},
                {"City", typeof (string)},
                {"Postcode", typeof (string)},
                {"State", typeof (string)}
            });
            patient.MemberCollection.Add(address);

            var children = new Member { Name = "ChildCollection", Type = typeof(Array) };
            children.Add(new Dictionary<string, Type>
            {
                {"Name", typeof(string)},
                {"Gender", typeof(string)},
                {"Age", typeof(int)},
                {"Dob", typeof(DateTime)},
            });

            return patient;
        }
        public static WorkflowDefinition CreateWorkflowDefinition()
        {
            var wd = new WorkflowDefinition
            {
                Id="sample-wd",
                Name = "Sample Workflow"
            };
            wd.VariableDefinitionCollection.Add(new SimpleVariable{Name = "Title", Type = typeof(string)});

            return wd;
        }
    }
}