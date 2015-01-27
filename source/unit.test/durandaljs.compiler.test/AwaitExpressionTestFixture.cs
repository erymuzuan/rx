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
var result2 = await app.ShowMessageAsync(""Async2 "" +  result, new []{""Yes"", ""No""}  );
var result3 = await app.ShowMessageAsync(""Async3 "" +  result2, new []{""Yes"", ""No""}  );
// if(result3*** == ""No"") return;
await app.ShowMessageAsync(""Thank you "" +  result2, new []{""OK""}  );

logger.Info(""result : "" + result);
logger.Info(""result2 : "" + result2);
logger.Info(""result3 : "" + result3);
";
            var cr = await compiler.CompileAsync<Task>(CODE, patient);

            Assert.IsTrue(cr.Success);
            const string EXPECTED = @"
                var name = $data.Name();       
                var __tcs = app.showMessage('Async1', ['Yes','No']);            
                promise1.then(function(result){
                    return app.showMessage('Async2' + result, ['Yes','No']);
                }).then(function(result2){                    
                    return app.showMessage('Async3' + result2, ['Yes','No']);
                }).then(function(result3){
                    logger.info('result :' + result);
                    logger.info('result2 :' + result2);
                    logger.info('result3 :' + result3);
                });          

                return __tcs.promise();
            
";
            EXPECTED.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToList()
                .ForEach(x => StringAssert.Contains(x, cr.Code));

        }

        [Test]
        [Trace(Verbose = true)]
        public async Task AwaitWithLocalVariableDeclaration()
        {
            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new StatementCompiler();
            const string CODE = @"
var name = item.Name;
var result = await app.ShowMessageAsync(""Are you sure you want to delete"" + name, new []{""Yes"", ""No""} );
if(result == ""Yes"")
{
    logger.Info(""User say Yes"");
}

return null;";
            var cr = await compiler.CompileAsync<Task<string>>(CODE, patient);

            Assert.IsTrue(cr.Success);
            Console.WriteLine(cr.Code);
            StringAssert.Contains("return __tcs1.promise()", cr.Code);
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
            var cr = await compiler.CompileAsync<Task>(CODE, patient);

            Assert.IsTrue(cr.Success);
            Console.WriteLine(cr.Code);

            StringAssert.Contains("logger.info", cr.Code);
        }
    }
}