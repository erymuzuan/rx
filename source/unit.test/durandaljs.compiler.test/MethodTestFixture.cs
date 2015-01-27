using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    public class MethodTestFixture
    {
        [Test]
        [Trace(Verbose = true)]
        public async Task LoggerInfoWithReturnNull()
        {
            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new StatementCompiler();
            const string CODE = @"logger.Info(""The name is \""Slash\"" "");
return null;";
            var cr = await compiler.CompileAsync<string>(CODE, patient);

            Assert.IsTrue(cr.Success);
            Assert.AreEqual(@"logger.info('The name is ""Slash"" ');
return null;
", cr.Code);
        }



        [Test]
        [Trace(Verbose = true)]
        public async Task LoggerWithItem()
        {
            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new StatementCompiler();
            const string CODE = @"logger.Info(""The name is "" + item.Name);
return null;";
            var cr = await compiler.CompileAsync<string>(CODE, patient);

            Assert.IsTrue(cr.Success);
            Assert.AreEqual(@"logger.info('The name is ' + $data.Name());
return null;
", cr.Code);
        }

        [Test]
        [Trace(Verbose = true)]
        public async Task NestedAwaitWithLocalVariableDeclaration()
        {
            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new StatementCompiler();
            const string CODE = @"
var name = item.Name;
var result = await app.ShowMessageAsync(""Are you sure you want to delete"" + name, new []{""Yes"", ""No""} );
var result1 = await app.ShowMessageAsync(""You say "" +  result, new []{""Yes"", ""No""}  );
if(result1 == ""Yes"")
{
    logger.Info(""User say"" + result1);
}";
            var cr = await compiler.CompileAsync<Task>(CODE, patient);

            Assert.IsTrue(cr.Success);
            Console.WriteLine(cr.Code);
            StringAssert.Contains("return __tcs1.promise()", cr.Code);
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

        [Test]
        [Trace(Verbose = true)]
        public async Task LambdaExpression()
        {
            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new StatementCompiler();
            const string CODE = @"
            var numbers = new []{1,2,3,4,5};
            var odd = numbers.Where(x => x % 2 != 0);

            return 0;

";
            var cr = await compiler.CompileAsync<object>(CODE, patient);

            Assert.IsTrue(cr.Success);
            Console.WriteLine(cr.Code);

            // TODO : write extension method called "filter" to use underscorejs or whatever int the array prototype
            StringAssert.Contains("numbers.filter( function(x)", cr.Code);
            StringAssert.Contains("x % 2 !== 0", cr.Code);
        }
    }
}
