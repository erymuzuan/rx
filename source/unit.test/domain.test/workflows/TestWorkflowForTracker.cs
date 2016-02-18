using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace domain.test.workflows
{
    public class TestWorkflowForTracker : Workflow
    {
        public override async Task<ActivityExecutionResult> ExecuteAsync(string activityId, string correlation = null)
        {
            var result = new ActivityExecutionResult
            {
                Status = ActivityExecutionStatus.Success,
                Correlation = correlation
            };
            switch (activityId)
            {
                case "A":
                    result.NextActivities = new[] { "B" };
                    break;
                case "B":
                    result.NextActivities = new[] { "C" };
                    break;
                default: throw new Exception("Whoaaaa");
            }
            await this.SaveAsync(activityId, result);
            return result;
        }
    }
}