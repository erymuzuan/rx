using System;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Integrations.Adapters;
using Bespoke.Sph.Integrations.Adapters.Columns;
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
                TrustedConnection = true,
                Server = "(localdb)\\ProjectsV12",
                Database = "Commerce",
                Name = "__sprocSqlWithResultsetTest"
            };

            var productByCategory = new SprocOperationDefinition
            {
                MethodName = "CMRC_ProductsByCategory",
                Name = "CmrcProductsByCategory",
            };

            var categoryId = new IntColumn
            {
                Name = "@CategoryID"
            };

            productByCategory.RequestMemberCollection.Add(categoryId);



            var retVal = new IntColumn
            {
                Name = "@return_value"
            };

            var resultset1 = new ComplexMember
            {
                Name = "CMRC_ProductsByCategoryResult1Collection",
                AllowMultiple = true
            };
            resultset1.MemberCollection.Add(new NullableIntColumn
            {
                Name = "ProductId"
            });
            resultset1.MemberCollection.Add(new StringColumn
            {
                Name = "ModelName"
            });
            resultset1.MemberCollection.Add(new NullableIntColumn
            {
                Name = "UnitCost",
                Type = typeof(decimal)
            });
            resultset1.MemberCollection.Add(new StringColumn
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
            dynamic commerce = dll.CreateInstance(($"{ConfigurationManager.ApplicationName}.Adapters.{productByCategory.Schema}.{adapter.Name}.{adapter.Name}"));
            Assert.IsNotNull(commerce);
            commerce.ConnectionString = @"server=(localdb)\ProjectsV12;database=Commerce;trusted_connection=yes";
            dynamic request =
                dll.CreateInstance($"{ConfigurationManager.ApplicationName}.Adapters.{productByCategory.Schema}.{adapter.Name}.{productByCategory.MethodName.ToCsharpIdentitfier()}Request");
            Assert.IsNotNull(request);
            request.CategoryID = 14;


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
                TrustedConnection = true,
                Server = "(localdb)\\ProjectsV12",
                Database = "AdventureWorks",
                Name = ADAPTER_NAME
            };

            var uspUpdateEmployeePersonalInfo = new SprocOperationDefinition
            {
                MethodName = "uspUpdateEmployeePersonalInfo",
                Name = "uspUpdateEmployeePersonalInfo",
            };

            var businessEntityId = new StringColumn
            {
                Name = "@BusinessEntityID",
                Type = typeof(int)
            };
            var nationalIdNumber = new StringColumn
            {
                Name = "@NationalIDNumber"
            };
            var birthDate = new DateTimeColumn
            {
                Name = "@BirthDate"
            };
            var maritalStatus = new StringColumn
            {
                Name = "@MaritalStatus"
            };
            var gender = new StringColumn
            {
                Name = "@Gender"
            };
            uspUpdateEmployeePersonalInfo.RequestMemberCollection.Add(businessEntityId);
            uspUpdateEmployeePersonalInfo.RequestMemberCollection.Add(nationalIdNumber);
            uspUpdateEmployeePersonalInfo.RequestMemberCollection.Add(birthDate);
            uspUpdateEmployeePersonalInfo.RequestMemberCollection.Add(maritalStatus);
            uspUpdateEmployeePersonalInfo.RequestMemberCollection.Add(gender);


            var retVal = new IntColumn
            {
                Name = "@return_value"
            };

            var modifiedDate = new DateTimeColumn
            {
                Name = "@ModifiedDate"
            };
            uspUpdateEmployeePersonalInfo.ResponseMemberCollection.Add(retVal);
            uspUpdateEmployeePersonalInfo.ResponseMemberCollection.Add(modifiedDate);

            adapter.OperationDefinitionCollection.Add(uspUpdateEmployeePersonalInfo);

            await adapter.OpenAsync(true);
            var cr = await adapter.CompileAsync();
            Assert.AreEqual(true, cr.Result);

            var dll = Assembly.LoadFile(cr.Output);
            dynamic adw = dll.CreateInstance($"{ConfigurationManager.ApplicationName}.Adapters.{adapter.Name}.{adapter.Name}");
            Assert.IsNotNull(adw);
            adw.ConnectionString = @"server=(localdb)\ProjectsV12;database=AdventureWorks;trusted_connection=yes";
            dynamic request =
                dll.CreateInstance(($"{ConfigurationManager.ApplicationName}.Adapters.{ADAPTER_NAME}.{uspUpdateEmployeePersonalInfo.MethodName.ToCsharpIdentitfier()}Request"));
            Assert.IsNotNull(request);
            request.BusinessEntityID = 102;
            request.NationalIDNumber = "360868122";
            request.BirthDate = new DateTime(1977, 11, 26);
            request.MaritalStatus = "S";
            request.Gender = "M";


            var response = await adw.UspUpdateEmployeePersonalInfoAsync(request);

            Assert.AreEqual(DateTime.Today, response.ModifiedDate.Date);


        }
    }
}
