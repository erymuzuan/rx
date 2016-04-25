using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IWorkflowService
    {
        Task<T> GetInstanceAsync<T>(WorkflowDefinition wd, string correlationName, string correlationValue) where T : Workflow, new();
        Task SaveInstanceAsync(Correlation correlation);
        Task<LoadOperation<WorkflowPresentation>> GetPendingWorkflowsAsync<T>(string activityId,
            string[] fields,
            IEnumerable<Filter> predicates,
            int from = 0,
            int size = 20) where T : Workflow, new();
        Task<T> GetOneAsync<T>(string id) where T : Workflow, new();
        Task<IEnumerable<T>> SearchAsync<T>(IEnumerable<Filter> predicates) where T : Workflow, new();
    }
}