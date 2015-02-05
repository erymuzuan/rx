using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;
using Bespoke.Sph.OdataQueryCompilers;
using NUnit.Framework;

namespace odata.provider.test
{
    [TestFixture]
    public class PredicateExpressionTestFixture
    {
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

            var address = new Member { Name = "Address" };
            address.Add(new Dictionary<string, Type>
            {
                {"CreatedDate", typeof (DateTime)},
                {"Street", typeof (string)},
                {"Street2", typeof (string)},
                {"City", typeof (string)},
                {"Postcode", typeof (string)},
                {"Country", typeof (string)},
                {"State", typeof (string)}
            });
            var urban = address.AddMember("Urban", typeof(bool));
            urban.IsNullable = false;

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
        [Test]
        public async Task Eq()
        {
            var patient = CreatePatientDefinition();
            var compiler = new ExpressionCompiler();
            var cr = await compiler.CompileAsync<bool>("item.Mrn == \"123\"", patient);

            var odataCompiler = new OdataQueryExpressionCompiler();

            var crr = odataCompiler.CompileExpression(cr.Tag.statement, cr.Tag.model);
            Console.WriteLine(crr);
        }
    }
}