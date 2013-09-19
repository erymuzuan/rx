using System.Web.Mvc;

namespace web.sph.portal.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pricing()
        {
            return View();
        }

        public ActionResult Contact(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        public ActionResult Features()
        {
            return View();
        }


        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult RumahPeranginan()
        {
            return View();
        }

        public ActionResult Quarters()
        {
            return View();
        }

        public ActionResult Office()
        {
            return View();
        }

        public ActionResult RuangKomersil()
        {
            return View();
        }

    }
}
