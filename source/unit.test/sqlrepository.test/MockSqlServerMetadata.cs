using Bespoke.Sph.SqlRepository;

namespace Bespoke.Sph.Tests.SqlServer
{
    public class MockSqlServerMetadata : ISqlServerMetadata
    {
        public Table GetTable(string name)
        {
            return new Table
            {
                Name = name,
                Columns = new[]
                {
                    new Column{Name = "Id",IsNullable = false,CanRead = true, CanWrite = true,SqlType = "VARCHAR(255)"},
                    new Column{Name = "Name",IsNullable = false,CanRead = true, CanWrite = true,SqlType = "VARCHAR(255)"},
                    new Column{Name = "JSONN",IsNullable = false,CanRead = true, CanWrite = true,SqlType = "VARCHAR(MAX)"},
                }
            };
        }
    }
}