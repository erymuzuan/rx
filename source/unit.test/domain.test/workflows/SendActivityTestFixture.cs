using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Moq;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class SendActivityTestFixture
    {
        private readonly string m_schemaStoreId = Guid.NewGuid().ToString();

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

            var code = sendToEmployees.GeneratedExecutionMethodCode(wd);
            StringAssert.Contains( "await adapter.InsertAsync(this.staff);",code);

            const string ADAPTER_PATH = @"C:\project\work\sph\bin\output\Dev.MySqlSampleTest001.dll";
            File.Copy(ADAPTER_PATH, AppDomain.CurrentDomain.BaseDirectory + @"\Dev.MySqlSampleTest001.dll",true);
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

            Assert.IsNotNull(wf.staff);

            await wf.StartAsync();
        }
    }
}