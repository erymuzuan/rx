using System;
using System.Collections.Generic;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class Space : SpatialEntity
    {
        
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", this.RegistrationNo, this.UnitNo,this.State);
        }

        public int[] ApplicationTemplateOptions { get; set; }
    }
   
}
