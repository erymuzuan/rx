using System.Web.Optimization;
using Bespoke.Sph.Commerspace.Web.App_Start;

namespace Bespoke.Sph.Commerspace.Web.App_Start
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