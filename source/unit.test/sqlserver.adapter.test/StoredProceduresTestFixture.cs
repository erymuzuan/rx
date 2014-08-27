using System.Threading.Tasks;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace sqlserver.adapter.test
{
    [TestClass]
    public class StoredProceduresTestFixture
    {
        [TestMethod]
        public async Task SprocToSqlOperation()
        {
            var adapter = new SqlServerAdapter
            {
                Schema = "HumanResources",
                TrustedConnection = true,
                Server = "(localdb)\\ProjectsV12",
                Database = "AdventureWorks",
                Tables = new AdapterTable[]{},
                Name = "__sprocSqlTest"
            };

            var uspUpdateEmployeePersonalInfo = new SprocOperationDefinition
            {
                MethodName = "uspUpdateEmployeePersonalInfo",
                Name = "uspUpdateEmployeePersonalInfo",
            };

            var businessEntityID = new SprocParameter
            {
                Name = "@BusinessEntityID",
                Type = typeof(int),
                SqlType  = "nchar",
                Position = 5,
                MaxLength = null
            };
            var nationalIDNumber = new SprocParameter
            {
                Name = "@NationalIDNumber",
                Type = typeof(int),
                SqlType  = "nchar",
                Position = 5,
                MaxLength = null
            };
            var birthDate = new SprocParameter
            {
                Name = "@BirthDate",
                Type = typeof(int),
                SqlType  = "nchar",
                Position = 5,
                MaxLength = null
            };
            var maritalStatus = new SprocParameter
            {
                Name = "@MaritalStatus",
                Type = typeof(int),
                SqlType  = "nchar",
                Position = 5,
                MaxLength = null
            };
            var gender = new SprocParameter
            {
                Name = "@Gender",
                Type = typeof(string),
                SqlType  = "nchar",
                Position = 5,
                MaxLength = null
            };
            uspUpdateEmployeePersonalInfo.RequestMemberCollection.Add(businessEntityID);
            uspUpdateEmployeePersonalInfo.RequestMemberCollection.Add(nationalIDNumber);
            uspUpdateEmployeePersonalInfo.RequestMemberCollection.Add(birthDate);
            uspUpdateEmployeePersonalInfo.RequestMemberCollection.Add(maritalStatus);
            uspUpdateEmployeePersonalInfo.RequestMemberCollection.Add(gender);
            adapter.OperationDefinitionCollection.Add(uspUpdateEmployeePersonalInfo);

            await adapter.OpenAsync(true);
            var cr = await adapter.CompileAsync();
            Assert.AreEqual(true,cr);

        }
    }
}
