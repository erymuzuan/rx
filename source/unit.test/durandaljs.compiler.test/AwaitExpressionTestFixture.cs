using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    public class AwaitExpressionTestFixture
    {
        [Test]
        [Trace(Verbose = true)]
        public async Task NestedAwaitWithLocalVariableDeclaration()
        {
            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new StatementCompiler();
            const string CODE = @"
var name = item.Name;

var result = await app.ShowMessageAsync(""Async1"" + name, new []{""Yes"", ""No""} );
logger.Info(""result "" + result);

var result2 = await app.ShowMessageAsync(""Async2 "" +  result, new []{""Yes"", ""No""}  );
logger.Info(""result : "" + result);
logger.Info(""result2 : "" + result2);

var result3 = await app.ShowMessageAsync(""Async3 "" +  result2, new []{""Yes"", ""No""}  );
// if(result3*** == ""No"") return;
logger.Info(""result : "" + result);
logger.Info(""result2 : "" + result2);
logger.Info(""result3 : "" + result3);

await app.ShowMessageAsync(""Thank you "" +  result2, new []{""OK""}  );

logger.Info(""result : "" + result);
logger.Info(""result2 : "" + result2);
logger.Info(""result3 : "" + result3);
";

            const string EXPECTED = @"
                var name = $data.Name();       
                return app.showMessage('Async1' + name, ['Yes', 'No'])
                    .then(function(result){
                        return app.showMessage('Async2' + result, ['Yes', 'No']);
                    }).then(function(result2){                    
                        return app.showMessage('Async3' + result2, ['Yes', 'No']);
                    }).then(function(result3){
                        logger.info('result :' + result);
                        logger.info('result2 :' + result2);
                        logger.info('result3 :' + result3);
                    });
            
";
            var cr = await compiler.CompileAsync<Task>(CODE, patient);
            AssertCodes(EXPECTED, cr);

        }

        [Test]
        [Trace(Verbose = true)]
        public async Task AwaitWithLocalVariableDeclaration()
        {
            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new StatementCompiler();
            const string CODE = @"
var name = item.Name;
var dr = await app.ShowMessageAsync(""Name"" + name, new []{""Yes"", ""No""} );
logger.Info(""User say "" + dr);
";

            const string EXPECTED = @"
                var name = $data.Name();       
                return app.showMessage('Name' + name, ['Yes', 'No'])
                        .then(function(dr){
                            logger.info('User say ' + dr);
                        });"
                ;
            var cr = await compiler.CompileAsync<Task>(CODE, patient);
            AssertCodes(EXPECTED, cr);
        }

        private static void AssertCodes(string expected, SnippetCompilerResult cr)
        {
            Assert.IsTrue(cr.Success);
            expected.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToList()
                .ForEach(x => StringAssert.Contains(x, cr.Code));
        }

        [Test]
        [Trace(Verbose = true)]
        public async Task AwaitExpresion()
        {
            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new StatementCompiler();
            const string CODE = @"
            await app.ShowMessageAsync(""Are you sure you want to delete"" , new []{""OK""} );
            logger.Info(""The user click OK"");

";
            const string EXPECTED = @"return app.showMessage('Are you sure you want to delete', ['OK'])
.then(function(){
       logger.info('The user click OK');;
});";
            var cr = await compiler.CompileAsync<Task>(CODE, patient);
            AssertCodes(EXPECTED, cr);
        }
    }
}