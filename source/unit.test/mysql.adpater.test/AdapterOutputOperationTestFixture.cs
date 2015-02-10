using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using Bespoke.Sph.RoslynScriptEngines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using sqlserver.adapter.test;

namespace mysql.adpater.test
{
    [TestClass]
    public class AdapterOutputOperationTestFixture
    {
        public const string ADAPTER_NAME = "__MySqlTestAdapter";
        [TestInitialize]
        public void Init()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            var employees = new AdapterTable { Name = "employees" };
            var titles = new AdapterTable { Name = "titles" };
            employees.ChildRelationCollection.Add(new TableRelation
            {
                Column = "emp_no",
                ForeignColumn = "emp_no",
                Table = "titles"
            });
            m_adapter = new MySqlAdapter
             {
                 Name = ADAPTER_NAME,
                 Schema = "employees",
                 Database = "employees",
                 UserId = "root",
                 Password = "",
                 Server = "localhost",
                 Tables = new[] { employees, titles }
             };
            var bin = ConfigurationManager.WebPath + @"\bin\Dev." + m_adapter.Name + ".dll";
            if (File.Exists(bin))
                File.Delete(bin);
            var pdb = ConfigurationManager.WebPath + @"\bin\Dev." + m_adapter.Name + ".pdb";
            if (File.Exists(pdb))
                File.Delete(pdb);

        }
        private static MySqlAdapter m_adapter;
        private static Assembly m_dll;
        public async Task CompileAsync()
        {
            var bin = ConfigurationManager.WebPath + @"\bin\Dev." + m_adapter.Name + ".dll";
            if (File.Exists(bin))
            {
                m_dll = Assembly.LoadFile(bin);
                return;
            }

            await m_adapter.OpenAsync();

            var result = await m_adapter.CompileAsync(new CompilerOptions());
            m_dll = Assembly.LoadFile(result.Output);
            File.Copy(result.Output, bin, true);
            await Task.Delay(1000);

        }

        public dynamic CreateAdapter(string table)
        {
            var adapterType = m_dll.GetType(string.Format("Dev.Adapters.employees.{0}.{1}Adapter", ADAPTER_NAME, table));
            dynamic adapter = Activator.CreateInstance(adapterType);
            Assert.IsNotNull(adapter);

            return adapter;

        }
        public dynamic CreateEmployee()
        {
            var personType = m_dll.GetType(string.Format("Dev.Adapters.employees.{0}.employees", ADAPTER_NAME));
            dynamic prs = Activator.CreateInstance(personType);
            Assert.IsNotNull(prs);

            return prs;

        }

        [TestMethod]
        public async Task GetEmployeeInstanceTest()
        {
            await this.CompileAsync();

            var adapter = this.CreateAdapter("employees");
            var emp = await adapter.LoadOneAsync(10001);
            Assert.AreEqual("Georgi", emp.first_name);


        }

        [TestMethod]
        public async Task InsertEmployeeInstanceTest()
        {
            await m_adapter.ExecuteNonQueryAsync("DELETE FROM employees WHERE first_name = 'Erymuzuan'");
            var no = await m_adapter.GetDatabaseScalarAsync<int>("SELECT MAX(emp_no) FROM employees");
            await this.CompileAsync();
            var emp = this.CreateEmployee();
            emp.emp_no = no + 1;
            emp.first_name = "Erymuzuan";
            emp.last_name = "Mustapa";
            emp.gender = "M";
            emp.hire_date = new DateTime(2005, 1, 5);
            emp.birth_date = new DateTime(1979, 9, 9);


            var adapter = this.CreateAdapter("employees");
            await adapter.InsertAsync(emp);
            var count = await m_adapter.GetDatabaseScalarAsync<long>("SELECT COUNT(*) FROM employees WHERE first_name = 'Erymuzuan'");
            Assert.AreEqual(1, count);

        }
        [TestMethod]
        public async Task DeleteEmployeeInstanceTest()
        {
            await m_adapter.ExecuteNonQueryAsync("DELETE FROM employees WHERE first_name = 'Erymuzuan'");
            var no = await m_adapter.GetDatabaseScalarAsync<int>("SELECT MAX(emp_no) FROM employees");
            await this.CompileAsync();
            var emp = this.CreateEmployee();
            emp.emp_no = no + 1;
            emp.first_name = "Erymuzuan";
            emp.last_name = "Mustapa";
            emp.gender = "M";
            emp.hire_date = new DateTime(2005, 1, 5);
            emp.birth_date = new DateTime(1979, 9, 9);


            var adapter = this.CreateAdapter("employees");
            await adapter.InsertAsync(emp);
            var empNo = await m_adapter.GetDatabaseScalarAsync<int>("SELECT emp_no FROM employees WHERE first_name = 'Erymuzuan'");

            await adapter.DeleteAsync(empNo);

        }


        [TestMethod]
        public async Task UpdateEmployeeInstanceTest()
        {
            await this.CompileAsync();

            var adapter = this.CreateAdapter("employees");
            var emp = await adapter.LoadOneAsync(10001);
            var gender = emp.gender == "M" ? "F" : "M";
            emp.gender = gender;


            Assert.AreEqual(gender, emp.gender);

            await adapter.UpdateAsync(emp);
            var ooo = await m_adapter.GetDatabaseScalarAsync<string>("SELECT gender FROM employees WHERE emp_no = @no", new MySqlParameter("@no", emp.emp_no));
            Assert.AreEqual(gender, ooo);

        }



    }
}
