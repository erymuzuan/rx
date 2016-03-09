using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Xunit;

namespace domain.test.workflows
{
    public class WorkflowExecutionTest : WorkflowTestBase
    {

        [Fact]
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
            var wf = this.CreateInstance(wd, result.Output);
            Assert.NotNull(wf);
        }


        [Fact]
        public async Task Listen()
        {
            var wd = this.Create("Wf502");
            wd.ActivityCollection.Add(new ReceiveActivity
            {
                Name = "Starts",
                Operation = "StartsSubmit",
                MessagePath = "alamat",
                IsInitiator = true,
                WebId = "_A_",
                NextActivityWebId = "_B_"
            });

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

            var scree2 = new ReceiveActivity { WebId = "_B11_", Operation = "Screen2", MessagePath = "alamat", NextActivityWebId = "_C_", Name = "Screen 2" };
            wd.ActivityCollection.Add(scree2);

            wd.ActivityCollection.Add(new EndActivity { WebId = "_C_", Name = "Habis" });
            var result = await this.CompileAsync(wd, true);
            var wf = this.CreateInstance(wd, result.Output);
            await wf.StartAsync();

            var resultA = await wf.ExecuteAsync("_A_");
            Assert.Equal(new[] { "_B_" }, resultA.NextActivities);
        }

        [Fact]
        public async Task CompileAndRun()
        {
            var wd = this.Create("Wf500");

            var pohon = new ReceiveActivity
            {
                Name = "Pohon",
                WebId = "_A_",
                IsInitiator = true,
                NextActivityWebId = "_B_"
            };

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


            var approval = new ReceiveActivity
            {
                WebId = "_C_",
                Name = "Kelulusan",
                NextActivityWebId = "_WA_",
            };
            wd.ActivityCollection.Add(approval);


            wd.ActivityCollection.Add(new EndActivity { WebId = "_D_", Name = "habis" });

            var compilerResult = await this.CompileAsync(wd);
            var wf = this.CreateInstance(wd, compilerResult.Output);
            var execResult = await wf.StartAsync();
            Console.WriteLine(wf);
            Assert.Equal(new[] { "_B_" }, execResult.NextActivities);



        }

        [Fact]
        public async Task EmailFieldExpression()
        {
            var wd = this.Create("Wf501");

            var pohon = new ReceiveActivity
            {
                Name = "Pohon",
                WebId = "_A_",
                IsInitiator = true,
                NextActivityWebId = "_EMAIL_"
            };

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
                Name = "Email me",
                Retry = 5,
                RetryInterval = 50 * 1000

            };
            wd.ActivityCollection.Add(email);


            wd.ActivityCollection.Add(new EndActivity { WebId = "_END_", Name = "habis" });

            var compile = await this.CompileAsync(wd, true);
            var wf = this.CreateInstance(wd, compile.Output);
            var execResult = await wf.StartAsync();
            Console.WriteLine(execResult);
            Assert.Equal(new[] { "_EMAIL_" }, execResult.NextActivities);



        }
    }
}
