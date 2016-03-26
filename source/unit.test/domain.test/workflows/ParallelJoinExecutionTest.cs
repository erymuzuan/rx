using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Xunit;

namespace domain.test.workflows
{
    [Trait("Category", "Workflow")]
    [Trait("Category", "Parallel")]
    public class ParallelJoinExecutionTest : WorkflowTestBase
    {

        private WorkflowDefinition CreateParallelAndJoinWorkflow()
        {
            var wd = this.Create("Wf12");
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

        [Fact]
        public async Task Join()
        {
            var wd = this.CreateParallelAndJoinWorkflow();
            var br = wd.ValidateBuild();
            br.Errors.ForEach(Console.WriteLine);

            Assert.True(br.Result);
            var result =await this.CompileAsync(wd);
            var wf = this.CreateInstance(wd, result.Output);
            await wf.StartAsync();

            var resultB = await wf.ExecuteAsync("_B_");
            Assert.NotNull(resultB);
            Assert.Equal(new[] { "C0", "C1" }, resultB.NextActivities);

            // when 1st of the predessor fired, it should initiate the Join to wait for others
            var resultC0 = await wf.ExecuteAsync("C0");
            Assert.NotNull(resultC0);
            Assert.Equal(new[] { "D" }, resultC0.NextActivities);

            // fire C0
            var tracker = await wf.GetTrackerAsync();
            Assert.NotNull(tracker);
            Assert.True(tracker.WaitingJoinList.ContainsKey("D"));
            Assert.Equal(new[] { "C0", "C1" }, tracker.WaitingJoinList["D"].ToArray());
            Assert.True(tracker.FiredJoinList.ContainsKey("D"));
            Assert.Equal(new[] { "C0" }, tracker.FiredJoinList["D"].ToArray());

            await Task.Delay(500);
            // now execute C1
            var resultC1 = await wf.ExecuteAsync("C1");
            Assert.NotNull(resultC1);
            Assert.Equal(new[] { "D" }, resultC1.NextActivities);
            Assert.Equal(new[] { "C0", "C1" }, tracker.FiredJoinList["D"].ToArray());

        }

        [Fact]
        public async Task Parallel()
        {
            var wd = this.Create("Wf10");
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


            Assert.True(br.Result);
            var result =await this.CompileAsync(wd, true);
            var wf = this.CreateInstance(wd, result.Output);
            await wf.StartAsync();

            await Task.Delay(500);
            var resultB = await wf.ExecuteAsync("_B_");

            Assert.NotNull(resultB);
        }
    }
}