using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using MySql.Data.MySqlClient;

namespace Bespoke.Sph.Integrations.Adapters
{
    [EntityType(typeof(Adapter))]
    [Export("AdapterDesigner", typeof(Adapter))]
    [DesignerMetadata(Name = "MySql database", PngIcon = "~/images/mysql-24-black.png", RouteTableProvider = typeof(MySqlServerRouteProvider), Route = "adapter.mysql/0")]
    public class MySqlAdapter : Adapter
    {
        private string m_connectionString;

        protected override Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces)
        {
            throw new NotImplementedException();
        }

        protected override async Task<Tuple<string, string>> GenerateOdataTranslatorSourceCodeAsync()
        {
            using (var conn = new MySqlConnection(""))
            using (var cmd = new MySqlCommand("", conn))
            {
                await conn.OpenAsync();

                throw new NotImplementedException();
            }
        }

        protected override Task<Tuple<string, string>> GeneratePagingSourceCodeAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task<TableDefinition> GetSchemaDefinitionAsync(string table)
        {
            throw new NotImplementedException();
        }


        public override string OdataTranslator
        {
            get { return ""; }
        }

        public string ConnectionString
        {
            get
            {

                if (string.IsNullOrWhiteSpace(m_connectionString))
                    m_connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", this.Server, this.Database, this.UserId, this.Password);
                return m_connectionString;
            }
            set { m_connectionString = value; }
        }

        public string Password { get; set; }
        public string UserId { get; set; }
        public string Server { get; set; }

        public string Database { get; set; }
    }
}
