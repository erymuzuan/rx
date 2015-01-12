using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class TryScope : Scope
    {
        public string GenerateCode(WorkflowDefinition wd, Activity activity)
        {
            var execute = new StringBuilder();
            execute.AppendLine("try ");
            execute.AppendLine("{ ");
            execute.AppendLinf("    result = await this.{0}().ConfigureAwait(false);", activity.MethodName);
            execute.AppendLine("}");


            foreach (var xy in this.CatchScopeCollection)
            {
                execute.AppendLinf("catch ({0})", xy.ExceptionType);
                execute.AppendLine("{");

                var xy1 = xy;
                var catchActivities = wd.ActivityCollection.FindAll(a => a.CatchScope == xy1.Id)
                    .ToList();

                var firstCatchActivity = catchActivities.First();
                foreach (var act in catchActivities)
                {
                    firstCatchActivity = act;
                    var isThereAnyPreviousActivitiesPointToMe = catchActivities.Any(c => c.NextActivityWebId == act.WebId);
                    if (!isThereAnyPreviousActivitiesPointToMe)
                        break;
                }

                execute.AppendLinf("    return new ActivityExecutionResult {{ NextActivities = new []{{\"{0}\"}} }};", firstCatchActivity.WebId);
                execute.AppendLine("}");
            }

            return execute.ToString();
        }
    }
}