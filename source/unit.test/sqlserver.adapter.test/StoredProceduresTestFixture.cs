using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParameterDirection = System.Data.ParameterDirection;

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
                Type = typeof(string),
                SqlType  = "nvarchar",
                Position = 5,
                MaxLength = null
            };
            var birthDate = new SprocParameter
            {
                Name = "@BirthDate",
                Type = typeof(DateTime),
                SqlType  = "nchar",
                Position = 5,
                MaxLength = null
            };
            var maritalStatus = new SprocParameter
            {
                Name = "@MaritalStatus",
                Type = typeof(string),
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


            var retVal = new SprocResultMember
            {
                Name = "@return_value",
                Type = typeof(int),
                SqlDbType = SqlDbType.Int
            };

            var modifiedDate = new SprocResultMember
            {
                Name = "@ModifiedDate",
                Type = typeof (DateTime),
                SqlDbType = SqlDbType.DateTime
            };
            uspUpdateEmployeePersonalInfo.ResponseMemberCollection.Add(retVal);
            uspUpdateEmployeePersonalInfo.ResponseMemberCollection.Add(modifiedDate);

            adapter.OperationDefinitionCollection.Add(uspUpdateEmployeePersonalInfo);

            await adapter.OpenAsync(true);
            var cr = await adapter.CompileAsync();
            Assert.AreEqual(true,cr.Result);

            var dll = Assembly.LoadFile(cr.Output);
            dynamic adw = dll.CreateInstance(string.Format("Dev.Adapters.{0}.{1}", adapter.Schema, adapter.Name));
            Assert.IsNotNull(adw);
            adw.ConnectionString = @"server=(localdb)\ProjectsV12;database=AdventureWorks;trusted_connection=yes";
            dynamic request =
                dll.CreateInstance(string.Format("Dev.Adapters.{0}.{1}Request", adapter.Schema,
                    uspUpdateEmployeePersonalInfo.MethodName.ToCsharpIdentitfier()));
            Assert.IsNotNull(request);
            request.@BusinessEntityID = 102;
            request.@NationalIDNumber = "360868122";
            request.@BirthDate = new DateTime(1977,11,26);
            request.@MaritalStatus = "S";
            request.@Gender = "M";


            var response = await adw.UspUpdateEmployeePersonalInfoAsync(request);

            Assert.AreEqual(DateTime.Now, response.@ModifiedDate);


        }
    }

    public static class ReflectionHelper
    {
        public static dynamic CreateInstance(this Assembly dll, string type)
        {
            var typee = dll.GetType(type);
            dynamic obj = Activator.CreateInstance(typee);
            if(null == obj)throw new InvalidOperationException("Cannot create object " + type);
            return obj;
        }
    }
}
