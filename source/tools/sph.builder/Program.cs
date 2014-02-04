using Bespoke.Sph.Domain;

namespace sph.builder
{
    public class Program
    {

        static void Main()
        {
            var wdBuilder = new WorkflowDefinitionBuilder();
            wdBuilder.Restore().Wait();

            var triggerBuilder = new TriggerBuilder();
            triggerBuilder.Initialize();
            triggerBuilder.Restore().Wait();

            var edBuilder = new EntityDefinitionBuilder();
            edBuilder.Initialize();
            edBuilder.Restore().Wait();

            var formBuilder = new Builder<EntityForm>();
            formBuilder.Initialize();
            formBuilder.Restore().Wait();

            var viewBuilder = new Builder<EntityView>();
            viewBuilder.Initialize();
            viewBuilder.Restore().Wait();

            var orgBuilder = new Builder<Organization>();
            orgBuilder.Initialize();
            orgBuilder.Restore().Wait();

            var settingBuilder = new Builder<Setting>();
            settingBuilder.Initialize();
            settingBuilder.Restore().Wait();

            var pageBuilder = new Builder<Page>();
            pageBuilder.Initialize();
            pageBuilder.Restore().Wait();

            var designationBuilder = new Builder<Designation>();
            designationBuilder.Initialize();
            designationBuilder.Restore().Wait();


            var rdlBuilder = new Builder<ReportDefinition>();
            rdlBuilder.Initialize();
            rdlBuilder.Restore().Wait();

            var rsBuilder = new Builder<ReportDelivery>();
            rsBuilder.Initialize();
            rsBuilder.Restore().Wait();
        }



    }
}
