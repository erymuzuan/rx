using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace assembly.test
{
    public class BudgetServiceAgent
    {
        public async Task<object> GetBudgetAsync(int id)
        {
            await Task.Delay(500);
            return 50000;
        }
         
    }

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
        public string SayBuildingName(Designation masjid, string greet)
        {
            return string.Format( greet + " " + masjid.Name);
        }
        public async Task<object> GreetAsync(Designation masjid, string greet)
        {
            await Task.Delay(500);
            return string.Format( greet + " " + masjid.Name);
        }
        public async Task<object> GreetAsync(Designation masjid, string greet, bool warning)
        {
            await Task.Delay(500);
            return string.Format( greet + " warning " + masjid.Name);
        }
        public async Task<object> GreetAsync(Designation masjid, string greet, string warning)
        {
            await Task.Delay(500);
            return string.Format( greet + " warning " + masjid.Name);
        }
    }
}