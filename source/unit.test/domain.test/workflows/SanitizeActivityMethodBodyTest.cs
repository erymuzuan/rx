using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class SanitizeActivityMethodBodyTest
    {
        [Test]
        public void WithException()
        {
            var join = new ThrowExceptionActivity { Name = "TestAct", WebId = "TestAct" };
            var wd = new WorkflowDefinition { Name = "TestActThrow", Version = 0 };
            var code = wd.SanitizeMethodBody(join);
            Assert.IsFalse(string.IsNullOrWhiteSpace(code));
            StringAssert.DoesNotContain("async ", code);
            StringAssert.DoesNotContain("return ", code);
            StringAssert.DoesNotContain("result ", code);
            StringAssert.Contains("var now", code);
        }

        [Test]
        public void NormalResult()
        {
            var join = new NormalCodeActivity { Name = "TestAct", WebId = "TestAct" };
            var wd = new WorkflowDefinition { Name = "TestActThrow", Version = 0 };
            var code = wd.SanitizeMethodBody(join);
            Assert.IsFalse(string.IsNullOrWhiteSpace(code));
            StringAssert.DoesNotContain("async ", code);
            StringAssert.Contains("return Task.FromResult(result);", code);
        }
        [Test]
        public void NormalAsyncResult()
        {
            var join = new NormalCodeActivity { Name = "TestAct", WebId = "TestAct", IsAsyncMethod = true };
            var wd = new WorkflowDefinition { Name = "TestActThrow", Version = 0 };
            var code = wd.SanitizeMethodBody(join);
            Assert.IsFalse(string.IsNullOrWhiteSpace(code));
            StringAssert.Contains("async ", code);
            StringAssert.Contains("return result;", code);
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