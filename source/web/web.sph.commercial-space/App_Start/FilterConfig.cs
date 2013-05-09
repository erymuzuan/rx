using System.Web;
using System.Web.Mvc;

namespace web.sph.commercial_space
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}