using System.Threading.Tasks;
using Bespoke.Dev_1.Domain;

namespace assembly.test
{
    public class AssemblyClassToTest
    {
        public string SayHello(string name)
        {
            return string.Format("Hello " + name);
        }
        public string Greet(string greet, string name)
        {
            return string.Format(greet + " " + name);
        }
        public string SayCustomerName(Customer customer, string greet)
        {
            return string.Format( greet + " " + customer.FullName);
        }
        public async Task<object> GreetAsync(Customer customer, string greet)
        {
            await Task.Delay(500);
            return string.Format( greet + " " + customer.FullName);
        }
        public async Task<object> GreetAsync(Customer customer, string greet, bool warning)
        {
            await Task.Delay(500);
            return string.Format( greet + " warning " + customer.FullName);
        }
        public async Task<object> GreetAsync(Customer customer, string greet, string warning)
        {
            await Task.Delay(500);
            return string.Format( greet + " warning " + customer.FullName);
        }
    }
}