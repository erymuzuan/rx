using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Xunit;
namespace domain.test.workflows
{
    public class WorkflowCompilerTest : WorkflowTestBase
    {

        [Fact]
        public async Task CompileError()
        {
            var wd = new WorkflowDefinition { Name = "Test Workflow", SchemaStoreId = "schema-storeid" , Id = "test-workflow"};
            var screen = new ScreenActivity { Name = "Pohon", IsInitiator = true, WebId = "A", NextActivityWebId = "B" };
            screen.FormDesign.FormElementCollection.Add(new TextBox { Label = "Nama", Path = "Nama" });
            wd.ActivityCollection.Add(screen);

            var exp = new ExpressionActivity { WebId = "B", Name = "Expression B", Expression = "tet test-----", NextActivityWebId = "C" };
            wd.ActivityCollection.Add(exp);
            wd.ActivityCollection.Add(new EndActivity { Name = "C", WebId = "C" });

            var result =await this.CompileAsync(wd, true, assertError: false);

            Assert.False(result.Result);
            Assert.Equal(1, result.Errors.Count);
            Assert.Contains("; expected", result.Errors[0].Message);
            Assert.Contains(exp.Expression, result.Errors[0].Code);
            Assert.Equal(exp.WebId, result.Errors[0].ItemWebId);

        }
    }
}