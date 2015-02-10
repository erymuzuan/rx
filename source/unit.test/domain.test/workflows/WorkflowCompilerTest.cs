using Bespoke.Sph.Domain;
using NUnit.Framework;
using System.Threading.Tasks;


namespace domain.test.workflows
{
    public class WorkflowCompilerTest : WorkflowTestBase
    {

        [Test]
        public async Task CompileError()
        {
            var wd = new WorkflowDefinition { Name = "Test Workflow", SchemaStoreId = "schema-storeid" , Id = "test-workflow"};
            var screen = new ScreenActivity { Name = "Pohon", IsInitiator = true, WebId = "A", NextActivityWebId = "B" };
            wd.ActivityCollection.Add(screen);

            var exp = new ExpressionActivity { WebId = "B", Name = "Expression B", Expression = "tet test-----", NextActivityWebId = "C" };
            wd.ActivityCollection.Add(exp);
            wd.ActivityCollection.Add(new EndActivity { Name = "C", WebId = "C" });

            var result =await this.CompileAsync(wd, true, assertError: false).ConfigureAwait(false);

            Assert.IsFalse(result.Result);
            Assert.AreEqual(1, result.Errors.Count);
            StringAssert.Contains("; expected", result.Errors[0].Message);
            StringAssert.Contains(exp.Expression, result.Errors[0].Code);
            Assert.AreEqual(exp.WebId, result.Errors[0].ItemWebId);

        }
    }
}