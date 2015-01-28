using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    public class LambdaExpressionTestFixture
    {
        [Test]
        [Trace(Verbose = true)]
        public async Task LambdaExpressionWhere()
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

        [Test]
        [Trace(Verbose = true)]
        public async Task ListForEach()
        {
            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new StatementCompiler();
            const string CODE = @"
            var numbers = new []{1,2,3,4,5};
            numbers.ToList().ForEach(x => logger.Info(""no : ""  + x));

            return numbers[0];

";
            var cr = await compiler.CompileAsync<int>(CODE, patient);

            Assert.IsTrue(cr.Success);
            Console.WriteLine(cr.Code);

            // TODO : write extension method called "filter" to use underscorejs or whatever int the array prototype
            StringAssert.Contains("numbers.filter( function(x)", cr.Code);
            StringAssert.Contains("x % 2 !== 0", cr.Code);
        }

        [Test]
        [Trace(Verbose = true)]
        public async Task LambdaExpressionCountAll()
        {
            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new StatementCompiler();
            const string CODE = @"
            var numbers = new []{1,2,3,4,5};
            return numbers.Count();

";
            var cr = await compiler.CompileAsync<int>(CODE, patient);

            Assert.IsTrue(cr.Success);
            Console.WriteLine(cr.Code);

            // TODO : write extension method called "filter" to use underscorejs or whatever int the array prototype
            StringAssert.Contains("numbers.length", cr.Code);
        }

        [Test]
        [Trace(Verbose = true)]
        public async Task LambdaExpressionCount()
        {
            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new StatementCompiler();
            const string CODE = @"
            var numbers = new []{1,2,3,4,5};
            var odd = numbers.Count(x => x % 2 != 0);

            return odd;

";
            var cr = await compiler.CompileAsync<int>(CODE, patient);

            Assert.IsTrue(cr.Success);
            Console.WriteLine(cr.Code);

            // TODO : write extension method called "filter" to use underscorejs or whatever int the array prototype
            StringAssert.Contains("numbers.count( function(x)", cr.Code);
            StringAssert.Contains("x % 2 !== 0", cr.Code);
        }

        [Test]
        [Trace(Verbose = false)]
        public async Task LambdaExpressionSelect()
        {
            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new StatementCompiler();
            const string CODE = @"
            var numbers = new []{1,2,3,4,5};
            var odd = numbers.Select(x => new { 
                        no = x, 
                        name = ""n_"" + x, 
                        display = string.Format(""{0}"", x),
                        age = x + x
                    });

            return odd;

";
            var cr = await compiler.CompileAsync<object>(CODE, patient);

            Assert.IsTrue(cr.Success);
            Console.WriteLine(cr.Code);

            StringAssert.Contains("numbers.map( function(x)", cr.Code);
            StringAssert.Contains("no : x", cr.Code);
            StringAssert.Contains("name : 'n_' + x", cr.Code);
        }
    }
}