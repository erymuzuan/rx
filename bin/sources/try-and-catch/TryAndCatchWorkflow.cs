using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_TryAndCatch_0
{
    [EntityType(typeof(Workflow))]
    public partial class TryAndCatchWorkflow : Bespoke.Sph.Domain.Workflow
    {
        public TryAndCatchWorkflow()
        {
            this.Name = "Try and catch";
            this.Version = 0;
            this.WorkflowDefinitionId = "try-and-catch";
        }

        public System.String Status { get; set; }



        public override async Task<ActivityExecutionResult> StartAsync()
        {
            this.SerializedDefinitionStoreId = "wd.try-and-catch.0";
            var result = await this.AAsync().ConfigureAwait(false);
            return result;
        }


        public override async Task<ActivityExecutionResult> ExecuteAsync(string activityId, string correlation = null)
        {
            this.SerializedDefinitionStoreId = "wd.try-and-catch.0";
            ActivityExecutionResult result = null;
            switch (activityId)
            {
                case "A":
                    try
                    {
                        result = await this.AAsync().ConfigureAwait(false);
                    }
                    catch (InvalidOperationException)
                    {
                        return new ActivityExecutionResult { NextActivities = new[] { "F" } };
                    }

                    break;
                case "B":
                    try
                    {
                        result = await this.BAsync().ConfigureAwait(false);
                    }
                    catch (InvalidOperationException)
                    {
                        return new ActivityExecutionResult { NextActivities = new[] { "F" } };
                    }

                    break;
                case "C":
                    result = await this.CAsync().ConfigureAwait(false);
                    break;
                case "F":
                    result = await this.FAsync().ConfigureAwait(false);
                    break;
                case "D":
                    result = await this.DAsync().ConfigureAwait(false);
                    break;
                case "E":
                    result = await this.EAsync().ConfigureAwait(false);
                    break;
                case "Z":
                    result = await this.ZAsync().ConfigureAwait(false);
                    break;
            }
            result.Correlation = correlation;
            await this.SaveAsync(activityId, result).ConfigureAwait(false);
            return result;
        }

        //exec:A
        public Task<ActivityExecutionResult> AAsync()
        {

            var result = new ActivityExecutionResult { Status = ActivityExecutionStatus.Success };
            var item = this;

            result.NextActivities = new[] { "B" };


            return Task.FromResult(result);
        }

        //exec:B
        public Task<ActivityExecutionResult> BAsync()
        {

            var result = new ActivityExecutionResult { Status = ActivityExecutionStatus.Success };
            var item = this;

            if (this.Status == "A")
            {
                throw new InvalidOperationException("Test message");
            }
            result.NextActivities = new[] { "C" };


            return Task.FromResult(result);
        }

        //exec:C
        public Task<ActivityExecutionResult> CAsync()
        {

            var result = new ActivityExecutionResult { Status = ActivityExecutionStatus.Success };
            var item = this;

            result.NextActivities = new[] { "" };


            return Task.FromResult(result);
        }

        //exec:F
        public Task<ActivityExecutionResult> FAsync()
        {

            var result = new ActivityExecutionResult { Status = ActivityExecutionStatus.Success };
            var item = this;

            result.NextActivities = new[] { "D" };


            return Task.FromResult(result);
        }

        //exec:D
        public Task<ActivityExecutionResult> DAsync()
        {

            var result = new ActivityExecutionResult { Status = ActivityExecutionStatus.Success };
            var item = this;

            result.NextActivities = new[] { "E" };


            return Task.FromResult(result);
        }

        //exec:E
        public Task<ActivityExecutionResult> EAsync()
        {

            var result = new ActivityExecutionResult { Status = ActivityExecutionStatus.Success };
            var item = this;

            result.NextActivities = new[] { "" };


            return Task.FromResult(result);
        }

        //exec:Z
        public Task<ActivityExecutionResult> ZAsync()
        {

            var result = new ActivityExecutionResult { Status = ActivityExecutionStatus.Success };
            var item = this;

            result.NextActivities = new[] { "" };


            return Task.FromResult(result);
        }

    }
}
