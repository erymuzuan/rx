using System;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace durandaljs.compiler.test
{
    [TestClass]
    public class ButtonCommandTestFixture
    {
        [TestMethod]
        public void CommandWithAsync()
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
";

            var button = new Button
            {
                IsAsynchronous = true,
                Command = COMMAND

            };

            var compiler = new ButtonCompiler();
            var code = compiler.Compile(button);
            Console.WriteLine(code);
        }
    }
}
