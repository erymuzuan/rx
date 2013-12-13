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
        public void TriggerSchedule()
        {
            var wd = this.Create(503);
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


        [Test]
        public async Task Listen()
        {
            var wd = this.Create(502);
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
            Assert.AreEqual(new[] { "_B_" }, resultA.NextActivities);
        }






        [Test]
        public void CompileAndRun()
        {
            var wd = this.Create(500);

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
            var wd = this.Create(501);

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
