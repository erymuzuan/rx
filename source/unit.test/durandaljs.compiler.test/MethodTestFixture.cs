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
        public async Task LoggerInfoWithItem()
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
        public async Task AppShowMessage()
        {
            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new StatementCompiler();
            const string CODE = @"
var name = item.Name;
var result = await app.ShowMessageAsync(""Are you sure you want to delete"" + name, new []{""Yes"", ""No""} );
if(result == ""Yes"")
{
    return string.Empty;
}
if(result == ""No"")
    await app.ShowMessageAsync(item.Name,new []{""OK""});

return null;";
            var cr = await compiler.CompileAsync<Task<string>>(CODE, patient);

            Assert.IsTrue(cr.Success);
            Console.WriteLine(cr.Code);
            Assert.AreEqual(@"logger.info('The name is ' + $data.Name(),new []{""OK""});
return null;
", cr.Code);
        }
    }
}
