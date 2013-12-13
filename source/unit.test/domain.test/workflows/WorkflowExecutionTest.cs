using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class WorkflowExecutionTest : WorkflowTestBase
    {

        [Test]
        public void BuildValidation()
        {
            var wd = new WorkflowDefinition { Name = "3 Is Three" };
            var screen = new ScreenActivity { Name = "Pohon", IsInitiator = true, WebId = Guid.NewGuid().ToString() };
            screen.FormDesign.FormElementCollection.Add(new TextBox { Label = "Nama", Path = string.Empty });
            wd.ActivityCollection.Add(screen);


            var result = wd.ValidateBuild();
            Console.WriteLine(result.ToJsonString());
            Assert.IsFalse(result.Result);
            Assert.AreEqual(2, result.Errors.Count);
            Assert.AreEqual("Name must be started with letter.You cannot use symbol or number as first character", result.Errors[0].Message);
            Assert.AreEqual("[ScreenActivity] : Pohon => 'Nama' does not have path", result.Errors[1].Message);

        }

        [Test]
        public void BuildValidationMissingWebId()
        {
            var wd = new WorkflowDefinition { Name = "Test Workflow" };
            var screen = new ScreenActivity { Name = "Pohon", IsInitiator = true };
            screen.FormDesign.FormElementCollection.Add(new TextBox { Label = "Nama", Path = "Nama" });
            wd.ActivityCollection.Add(screen);


            var result = wd.ValidateBuild();
            Console.WriteLine(result.ToJsonString());
            Assert.IsFalse(result.Result);
            Assert.AreEqual(1, result.Errors.Count);
            StringAssert.Contains("Missing webid", result.Errors[0].ToString());

        }

        [Test]
        public void CompileError()
        {
            var wd = new WorkflowDefinition { Name = "Test Workflow", SchemaStoreId = "schema-storeid" };
            var screen = new ScreenActivity { Name = "Pohon", IsInitiator = true, WebId = "A", NextActivityWebId = "B" };
            screen.FormDesign.FormElementCollection.Add(new TextBox { Label = "Nama", Path = "Nama" });
            wd.ActivityCollection.Add(screen);

            var exp = new ExpressionActivity { WebId = "B", Name = "Expression B", Expression = "tet test-----", NextActivityWebId = "C" };
            wd.ActivityCollection.Add(exp);
            wd.ActivityCollection.Add(new EndActivity { Name = "C", WebId = "C" });

            var result = this.Compile(wd, true, assertError: false);

            Assert.IsFalse(result.Result);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(exp.WebId, result.Errors[0].ItemWebId);
            StringAssert.Contains(exp.Expression, result.Errors[0].Code);
            StringAssert.Contains("; expected", result.Errors[0].Message);

        }


        [Test]
        public void BuildValidationDuplicateWebId()
        {
            var wd = new WorkflowDefinition { Name = "Test Workflow" };
            var screen = new ScreenActivity { Name = "Pohon", IsInitiator = true, WebId = "A", NextActivityWebId = "B" };
            var screen2 = new ScreenActivity { Name = "Pohon 2", IsInitiator = false, WebId = "A", NextActivityWebId = "C" };
            screen.FormDesign.FormElementCollection.Add(new TextBox { Label = "Nama", Path = "Nama" });
            wd.ActivityCollection.Add(screen);
            wd.ActivityCollection.Add(screen2);

            var result = wd.ValidateBuild();
            Console.WriteLine(result.ToJsonString());
            Assert.IsFalse(result.Result);
            Assert.AreEqual(2, result.Errors.Count);
            StringAssert.Contains("Duplicate webid", result.Errors[0].ToString());

        }




        private WorkflowDefinition Create()
        {
            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", WorkflowDefinitionId = 8, SchemaStoreId = "schema-storeid" };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "email", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "pemohon", TypeName = "Applicant" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "alamat", TypeName = "Address" });

            return wd;
        }

        private WorkflowCompilerResult Compile(WorkflowDefinition wd, bool verbose = false, bool assertError = true)
        {
            this.BinaryStore.Setup(x => x.GetContent("wd-storeid"))
               .Returns(new BinaryStore { Content = Encoding.Unicode.GetBytes(wd.ToXmlString()), StoreId = "wd-storeid" });

            wd.Version = 25;
            var options = new CompilerOptions
            {
                IsDebug = true,
                SourceCodeDirectory = @"d:\temp\sph",
                IsVerbose = verbose
            };
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\web.sph.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll")));


            var result = wd.Compile(options);
            result.Errors.ForEach(Console.WriteLine);
            if (assertError)
                Assert.IsTrue(result.Result, result.ToJsonString(Formatting.Indented));

            return result;
        }

        private Workflow Run(WorkflowDefinition wd, string dll, Action<Task<ActivityExecutionResult>> continuationAction)
        {
            // try to instantiate the Workflow
            var assembly = Assembly.LoadFrom(dll);
            var wfTypeName = string.Format("Bespoke.Sph.Workflows_{0}_{1}.{2}", wd.WorkflowDefinitionId, wd.Version,
                wd.WorkflowTypeName);

            var wfType = assembly.GetType(wfTypeName);
            Assert.IsNotNull(wfType, wfTypeName + " is null");

            var wf = Activator.CreateInstance(wfType) as Workflow;
            Assert.IsNotNull(wf);

            wf.SerializedDefinitionStoreId = "wd-storeid";
            XmlSerializerService.RegisterKnownTypes(typeof(Workflow), wfType);

            var pemohonProperty = wf.GetType().GetProperty("pemohon");
            Assert.IsNotNull(pemohonProperty);
            dynamic pemohon = pemohonProperty.GetValue(wf);
            pemohon.Age = 28;

            wf.WorkflowDefinition = wd;
            wf.StartAsync().ContinueWith(_ =>
            {
                if (null != continuationAction)
                    continuationAction(_);

            }).Wait();

            return wf;
        }


        [Test]
        public void Delay()
        {
            var wd = this.Create();
            wd.ActivityCollection.Add(new ScreenActivity { Name = "Start isi borang", IsInitiator = true, WebId = "_A_", NextActivityWebId = "_WA_" });
            wd.ActivityCollection.Add(new DelayActivity { Name = "Wait a second", Seconds = 1, WebId = "_WA_", NextActivityWebId = "_D_" });
            wd.ActivityCollection.Add(new EndActivity { WebId = "_C_", Name = "Habis" });
            var result = this.Compile(wd);
            this.Run(wd, result.Output, Console.WriteLine);
        }


        [Test]
        public void TriggerSchedule()
        {
            var wd = this.Create();
            wd.ActivityCollection.Add(new ScheduledTriggerActivity { Name = "Starts", IsInitiator = true, WebId = "_A_", NextActivityWebId = "_B_" });

            var send = new NotificationActivity
            {
                Name = "Send email",
                WebId = "_B_",
                NextActivityWebId = "_C_",
                Subject = "erymuzuan@@bespoke.com.my",
                Body = "erymuzuan@@bespoke.com.my",
                From = "erymuzuan@@bespoke.com.my",
                To = "erymuzuan@@gmail.com.my"
            };

            wd.ActivityCollection.Add(send);

            wd.ActivityCollection.Add(new EndActivity { WebId = "_C_", Name = "Habis" });
            var result = this.Compile(wd, true);
            this.Run(wd, result.Output, Console.WriteLine);

        }

        private WorkflowDefinition CreateParallelAndJoinWorkflow()
        {
            var wd = this.Create();
            wd.ActivityCollection.Add(new ExpressionActivity
            {
                Name = "Starts",
                IsInitiator = true,
                WebId = "_A_",
                NextActivityWebId = "_B_",
                Expression = "System.Console.WriteLine(\"Starting now \");"
            });

            var parallel = new ParallelActivity { Name = "Parallel", WebId = "_B_" };
            wd.ActivityCollection.Add(parallel);

            var w1 = new ParallelBranch { Name = "Worker 1", NextActivityWebId = "C0" };
            parallel.ParallelBranchCollection.Add(w1);

            var w2 = new ParallelBranch { Name = "Worker 2", NextActivityWebId = "C1" };
            parallel.ParallelBranchCollection.Add(w2);

            var c0 = new ExpressionActivity
            {
                Name = "C0",
                WebId = "C0",
                NextActivityWebId = "D",
                Expression = "await Task.Delay(300);"
            };
            wd.ActivityCollection.Add(c0);

            var c1 = new ExpressionActivity
            {
                Name = "C1",
                WebId = "C1",
                NextActivityWebId = "D",
                Expression = "await Task.Delay(500);"
            };
            wd.ActivityCollection.Add(c1);

            var jn = new JoinActivity { Name = "Join 1", WebId = "D", NextActivityWebId = "End" };
            wd.ActivityCollection.Add(jn);

            wd.ActivityCollection.Add(new EndActivity { Name = "End", WebId = "End" });
            return wd;

        }

        [Test]
        public async Task Join()
        {
            var wd = this.CreateParallelAndJoinWorkflow();
            var br = wd.ValidateBuild();
            br.Errors.ForEach(Console.WriteLine);

            Assert.IsTrue(br.Result);
            var result = this.Compile(wd);
            var wf = this.Run(wd, result.Output, Console.WriteLine);

            var resultB = await wf.ExecuteAsync("_B_");
            Assert.IsNotNull(resultB);
            Assert.AreEqual(new[] { "C0", "C1" }, resultB.NextActivities);

            // when 1st of the predessor fired, it should initiate the Join to wait for others
            var resultC0 = await wf.ExecuteAsync("C0");
            Assert.IsNotNull(resultC0);
            Assert.AreEqual(new[] { "D" }, resultC0.NextActivities);

            // fire C0
            var tracker = await wf.GetTrackerAsync();
            Assert.IsNotNull(tracker);
            Assert.IsTrue(tracker.WaitingJoinList.ContainsKey("D"));
            Assert.AreEqual(new[] { "C0", "C1" }, tracker.WaitingJoinList["D"].ToArray());
            Assert.IsTrue(tracker.FiredJoinList.ContainsKey("D"));
            Assert.AreEqual(new[] { "C0" }, tracker.FiredJoinList["D"].ToArray());

            await Task.Delay(500);
            // now execute C1
            var resultC1 = await wf.ExecuteAsync("C1");
            Assert.IsNotNull(resultC1);
            Assert.AreEqual(new[] { "D" }, resultC1.NextActivities);
            CollectionAssert.AreEqual(new[] { "C0", "C1" }, tracker.FiredJoinList["D"].ToArray());

        }


        [Test]
        public async Task Parallel()
        {
            var wd = this.Create();
            wd.ActivityCollection.Add(new ExpressionActivity
            {
                Name = "Starts",
                IsInitiator = true,
                WebId = "_A_",
                NextActivityWebId = "_B_",
                Expression = "System.Console.WriteLine(\"Starting now \");"
            });

            var parallel = new ParallelActivity { Name = "Parallel", WebId = "_B_" };
            wd.ActivityCollection.Add(parallel);

            var w1 = new ParallelBranch { Name = "Worker 1", NextActivityWebId = "C0" };
            parallel.ParallelBranchCollection.Add(w1);

            var w2 = new ParallelBranch { Name = "Worker 2", NextActivityWebId = "C1" };
            parallel.ParallelBranchCollection.Add(w2);

            var br = wd.ValidateBuild();
            br.Errors.ForEach(Console.WriteLine);


            Assert.IsTrue(br.Result);
            var result = this.Compile(wd, true);
            var wf = this.Run(wd, result.Output, Console.WriteLine);
            await Task.Delay(500);
            var resultB = await wf.ExecuteAsync("_B_");

            Assert.IsNotNull(resultB);
        }

        [Test]
        public async Task Listen()
        {
            var wd = this.Create();
            wd.ActivityCollection.Add(new ScreenActivity { Name = "Starts", IsInitiator = true, WebId = "_A_", NextActivityWebId = "_B_" });

            var listen = new ListenActivity
            {
                WebId = "_B_",
                Name = "Listen"
            };
            var verify = new ListenBranch
            {
                Name = "Wait for verification",
                WebId = "_B1_",
                NextActivityWebId = "_B11_"
            };
            var lapsed = new ListenBranch
            {
                Name = "Lapsed for verification",
                WebId = "_B2_",
                NextActivityWebId = "_B21_"
            };

            listen.ListenBranchCollection.Add(verify);
            listen.ListenBranchCollection.Add(lapsed);
            wd.ActivityCollection.Add(listen);

            var delay = new DelayActivity { WebId = "_B21_", NextActivityWebId = "_C_", Name = "Lapse" };
            wd.ActivityCollection.Add(delay);

            var scree2 = new ScreenActivity { WebId = "_B11_", NextActivityWebId = "_C_", Name = "Screen 2" };
            wd.ActivityCollection.Add(scree2);

            wd.ActivityCollection.Add(new EndActivity { WebId = "_C_", Name = "Habis" });
            var result = this.Compile(wd, true);
            var wf = this.Run(wd, result.Output, Console.WriteLine);

            var resultA = await wf.ExecuteAsync("_A_");
            Assert.AreEqual(new []{"_B_"}, resultA.NextActivities);
        }






        [Test]
        public void CompileAndRun()
        {
            var wd = this.Create();

            var pohon = new ScreenActivity
            {
                Title = "Pohon",//[A-Z|a-z|.]
                Name = "Pohon",
                ViewVirtualPath = "~/Views/Workflows_8_1/pohon.cshtml",
                WebId = "_A_",
                IsInitiator = true,
                NextActivityWebId = "_B_"
            };
            pohon.FormDesign.FormElementCollection.Add(new TextBox { Path = "Nama", Label = "Test" });
            pohon.FormDesign.FormElementCollection.Add(new TextBox { Path = "Title", Label = "Tajuk" });
            wd.ActivityCollection.Add(pohon);

            var decide = new DecisionActivity
            {
                Name = "Check applicant age group",
                WebId = "_B_",
                NextActivityWebId = "_C_"

            };
            decide.DecisionBranchCollection.Add(new DecisionBranch
            {
                Name = "Less than 25",
                Expression = "item.pemohon.Age < 25",
                NextActivityWebId = "_C_Below25"
            });
            decide.DecisionBranchCollection.Add(new DecisionBranch
            {
                Name = "25 to 50",
                Expression = "item.pemohon.Age >= 25 && item.pemohon.Age < 50",
                NextActivityWebId = "CREATE_BUILDING"
            });
            decide.DecisionBranchCollection.Add(new DecisionBranch
            {
                Name = "all else",
                Expression = "item.pemohon.Age >= 50",
                NextActivityWebId = "_EMAIL_",
                IsDefault = true
            });

            wd.ActivityCollection.Add(decide);

            var email = new NotificationActivity
            {
                From = "erymuzuan@gmail.com",
                To = "=item.email",
                Subject = "Ada permohonan baru @Model.Title",
                Body = "Permohonan baru di @Model.Title oleh @Model.pemohon.MyKad",
                WebId = "_EMAIL_",
                NextActivityWebId = "_C_",
                UserName = "admin"
                ,
                Name = "Email me"

            };
            wd.ActivityCollection.Add(email);


            var approval = new ScreenActivity
            {
                Title = "Kelulusan",
                WebId = "_C_",
                Name = "Kelulusan",
                NextActivityWebId = "_WA_",
                ViewVirtualPath = "d"
            };
            wd.ActivityCollection.Add(approval);


            wd.ActivityCollection.Add(new EndActivity { WebId = "_D_", Name = "habis" });

            var land = new CreateEntityActivity { Name = "Create Building", EntityType = "Building", NextActivityWebId = "_D_", WebId = "CREATE_BUILDING" };
            land.PropertyMappingCollection.Add(new SimpleMapping { Source = "Title", Destination = "Name" });
            wd.ActivityCollection.Add(land);



            var result = this.Compile(wd);
            this.Run(wd, result.Output, r2 =>
            {
                var result2 = r2.Result;
                Console.WriteLine(result2);
                Assert.AreEqual(new[] { "_B_" }, result2.NextActivities);
            });


        }

        [Test]
        public void EmailFieldExpression()
        {
            var wd = this.Create();

            var pohon = new ScreenActivity
            {
                Title = "Pohon",//[A-Z|a-z|.]
                Name = "Pohon",
                ViewVirtualPath = "~/Views/Workflows_8_1/pohon.cshtml",
                WebId = "_A_",
                IsInitiator = true,
                NextActivityWebId = "_EMAIL_"
            };
            pohon.FormDesign.FormElementCollection.Add(new TextBox { Path = "Nama", Label = "Test" });
            pohon.FormDesign.FormElementCollection.Add(new TextBox { Path = "Title", Label = "Tajuk" });
            wd.ActivityCollection.Add(pohon);


            var email = new NotificationActivity
            {
                From = "=\"erymuzuan@gmail.com\"",
                To = "=item.email",
                Subject = "Ada permohonan baru @Model.Title",
                Body = "Permohonan baru di @Model.Title oleh @Model.pemohon.MyKad",
                WebId = "_EMAIL_",
                NextActivityWebId = "_END_",
                UserName = "admin",
                Name = "Email me"

            };
            wd.ActivityCollection.Add(email);


            wd.ActivityCollection.Add(new EndActivity { WebId = "_END_", Name = "habis" });

            var result = this.Compile(wd, true);
            this.Run(wd, result.Output, r2 =>
            {
                var result2 = r2.Result;
                Console.WriteLine(result2);
                Assert.AreEqual(new[] { "_EMAIL_" }, result2.NextActivities);
            });


        }
    }
}
