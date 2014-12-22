using System;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using domain.test.reports;
using Moq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using sqlserver.adapter.test;

namespace domain.test.workflows
{
    [TestFixture]
    public class SendActivityTestFixture
    {
        private readonly string m_schemaStoreId = Guid.NewGuid().ToString();

        public const string MySQL = "Server=localhost;Database=employees;Uid=root;Pwd=";
        [SetUp]
        public void Init()
        {
            var doc = new BinaryStore
            {
                Content = File.ReadAllBytes(@".\workflows\PemohonWakaf.xsd")
            };
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);
            store.Setup(x => x.GetContent(m_schemaStoreId))
                .Returns(doc);
            ObjectBuilder.AddCacheList(store.Object);



        }


        [Test]
        public async Task SendActivityWithInitializingCorrelationSet()
        {
            var trackerRepository = new MockRepository<Tracker>();
            trackerRepository.AddToDictionary("System.Linq.IQueryable`1[Bespoke.Sph.Domain.Tracker]",null);

            ObjectBuilder.AddCacheList<IRepository<Tracker>>(trackerRepository);
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());

            var wd = new WorkflowDefinition { Name = "Insert employee into MySql with correction set", Id = "mysql-insert-employee-crs", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "empNo", Type = typeof(int) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "staff", TypeName = "Dev.Adapters.employees.MySqlSampleTest001.employees" });

            var crt = new CorrelationType { Name = "emp_no" };
            crt.CorrelationPropertyCollection.Add(new CorrelationProperty { Path = "staff.emp_no" });
            crt.CorrelationPropertyCollection.Add(new CorrelationProperty { Path = "staff.birth_date" });
            wd.CorrelationTypeCollection.Add(crt);
            wd.CorrelationSetCollection.Add(new CorrelationSet { Type =crt.Name, Name = "empno" });

            var sendToEmployees = new SendActivity
            {
                Name = "Create employee",
                Adapter = "Dev.Adapters.employees.MySqlSampleTest001.employeesAdapter",
                NextActivityWebId = "B",
                IsInitiator = true,
                AdapterAssembly = "Dev.MySqlSampleTest001",
                WebId = "A",
                Method = "InsertAsync",
                ArgumentPath = "staff",
                ReturnValuePath = "empNo",


            };
            sendToEmployees.InitializingCorrelationSetCollection.Add("empno");
            wd.ActivityCollection.Add(sendToEmployees);

            var code = sendToEmployees.GenerateExecMethodBody(wd);
            StringAssert.Contains("await adapter.InsertAsync(this.staff);", code);

            const string ADAPTER_PATH = @"C:\project\work\sph\bin\output\Dev.MySqlSampleTest001.dll";
            File.Copy(ADAPTER_PATH, AppDomain.CurrentDomain.BaseDirectory + @"\Dev.MySqlSampleTest001.dll", true);
            var options = new CompilerOptions();
            options.AddReference(AppDomain.CurrentDomain.BaseDirectory + @"\Dev.MySqlSampleTest001.dll");
            options.AddReference(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll"));
            options.AddReference(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\core.sph.dll"));
            options.AddReference(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll"));

            var cr = wd.Compile(options);
            cr.Errors.ForEach(Console.WriteLine);
            Assert.IsTrue(cr.Result);

            var wfDll = Assembly.LoadFile(cr.Output);
            dynamic wf = Activator.CreateInstance(wfDll.GetType("Bespoke.Sph.Workflows_MysqlInsertEmployeeCrs_0.MysqlInsertEmployeeCrsWorkflow"));


            wf.staff.emp_no = 784528;
            wf.staff.gender = "M";
            wf.staff.first_name = "Marco";
            wf.staff.last_name = "Pantani";
            wf.staff.birth_date = new DateTime(1970, 2, 1);
            wf.staff.hire_date = new DateTime(1995, 5, 21);
            wf.WorkflowDefinition = wd;
            wf.Id = Guid.NewGuid().ToString();

            await MySQL.ExecuteMySqlNonQueryAsync("DELETE FROM `employees` WHERE `emp_no` = 784528");

            Assert.IsNotNull(wf.staff);
            await wf.StartAsync();
        }




        [Test]
        public async Task Compile()
        {

            var wd = new WorkflowDefinition { Name = "Insert new employee into MySql", Id = "mysql-insert-employee", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "empNo", Type = typeof(int) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "staff", TypeName = "Dev.Adapters.employees.employees" });

            var sendToEmployees = new SendActivity
            {
                Name = "Create employee",
                Adapter = "Dev.Adapters.employees.employeesAdapter",
                NextActivityWebId = "B",
                IsInitiator = true,
                AdapterAssembly = "Dev.MySqlSampleTest001",
                WebId = "A",
                Method = "InsertAsync",
                ArgumentPath = "staff",
                ReturnValuePath = "empNo",

            };
            wd.ActivityCollection.Add(sendToEmployees);

            var code = sendToEmployees.GenerateExecMethodBody(wd);
            StringAssert.Contains("await adapter.InsertAsync(this.staff);", code);

            const string ADAPTER_PATH = @"C:\project\work\sph\bin\output\Dev.MySqlSampleTest001.dll";
            File.Copy(ADAPTER_PATH, AppDomain.CurrentDomain.BaseDirectory + @"\Dev.MySqlSampleTest001.dll", true);
            var options = new CompilerOptions();
            options.AddReference(AppDomain.CurrentDomain.BaseDirectory + @"\Dev.MySqlSampleTest001.dll");
            options.AddReference(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll"));
            options.AddReference(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\core.sph.dll"));
            options.AddReference(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll"));

            var cr = wd.Compile(options);
            cr.Errors.ForEach(Console.WriteLine);
            Assert.IsTrue(cr.Result);

            var wfDll = Assembly.LoadFile(cr.Output);
            dynamic wf = Activator.CreateInstance(wfDll.GetType("Bespoke.Sph.Workflows_MysqlInsertEmployee_0.MysqlInsertEmployeeWorkflow"));


            wf.staff.emp_no = 784528;
            wf.staff.gender = "M";
            wf.staff.first_name = "Marco";
            wf.staff.last_name = "Pantani";
            wf.staff.birth_date = new DateTime(1970, 2, 1);
            wf.staff.hire_date = new DateTime(1995, 5, 21);


            await MySQL.ExecuteMySqlNonQueryAsync("DELETE FROM `employees` WHERE `emp_no` = 784528");

            Assert.IsNotNull(wf.staff);
            await wf.StartAsync();
        }



        [Test]
        [ExpectedException(typeof(MySqlException))]
        public async Task CompileWithRetry()
        {

            var wd = new WorkflowDefinition { Name = "Insert new employee into MySql", Id = "mysql-insert-employee", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "empNo", Type = typeof(int) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "staff", TypeName = "Dev.Adapters.employees.employees" });

            var sendToEmployees = new SendActivity
            {
                Name = "Create employee",
                Adapter = "Dev.Adapters.employees.employeesAdapter",
                NextActivityWebId = "B",
                IsInitiator = true,
                AdapterAssembly = "Dev.MySqlSampleTest001",
                WebId = "A",
                Method = "InsertAsync",
                ArgumentPath = "staff",
                ReturnValuePath = "empNo",

            };
            wd.ActivityCollection.Add(sendToEmployees);

            var filter = new ExceptionFilter
            {
                MaxRequeue = 3,
                Interval = 1,
                IntervalPeriod = "seconds",
                TypeName = "MySql.Data.MySqlClient.MySqlException"
            };
            sendToEmployees.ExceptionFilterCollection.Add(filter);

            var code = sendToEmployees.GenerateExecMethodBody(wd);
            StringAssert.Contains("await adapter.InsertAsync(this.staff);", code);

            const string ADAPTER_PATH = @"C:\project\work\sph\bin\output\Dev.MySqlSampleTest001.dll";
            File.Copy(ADAPTER_PATH, AppDomain.CurrentDomain.BaseDirectory + @"\Dev.MySqlSampleTest001.dll", true);
            var options = new CompilerOptions();
            options.AddReference(AppDomain.CurrentDomain.BaseDirectory + @"\Dev.MySqlSampleTest001.dll");
            options.AddReference(AppDomain.CurrentDomain.BaseDirectory + @"\MySql.Data.dll");
            options.AddReference(typeof(DbException));
            options.AddReference(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll"));
            options.AddReference(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\core.sph.dll"));
            options.AddReference(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll"));

            var cr = wd.Compile(options);
            cr.Errors.ForEach(Console.WriteLine);
            Assert.IsTrue(cr.Result);

            var wfDll = Assembly.LoadFile(cr.Output);
            dynamic wf = Activator.CreateInstance(wfDll.GetType("Bespoke.Sph.Workflows_MysqlInsertEmployee_0.MysqlInsertEmployeeWorkflow"));


            wf.staff.emp_no = 10100;
            wf.staff.gender = "M";
            wf.staff.first_name = "Marco";
            wf.staff.last_name = "Pantani";
            wf.staff.birth_date = new DateTime(1970, 2, 1);
            wf.staff.hire_date = new DateTime(1995, 5, 21);

            Assert.IsNotNull(wf.staff);

            await wf.StartAsync();
        }
    }
}