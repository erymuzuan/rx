using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IWorkflowService
    {
        Task<T> GetInstanceAsync<T>(WorkflowDefinition wd, string correlationName, string correlationValue) where T : Workflow, new();
        Task SaveInstanceAsync(Correlation correlation);
        Task<string[]> GetPendingWorkflowsAsync<T>(string activityId, IDictionary<string, object> variables) where T : Workflow, new();
        Task<T> GetOneAsync<T>(string id) where T : Workflow, new();
        Task<IEnumerable<T>> SearchAsync<T>(string search) where T : Workflow, new();
    }
}