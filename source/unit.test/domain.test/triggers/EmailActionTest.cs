using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace domain.test.triggers
{
    [TestFixture]
    class EmailActionTest
    {
        [SetUp]
        public void Init()
        {
           ObjectBuilder.AddCacheList<ILogger>(new Logger()); 
        }
        [Test]
        public async Task Email()
        {
            var customer = this.GetCustomerInstance();
            customer.FullName = "Mercedes";
            ObjectBuilder.AddCacheList<ITemplateEngine>(new MockTemplateEngine());
            CustomAction email = new EmailAction
            {
                To = "ruzzaima@@bespoke.com.my",
                SubjectTemplate = "test @Model.FullName",
                From = "admin@@bespoke.com.my",
                BodyTemplate = "What ever"
            };
            if (email.UseAsync)
              await  email.ExecuteAsync(new RuleContext(customer));
            else
                email.Execute(new RuleContext(customer));


        }
        [Test]
        public async Task EmailWithModel()
        {
            var customer = this.GetCustomerInstance();
            customer.FullName = "Ferarri";
            customer.Contact.Email = "ruzzaima@bespoke.com.my";
            ObjectBuilder.AddCacheList<ITemplateEngine>(new MockTemplateEngine());
            CustomAction email = new EmailAction
            {
                To ="@Model.Email" ,
                SubjectTemplate = "test @Model.FullName",
                From = "admin@bespoke.com.my",
                BodyTemplate = "What ever"
            };
            if (email.UseAsync)
                await email.ExecuteAsync(new RuleContext(customer));
            else
                email.Execute(new RuleContext(customer));


        }
        /**/
    }
}
