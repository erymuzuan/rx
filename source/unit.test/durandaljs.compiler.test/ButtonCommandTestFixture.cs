using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;
using Bespoke.Sph.FormCompilers.DurandalJs.FormElements;
using NUnit.Framework;


namespace durandaljs.compiler.test
{
    [TestFixture]
    public class ButtonCommandTestFixture
    {
        [Test]
        [Ignore]
        public void CommandWithOneAsyncMethod()
        {
            const string COMMAND = @"

var g = item.Name;
var k = item.GetName();
var result = await context.LoadOneAsync<State>(x => x.Name == ""Kelantan"");

var m = result.M;
var n = result.GetN();
if(null != result)
    item.Name = result.Name;
else
    item.Name = ""Unknown"";
if (null != result)
        item.Name = result.Name;
    else
        item.Name = ""Unknown"";
     
     
        
    if ( ""Test"" == result.Name)
    	logger.Info(""The name is {0}"", result.Name);
";

            var button = new Button
            {
                IsAsynchronous = true,
                Command = COMMAND

            };

            var compiler = new ButtonCompiler();
            var code = compiler.Compile(button);
            StringAssert.Contains(code, ".loadOneAsync");
            StringAssert.Contains(code, " Name eq 'Kelantan'");
        }
    }
}
