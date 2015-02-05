using System.Threading.Tasks;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    public class WhileTestFixture : StatementTestFixture
    {
        [Test]
        public async Task Do()
        {
            await AssertAsync<string[]>(@"

            var names = ['Patient', 'Name', 'Age gt 25'];
            var length = 0;

            do {
                logger.info('Name : ' + names.length);
                length++;
             } while( length < names.length);

            return names;
            ",
                @"
            var names = new []{""Patient"", ""Name"", ""Age gt 25""};
            var length = 0;
            do
            {
                logger.Info(""Name : "" + names.Length);
                length++;   
            }while( length < names.Length);

            return names;
            ");
        }
        [Test]
        public async Task While()
        {
            await AssertAsync<string[]>(@"

            var names = ['Patient', 'Name', 'Age gt 25'];

            var length = 0;

            while( length < names.length){
                logger.info('Name : ' + names.length);
                length++;
             }
            return names;
            ",
                @"
            var names = new []{""Patient"", ""Name"", ""Age gt 25""};
            var length = 0;
            while(length < names.Length)
            {
                logger.Info(""Name : "" + names.Length);
                length++;   
            }
            return names;
            ");
        }
    }
}