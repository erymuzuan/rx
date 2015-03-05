using System;

namespace Bespoke.Sph.Web.ViewModels
{
    public class SphSecurityToken
    {
        public string Username { get; set; }
        public DateTime Issued { get; set; }
        public DateTime Expired { get; set; }
        public string[] Roles { get; set; }
        public string Id { get; set; }
    }
}