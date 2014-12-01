using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using Bespoke.Sph.RoslynScriptEngines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using sqlserver.adapter.test;

namespace mysql.adpater.test
{
    [TestClass]
    public class AdapterOutputOperationTestFixture
    {

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
                 Name = "__MySqlTestAdapter",
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

            var result = await m_adapter.CompileAsync();
            m_dll = Assembly.LoadFile(result.Output);
            File.Copy(result.Output, bin, true);
            Console.WriteLine("copying files");
            await Task.Delay(1000);

        }

        public dynamic CreateAdapter(string table)
        {
            var adapterType = m_dll.GetType("Dev.Adapters.employees." + table + "Adapter");
            dynamic adapter = Activator.CreateInstance(adapterType);
            Assert.IsNotNull(adapter);

            return adapter;

        }
        public dynamic CreateEmployee()
        {
            var personType = m_dll.GetType("Dev.Adapters.employees.employees");
            dynamic prs = Activator.CreateInstance(personType);
            Assert.IsNotNull(prs);

            return prs;

        }

        [TestMethod]
        public async Task InsertEmployeeInstanceTest()
        {
            await m_adapter.ExecuteNonQueryAsync("DELETE FROM employees WHERE first_name = 'Erymuzuan'");
            var no = await m_adapter.GetDatabaseScalarAsync<int>("SELECT MAX(emp_no) FROM employees");
            await this.CompileAsync();
            var emp = this.CreateEmployee();
            emp.emp_no = no +1;
            emp.first_name = "Erymuzuan";
            emp.last_name = "Mustapa";
            emp.gender = "M";
            emp.hire_date = new DateTime(2005, 1, 5);
            emp.birth_date = new DateTime(1979, 9, 9);


            var adapter = this.CreateAdapter("employees");
            await adapter.InsertAsync(emp);
            var count =await m_adapter.GetDatabaseScalarAsync<long>("SELECT COUNT(*) FROM employees WHERE first_name = 'Erymuzuan'");
            Assert.AreEqual(1, count);

        }



    }
}
