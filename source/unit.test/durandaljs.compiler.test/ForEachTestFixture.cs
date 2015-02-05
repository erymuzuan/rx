using System.Threading.Tasks;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    public class ForEachTestFixture : StatementTestFixture
    {

        

        [Test]
        public async Task ForEach()
        {
            await AssertAsync<string[]>(@"
            var names = ['Patient', 'Name', 'Age gt 25'];
            names.each(function(n) {
                logger.info('Name : ' + n);
            });
            return names;
            ",
                @"
            var names = new []{""Patient"", ""Name"", ""Age gt 25""};
            foreach(var n in names)
            {
                logger.Info(""Name : "" + n);
            }
            return names;
            ");
        }

        [Test]
        public async Task ForEachWithBreak()
        {
            await AssertAsync<string[]>(@"
            var names = ['Patient', 'Name', 'Age gt 25'];
            names.each(function(n) {
                logger.info('Name : ' + n);
                if( n === 'Patient'){
                    break;
                }
            });
            return names;
            ",
                @"
            var names = new []{""Patient"", ""Name"", ""Age gt 25""};
            foreach(var n in names)
            {
                logger.Info(""Name : "" + n);
                if(n == ""Patient"")break;
            }
            return names;
            ");
        }

        [Test]
        public async Task ForEachWithContinue()
        {
            await AssertAsync<string[]>(@"
            var names = ['Patient', 'Name', 'Age gt 25'];
            names.each(function(n) {
                logger.info('Name : ' + n);
                if( n === 'Patient'){
                    continue;
                }
                logger.info('Name : ' + n);
            });
            return names;
            ",
                @"
            var names = new []{""Patient"", ""Name"", ""Age gt 25""};
            foreach(var n in names)
            {
                logger.Info(""Name : "" + n);
                if(n == ""Patient"")continue;
                logger.Info(""Name : "" + n);
            }
            return names;
            ");
        }
    }
}