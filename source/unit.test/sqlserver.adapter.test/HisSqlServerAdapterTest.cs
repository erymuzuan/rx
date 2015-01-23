using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using Bespoke.Sph.RoslynScriptEngines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace sqlserver.adapter.test
{
    [TestClass]
    public class HisSqlServerAdapterTest
    {
        public const string ADAPTER_NAME = "His";

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            Console.WriteLine("init................");
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            var tables = new[]
            {
                new AdapterTable {Name = "Patient"},
                new AdapterTable {Name = "Department"},
                new AdapterTable {Name = "PatientDepartment"}
            };
            m_sql = new SqlServerAdapter
            {
                Server = @"(localdb)\ProjectsV12",
                Database = "His",
                TrustedConnection = true,
                Schema = "dbo",
                Tables = tables,
                Name = ADAPTER_NAME,
                Description = "A test"

            };

            var bin = ConfigurationManager.WebPath + @"\bin\Dev.AdventureWorksPersons.dll";
            if (File.Exists(bin))
                File.Delete(bin);
            var pdb = ConfigurationManager.WebPath + @"\bin\Dev.AdventureWorksPersons.pdb";
            if (File.Exists(pdb))
                File.Delete(pdb);
        }

        private static SqlServerAdapter m_sql;
        private static Assembly m_dll;

        public async Task CompileAsync()
        {
            var bin = ConfigurationManager.WebPath + @"\bin\Dev.AdventureWorksPersons.dll";
            if (File.Exists(bin))
            {
                m_dll = Assembly.LoadFile(bin);
                return;
            }

            await m_sql.OpenAsync();

            var result = await m_sql.CompileAsync();
            m_dll = Assembly.LoadFile(result.Output);
            File.Copy(result.Output, bin, true);
            Console.WriteLine("copying files");
            await Task.Delay(1000);

        }


        private dynamic CreatePatient()
        {
            var patientType = m_dll.GetType("DevV1.Adapters.dbo." + ADAPTER_NAME + ".Patient");
            dynamic prs = Activator.CreateInstance(patientType);
            Assert.IsNotNull(prs);

            prs.Name = "Ruzzaima";
            prs.Mrn = "Ruzzaima";
            prs.Income = 2500m;
            prs.Gender = "f";
            prs.Dob = new DateTime(1999, 9, 9);
            return prs;
        }

        [TestMethod]
        public async Task AdapterInsert()
        {
            await this.CompileAsync();
            var patient = this.CreatePatient();
            // delete
            await
                m_sql.ExecuteNonQueryAsync("DELETE FROM dbo.Patient WHERE [Name] = @name",
                    new[] { new SqlParameter("@Name", patient.Name) });
            var adapter = this.GetAdapter();
            await adapter.InsertAsync(patient);
        }

        private dynamic GetAdapter()
        {
            var adapterType = m_dll.GetType("DevV1.Adapters.dbo." + ADAPTER_NAME + ".PatientAdapter");
            Assert.IsNotNull(adapterType);
            dynamic adapter = Activator.CreateInstance(adapterType);
            return adapter;
        }

        [TestMethod]
        public async Task AdapterLoadOne()
        {
            await this.CompileAsync();
            var adapter = this.GetAdapter();
            var id = m_sql.GetDatabaseScalarValue<int>(
                "SELECT TOP 1 [BusinessEntityID] FROM [Person].[Person] ORDER BY NEWID()");
            var firstName = m_sql.GetDatabaseScalarValue<string>(
                "SELECT [FirstName] FROM [Person].[Person] WHERE [BusinessEntityID] = @BusinessEntityID",
                new SqlParameter("@BusinessEntityID", id));

            var person2 = await adapter.LoadOneAsync(id);
            Assert.IsNotNull(person2);
            Assert.AreEqual(firstName, person2.FirstName);


        }
    }
}