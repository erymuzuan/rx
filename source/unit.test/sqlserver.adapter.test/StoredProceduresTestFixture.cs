using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace sqlserver.adapter.test
{
    [TestClass]
    public class StoredProceduresTestFixture
    {
        [TestMethod]
        public async Task SprocToSqlOperationWithResultset()
        {
            var adapter = new SqlServerAdapter
            {
                Schema = "dbo",
                TrustedConnection = true,
                Server = "(localdb)\\ProjectsV12",
                Database = "Commerce",
                Tables = new AdapterTable[] { },
                Name = "__sprocSqlWithResultsetTest"
            };

            var productByCategory = new SprocOperationDefinition
            {
                MethodName = "CMRC_ProductsByCategory",
                Name = "CmrcProductsByCategory",
            };

            var categoryId = new SprocParameter
            {
                Name = "@CategoryID",
                Type = typeof(int),
                SqlType = "int",
                Position = 0,
                MaxLength = null
            };

            productByCategory.RequestMemberCollection.Add(categoryId);



            var retVal = new SprocResultMember
            {
                Name = "@return_value",
                Type = typeof(int),
                SqlDbType = SqlDbType.Int
            };

            var resultset1 = new SprocResultMember
            {
                Name = "CMRC_ProductsByCategoryResult1Collection",
                Type = typeof(Array)
            };
            resultset1.MemberCollection.Add(new SprocResultMember
            {
                Name = "ProductId",
                Type = typeof(int)
            });
            resultset1.MemberCollection.Add(new SprocResultMember
            {
                Name = "ModelName",
                Type = typeof(string)
            });
            resultset1.MemberCollection.Add(new SprocResultMember
            {
                Name = "UnitCost",
                Type = typeof(decimal)
            });
            resultset1.MemberCollection.Add(new SprocResultMember
            {
                Name = "ProductImage",
                Type = typeof(string)
            });
            productByCategory.ResponseMemberCollection.Add(retVal);
            productByCategory.ResponseMemberCollection.Add(resultset1);

            adapter.OperationDefinitionCollection.Add(productByCategory);

            await adapter.OpenAsync(true);
            var cr = await adapter.CompileAsync();
            Assert.AreEqual(true, cr.Result);

            var dll = Assembly.LoadFile(cr.Output);
            dynamic commerce = dll.CreateInstance(string.Format("Dev.Adapters.{0}.{1}.{1}", adapter.Schema, adapter.Name));
            Assert.IsNotNull(commerce);
            commerce.ConnectionString = @"server=(localdb)\ProjectsV12;database=Commerce;trusted_connection=yes";
            dynamic request =
                dll.CreateInstance(string.Format("Dev.Adapters.{0}.{1}.{2}Request", adapter.Schema, adapter.Name,
                    productByCategory.MethodName.ToCsharpIdentitfier()));
            Assert.IsNotNull(request);
            request.@CategoryID = 14;


            var response = await commerce.CmrcProductsByCategoryAsync(request);
            Assert.AreEqual(5, response.CMRC_ProductsByCategoryResult1Collection.Count);
            Assert.AreEqual(360, response.CMRC_ProductsByCategoryResult1Collection[0].ProductId);


        }

        public const string ADAPTER_NAME = "__sprocSqlTest";
        [TestMethod]
        public async Task SprocToSqlOperation()
        {
            var adapter = new SqlServerAdapter
            {
                Schema = "HumanResources",
                TrustedConnection = true,
                Server = "(localdb)\\ProjectsV12",
                Database = "AdventureWorks",
                Tables = new AdapterTable[] { },
                Name = ADAPTER_NAME
            };

            var uspUpdateEmployeePersonalInfo = new SprocOperationDefinition
            {
                MethodName = "uspUpdateEmployeePersonalInfo",
                Name = "uspUpdateEmployeePersonalInfo",
            };

            var businessEntityId = new SprocParameter
            {
                Name = "@BusinessEntityID",
                Type = typeof(int),
                SqlType = "nchar",
                Position = 5,
                MaxLength = null
            };
            var nationalIdNumber = new SprocParameter
            {
                Name = "@NationalIDNumber",
                Type = typeof(string),
                SqlType = "nvarchar",
                Position = 5,
                MaxLength = null
            };
            var birthDate = new SprocParameter
            {
                Name = "@BirthDate",
                Type = typeof(DateTime),
                SqlType = "nchar",
                Position = 5,
                MaxLength = null
            };
            var maritalStatus = new SprocParameter
            {
                Name = "@MaritalStatus",
                Type = typeof(string),
                SqlType = "nchar",
                Position = 5,
                MaxLength = null
            };
            var gender = new SprocParameter
            {
                Name = "@Gender",
                Type = typeof(string),
                SqlType = "nchar",
                Position = 5,
                MaxLength = null
            };
            uspUpdateEmployeePersonalInfo.RequestMemberCollection.Add(businessEntityId);
            uspUpdateEmployeePersonalInfo.RequestMemberCollection.Add(nationalIdNumber);
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
                Type = typeof(DateTime),
                SqlDbType = SqlDbType.DateTime
            };
            uspUpdateEmployeePersonalInfo.ResponseMemberCollection.Add(retVal);
            uspUpdateEmployeePersonalInfo.ResponseMemberCollection.Add(modifiedDate);

            adapter.OperationDefinitionCollection.Add(uspUpdateEmployeePersonalInfo);

            await adapter.OpenAsync(true);
            var cr = await adapter.CompileAsync();
            Assert.AreEqual(true, cr.Result);

            var dll = Assembly.LoadFile(cr.Output);
            dynamic adw = dll.CreateInstance(string.Format("Dev.Adapters.{0}.{1}.{1}", adapter.Schema, adapter.Name));
            Assert.IsNotNull(adw);
            adw.ConnectionString = @"server=(localdb)\ProjectsV12;database=AdventureWorks;trusted_connection=yes";
            dynamic request =
                dll.CreateInstance(string.Format("Dev.Adapters.{0}.{2}.{1}Request", adapter.Schema,
                    uspUpdateEmployeePersonalInfo.MethodName.ToCsharpIdentitfier(), ADAPTER_NAME));
            Assert.IsNotNull(request);
            request.@BusinessEntityID = 102;
            request.@NationalIDNumber = "360868122";
            request.@BirthDate = new DateTime(1977, 11, 26);
            request.@MaritalStatus = "S";
            request.@Gender = "M";


            var response = await adw.UspUpdateEmployeePersonalInfoAsync(request);

            Assert.AreEqual(DateTime.Today, response.@ModifiedDate.Date);


        }
    }
}
