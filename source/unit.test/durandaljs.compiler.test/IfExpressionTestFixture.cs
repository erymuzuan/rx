using System.Threading.Tasks;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    public class IfExpressionTestFixture : StatementTestFixture
    {
        [Test]
        public async Task If()
        {
            await AssertAsync<object>(@"
if( 'Test' === 'test'){

}", @"
if(""Test"" == ""test"")
{
    
}

return null;

");
        }
        [Test]
        public async Task IfWithAsync()
        {
            await AssertAsync<Task<object>>(@"
if( 'Test' === 'test'){

}", @"
if(item.Name  == ""test"")
{
    await app.ShowMessageAsync(""test"", new [] {""test""});
}

return null;

");
        }
        [Test]
        public async Task Block()
        {
            await AssertAsync<object>(@"
if( $data.Name() === 'test'){
    logger.warning('Cannot be here');
}", @"
if(item.Name == ""test"")
{
    logger.Warning(""Cannot be here"");
}

return null;

");
        }
        [Test]
        public async Task Else()
        {
            await AssertAsync<object>(@"
if( $data.Name() === 'test'){
    logger.warning('Cannot be here');
}
else {
    logger.info('What ever');
}",
  
@"
if(item.Name == ""test"")
    logger.Warning(""Cannot be here"");
else
    logger.Info(""What ever"");

return null;

");
        }
        [Test]
        public async Task ElseBlock()
        {
            await AssertAsync<object>(@"
if( $data.Name() === 'test'){
    logger.warning('Cannot be here');
    logger.warning('Cannot be here 2');
}
else {
    logger.info('What ever');
    logger.info('What ever 3');
}",
  
@"
if(item.Name == ""test"")
{
    logger.Warning(""Cannot be here"");
    logger.Warning(""Cannot be here 2"");
}
else
{
    logger.Info(""What ever"");
    logger.Info(""What ever 3"");
}

return null;

");
        }
    }
}