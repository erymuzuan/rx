using System;
using Bespoke.Sph.Domain;

namespace sph.builder
{
    public class Program
    {

        static void Main()
        {
            Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-");
            Console.WriteLine("-*                                           *-");
            Console.WriteLine("-*                     SPH                   *-");
            Console.WriteLine("-*                                          -*-");
            Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Welcome to SPH platform command line build tools");
            Console.WriteLine("This tool will truncate all your system data(sph) and rebuild it from your source folder");
            Console.ResetColor();
            Console.WriteLine("Enter \"y\" to continue");

            var cont = Console.ReadLine();
            if (cont != "y")
            {
                Console.WriteLine("BYE.");
                return;
            }
            var edBuilder = new EntityDefinitionBuilder();
            edBuilder.Initialize();
            edBuilder.Restore().Wait();

            var wdBuilder = new WorkflowDefinitionBuilder();
            wdBuilder.Restore().Wait();


            var triggerBuilder = new TriggerBuilder();
            triggerBuilder.Initialize();
            triggerBuilder.Restore().Wait();


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
