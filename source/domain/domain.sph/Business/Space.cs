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

        public object ValidateBusinessRule(IEnumerable<BusinessRule> businessRules)
        {
            var context = new RuleContext(this);
            var message = "";
            var messages = new List<string>();
            foreach (var br in businessRules)
            {
                
                foreach (var r in br.RuleCollection)
                {   
                    var result = r.Execute(context);
                    message = result ? "rule berjaya" : br.ErrorMessage;
                }
                messages.Add(message);
            }
            return string.Join(",",messages);
        }
    }
   
}
