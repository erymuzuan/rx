using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebApi
{
    public static class EndpointPermissionFactory
    {
        public static EndpointPermissonSetting CreatePut(OperationEndpoint operation)
        {
            return new EndpointPermissonSetting(operation, "Put");
        }
        public static EndpointPermissonSetting CreatePost(OperationEndpoint operation)
        {
            return new EndpointPermissonSetting(operation, "Post");
        }
        public static EndpointPermissonSetting CreateDelete(OperationEndpoint operation)
        {
            return new EndpointPermissonSetting(operation, "Delete");
        }
        public static EndpointPermissonSetting CreatePatch(OperationEndpoint operation)
        {
            return new EndpointPermissonSetting(operation, "Patch");
        }
        public static EndpointPermissonSetting CreateGetAction(QueryEndpoint query)
        {
            return new EndpointPermissonSetting
            {
                Parent = query.Entity,
                Controller = query.TypeName.Replace("Controller", ""),
                Action = "GetAction",
                Claims = Array.Empty<ClaimSetting>()
            };
        }
        public static EndpointPermissonSetting CreateGetCount(QueryEndpoint query)
        {
            return  new EndpointPermissonSetting
            {
                Parent = query.Entity,
                Controller = query.TypeName.Replace("Controller", ""),
                Action = "GetCount",
                Claims = Array.Empty<ClaimSetting>()
            };
        }
        public static EndpointPermissonSetting CreateSearch(EntityDefinition ed)
        {
            return new EndpointPermissonSetting
            {
                Parent = ed.Name,
                Controller = $"{ed.Name}ServiceContract",
                Action = "Search",
                Claims = Array.Empty<ClaimSetting>()
            };
        }
        public static EndpointPermissonSetting CreateOdata(EntityDefinition ed)
        {
            return new EndpointPermissonSetting
            {
                Parent = ed.Name,
                Controller = $"{ed.Name}ServiceContract",
                Action = "OdataApi",
                Claims = Array.Empty<ClaimSetting>()
            };
        }
        public static EndpointPermissonSetting CreateGetOneById(EntityDefinition ed)
        {
            return new EndpointPermissonSetting
            {
                Parent = ed.Name,
                Controller = $"{ed.Name}ServiceContract",
                Action = "GetOneByIdAsync",
                Claims = Array.Empty<ClaimSetting>()
            };
        }

    }
}