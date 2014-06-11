﻿using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using NUnit.Framework;
using Oracle.ManagedDataAccess.Client;

namespace oracle.adapter.test
{
    [TestFixture]
    public class OracleAdapterTest
    {
        private async Task<T> GetScalarAsync<T>(string cs, string sql)
        {
            using (var conn = new OracleConnection(cs))
            using (var cmd = new OracleCommand(sql, conn))
            {
                await conn.OpenAsync();
                var val = (await cmd.ExecuteScalarAsync());
                Console.WriteLine(val.GetType().FullName);
                return (T)val;
            }
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

            var max = await this.GetScalarAsync<decimal>(ora.ConnectionString, "SELECT MAX(EMPLOYEE_ID) FROM HR.EMPLOYEES");
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
    }
}
