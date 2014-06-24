﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using MySql.Data.MySqlClient;

namespace mysql.adapter
{
    public class MySqlAdapter : Adapter
    {
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

        protected override Task<TableDefinition> GetSchemaDefinitionAsync(string table)
        {
            throw new NotImplementedException();
        }

        public override string OdataTranslator
        {
            get { throw new NotImplementedException(); }
        }
    }
}