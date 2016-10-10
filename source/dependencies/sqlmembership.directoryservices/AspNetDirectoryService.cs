﻿using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.DirectoryServices
{
    public class AspNetDirectoryService : IDirectoryService
    {
        public AspNetDirectoryService()
        {
            try
            {
                ConfigurationManager.AddConnectionString();
            }
            catch (ConfigurationException)
            {
                // ignore
            }
        }
        public string CurrentUserName
        {
            get
            {
                string user;
                var context = HttpContext.Current.GetOwinContext();
                if (null != context)
                {
                    user = context.Request.User.Identity.Name;
                    if (!string.IsNullOrEmpty(user)) return user;
                }

                user = HttpContext.Current?.User.Identity.Name;
                if (!string.IsNullOrWhiteSpace(user)) return user;
                user = Membership.GetUser()?.UserName;
                if (!string.IsNullOrWhiteSpace(user)) return user;

                if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                    return Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrWhiteSpace(Environment.UserDomainName))
                    return Environment.UserDomainName;
                return Environment.UserName;
            }
        }

        public Task<string[]> GetUserInRolesAsync(string role)
        {
            if (!Roles.RoleExists(role))
                throw new InvalidOperationException($"The role '{role}' was not found, you can use .\\utils\\mru.exe -r {role} -c .\\web\\web.config to add the role manually, or you should have configured Designation and roles correctly ");
            return Task.FromResult(Roles.GetUsersInRole(role));
        }

        public Task<string[]> GetUserRolesAsync(string userName)
        {
            return Task.FromResult(Roles.GetRolesForUser(userName));
        }

        public Task<bool> AuthenticateAsync(string userName, string password)
        {
            return Task.FromResult(Membership.ValidateUser(userName, password));
        }

        public Task<UserProfile> GetUserAsync(string userName)
        {
            var user = Membership.GetUser(userName);
            if (null != user)
                return Task.FromResult(new UserProfile
                {
                    UserName = userName,
                    IsLockedOut = user.IsLockedOut

                });
            return Task.FromResult<UserProfile>(null);
        }
    }
}