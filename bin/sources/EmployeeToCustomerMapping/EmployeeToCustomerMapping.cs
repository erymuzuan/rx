using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Dev.Integrations.Transforms
{
   public class EmployeeToCustomerMapping
   {
           public Task<Bespoke.Dev_1.Domain.Customer> TransformAsync(Dev.Adapters.HR.EMPLOYEES item)
           {
               var dest =  new Bespoke.Dev_1.Domain.Customer();
               dest.CustomerId = item.EMPLOYEE_ID;
               dest.Contact.Email = item.EMAIL;
               
               var date1 = item.HIRE_DATE;
               var value1 = 15;
               dest.RegisteredDate = date1.AddDays(value1);
               
               dest.FullName = item.FIRST_NAME + " " + item.LAST_NAME;


               return Task.FromResult(dest);
           }

//TODO : return the list of destinations objects
   }
}
