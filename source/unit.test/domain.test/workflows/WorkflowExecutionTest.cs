using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class WorkflowExecutionTest : WorkflowTestBase
    {

        [Test]
        public async Task TriggerSchedule()
        {
            var wd = this.Create("Wf503");
            wd.ActivityCollection.Add(new ScheduledTriggerActivity
            {
                Name = "Starts",
                IsInitiator = true,
                WebId = "_A_",
                NextActivityWebId = "_B_"
            });

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
            var result = await this.CompileAsync(wd, true);
            var wf = this.CreateInstance(wd, result.Buffer);
            Assert.IsNotNull(wf);
        }


        [Test]
        public async Task Listen()
        {
            var wd = this.Create("Wf502");
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
            var result = await this.CompileAsync(wd, true).ConfigureAwait(false);
            var wf = this.CreateInstance(wd, result.Buffer);
            await wf.StartAsync();

            var resultA = await wf.ExecuteAsync("_A_");
            Assert.AreEqual(new[] { "_B_" }, resultA.NextActivities);
        }

        [Test]
        public async Task CompileAndRun()
        {
            var wd = this.Create("Wf500");

            var pohon = new ScreenActivity
            {
                Title = "Pohon",//[A-Z|a-z|.]
                Name = "Pohon",
                ViewVirtualPath = "~/Views/Workflows_8_1/pohon.cshtml",
                WebId = "_A_",
                IsInitiator = true,
                NextActivityWebId = "_B_"
            };
            //pohon.FormDesign.FormElementCollection.Add(new TextBox { Path = "Nama", Label = "Test" });
            //pohon.FormDesign.FormElementCollection.Add(new TextBox { Path = "Title", Label = "Tajuk" });
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

            var compilerResult = await this.CompileAsync(wd).ConfigureAwait(false);
            var wf = this.CreateInstance(wd, compilerResult.Buffer);
            var execResult = await wf.StartAsync();
            Console.WriteLine(wf);
            Assert.AreEqual(new[] { "_B_" }, execResult.NextActivities);



        }

        [Test]
        public async Task EmailFieldExpression()
        {
            var wd = this.Create("Wf501");

            var pohon = new ScreenActivity
            {
                Title = "Pohon",//[A-Z|a-z|.]
                Name = "Pohon",
                ViewVirtualPath = "~/Views/Workflows_8_1/pohon.cshtml",
                WebId = "_A_",
                IsInitiator = true,
                NextActivityWebId = "_EMAIL_"
            };
            //pohon.FormDesign.FormElementCollection.Add(new TextBox { Path = "Nama", Label = "Test" });
            //pohon.FormDesign.FormElementCollection.Add(new TextBox { Path = "Title", Label = "Tajuk" });
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

            var cr = await this.CompileAsync(wd, true).ConfigureAwait(false);
            var wf = this.CreateInstance(wd, cr.Buffer);
            var execResult = await wf.StartAsync();
            Console.WriteLine(execResult);
            Assert.AreEqual(new[] { "_EMAIL_" }, execResult.NextActivities);



        }
    }
}
