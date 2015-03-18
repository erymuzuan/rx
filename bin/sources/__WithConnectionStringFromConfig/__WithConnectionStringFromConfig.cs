using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace DevV1.Integrations.Transforms
{
    public class __WithConnectionStringFromConfig
    {
        public async Task<Bespoke.DevV1_customer.Domain.Customer> TransformAsync(Bespoke.DevV1_patient.Domain.Patient item)
        {
            var dest = new Bespoke.DevV1_customer.Domain.Customer();

            //:ConfigurationSettingFunctoid:conn
            var __config2 = ConfigurationManager.ConnectionStrings["His"].ConnectionString;
            //lookup:SqlServerLookup:lookup
            object __result0 = null;
            var __connectionString0 = @__config2;
            const string __text0 = "SELECT [Income] FROM [dbo].[Patient] WHERE Mrn = @value1";
            using (var __conn = new System.Data.SqlClient.SqlConnection(__connectionString0))
            using (var __cmd = new System.Data.SqlClient.SqlCommand(__text0, __conn))
            {
                __cmd.Parameters.AddWithValue("@value1", item.Mrn);
                await __conn.OpenAsync();
                __result0 = await __cmd.ExecuteScalarAsync();
                if (__result0 == DBNull.Value || null == __result0) __result0 = decimal.Zero;
            }


            dest.Revenue = (decimal)__result0;


            return dest;
        }

        //TODO : return the list of destinations objects
    }
}
