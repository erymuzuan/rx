using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace durandaljs.compiler.test
{
    class MockDirectoryService : IDirectoryService
    {
        public string CurrentUserName
        {
            get { return "erymuzuan"; }
        }

        public Task<string[]> GetUserInRolesAsync(string role)
        {
            return Task.FromResult(new []{"erymuzuan"});
        }

        public Task<string[]> GetUserRolesAsync(string userName)
        {
            return Task.FromResult(new[] { "developers" });
        }

        public Task<bool> AuthenticateAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<UserProfile> GetUserAsync(string userName)
        {
            var profile = new UserProfile
            {
                UserName = "erymuzuan",
                Email = "erymuzuan@gmail.com"
            };
            return Task.FromResult(profile);
        }
    }
}