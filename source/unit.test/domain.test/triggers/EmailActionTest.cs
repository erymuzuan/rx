using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Xunit;
using Xunit.Abstractions;

namespace domain.test.triggers
{

    public class EmailActionTest
    {
        public ITestOutputHelper Console { get; }

        public EmailActionTest(ITestOutputHelper console)
        {
            Console = console;
            ObjectBuilder.AddCacheList<ILogger>(new Logger());
        }
        [Fact]
        public async Task Email()
        {
            var customer = this.GetCustomerInstance();
            customer.FirstName = "Mercedes";
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
            customer.FirstName = "Ferarri";
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

            Console.WriteLine("email sent, check your mail folder");
        }
        /**/
    }
}
