using System.Threading.Tasks;

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
}