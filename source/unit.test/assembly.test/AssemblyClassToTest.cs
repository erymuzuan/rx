using System;
using System.Threading.Tasks;
using Bespoke.DevV1.Customers.Domain;
using Bespoke.Sph.Domain;

namespace assembly.test
{
    public class TestClassBase
    {
        public string BaseStringProperty { get; set; }
    }
    public class AssemblyClassToTest : TestClassBase
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
            return string.Format(greet + " " + customer.FirstName);
        }
        public async Task<object> GreetAsync(Customer customer, string greet)
        {
            await Task.Delay(500);
            return string.Format(greet + " " + customer.FirstName);
        }
        public async Task<object> GreetAsync(Customer customer, string greet, bool warning)
        {
            await Task.Delay(500);
            return string.Format(greet + " warning " + customer.FirstName);
        }
        public async Task<string> SayHelloAsync(Customer customer, string greet, bool warning)
        {
            await Task.Delay(500);
            return string.Format(greet + " warning " + customer.FirstName);
        }
        public async Task<DateTime?> GetNullableDateTime(DateTime date)
        {
            await Task.Delay(500);
            return date;
        }
        public async Task<object> GreetAsync(Customer customer, string greet, string warning)
        {
            await Task.Delay(500);
            return string.Format(greet + " warning " + customer.FirstName);
        }

        public int? NullableInt32Property { get; set; }
        public DateTime? NullableDateTimeProperty { get; set; }
        public DateTime DateTimeProperty { get; set; }
        public decimal DecimalProperty { get; set; }
        public decimal? NullalbeDecimalProperty { get; set; }
        public bool? NullalbeBooleanProperty { get; set; }
        public bool BooleanProperty { get; set; }
        public int Int32Property { get; set; }
        public string StringProperty { get; set; }
        public Child2 Child { get; set; }
        public Options Option { get; set; }
        public Options? Option2 { get; set; }
        public ObjectCollection<string> JustStringCollection { get; } = new ObjectCollection<string>();
        public ObjectCollection<Child> ChildCollection { get; } = new ObjectCollection<Child>();
        public ObjectCollection<Customer> Customers { get; } = new ObjectCollection<Customer>();
    }

    public enum Options
    {
        One,
        Two,
        Three
    }

    public class Child2 : DomainObject
    {
        public int? ChildNullableInt32Property { get; set; }
        public int ChildInt32Property { get; set; }
        public string ChildStringProperty { get; set; }
        public ObjectCollection<string> ChildJustStringCollection { get; } = new ObjectCollection<string>();
    }

    public class Child : DomainObject
    {
        public int? ChildNullableInt32Property { get; set; }
        public int ChildInt32Property { get; set; }
        public string ChildStringProperty { get; set; }
        public ObjectCollection<string> ChildJustStringCollection { get; } = new ObjectCollection<string>();
    }
}