using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace DevV1.Integrations.Transforms
{
    public class Sample3
    {
        class Input
        {
            public DevV1.Adapters.dbo.ima_his.Patient Patient { get; set; }
            public Bespoke.DevV1_state.Domain.State State { get; set; }
            public Bespoke.DevV1_district.Domain.District District { get; set; }

        }
        public Task<Bespoke.DevV1_customer.Domain.Customer> TransformAsync(DevV1.Adapters.dbo.ima_his.Patient patient, Bespoke.DevV1_state.Domain.State state, Bespoke.DevV1_district.Domain.District district)
        {
            var item = new Sample3.Input();
            item.Patient = patient;
            item.State = state;
            item.District = district;
            var dest = new Bespoke.DevV1_customer.Domain.Customer();


            dest.FullName = item.Patient.Name;
            dest.Address.State = item.State.Name;
            dest.Address.District = item.District.Name;
            dest.CreatedDate = item.Patient.Dob;
            ;


            return Task.FromResult(dest);
        }

        //TODO : return the list of destinations objects
    }
}
