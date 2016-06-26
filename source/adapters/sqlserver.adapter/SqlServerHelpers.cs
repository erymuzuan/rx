using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public static class SqlServerHelpers
    {

        public static Type GetClrType(this string sqlType)
        {
            var lowerer = sqlType.ToEmptyString().ToLowerInvariant();
            switch (lowerer)
            {
                case "xml": return typeof(System.Xml.XmlDocument);
                case "char":
                case "nchar":
                case "ntext":
                case "text":
                case "nvarchar":
                case "varchar": return typeof(string);
                case "varbinary":
                case "timestamp":
                case "rowversion":
                case "binary":
                case "image": return typeof(byte[]);
                case "uniqueidentifier": return typeof(Guid);
                case "bigint": return typeof(long);
                case "smallint": return typeof(short);
                case "tinyint": return typeof(byte);
                case "int": return typeof(int);
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime": return typeof(DateTime);
                case "datetimeoffset": return typeof(DateTimeOffset);
                case "time": return typeof(TimeSpan);
                case "bit": return typeof(bool);
                case "numeric":
                case "smallmoney":
                case "decimal":
                case "money": return typeof(decimal);
                case "real": return typeof(float);
                case "float": return typeof(double);
                case "hierarchyid": 
                case "sql_variant": return typeof(object);
            }
            throw new Exception($"No mapping for {sqlType}");

        }

        private static int GetScore(this IColumnGeneratorMetadata x, ColumnMetadata mt)
        {
            var score = 0;

            if (x.IsNullable == ThreeWayBoolean.True && mt.IsNullable)
                score++;
            if (x.IsNullable == ThreeWayBoolean.True && !mt.IsNullable)
                return -1;

            if (x.IsNullable == ThreeWayBoolean.False && !mt.IsNullable)
                score++;
            if (x.IsNullable == ThreeWayBoolean.False && mt.IsNullable)
                return -1;

            if (x.IsComputed == ThreeWayBoolean.True && mt.IsComputed)
                score += 10;
            if (x.IsComputed == ThreeWayBoolean.True && !mt.IsComputed)
                return -1;
            if (x.IsComputed == ThreeWayBoolean.False && !mt.IsComputed)
                score += 10;
            if (x.IsComputed == ThreeWayBoolean.False && mt.IsComputed)
                return -1;

            if (x.IsIdentity == ThreeWayBoolean.True && mt.IsIdentity)
                score += 10;
            if (x.IsIdentity == ThreeWayBoolean.True && !mt.IsIdentity)
                return -1;
            if (x.IsIdentity == ThreeWayBoolean.False && !mt.IsIdentity)
                score += 10;
            if (x.IsIdentity == ThreeWayBoolean.False && mt.IsIdentity)
                return -1;

            var loweredType = mt.DbType.ToLowerInvariant();
            if (null != x.IncludeTypes)
            {
                var includes = x.IncludeTypes.Select(t => t.ToString().ToLowerInvariant()).ToArray();
                if (includes.Contains(loweredType))
                    score++;
                if (!includes.Contains(loweredType))
                    return -1;
            }

            if (null != x.ExcludeTypes)
            {
                var excludes = x.ExcludeTypes.Select(t => t.ToString().ToLowerInvariant()).ToArray();
                if (!excludes.Contains(loweredType))
                    score++;
                if (excludes.Contains(loweredType))
                    return -1;
            }

            return score;
        }

        public static async Task<Column> ReadColumnAsync(this IDataReader reader,Adapter adapter,  TableDefinition table = null)
        {
            var developerService = ObjectBuilder.GetObject<SqlAdapterDeveloperService>();
            var mt = ColumnMetadata.Read(reader);

            var scores = (from g in developerService.ColumnGenerators
                          let s = g.Metadata.GetScore(mt)
                          where s >= 0
                          orderby s descending
                          select g).ToList();
            var generator = scores.FirstOrDefault();
            if (null == generator)
                throw new InvalidOperationException($"Cannot find column generator for {mt}");
            try
            {
                var col = generator.Value.Initialize(adapter, table, mt);
                return col;
            }
            catch (Exception e)
            {
                var oc = table?.ColumnCollection.SingleOrDefault(x => x.Name == mt.Name);
                if (null != oc) oc.Unsupported = true;
                var exc = new NotSupportedException($"Fail to initilize column [{table?.Schema}].[{table?.Name}].{mt}", e) { Data = { { "col", mt.ToJson() } } };
                await ObjectBuilder.GetObject<ILogger>().LogAsync(new LogEntry(exc));
            }
            return null;
        }
        public static async Task<Column> ReadColumnAsync(this IDictionary<string,object> reader,Adapter adapter,  TableDefinition table = null)
        {
            var developerService = ObjectBuilder.GetObject<SqlAdapterDeveloperService>();
            var mt = ColumnMetadata.Read(reader);

            var scores = (from g in developerService.ColumnGenerators
                          let s = g.Metadata.GetScore(mt)
                          where s >= 0
                          orderby s descending
                          select g).ToList();
            var generator = scores.FirstOrDefault();
            if (null == generator)
                throw new InvalidOperationException($"Cannot find column generator for {mt}");
            try
            {
                var col = generator.Value.Initialize(adapter, table, mt);
                return col;
            }
            catch (Exception e)
            {
                var oc = table?.ColumnCollection.SingleOrDefault(x => x.Name == mt.Name);
                if (null != oc) oc.Unsupported = true;
                var exc = new NotSupportedException($"Fail to initilize column [{table?.Schema}].[{table?.Name}].{mt}", e) { Data = { { "col", mt.ToJson() } } };
                await ObjectBuilder.GetObject<ILogger>().LogAsync(new LogEntry(exc));
            }
            return null;
        }

    }
}
