using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Spatial;
using System.Threading.Tasks;
using Bespoke.SphCommercialSpaces.Domain;
using System.Linq;

namespace Bespoke.Sph.SqlRepository
{
    public class SqlSpatial<T> : ISpatialService<T> where T : SpatialEntity
    {
        private readonly string m_connectionString;
        public const int SRID = 4326;

        public SqlSpatial()
        {
            m_connectionString = ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;
        }

        public SqlSpatial(string connectionString)
        {
            m_connectionString = connectionString;
        }
        public async Task UpdateAsync(T item)
        {
            var sql = string.Format("UPDATE [{0}].[{1}] SET" +
                                    " [Wkt] = @Wkt," +
                                    " [Path] = @Path," +
                                    " [EncodedWkt] = @EncodedWkt" +
                                    " WHERE [{1}Id] = @Id",
                                    "Sph",
                                    typeof(T).Name);
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql,conn))
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;


                var properties = typeof (T).GetProperties();

                var geog = SQLSpatialTools.Functions.MakeValidGeographyFromText(item.Wkt, SRID);

                cmd.Parameters.AddWithValue("@Wkt", item.Wkt);
                var path = cmd.Parameters.AddWithValue("@Path", geog);
                path.UdtTypeName = "GEOGRAPHY";

                cmd.Parameters.AddWithValue("@EncodedWkt", item.EncodedWkt);
                cmd.Parameters.AddWithValue("@Id", (int)properties.Single(p => p.Name == typeof(T).Name + "Id").GetValue(item));



                await conn.OpenAsync();
                var rows = await cmd.ExecuteNonQueryAsync();
                if(rows != 1)
                    throw new InvalidOperationException("Cannot find the row " + item);


            }

        }

        public Task<string> GetEncodedPathAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetWktAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Geography> GetGeographyAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ContainsAsync(Expression<Func<T, bool>> predicate, LatLng point)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetNeighboursAsync(Expression<Func<T, bool>> predicate, double distance)
        {
            throw new NotImplementedException();
        }
    }
}
