

namespace Bespoke.Sph.Domain
{
    public  class JsRoute
    {
        public string Role { set; get; }
        public string GroupName { set; get; }
        public string Route { set; get; }
        public string ModuleId{ set; get; }
        public string Title{ set; get; }
        public bool Nav{ set; get; }
        public string Icon{ set; get; }
        public string Caption { set; get; }
        public JsRouteSetting Settings { set; get; }
        public bool ShowWhenLoggedIn { get; set; }
        public bool IsAdminPage { get; set; }

    }
}