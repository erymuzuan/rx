using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Delay", TypeName = "Delay", Description = "Wait for a certain time")]
    public partial class DelayActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            if(string.IsNullOrWhiteSpace(this.NextActivityWebId))
                result.Errors.Add(new BuildError(this.WebId, string.Format("[DelayActivity] ->{0} is missing next activity", this.Name)));

            if (this.Miliseconds + this.Seconds + this.Hour + this.Days == 0 && string.IsNullOrWhiteSpace(this.Expression))
            {
                result.Errors.Add(new BuildError(this.WebId,
                    string.Format("[DelayActivity] -\"{0}\" Set the wait time or expression", this.Name)));
            }
            if (this.Miliseconds + this.Seconds + this.Hour + this.Days < 0)
            {
                result.Errors.Add(new BuildError(this.WebId,
                    string.Format("[DelayActivity] -\"{0}\" Set the wait time span cannot be back dated", this.Name)));
            }

            if (this.Miliseconds + this.Seconds + this.Hour + this.Days > 0 && !string.IsNullOrWhiteSpace(this.Expression))
            {
                result.Errors.Add(new BuildError(this.WebId, string.Format("[DelayActivity] -\"{0}\" Set the wait time OR expression ONLY not both", this.Name)));
            }
            // TODO : validate it's a valid C# expression
            return result;
        }

        public override string GenerateInitAsyncMethod(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            code.AppendLinf("   public async Task<InitiateActivityResult> InitiateAsync{0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLinf("       var self = this.GetActivity<DelayActivity>(\"{0}\");", this.WebId);
            code.AppendLine("       await self.CreateTaskSchedulerAsync(this);");
            code.AppendLine("       var result = new InitiateActivityResult{ Correlation = Guid.NewGuid().ToString() };");
            code.AppendLine("       return result;");
            code.AppendLine("   }");

            return code.ToString();
        }

        public async Task CreateTaskSchedulerAsync(Workflow wf)
        {
            var ts = ObjectBuilder.GetObject<ITaskScheduler>();
            var dateTime = DateTime.Now.AddDays(this.Days)
                .AddHours(this.Hour)
                .AddSeconds(this.Seconds)
                .AddMilliseconds(this.Miliseconds);
            if (!string.IsNullOrWhiteSpace(this.Expression))
            {
                var name = "EvaluateExpression" + this.MethodName;
                var method = wf.GetType().GetMethod(name);
                dateTime = (DateTime)method.Invoke(wf, null);
            }


            var task = new ScheduledActivityExecution
            {
                InstanceId = wf.Id,
                ActivityId = this.WebId,
                Name = this.Name
            };
            await ts.AddTaskAsync(dateTime, task);
        }

        public override async Task CancelAsync(Workflow wf)
        {
            var ts = ObjectBuilder.GetObject<ITaskScheduler>();
            var task = new ScheduledActivityExecution
            {
                InstanceId = wf.Id,
                ActivityId = this.WebId,
                Name = this.Name
            };
            await ts.DeleteAsync(task);

            var tracker = await wf.GetTrackerAsync();
            tracker.CancelAsyncList(this.WebId);
            await tracker.SaveAsync();
        }

        public override bool IsAsync
        {
            get { return true; }
        }

        public override string GenerateExecMethodBody(WorkflowDefinition wd)
        {
            if (string.IsNullOrWhiteSpace(this.NextActivityWebId))
                throw new InvalidOperationException("NextActivityWebId is null or empty for " + this.Name);

            var code = new StringBuilder();
            
            code.AppendLine(this.ExecutingCode);
            code.AppendLinf("       this.State = \"Ready\";");
            code.AppendLine("       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            code.AppendLinf("       result.NextActivities = new[]{{\"{0}\"}};", this.NextActivityWebId);
            code.AppendLine(this.ExecutedCode);
            
            if (!string.IsNullOrWhiteSpace(this.Expression))
            {
                var evaluateBody = new StringBuilder();
                evaluateBody.AppendLinf("   public System.DateTime EvaluateExpression{0}()", this.MethodName);
                evaluateBody.AppendLine("   {");
                evaluateBody.AppendLine("       var item = this;");
                evaluateBody.AppendLinf(this.Expression.Trim().EndsWith(";") ? "       {0}" : "       return {0};", this.Expression);
                evaluateBody.AppendLine("   }");


                this.OtherMethodCollection.Clear();
                this.OtherMethodCollection.Add(new Method
                {
                    Code = evaluateBody.ToString()
                });

            }

            return code.ToString();
        }


        public override async Task TerminateAsync(Workflow wf)
        {
            var ts = ObjectBuilder.GetObject<ITaskScheduler>();
            var task = new ScheduledActivityExecution
            {
                InstanceId = wf.Id,
                ActivityId = this.WebId,
                Name = this.Name
            };
            await ts.DeleteAsync(task);
        }
    }
}