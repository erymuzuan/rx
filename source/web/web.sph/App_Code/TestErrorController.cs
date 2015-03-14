using System;
using System.Web.Mvc;

namespace web.sph.App_Code
{
    public class TestErrorController : Controller
    {
        // GET: TestError
        public ActionResult Index()
        {
            throw new Exception("This is a test error from your server");
        }
    }
}