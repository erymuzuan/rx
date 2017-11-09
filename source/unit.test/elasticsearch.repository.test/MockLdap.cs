﻿using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Tests.Elasticsearch
{
    internal class MockLdap : IDirectoryService
    {
        public string CurrentUserName => "test";

        public Task<string[]> GetUserInRolesAsync(string role)
        {
            throw new System.NotImplementedException();
        }

        public Task<string[]> GetUserRolesAsync(string userName)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AuthenticateAsync(string userName, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserProfile> GetUserAsync(string userName)
        {
            throw new System.NotImplementedException();
        }
    }
}