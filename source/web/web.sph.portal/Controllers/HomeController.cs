using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

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

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactViewModel model)
        {
            var smtp = new SmtpClient();
            var message = new MailMessage("station.ss@bespoke.com.my", "sales@bespoke.com.my", "SPH portal inquiry form",
                model.ToString());
            smtp.Send(message);
            var result = new { message = "Thank you for your interest" };

            if (Request.ContentType == "application/json; charset=utf-8")
            {
                this.Response.ContentType = "application/json; charset=utf-8";
                return Content(JsonConvert.SerializeObject(result));
            }

            return View(result);
        }

        public ActionResult Features()
        {
            return View();
        }

        public ActionResult Demo()
        {
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }
        public ActionResult RestHouses()
        {
            return View();
        }

        public ActionResult Quarters()
        {
            return View();
        }
        public ActionResult Office()
        {
            ViewBag.Message = "pejabat";

            return View();
        }
        public ActionResult CommercialSpaces()
        {
            return View();
        }
        public ActionResult Halls()
        {
            return View();
        }
        public ActionResult Rp()
        {
            ViewBag.Message = "Rumah peranginan";

            return View();
        }
        public ActionResult Rk()
        {
            ViewBag.Message = "ruang komersial";

            return View();
        }
    }

    public class ContactViewModel
    {
        public string Name { get; set; }
        public string Organization { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }

        public override string ToString()
        {
            var message = new StringBuilder();
            message.AppendLine("Name : " + this.Name);
            message.AppendLine("Organization : " + this.Organization);
            message.AppendLine("Email : " + this.Email);
            message.AppendLine("Telephone : " + this.Telephone);
            message.AppendLine("Message : \r\n" + this.Message);

            return message.ToString();
        }
    }
}
