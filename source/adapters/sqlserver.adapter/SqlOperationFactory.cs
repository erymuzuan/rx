using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public static class SqlOperationFactory
    {
        public static async Task<SqlOperationDefinition> CreateAsync(this SqlServerAdapter adapter, string type, string schema, string name)
        {
            SqlOperationDefinition op;
            switch (type)
            {
                case "P":
                case "P ":
                    op = new SprocOperationDefinition { Schema = schema, Name = name, ObjectType = "P" };
                    break;
                case "TF":
                    op = new TableValuedFunction { Schema = schema, Name = name, ObjectType = "TF" };
                    break;
                case "IF":
                    op = new TableValuedFunction { Schema = schema, Name = name, ObjectType = "IF" };
                    break;
                case "FN":
                    op = new ScalarValuedFunction { Schema = schema, Name = name, ObjectType = "FN" };
                    break;
                default:
                    throw new ArgumentException($"Cannot identify SQL Server object of type '{type}'", nameof(type));
            }
            op.RequestMemberCollection.Clear();
            op.ResponseMemberCollection.Clear();
            try
            {
                await op.InitializeRequestMembersAsync(adapter);
            }
            catch (Exception e)
            {
                throw new NotSupportedException($"Fail to initialize {op}", e) { Data = { { "operation", op.ToJson() } } };
            }
            return op;
        }

    }
}