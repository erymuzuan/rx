using System;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class DelayActivity : Activity
    {

        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            if (this.Miliseconds + this.Seconds + this.Hour + this.Days == 0 && string.IsNullOrWhiteSpace(this.Expression))
            {
                result.Errors.Add(new BuildError(this.WebId, 
                    string.Format("[DelayActivity] -\"{0}\" Set the wait time or expression", this.Name) ));
            }

            if (this.Miliseconds + this.Seconds + this.Hour + this.Days > 0 && !string.IsNullOrWhiteSpace(this.Expression))
            {
                result.Errors.Add(new BuildError(this.WebId,string.Format("[DelayActivity] -\"{0}\" Set the wait time OR expression ONLY not both", this.Name) ));
            }
            // TODO : validate it's a valid C# expression
            return result;
        }

        public async override Task InitiateAsync(Workflow wf)
        {
            var ts = ObjectBuilder.GetObject<ITaskScheduler>();
            var script = ObjectBuilder.GetObject<IScriptEngine>();
            var dateTime = DateTime.Now.AddDays(this.Days)
                .AddHours(this.Hour)
                .AddSeconds(this.Seconds)
                .AddMilliseconds(this.Miliseconds);
            if (!string.IsNullOrWhiteSpace(this.Expression))
                dateTime = script.Evaluate<DateTime, Workflow>(this.Expression, wf);


            var task = new ScheduledActivityExecution
            {
                InstanceId = wf.WorkflowId,
                ActivityId = this.WebId
            };
            await ts.AddTaskAsync(dateTime, task);
        }

        public async override Task CancelAsync(Workflow wf)
        {
            var ts = ObjectBuilder.GetObject<ITaskScheduler>();
            var task = new ScheduledActivityExecution
            {
                InstanceId = wf.WorkflowId,
                ActivityId = this.WebId
            };
            await ts.DeleteAsync(task);
        }

        public override bool IsAsync
        {
            get { return true; }
        }

        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            if (string.IsNullOrWhiteSpace(this.NextActivityWebId))
                throw new InvalidOperationException("NextActivityWebId is null or empty for " + this.Name);

            var code = new StringBuilder();
            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine(this.BeforeExcuteCode);
            code.AppendLinf("       this.CurrentActivityWebId = \"{0}\";", this.NextActivityWebId);
            code.AppendLinf("       await this.SaveAsync(\"{0}\");", this.WebId);
            code.AppendLine("       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            code.AppendLine(this.AfterExcuteCode);
            code.AppendLine("       return result;");
            code.AppendLine("   }");

            return code.ToString();
        }
    }
}