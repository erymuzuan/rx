using System;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using Newtonsoft.Json;
using NUnit.Framework;

namespace oracle.adapter.test
{
    [TestFixture]
    public class OracleAdapterTest
    {
        [SetUp]
        public void Init()
        {
            Console.WriteLine("cleaning all...");
            var bin = ConfigurationManager.WebPath + @"\bin\Dev.HR_EMPLOYEES.dll";
            if (File.Exists(bin))
                File.Delete(bin);
            var pdb = ConfigurationManager.WebPath + @"\bin\Dev.HR_EMPLOYEES.pdb";
            if (File.Exists(pdb))
                File.Delete(pdb);

        }

        [Test]
        public void PagingSimpleSelect()
        {
            var paging = new __NAMESPACE__.OraclePagingTranslator();
            var sql = paging.Translate("SELECT EMPLOYEE_ID,FIRST_NAME,LAST_NAME FROM HR.EMPLOYEES WHERE SALARY > 5000 AND SALARY <=10000 ORDER BY EMPLOYEE_ID", 1, 20);
            StringAssert.Contains("ROW_NUMBER()", sql);
        }

        [Test]
        public void PagingSimpleSelectAll()
        {
            var paging = new __NAMESPACE__.OraclePagingTranslator();
            var sql = paging.Translate("SELECT * FROM HR.EMPLOYEES WHERE SALARY > 5000 AND SALARY <=10000 ORDER BY LAST_NAME", 1, 20);
            StringAssert.Contains("ROW_NUMBER()", sql);
            StringAssert.Contains("a.*", sql);
        }

        [Test]
        public async Task TestDosmPpb()
        {
            var ora = new OracleAdapter
            {
                Server = "i90009638.cloudapp.net",
                UserId = "system",
                Password = "gsxr750wt",
                Sid = "XE",
                Tables = new[] { new AdapterTable { Name = "PPB" } },
                Name = "NEWSSSIT_MonthlySurvey",
                Description = "PPM Survey",
                Schema = "NEWSSSIT"
            };
            await ora.OpenAsync();

            var result = await ora.CompileAsync();
            Assert.IsTrue(File.Exists(result.Output));



        }
        [Test]
        public async Task HrSchema()
        {
            var tables = new AdapterTable[3];
            tables[0] = new AdapterTable
            {
                Name = "EMPLOYEES",
                Parents = new[] { "DEPARTMETS" },
                Children = new[] { "JOB_HISTORY" }
            };
            tables[1] = new AdapterTable { Name = "JOBS" };
            tables[2] = new AdapterTable { Name = "DEPARTMENTS" };
            var ora = new OracleAdapter
            {
                Server = "i90009638.cloudapp.net",
                UserId = "system",
                Password = "gsxr750wt",
                Sid = "XE",
                Tables = tables,
                Name = "HR_EMPLOYEES",
                Description = "Ora HR Countries",
                Schema = "HR"
            };
            await ora.OpenAsync();

            var result = await ora.CompileAsync();
            var dll = Assembly.LoadFile(result.Output);
            var employeeType = dll.GetType("Dev.Adapters.HR.EMPLOYEES");
            dynamic emp = Activator.CreateInstance(employeeType);
            Assert.IsNotNull(emp);

            var max = await ora.GetScalarAsync<decimal>("SELECT MAX(EMPLOYEE_ID) FROM HR.EMPLOYEES");
            emp.EMPLOYEE_ID = Convert.ToInt32(max) + 1;
            emp.FIRST_NAME = Guid.NewGuid().ToString().Substring(0, 8);
            emp.LAST_NAME = "mustapa";
            emp.EMAIL = string.Format("ery{0}@gmail.com.my", emp.EMPLOYEE_ID);
            emp.PHONE_NUMBER = "0123889200";
            emp.HIRE_DATE = new DateTime(2000, 1, 1);
            emp.JOB_ID = "IT_PROG";
            emp.SALARY = 2000;
            emp.DEPARTMENT_ID = 210;

            var oraType = employeeType.Assembly.GetType("Dev.Adapters.HR.EMPLOYEESAdapter");
            Assert.IsNotNull(oraType);
            dynamic oradb = Activator.CreateInstance(oraType);

            // delete
            await oradb.DeleteAsync(emp.EMPLOYEE_ID);
            // insert
            await oradb.InsertAsync(emp);
            // load
            var emp2 = await oradb.LoadOneAsync(emp.EMPLOYEE_ID);
            Assert.IsNotNull(emp2);
            Assert.AreEqual(emp.FIRST_NAME, emp2.FIRST_NAME);

            // update
            emp2.FIRST_NAME = "erymuzuan";
            var updatedRow = await oradb.UpdateAsync(emp2);
            Assert.AreEqual(1, updatedRow);
            var emp3 = await oradb.LoadOneAsync(emp.EMPLOYEE_ID);
            Assert.IsNotNull(emp3);
            Assert.AreEqual("erymuzuan", emp3.FIRST_NAME);



        }

        [Test]
        public async Task HrHttpApi()
        {
            var tables = new AdapterTable[3];
            tables[0] = new AdapterTable
            {
                Name = "EMPLOYEES",
                Parents = new[] { "DEPARTMETS" },
                Children = new[] { "JOB_HISTORY" }

            };
            tables[1] = new AdapterTable { Name = "JOBS" };
            tables[2] = new AdapterTable { Name = "DEPARTMENTS" };
            var ora = new OracleAdapter
            {
                Server = "i90009638.cloudapp.net",
                UserId = "system",
                Password = "gsxr750wt",
                Sid = "XE",
                Tables = tables,
                Name = "HR_EMPLOYEES",
                Description = "Ora HR Countries",
                Schema = "HR"
            };
            await ora.OpenAsync();

            var result = await ora.CompileAsync();
            Assert.IsTrue(result.Result);

            var bin = ConfigurationManager.WebPath + @"\bin\Dev.HR_EMPLOYEES.dll";
            var pdb = ConfigurationManager.WebPath + @"\bin\Dev.HR_EMPLOYEES.pdb";
            File.Copy(result.Output, bin, true);
            File.Copy(result.Output.Replace(".dll", ".pdb"), pdb, true);

            dynamic emp = new ExpandoObject();

            var max = await ora.GetScalarAsync<decimal>("SELECT MAX(EMPLOYEE_ID) FROM HR.EMPLOYEES");
            emp.EMPLOYEE_ID = Convert.ToInt32(max) + 1;
            emp.FIRST_NAME = Guid.NewGuid().ToString().Substring(0, 8);
            emp.LAST_NAME = "mustapa";
            emp.EMAIL = string.Format("ery{0}@gmail.com.my", emp.EMPLOYEE_ID);
            emp.PHONE_NUMBER = "0123889200";
            emp.HIRE_DATE = new DateTime(2000, 1, 1);
            emp.JOB_ID = "IT_PROG";
            emp.SALARY = 2000;
            emp.DEPARTMENT_ID = 210;

            // HTTP API - insert
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4436");
                var json = JsonConvert.SerializeObject(emp, Formatting.Indented);
                Console.WriteLine(json);
                var content = new StringContent(json);
                var response = await client.PostAsync("/api/hr/employees", content);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
                var id =await ora.GetScalarAsync<int>("SELECT MAX(EMPLOYEE_ID) FROM EMPLOYEE_ID");
                Assert.AreEqual(emp.EMPLOYEE_ID, id);
            }





        }
    }
}
