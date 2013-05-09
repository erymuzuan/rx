using System;
using System.Web.Optimization;
using Bespoke.Sph.Commerspace.Web.App_Start;

[assembly: WebActivator.PostApplicationStartMethod(
    typeof(web.sph.commercial_space.App_Start.HotTowelConfig), "PreStart")]

namespace web.sph.commercial_space.App_Start
{
    public static class HotTowelConfig
    {
        public static void PreStart()
        {
            // Add your start logic here
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}