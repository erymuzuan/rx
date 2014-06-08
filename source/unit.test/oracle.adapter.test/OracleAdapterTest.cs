using System;
using System.Threading.Tasks;
using Bespoke.Sph.Integrations.Adapters;
using NUnit.Framework;

namespace oracle.adapter.test
{
    [TestFixture]
    public class OracleAdapterTest
    {
        [Test]
        public async Task TestOra()
        {
            var ora = new OracleAdapter
            {
                ConnectionString =  "Data Source=(DESCRIPTION="
             + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=i90009638.cloudapp.net)(PORT=1521)))"
             + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=XE)));"
             + "User Id=SYSTEM;Password=gsxr750wt;",
                Table = "EMPLOYEES",
                Name = "Hr Country",
                Description = "Ora HR Countries",
                Schema = "HR"
            };
            await ora.OpenAsync();

            var employeeType = await ora.CompileAsync();
            dynamic emp = Activator.CreateInstance(employeeType);
            Assert.IsNotNull(emp);

            emp.EMPLOYEE_ID = 2556;
            emp.FIRST_NAME = Guid.NewGuid().ToString().Substring(0,8);
            emp.LAST_NAME = "mustapa";
            emp.EMAIL = "erymuzuan@gmail.com.my";
            emp.PHONE_NUMBER = "0123889200";
            emp.HIRE_DATE = new DateTime(2000, 1, 1);
            emp.JOB_ID = "IT_PROG";
            emp.SALARY = 2000;
            emp.DEPARTMENT_ID = 210;

            var oraType = employeeType.Assembly.GetType("Dev.Adapters.HR.EMPLOYEESAdapter");
            Assert.IsNotNull(oraType);
            dynamic oradb = Activator.CreateInstance(oraType);

            // delete
            await oradb.DeleteAsync(emp);

            await oradb.InsertAsync(emp);

            var emp2 = await oradb.LoadOneAsync(emp.EMPLOYEE_ID);
            Assert.IsNotNull(emp2);
            Assert.AreEqual(emp.FIRST_NAME, emp2.FIRST_NAME);


        }
    }
}
