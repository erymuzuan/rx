using System;
using System.Collections.Generic;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;

namespace durandaljs.compiler.test
{
    static class HtmlCompileHelper
    {

        public static string CompileHtml(this string enableExpression, bool verbose = false, string visibleExpression = "true")
        {
            var patient = new EntityDefinition
            {
                Id = "patient",
                Name = "Patient",
                WebId = "patient-webid",
                Plural = "Patients"
            };
            patient.MemberCollection.Add(new Member{ Name = "Name", Type = typeof(string), IsNullable = true});
            patient.MemberCollection.Add(new Member{ Name = "MyKad", Type = typeof(string), IsNullable = true});
            patient.MemberCollection.Add(new Member{ Name = "Age", Type = typeof(int), IsNullable = true});
            patient.MemberCollection.Add(new Member{ Name = "Dob", Type = typeof(DateTime), IsNullable = true});
            patient.MemberCollection.Add(new Member{ Name = "RegisteredDate", Type = typeof(DateTime), IsNullable = false});
            patient.MemberCollection.Add(new Member{ Name = "IsMarried", Type = typeof(bool), IsNullable = false});

            var address = new Member{Name = "Address", Type = typeof(object)};
            address.Add(new Dictionary<string, Type>
            {
                {"Street",typeof(string)},
                {"Street2",typeof(string)},
                {"City",typeof(string)},
                {"Postcode",typeof(string)},
                {"State",typeof(string)}
            });
            patient.MemberCollection.Add(address);


            var button = new Button { Enable = enableExpression, Visible = visibleExpression };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button, patient);
            return html;
        }
    }
}