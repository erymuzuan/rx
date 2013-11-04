using System.Text;
using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace web.test
{

   
    public class GenerateCodeTest
    {
        [Test]
        public void GenerateCode()
        {
            var wd = new WorkflowDefinition {WorkflowDefinitionId = 1};
            

            var code = new StringBuilder();
            Assert.AreEqual(wd,code);
        }
    }
}
