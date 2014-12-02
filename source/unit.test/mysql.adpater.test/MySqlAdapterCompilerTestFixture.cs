using System;
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
        public async Task CompileOneTableWithChild()
        {
            var employees = new AdapterTable {Name = "employees"};
            var titles = new AdapterTable {Name = "titles"};
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
                Tables = new[]{employees,titles}
            };
            await adapter.OpenAsync(true);
            var cr = await adapter.CompileAsync();
            Assert.IsTrue(cr.Result);

            var dll = Assembly.LoadFile(cr.Output);
            dynamic controller = Activator.CreateInstance(dll.GetType("Dev.Adapters.employees.employeesController"));
            var result = await controller.GettitlesByemployees(10100);
            var json =JsonSerializerService.ToJsonString(result,true);
            StringAssert.Contains(json, "Senior Staff");
        }
    }
}
