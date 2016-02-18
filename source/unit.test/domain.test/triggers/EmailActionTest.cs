using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Xunit;

namespace domain.test.triggers
{

    public class EmailActionTest
    {
        public EmailActionTest()
        {
            ObjectBuilder.AddCacheList<ILogger>(new Logger());
        }
        [Fact]
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
                await email.ExecuteAsync(new RuleContext(customer));
            else
                email.Execute(new RuleContext(customer));


        }
        [Fact]
        public async Task EmailWithModel()
        {
            var customer = this.GetCustomerInstance();
            customer.FullName = "Ferarri";
            customer.Contact.Email = "ruzzaima@bespoke.com.my";
            ObjectBuilder.AddCacheList<ITemplateEngine>(new MockTemplateEngine());
            CustomAction email = new EmailAction
            {
                To = "@Model.Email",
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
