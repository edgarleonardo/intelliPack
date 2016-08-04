using IntelliPackWeb.Base;
using System.Web.Mvc;
using IntelliPack.DataAccessLayer.DataManagers;
namespace IntelliPackWeb.Controllers
{
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        [RequireHttps]
        public ActionResult Index()
        {
            ViewBag.isAuthenticated = "0";
            if (getCookies())
            {
                ViewBag.isAuthenticated = "1";
            }
            ViewBag.ListCouriers = GetListCouriers();
            return View("Index");
        }

        [AllowAnonymous]
        [RequireHttps]
        public ActionResult UserLocation(int Id, string partial_view)
        {
            UsersManager manager = new UsersManager();
            var result = manager.GetUsers(Id);
            return PartialView(partial_view, result);
        }

        [AllowAnonymous]
        [RequireHttps]
        public ActionResult ViewMap(int Id)
        {
            UsersManager manager = new UsersManager();
            var result = manager.GetUsers(Id);
            return View(result);
        }
    }
}