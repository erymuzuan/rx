using System.Threading.Tasks;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    public class ForTestFixture : StatementTestFixture
    {

        [Test]
        public async Task For()
        {
            await AssertAsync<string[]>(@"
            var names = ['Patient', 'Name', 'Age gt 25'];

            for (i = 0; i < names.length; i++) { 
                logger.info('Name : ' + names[i]);
             }
            return names;
            ",
                @"
            var names = new []{""Patient"", ""Name"", ""Age gt 25""};
            for (var i = 0; i < names.Length; i++)
            {
                logger.Info(""Name : "" + names[i]);                
            }
            return names;
            ");
        }
    }
}