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
        [Ignore("still working on await expression inside an if statement")]
        public async Task IfWithAsync()
        {
            await AssertAsync<Task<object>>(@"
                if( $data.Name() === 'test'){
                    var ok;
                    return app.showMessage('Are you sure?', ['Yes', 'No'])
                        .then(function(__temp0) {
                            ok = __temp0;
                            logger.info(ok);
                            
                            tcs.resolve(10);
                    });
                }

                logger.info('Finished');
                return 5;",
  

                @"

                if(item.Name  == ""test"")
                {
                    var ok = await app.ShowMessageAsync(""Are you sure?"", new [] {""Yes"",""No""});
                    logger.Info(ok);
                    return 10;
                }

                logger.Info(""Finished."");

                return 5;

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