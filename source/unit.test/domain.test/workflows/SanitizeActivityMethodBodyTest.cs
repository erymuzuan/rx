using Bespoke.Sph.Domain;
using Xunit;

namespace domain.test.workflows
{
    public class SanitizeActivityMethodBodyTest
    {
        [Fact]
        public void WithException()
        {
            var join = new ThrowExceptionActivity { Name = "TestAct", WebId = "TestAct" };
            var wd = new WorkflowDefinition { Name = "TestActThrow", Version = 0 };
            var code = wd.SanitizeMethodBody(join);
            Assert.NotNull(code);
            Assert.NotEmpty(code);

            Assert.DoesNotContain("async ", code);
            Assert.DoesNotContain("return ", code);
            Assert.DoesNotContain("result ", code);
            Assert.Contains("var now", code);
        }

        [Fact]
        public void NormalResult()
        {
            var join = new NormalCodeActivity { Name = "TestAct", WebId = "TestAct" };
            var wd = new WorkflowDefinition { Name = "TestActThrow", Version = 0 };
            var code = wd.SanitizeMethodBody(join);
            Assert.NotNull(code);
            Assert.NotEmpty(code);

            Assert.DoesNotContain("async ", code);
            Assert.Contains("return Task.FromResult(result);", code);
        }
        [Fact]
        public void NormalAsyncResult()
        {
            var join = new NormalCodeActivity { Name = "TestAct", WebId = "TestAct", IsAsyncMethod = true };
            var wd = new WorkflowDefinition { Name = "TestActThrow", Version = 0 };
            var code = wd.SanitizeMethodBody(join);
            Assert.NotNull(code);
            Assert.Contains("async ", code);
            Assert.Contains("return result;", code);
        }

        class NormalCodeActivity : Activity
        {
            public bool IsAsyncMethod { private get; set; }
            public override string GenerateExecMethodBody(WorkflowDefinition wd)
            {
                if (this.IsAsyncMethod)
                {

                    return @"
                    var result = new ActivityExecutionResult();
                    await Task.Delay(500).ConfigureAwait(false);
                    Console.WriteLine(result);";
                }
                return @"
                    var result = new ActivityExecutionResult();
                    ;
                    Console.WriteLine(result);";
            }
        }

        class ThrowExceptionActivity : Activity
        {
            public override string GenerateExecMethodBody(WorkflowDefinition wd)
            {
                return @"
                    var result = new ActivityExecutionResult();
                    ;
                    var now = DateTime.Now;;
                    throw new Exception(""What the hell !!!! "" + now);";
            }
        }

    }
}