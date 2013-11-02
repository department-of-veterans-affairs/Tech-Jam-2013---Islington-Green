using System.Web.Mvc;
using IG.Data;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Web.Security;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ValidateUser()
        {
            ViewBag.LoggedIn = Session["loggedIn"] = true;
            Membership.ValidateUser("test", "test");
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            ViewBag.LoggedIn = Session["loggedIn"] = false;
            return RedirectToAction("Index");
        }

        public ActionResult Data()
        {
            return RedirectToAction("Index", "Data");
        }

        public ActionResult Map()
        {
            return RedirectToAction("FindProvider", "Data");
        }
    }
}
