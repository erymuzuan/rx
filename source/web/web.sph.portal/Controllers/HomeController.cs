using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.sph.portal.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult Pricing()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Features()
        {
            ViewBag.Message = "Your features";

            return View();
        }

        public ActionResult Demo()
        {
            ViewBag.Message = "Your demo";

            return View();
        }

        public ActionResult Faq()
        {
            ViewBag.Message = "Your frequently ask question";

            return View();
        }

        public ActionResult Rp()
        {
            ViewBag.Message = "Rumah peranginan";

            return View();
        }

        public ActionResult Quarters()
        {
            ViewBag.Message = "Rumah kerajaan";

            return View();
        }

        public ActionResult Office()
        {
            ViewBag.Message = "pejabat";

            return View();
        }

        public ActionResult Rk()
        {
            ViewBag.Message = "ruang komersial";

            return View();
        }

    }
}
