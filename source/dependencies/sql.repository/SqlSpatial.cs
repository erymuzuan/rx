using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Spatial;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using System.Linq;
using Bespoke.Sph.Domain.QueryProviders;

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
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;


                var properties = typeof(T).GetProperties();

                var geog = SQLSpatialTools.Functions.MakeValidGeographyFromText(item.Wkt, SRID);

                cmd.Parameters.AddWithValue("@Wkt", item.Wkt);
                var path = cmd.Parameters.AddWithValue("@Path", geog);
                path.UdtTypeName = "GEOGRAPHY";

                cmd.Parameters.AddWithValue("@EncodedWkt", item.EncodedWkt);
                cmd.Parameters.AddWithValue("@Id", item.GetId());



                await conn.OpenAsync();
                var rows = await cmd.ExecuteNonQueryAsync();
                if (rows != 1)
                    throw new InvalidOperationException("Cannot find the row " + item);


            }

        }

        public async Task<string> GetEncodedPathAsync(Expression<Func<T, bool>> predicate)
        {
            var query = this.Translate(predicate);

            var sql = query.ToString().Replace("[Data]", "[EncodedWkt]");
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                await conn.OpenAsync();
                var path = await cmd.ExecuteScalarAsync();
                if (path == DBNull.Value) return null;

                return path as string;

            }
        }

        public Task<string> GetWktAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Geography> GetGeographyAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        private IQueryable<T> Translate(Expression<Func<T, bool>> predicate)
        {
            var provider = ObjectBuilder.GetObject<QueryProvider>();
            var query = new Query<T>(provider).Where(predicate);
            return query;
        }

        public async Task<LatLng> GetCenterAsync(Expression<Func<T, bool>> predicate)
        {

            var query = this.Translate(predicate);

            var sql = query.ToString().Replace("[Data]", "[Path].EnvelopeCenter().STAsText()");
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                await conn.OpenAsync();
                var center = await cmd.ExecuteScalarAsync();
                if (center == DBNull.Value) return null;
                var wkt = center as string;
                if (string.IsNullOrWhiteSpace(wkt)) return null;

                var point = new LatLng(wkt, true);
                return point;

            }
        }

        public async Task<IEnumerable<T>> ContainsAsync(Expression<Func<T, bool>> predicate, LatLng[] points)
        {
            var sql = new StringBuilder("SELECT ");
            sql.AppendFormat(" [{1}Id], [Data], [EncodedWkt] FROM [{0}].[{1}] ", "Sph", typeof(T).Name);
            sql.AppendFormat(" WHERE geography::STPolyFromText('POLYGON((");
            sql.Append(string.Join(",", points.ToArrayString()));
            sql.Append("))', 4326)");
            sql.Append(".STIntersects([Path]) = 1");



            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql.ToString(), conn) { CommandType = CommandType.Text })
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var list = new ObjectCollection<T>();
                    while (reader.Read())
                    {
                        var item = XmlSerializerService.DeserializeFromXml<T>(reader.GetString(1));
                        item.EncodedWkt = reader.GetString(2);
                        item.SetId(reader.GetInt32(0));
                        // item.GeoLocationId = reader.GetInt32(0);
                        list.Add(item);
                    }


                    return list;
                }

            }
        }

        public Task<IEnumerable<T>> GetNeighboursAsync(Expression<Func<T, bool>> predicate, double distance)
        {
            throw new NotImplementedException();
        }
    }
}
