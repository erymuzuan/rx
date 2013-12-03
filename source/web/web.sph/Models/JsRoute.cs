﻿namespace Bespoke.Sph.Web.Models
{
    public  class JsRoute
    {
        public string Role { set; get; }
        public string GroupName { set; get; }
        public string Url { set; get; }
        public string ModuleId{ set; get; }
        public string Name{ set; get; }
        public bool Visible{ set; get; }
        public string Icon{ set; get; }
        public string Caption { set; get; }
        public JsRouteSetting Settings { set; get; }
        public bool ShowWhenLoggedIn { get; set; }
        public bool IsAdminPage { get; set; }
    }
}