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
                new AdapterTable{Name = "Patient"},
                new AdapterTable{Name = "Department"},
                new AdapterTable{Name = "PatientDepartment"}
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

            var bin = ConfigurationManager.WebPath + @"\bin\Dev.Hisdbo.dll";
            if (File.Exists(bin))
                File.Delete(bin);
            var pdb = ConfigurationManager.WebPath + @"\bin\Dev.Hisdbo.pdb";
            if (File.Exists(pdb))
                File.Delete(pdb);
        }

        private static SqlServerAdapter m_sql;
        private static Assembly m_dll;
        public async Task CompileAsync()
        {
            var bin = ConfigurationManager.WebPath + @"\bin\Dev.Hisdbo.dll";
            if (File.Exists(bin))
            {
                m_dll = Assembly.LoadFile(bin);
                return;
            }

            await m_sql.OpenAsync();

            var result = await m_sql.CompileAsync(new CompilerOptions());
            m_dll = Assembly.LoadFile(result.Output);
            File.Copy(result.Output, bin, true);
            Console.WriteLine("copying files");
            await Task.Delay(1000);

        }


        private dynamic CreatePatient()
        {
            var personType = m_dll.GetType("Dev.Adapters.Patient." + ADAPTER_NAME + ".Patient");
            dynamic prs = Activator.CreateInstance(personType);
            Assert.IsNotNull(prs);
            prs.Mrn = Guid.NewGuid().ToString().Substring(0, 8);
            prs.Gender = "MALE";
            prs.Dob = new DateTime(1975,9,9);
            return prs;
        }

        [TestMethod]
        public async Task AdapterInsert()
        {
            await this.CompileAsync();
            var prs = this.CreatePatient();
            m_sql.ExecuteNonQuery("INSERT INTO dbp.BusinessEntity(rowguid, ModifiedDate) VALUES(@rowguid, @ModifiedDate)",
                new SqlParameter("@rowguid", prs.rowguid),
                new SqlParameter("@ModifiedDate", DateTime.Now));

            // delete
            var adapter = this.GetAdapter();
            await adapter.InsertAsync(prs);
        }

        private dynamic GetAdapter()
        {
            var adapterType = m_dll.GetType("Dev.Adapters.dbo." + ADAPTER_NAME + ".PatientAdapter");
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
                "SELECT TOP 1 [Id] FROM [dbo].[Patient] ORDER BY NEWID()");
            var mrn = m_sql.GetDatabaseScalarValue<string>(
                "SELECT [Mrn] FROM [dbo].[Patient] WHERE [Id] = @Id",
                new SqlParameter("@Id", id));

            var person2 = await adapter.LoadOneAsync(id);
            Assert.IsNotNull(person2);
            Assert.AreEqual(mrn, person2.Mrn);

        }

    }
}