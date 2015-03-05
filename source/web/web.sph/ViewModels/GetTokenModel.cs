using System;

namespace Bespoke.Sph.Web.ViewModels
{
    public class GetTokenModel
    {
        public string grant_type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public DateTime expiry { get; set; }
    }
}