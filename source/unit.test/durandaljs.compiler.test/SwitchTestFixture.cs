using System.Threading.Tasks;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    public class SwitchTestFixture : StatementTestFixture
    {
        [Test]
        public async Task Switch()
        {
            await AssertAsync<string>(@"
            var message = '';
            switch($data.Name())
            {
                case 'Ali' : return 'Ali is a muslim';
                case 'Michael':
                case 'John':
                    return item.Name + ' could be a christian';
                case '':
                    throw 'Name is empty';
                case 'unknow':
                    logger.info('unknow');
                    message = 'Too bad';
                    break;
                default:
                    message = $data.Name() + ' is not in the list';
                    break;              
            }
            return message;
            ",

                @"
            var message = string.Empty;
            switch(item.Name)
            {
                case ""Ali"" : return ""Ali is a muslim"";
                case ""Michael"":
                case ""John"":
                    return item.Name + "" could be a christian"";
                case """":
                    throw new Exception(""Name is empty"");
                case ""unknow"":
                    logger.Info(""unknow"");
                    message = ""Too bad"";
                    break;
                default:
                    message = item.Name + "" is not in the list"";
                    break;               

            }
            return message;
            ");
        }
    }
}