using System;
using System.IO;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Moq;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class TryCatchTest
    {
        private readonly string m_schemaStoreId = Guid.NewGuid().ToString();
        [SetUp]
        public void Init()
        {
            var doc = new BinaryStore
            {
                Content = File.ReadAllBytes(@".\workflows\PemohonWakaf.xsd")
            };
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);
            store.Setup(x => x.GetContent(m_schemaStoreId))
                .Returns(doc);
            ObjectBuilder.AddCacheList(store.Object);



        }

        [Test]
        public async Task TryAndCatch()
        {
            var wd = new WorkflowDefinition { Name = "Try and catch", Id = "try-and-catch", SchemaStoreId = m_schemaStoreId };

            const string TRY_SCOPE = "tryscope1";  
            const string CATCH_SCOPE = "catchscope1";
            var eaA = new ExpressionActivity { Name = "Activity A", WebId = "ActA", TryScope = TRY_SCOPE,IsInitiator  = true};
            var eaB = new ExpressionActivity { Name = "Activity B", WebId = "ActB", TryScope = TRY_SCOPE, NextActivityWebId = "ActC", Expression = "throw new System.IO.FileNotFoundException(\"cannot find it\");" };
            var eaC = new ExpressionActivity { Name = "Activity C", WebId = "ActC" };
            var eaF = new ExpressionActivity { Name = "Activity F", WebId = "ActF", CatchScope = CATCH_SCOPE, NextActivityWebId = "ActD" };
            var eaD = new ExpressionActivity { Name = "Activity D", WebId = "ActD", CatchScope = CATCH_SCOPE, NextActivityWebId = "ActE" };
            var eaE = new ExpressionActivity { Name = "Activity E", WebId = "ActE", CatchScope = CATCH_SCOPE };
            var eaZ = new ExpressionActivity { Name = "Activity Z", WebId = "ActZ" };



            wd.ActivityCollection.Add(eaA);
            wd.ActivityCollection.Add(eaB);
            wd.ActivityCollection.Add(eaC);
            wd.ActivityCollection.Add(eaF);
            wd.ActivityCollection.Add(eaD);
            wd.ActivityCollection.Add(eaE);
            wd.ActivityCollection.Add(eaZ);

            var tryScope = new WorkflowDefinition.TryScope() 
            { 
                Id = TRY_SCOPE
            };

            var catchScope1 = new WorkflowDefinition.CatchScope()
            {
                Id = CATCH_SCOPE,
                ExceptionType = "InvalidCastException",
                ExceptionVar = "qwe"
            };

            tryScope.CatchScopeCollection.Add(catchScope1);
            wd.TryScopeCollection.Add(tryScope);
            

            var options = new CompilerOptions();
            options.AddReference(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll"));
            options.AddReference(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\core.sph.dll"));
            options.AddReference(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll"));
            options.AddReference(typeof(JsonMediaTypeFormatter));

            var cr = wd.Compile(options);
            cr.Errors.ForEach(Console.WriteLine);
            Assert.IsTrue(cr.Result);
        }
    }
}