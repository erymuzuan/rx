using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace domain.test.workflows
{
    public class WorkflowCompilerTest : WorkflowTestBase
    {

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
    }
}