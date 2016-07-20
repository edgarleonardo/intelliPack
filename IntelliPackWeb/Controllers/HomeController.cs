using IntelliPackWeb.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntelliPack.DataAccessLayer.DataManagers;
using IntelliPack.DataAccessLayer.Models;
namespace IntelliPackWeb.Controllers
{
    public class HomeController : BaseController
    {
        [AllowAnonymous]
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
        public ActionResult UserLocation(int Id, string partial_view)
        {
            UsersManager manager = new UsersManager();
            var result = manager.GetUsers(Id);
            return PartialView(partial_view, result);
        }

        [AllowAnonymous]
        public ActionResult ViewMap(int Id)
        {
            UsersManager manager = new UsersManager();
            var result = manager.GetUsers(Id);
            return View(result);
        }
    }
}