using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bespoke.Sph.SqlRepository
{
    public partial class SqlRepository<T> 
    {
        public async Task<int> GetCountAsync(IQueryable<T> query)
        {
            var sql = query.ToString().Replace("[Data]", "COUNT(*)");
            return await this.GetCountAsync(sql).ConfigureAwait(false);
        }

        private async Task<int> GetCountAsync(string sql)
        {
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                var count = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
                if (count != DBNull.Value)
                    return (int)count;
            }
            return 0;
        }


        public async Task<TResult> GetMaxAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            var column = GetMemberName(selector);
            if (string.IsNullOrWhiteSpace(column)) throw new ArgumentException("Cannot determine the aggregate column name");
            var sql = query.ToString().Replace("[Data]", string.Format("MAX([{0}])", column));
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                var max = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
                if (max != DBNull.Value)
                    return (TResult)max;
            }
            return default(TResult);

        }

        public async Task<TResult> GetMinAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            var column = GetMemberName(selector);
            if (string.IsNullOrWhiteSpace(column)) throw new ArgumentException("Cannot determine the aggregate column name");
            var sql = query.ToString().Replace("[Data]", string.Format("MIN([{0}])", column));
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                var min = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
                if (min != DBNull.Value)
                    return (TResult)min;
            }
            return default(TResult);
        }

        public async Task<TResult> GetSumAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult?>> selector)
            where TResult : struct
        {
            var column = GetMemberName(selector);
            if (string.IsNullOrWhiteSpace(column)) throw new ArgumentException("Cannot determine the aggregate column name");
            var sql = query.ToString().Replace("[Data]", string.Format("SUM([{0}])", column));
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                var sum = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
                if (sum != DBNull.Value)
                    return (TResult)sum;
            }
            return default(TResult);
        }

        public async Task<TResult> GetSumAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            var column = GetMemberName(selector);
            if (string.IsNullOrWhiteSpace(column)) throw new ArgumentException("Cannot determine the aggregate column name");
            var sql = query.ToString().Replace("[Data]", string.Format("SUM([{0}])", column));
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                var sum = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
                if (sum != DBNull.Value)
                    return (TResult)sum;
            }
            return default(TResult);
        }


        public async Task<TResult> GetAverageAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            var column = GetMemberName(selector);
            if (string.IsNullOrWhiteSpace(column)) throw new ArgumentException("Cannot determine the aggregate column name");
            var sql = query.ToString().Replace("[Data]", string.Format("AVG([{0}])", column));
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                var avg = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
                if (avg != DBNull.Value)
                    return (TResult)avg;
            }
            return default(TResult);
        }

        public async Task<TResult> GetAverageAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult?>> selector) where TResult : struct
        {
            var column = GetMemberName(selector);
            if (string.IsNullOrWhiteSpace(column)) throw new ArgumentException("Cannot determine the aggregate column name");
            var sql = query.ToString().Replace("[Data]", string.Format("AVG([{0}])", column));
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                var avg = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
                if (avg != DBNull.Value)
                    return (TResult)avg;
            }
            return default(TResult);
        }

        private string GetMemberName<TResult>(Expression<Func<T, TResult>> selector)
        {
            var me = selector as LambdaExpression;
            if (null == me) return null;
            var body = me.Body as MemberExpression;
            if (body == null)
            {
                //This will handle Nullable<T> properties.
                var ubody = selector.Body as UnaryExpression;

                if (ubody != null)
                    body = ubody.Operand as MemberExpression;
                if (body == null)
                    throw new ArgumentException("Expression is not a MemberExpression", "selector");

            }
            return body.Member.Name;
        }

        public async Task<bool> ExistAsync(IQueryable<T> query)
        {
            return (await this.GetCountAsync(query).ConfigureAwait(false)) > 0;
        }

        public async Task<TResult> GetScalarAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)
        {
            var column = GetMemberName(selector);
            if (string.IsNullOrWhiteSpace(column)) throw new ArgumentException("Cannot determine the scalar column name");
            var sql = query.ToString().Replace("[Data]", string.Format("[{0}]", column));
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                var avg = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
                if (avg != DBNull.Value)
                    return (TResult)avg;
            }
            return default(TResult);
        }

    }

}
