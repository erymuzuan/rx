using System;
using System.Threading.Tasks;
using RxTech.DevV1.Customers.Domain;

namespace assembly.test
{
    public class AssemblyClassToTest
    {

        public static void VoidStaticMethod()
        {
            Console.WriteLine("Void static method".ToLower());
        }

        public static Task AsyncTaskStaticMethod()
        {
            Console.WriteLine("async static method".ToLower());
            return Task.FromResult(0);
        }

        public static Task<bool> AsyncTaskSomethingStaticMethod()
        {
            Console.WriteLine("async Task<bool> static method".ToLower());
            return Task.FromResult(true);
        }

        public void VoidInstanceMethod()
        {
            Console.WriteLine("Void static method".ToLower());
        }

        public Task AsyncTaskInstanceMethod()
        {
            Console.WriteLine("async static method".ToLower());
            return Task.FromResult(0);
        }

        public Task<bool> AsyncTaskSomethingInstanceMethod()
        {
            Console.WriteLine("async Task<bool> static method".ToLower());
            return Task.FromResult(true);
        }

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
            return string.Format(greet + " " + customer.FullName);
        }
        public async Task<object> GreetAsync(Customer customer, string greet)
        {
            await Task.Delay(500);
            return string.Format(greet + " " + customer.FullName);
        }
        public async Task<object> GreetAsync(Customer customer, string greet, bool warning)
        {
            await Task.Delay(500);
            return string.Format(greet + " warning " + customer.FullName);
        }
        public async Task<string> SayHelloAsync(Customer customer, string greet, bool warning)
        {
            await Task.Delay(500);
            return string.Format(greet + " warning " + customer.FullName);
        }
        public async Task<DateTime?> GetNullableDateTime(DateTime date)
        {
            await Task.Delay(500);
            return date;
        }
        public async Task<object> GreetAsync(Customer customer, string greet, string warning)
        {
            await Task.Delay(500);
            return string.Format(greet + " warning " + customer.FullName);
        }
    }
}