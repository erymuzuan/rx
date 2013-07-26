using System;
using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class EmailAction : CustomAction
    {
       public override void Execute(Entity item)
        {
            Console.WriteLine("Email...{0} : Subject : {1}", this.To, this.SubjectTemplate);
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