using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SqlRepository;
using sqlrepository.test.Models;
using Xunit;

namespace Bespoke.Sph.Tests.SqlServer
{
    public class SqlServerFixture : IDisposable
    {
        public const string CONNECTION_STRING =
                @"Data Source=.\DEV2016;Initial Catalog=rx_test_database;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
            ;

        public IReadOnlyRepository<Patient> Repository { get; }
        public readonly IReadOnlyList<Patient> Patients;
        public readonly EntityDefinition PatientSchema;

        public SqlServerFixture()
        {
            var ed = new EntityDefinition {Name = "Patient", Plural = "Patients", Id = "patient"};
            
            ed += ("FullName", typeof(string));
            ed += ("MaritalStatus", typeof(string));
            ed += ("Dob", typeof(DateTime));
            ed += ("Age", typeof(int));
            ed += ("Mrn", typeof(string));

            var spouse = new ComplexMember {Name = "Wife", TypeName = "Spouse"};
            spouse.Add(new Dictionary<string, Type>
            {
                {"Name", typeof(string)},
                {"Age", typeof(int)}
            });
            ed.MemberCollection.Add(spouse);


            Connection = new SqlConnection(CONNECTION_STRING);
            this.Repository = new ReadOnlyRepository<Patient>(CONNECTION_STRING, false, ed);

            this.InitializeIndexAsync()
                .ContinueWith(_ =>
                {
                    if (_.Exception != null)
                        throw _.Exception;
                }).Wait();

            var patients =
                from file in Directory.GetFiles(
                    $@"{ConfigurationManager.Home}\..\source\unit.test\sample-data-patients\", "*.json")
                select file.DeserializeFromJsonFile<Patient>();
            this.Patients = new List<Patient>(patients);

            this.PatientSchema = ed;
        }

        public async Task InitializeIndexAsync()
        {
            var count = await this.Connection.GetScalarValueAsync<int>("SELECT COUNT(*) FROM DevV1.Patient");

            if (count != 100)
                throw new InvalidOperationException(
                    "TODO : RUN source\\unit.test\\sample-data-patients\\create-database.linq");
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }

        public SqlConnection Connection { get; private set; }
    }

    [CollectionDefinition(SQLSERVER_COLLECTION)]
    public class SqlServerCollection : ICollectionFixture<SqlServerFixture>
    {
        public const string SQLSERVER_COLLECTION = "Sql collection";
    }
}