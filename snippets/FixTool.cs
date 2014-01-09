using System;

namespace ToolsHelper
{
    public abstract class FixTool
    {
        public string ConnectionString { get; set; }
        public int Number { get; set; }
        protected abstract void RunFix();
        public string PatchNumber
        {
            get
            {
                return "Patch" + this.Number;
            }
        }
        public void Run()
        {
            this.ConnectionString = PatchHelper.GetConnectionString();
            // check patch
            if (!PatchHelper.CanApplyPatch(this.PatchNumber))
                return;

            this.RunFix();

            PatchHelper.ApplyPatch(PatchNumber);
            Console.WriteLine("Press [Enter] to exit");
            Console.ReadLine();
        }

    }
}