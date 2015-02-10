using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mysql.adpater.test
{
    [TestClass]
    public class MySqlAdapterCompilerTestFixture
    {
        public const string ADAPTER_NAME_WITH_PROCEDURE = "__MySqlEmployeesWithProcedure";
        public const string ADAPTER_NAME = "__MySqlEmployees";
        [TestMethod]
        public async Task CompileOneTable()
        {
            var adapter = new MySqlAdapter
            {
                Name = ADAPTER_NAME,
                Database = "employees",
                Schema = "employees",
                Server = "localhost",
                UserId = "root",
                Password = "",
                Tables = new[]
                {
                    new AdapterTable
                    {
                        Name = "employees"
                    }
                }
            };
            await adapter.OpenAsync(true);
            var cr = await adapter.CompileAsync(new CompilerOptions());
            Assert.IsTrue(cr.Result);
        }


        [TestMethod]
        public async Task CompileOneProcedure()
        {
            var adapter = new MySqlAdapter
            {
                Name = ADAPTER_NAME_WITH_PROCEDURE,
                Database = "employees",
                Schema = "employees",
                Server = "localhost",
                UserId = "root",
                Password = "",
                Tables = new AdapterTable[] { }
            };

            var staff = new SprocResultMember { Name = "StaffCollection", Type = typeof(Array) };
            staff.MemberCollection.Add(new SprocResultMember { Name = "first_name", Type = typeof(string) });
            staff.MemberCollection.Add(new SprocResultMember { Name = "last_name", Type = typeof(string) });

            var sproc = new SprocOperationDefinition
            {
                Name = "getEmployeesByEmpNo",
                MethodName = "getEmployeesByEmpNo"
            };
            sproc.RequestMemberCollection.Add(new SprocParameter { Name = "@no", Type = typeof(int) });
            sproc.ResponseMemberCollection.Add(staff);

            adapter.OperationDefinitionCollection.Add(sproc);
            await adapter.OpenAsync(true);
            var cr = await adapter.CompileAsync(new CompilerOptions());
            Assert.IsTrue(cr.Result);


            var dll = Assembly.LoadFile(cr.Output);
            dynamic sprocAdapter = Activator.CreateInstance(dll.GetType(string.Format("Dev.Adapters.employees.{0}.{0}", ADAPTER_NAME_WITH_PROCEDURE)));
            sprocAdapter.ConnectionString = adapter.ConnectionString;

            dynamic request = Activator.CreateInstance(dll.GetType("Dev.Adapters.employees." + ADAPTER_NAME_WITH_PROCEDURE + ".GetEmployeesByEmpNoRequest"));
            request.@no = 10100;

            var result = await sprocAdapter.GetEmployeesByEmpNoAsync(request);
            var json = JsonSerializerService.ToJsonString(result, true);
            StringAssert.Contains(json, "Haraldson");
        }

        [TestMethod]
        public async Task CompileProcedureOutParameter()
        {
            const string adapterName = "__MySqlEmployeesProcedureOutParameter";
            var adapter = new MySqlAdapter
            {
                Name = adapterName,
                Database = "employees",
                Schema = "employees",
                Server = "localhost",
                UserId = "root",
                Password = "",
                Tables = new AdapterTable[] { }
            };

            var count = new SprocResultMember { Name = "@count", Type = typeof(long), SqlDbType = SqlDbType.Int };

            var sproc = new SprocOperationDefinition
            {
                Name = "getStaffCountByTitle",
                MethodName = "getStaffCountByTitle"
            };
            sproc.RequestMemberCollection.Add(new SprocParameter { Name = "@title", Type = typeof(string) });
            sproc.ResponseMemberCollection.Add(count);

            adapter.OperationDefinitionCollection.Add(sproc);
            await adapter.OpenAsync(true);
            var cr = await adapter.CompileAsync(new CompilerOptions());
            Assert.IsTrue(cr.Result);


            var dll = Assembly.LoadFile(cr.Output);
            dynamic sprocAdapter = Activator.CreateInstance(dll.GetType(string.Format("Dev.Adapters.employees.{0}.{0}", adapterName)));
            sprocAdapter.ConnectionString = adapter.ConnectionString;

            dynamic request = Activator.CreateInstance(dll.GetType("Dev.Adapters.employees." + adapterName + ".GetStaffCountByTitleRequest"));
            request.@title = "Senior Staff";

            var result = await sprocAdapter.GetStaffCountByTitleAsync(request);
            var json = JsonSerializerService.ToJsonString(result, true);
            StringAssert.Contains(json, "count");
        }

        [TestMethod]
        public async Task CompileOneTableWithChild()
        {
            var employees = new AdapterTable { Name = "employees" };
            var titles = new AdapterTable { Name = "titles" };
            employees.ChildRelationCollection.Add(new TableRelation
            {
                Column = "emp_no",
                ForeignColumn = "emp_no",
                Table = "titles"
            });
            const string ADAPTER_NAME2 = "__MySqlEmployeesWithChildTitles";
            var adapter = new MySqlAdapter
            {
                Name = ADAPTER_NAME2,
                Database = "employees",
                Schema = "employees",
                Server = "localhost",
                UserId = "root",
                Password = "",
                Tables = new[] { employees, titles }
            };
            await adapter.OpenAsync(true);
            var cr = await adapter.CompileAsync(new CompilerOptions());
            Assert.IsTrue(cr.Result);

            var dll = Assembly.LoadFile(cr.Output);
            dynamic controller = Activator.CreateInstance(dll.GetType("Dev.Adapters.employees." + ADAPTER_NAME2 + ".employeesController"));
            var result = await controller.GettitlesByemployees(10100);
            var json = JsonSerializerService.ToJsonString(result, true);
            StringAssert.Contains(json, "Senior Staff");
        }
    }
}
