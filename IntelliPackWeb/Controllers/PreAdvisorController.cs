using IntelliPack.DataAccessLayer.DataManagers;
using IntelliPack.DataAccessLayer.Models;
using IntelliPackWeb.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntelliPackWeb.Controllers
{
    public class PreAdvisorController : BaseController
    {
        [Authorize]
        public ActionResult Manage()
        {
            ViewBag.PackageTitles = "Preavisos Pendientes";
            getCookies();
            PreAvisoManager pk = new PreAvisoManager();
            var result = pk.GetPreAdvisor(userIdLogged, 1);
            if (result == null || result.Count == 0)
            {
                ViewBag.NotPackage = "1";
                return View(result);
            }
            else
            {
                return View(result);
            }
        }

        [Authorize]
        public ActionResult Concludas()
        {
            ViewBag.PackageTitles = "Preavisos Entregados";
            getCookies();
            PreAvisoManager pk = new PreAvisoManager();
            var result = pk.GetPreAdvisor(userIdLogged, 2);
            if (result == null || result.Count == 0)
            {
                ViewBag.NotPackage = "1";
                return View(result);
            }
            else
            {
                return View("Manage", result);
            }
        }
        [Authorize]
        public ActionResult AddPreavisos()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddPreavisos(PreAviso model)
        {
            getCookies();
            if (ModelState.IsValid)
            {
                try
                {
                    PreAvisoManager um = new PreAvisoManager();
                    um.Insert(model);
                    ViewBag.SavedOk = "1";
                    return RedirectToAction("Manage");
                }
                catch
                {
                    ViewBag.ErrorMessage = "Ha Ocurrido un Error: Intente en unos momentos.";
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Deleteclaims(PreAviso model)
        {
            getCookies();
            if (ModelState.IsValid)
            {
                try
                {
                    PreAvisoManager um = new PreAvisoManager();
                    um.Delete(model);
                    ViewBag.DeleteOk = "1";
                    return RedirectToAction("Manage");
                }
                catch
                {
                    ViewBag.ErrorMessage = "Ha Ocurrido un Error: Intente en unos momentos.";
                }
            }
            return RedirectToAction("Manage");
        }
       
        [Authorize]
        public ActionResult ViewRePreavisos(int recl_id)
        {
            try
            {
                getCookies();
                ReclamacionesManager pk = new ReclamacionesManager();
                var model = pk.GetReclamacionById(userIdLogged, recl_id);
                if (model != null)
                {
                    return View(model);
                }
                else
                {
                    ViewBag.ErrorMessage = "Ha Ocurrido un Error: No Se Encuentran Resultados.";

                }
                return RedirectToAction("Manage");
            }
            catch
            {
                ViewBag.ErrorMessage = "Ha Ocurrido un Error: Intente en unos momentos.";
            }
            return RedirectToAction("Manage");
        }
        [Authorize]
        public ActionResult EditPreavisos(int recl_id)
        {
            try
            {
                getCookies();
                ReclamacionesManager pk = new ReclamacionesManager();
                var model = pk.GetReclamacionById(userIdLogged, recl_id);
                if (model != null)
                {
                    return View(model);
                }
                else
                {
                    ViewBag.ErrorMessage = "Ha Ocurrido un Error: No Se Encuentran Resultados.";

                }
                return RedirectToAction("Manage");
            }
            catch
            {
                ViewBag.ErrorMessage = "Ha Ocurrido un Error: Intente en unos momentos.";
            }
            return RedirectToAction("Manage");
        }
        [Authorize]
        public ActionResult Deleteclaims(int recl_id)
        {
            try
            {
                getCookies();
                ReclamacionesManager pk = new ReclamacionesManager();
                var model = pk.GetReclamacionById(userIdLogged, recl_id);
                if (model != null)
                {
                    return View(model);
                }
                else
                {
                    ViewBag.ErrorMessage = "Ha Ocurrido un Error: No Se Encuentran Resultados.";

                }
                return RedirectToAction("Manage");
            }
            catch
            {
                ViewBag.ErrorMessage = "Ha Ocurrido un Error: Intente en unos momentos.";
            }
            return RedirectToAction("Manage");
        }
        [Authorize]
        [HttpPost]
        public ActionResult EditPreavisos(Reclamaciones model)
        {
            getCookies();
            if (ModelState.IsValid)
            {
                try
                {
                    model.CourierId = userIdLogged;
                    model.EmailCourier = userEmail;
                    ReclamacionesManager pk = new ReclamacionesManager();
                    pk.Update(model);
                    ViewBag.SavedOk = "1";
                    return RedirectToAction("Manage");
                }
                catch
                {
                    ViewBag.ErrorMessage = "Ha Ocurrido un Error: Intente en unos momentos.";
                }
            }
            return RedirectToAction("Manage");
        }
    }
}