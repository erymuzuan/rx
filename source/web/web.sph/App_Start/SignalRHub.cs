﻿using System;
using System.Reflection;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebSph.Hubs;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Bespoke.Sph.Web.App_Start.SignalRHub))]

namespace Bespoke.Sph.Web.App_Start
{
    public class SignalRHub
    {
        public void Configuration(IAppBuilder app)
        {
            Console.WriteLine("[Starting signalR Hubs]");
            app.MapSignalR();
            app.MapSignalR<MessageConnection>("/signalr_message");
        }


        public void RegisterSqlRepository(int id, string name)
        {
            var sqlAssembly = Assembly.Load("sql.repository");
            var sqlRepositoryType = sqlAssembly.GetType("Bespoke.Sph.SqlRepository.SqlRepository`1");

            var edAssembly = Assembly.Load(ConfigurationManager.ApplicationName + "." + name);
            var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, id, name);
            var edType = edAssembly.GetType(edTypeName);
            if (null == edType)
                Console.WriteLine("Cannot create type " + edTypeName);

            var reposType = sqlRepositoryType.MakeGenericType(edType);
            var repository = Activator.CreateInstance(reposType);

            var ff = typeof(IRepository<>).MakeGenericType(new[] { edType });

            ObjectBuilder.AddCacheList(ff, repository);
        }
    }
}