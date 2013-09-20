using System.Web.Mvc;

namespace web.sph.portal.Controllers
{
    public class HomeController : Controller
    {
        private const string Moto = "Easy, Dynamic and Professional";

        public ActionResult Index()
        {
            ViewBag.Moto = Moto;
            return View();
        }

        public ActionResult Pricing()
        {
            ViewBag.Moto = Moto;
            return View();
        }

        public ActionResult Contact(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        public ActionResult Features()
        {
            ViewBag.Moto = Moto;
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
            ViewBag.Moto = Moto;
            return View();
        }

        public ActionResult Office()
        {
            ViewBag.Moto = Moto;
            return View();
        }

        public ActionResult RuangKomersil()
        {
            ViewBag.Moto = Moto;
            return View();
        }

    }
}
