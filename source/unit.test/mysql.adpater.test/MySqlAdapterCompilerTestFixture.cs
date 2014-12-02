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
        [TestMethod]
        public async Task CompileOneTable()
        {
            var adapter = new MySqlAdapter
            {
                Name = "__MySqlEmployees",
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
            var cr = await adapter.CompileAsync();
            Assert.IsTrue(cr.Result);
        }


        [TestMethod]
        public async Task CompileOneProcedure()
        {
            var adapter = new MySqlAdapter
            {
                Name = "__MySqlEmployeesWithProcedure",
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
            var cr = await adapter.CompileAsync();
            Assert.IsTrue(cr.Result);


            var dll = Assembly.LoadFile(cr.Output);
            dynamic sprocAdapter = Activator.CreateInstance(dll.GetType("Dev.Adapters.employees.__MySqlEmployeesWithProcedure"));
            sprocAdapter.ConnectionString = adapter.ConnectionString;

            dynamic request = Activator.CreateInstance(dll.GetType("Dev.Adapters.employees.GetEmployeesByEmpNoRequest"));
            request.@no = 10100;

            var result = await sprocAdapter.GetEmployeesByEmpNoAsync(request);
            var json = JsonSerializerService.ToJsonString(result, true);
            StringAssert.Contains(json, "Haraldson");
        }

        [TestMethod]
        public async Task CompileProcedureOutParameter()
        {
            var adapter = new MySqlAdapter
            {
                Name = "__MySqlEmployeesProcedureOutParameter",
                Database = "employees",
                Schema = "employees",
                Server = "localhost",
                UserId = "root",
                Password = "",
                Tables = new AdapterTable[] { }
            };

            var count = new SprocResultMember { Name = "@count", Type = typeof(long), SqlDbType = SqlDbType.Int};

            var sproc = new SprocOperationDefinition
            {
                Name = "getStaffCountByTitle",
                MethodName = "getStaffCountByTitle"
            };
            sproc.RequestMemberCollection.Add(new SprocParameter { Name = "@title", Type = typeof(string) });
            sproc.ResponseMemberCollection.Add(count);

            adapter.OperationDefinitionCollection.Add(sproc);
            await adapter.OpenAsync(true);
            var cr = await adapter.CompileAsync();
            Assert.IsTrue(cr.Result);


            var dll = Assembly.LoadFile(cr.Output);
            dynamic sprocAdapter = Activator.CreateInstance(dll.GetType("Dev.Adapters.employees.__MySqlEmployeesProcedureOutParameter"));
            sprocAdapter.ConnectionString = adapter.ConnectionString;

            dynamic request = Activator.CreateInstance(dll.GetType("Dev.Adapters.employees.GetStaffCountByTitleRequest"));
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
            var adapter = new MySqlAdapter
            {
                Name = "__MySqlEmployeesWithChildTitles",
                Database = "employees",
                Schema = "employees",
                Server = "localhost",
                UserId = "root",
                Password = "",
                Tables = new[] { employees, titles }
            };
            await adapter.OpenAsync(true);
            var cr = await adapter.CompileAsync();
            Assert.IsTrue(cr.Result);

            var dll = Assembly.LoadFile(cr.Output);
            dynamic controller = Activator.CreateInstance(dll.GetType("Dev.Adapters.employees.employeesController"));
            var result = await controller.GettitlesByemployees(10100);
            var json = JsonSerializerService.ToJsonString(result, true);
            StringAssert.Contains(json, "Senior Staff");
        }
    }
}
