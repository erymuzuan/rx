using System;
using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class EmailAction : CustomAction
    {
       public override void Execute(Entity item)
        {
            Console.WriteLine("Email...");
        }

        public override Task ExecuteAsync(Entity item)
        {
            throw new NotImplementedException();
        }

        public override bool UseAsync
        {
            get { return false; }
        }
    }
}