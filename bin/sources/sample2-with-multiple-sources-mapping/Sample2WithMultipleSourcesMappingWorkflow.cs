using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_Sample2WithMultipleSourcesMapping_0
{
    [EntityType(typeof(Workflow))]
    public partial class Sample2WithMultipleSourcesMappingWorkflow : Bespoke.Sph.Domain.Workflow
    {
        public Sample2WithMultipleSourcesMappingWorkflow()
        {
            this.Name = "Sample2 With multiple sources mapping";
            this.Version = 0;
            this.WorkflowDefinitionId = "sample2-with-multiple-sources-mapping";
        }

        private Bespoke.DevV1_state.Domain.State m_AddressState = new Bespoke.DevV1_state.Domain.State();
        public Bespoke.DevV1_state.Domain.State AddressState
        {
            get { return m_AddressState; }
            set { m_AddressState = value; }
        }

        private Bespoke.DevV1_district.Domain.District m_District = new Bespoke.DevV1_district.Domain.District();
        public Bespoke.DevV1_district.Domain.District District
        {
            get { return m_District; }
            set { m_District = value; }
        }

        private Bespoke.DevV1_customer.Domain.Customer m_Customer = new Bespoke.DevV1_customer.Domain.Customer();
        public Bespoke.DevV1_customer.Domain.Customer Customer
        {
            get { return m_Customer; }
            set { m_Customer = value; }
        }

        private DevV1.Adapters.dbo.ima_his.Patient m_Patient = new DevV1.Adapters.dbo.ima_his.Patient();
        public DevV1.Adapters.dbo.ima_his.Patient Patient
        {
            get { return m_Patient; }
            set { m_Patient = value; }
        }




        public override async Task<ActivityExecutionResult> StartAsync()
        {
            this.SerializedDefinitionStoreId = "wd.sample2-with-multiple-sources-mapping.0";
            var result = await this.Expression0Async().ConfigureAwait(false);
            return result;
        }


        public override async Task<ActivityExecutionResult> ExecuteAsync(string activityId, string correlation = null)
        {
            this.SerializedDefinitionStoreId = "wd.sample2-with-multiple-sources-mapping.0";
            ActivityExecutionResult result = null;
            switch (activityId)
            {
                case "7113f92d-efe8-4233-a7ed-91943aba4d71":
                    result = await this.Expression0Async().ConfigureAwait(false);
                    break;
                case "af972647-c910-4d64-c29d-016574822847":
                    result = await this.End2Async().ConfigureAwait(false);
                    break;
                case "2d0f7564-18ce-455a-9a57-4bb32c576317":
                    result = await this.Mapping3Async().ConfigureAwait(false);
                    break;
            }
            result.Correlation = correlation;
            await this.SaveAsync(activityId, result).ConfigureAwait(false);
            return result;
        }

        //exec:7113f92d-efe8-4233-a7ed-91943aba4d71
        public Task<ActivityExecutionResult> Expression0Async()
        {

            var result = new ActivityExecutionResult { Status = ActivityExecutionStatus.Success };
            var item = this;
            Console.WriteLine("test");
            result.NextActivities = new[] { "2d0f7564-18ce-455a-9a57-4bb32c576317" };


            return Task.FromResult(result);
        }

        //exec:af972647-c910-4d64-c29d-016574822847
        public Task<ActivityExecutionResult> End2Async()
        {
            var result = new ActivityExecutionResult { Status = ActivityExecutionStatus.Success };
            result.NextActivities = new string[] { };
            this.State = "Completed";

            return Task.FromResult(result);
        }

        //exec:2d0f7564-18ce-455a-9a57-4bb32c576317
        public async Task<ActivityExecutionResult> Mapping3Async()
        {
            var map = new DevV1.Integrations.Transforms.Sample3();
            this.Customer = await map.TransformAsync(Patient, AddressState, District);

            var result = new ActivityExecutionResult { Status = ActivityExecutionStatus.Success };
            result.NextActivities = new[] { "af972647-c910-4d64-c29d-016574822847" };

            return result;
        }

    }
}
