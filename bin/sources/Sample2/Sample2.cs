using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace DevV1.Integrations.Transforms
{
    public class Sample2
    {
        public async Task<Bespoke.DevV1_customer.Domain.Customer> TransformAsync(Bespoke.DevV1_patient.Domain.Patient item)
        {
            var dest = new Bespoke.DevV1_customer.Domain.Customer();

            //:ConfigurationSettingFunctoid:7b24dd30-bc21-468d-c928-bd3cbb5f950f
            var __config2 = ConfigurationManager.ConnectionStrings["His"].ConnectionString;
            //:SqlServerLookup:9bd5fa71-8821-452d-af97-51112264ff98
            object __result0 = null;
            var __connectionString0 = @__config2;
            const string __text0 = "SELECT [Gender] From [dbo].[GenderCode] WHERE [Code] = @value1";
            using (var __conn = new System.Data.SqlClient.SqlConnection(__connectionString0))
            using (var __cmd = new System.Data.SqlClient.SqlCommand(__text0, __conn))
            {
                __cmd.Parameters.AddWithValue("@value1", item.Gender);
                await __conn.OpenAsync();
                __result0 = await __cmd.ExecuteScalarAsync();
                if (__result0 == DBNull.Value || null == __result0) __result0 = "NA";
            }


            dest.Gender = (string)__result0;


            return dest;
        }

        //TODO : return the list of destinations objects
    }
}
