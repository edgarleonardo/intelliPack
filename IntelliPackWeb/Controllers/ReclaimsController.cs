using IntelliPack.DataAccessLayer.DataManagers;
using IntelliPack.DataAccessLayer.Models;
using IntelliPackWeb.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntelliPackWeb.Controllers
{
    public class ReclaimsController : BaseController
    {
        [Authorize]
        public ActionResult Manage()
        {
            ViewBag.PackageTitles = "Reclamaciones";
            getCookies();
            ReclamacionesManager pk = new ReclamacionesManager();
            var result = pk.GetReclamaciones(userIdLogged, 1);
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
            ViewBag.PackageTitles = "Reclamaciones Concluidas";
            getCookies();
            ReclamacionesManager pk = new ReclamacionesManager();
            var result = pk.GetReclamaciones(userIdLogged, 0);

            ViewBag.NotPackage = "1";
            return View("Manage", result);
        }
        [Authorize]
        public ActionResult AddReclaims()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddReclaims(Reclamaciones model)
        {
            getCookies();
            if (ModelState.IsValid)
            {
                try
                {
                    UsersManager um = new UsersManager();
                    var user = um.GetUsersByCedula(model.ID);
                    if (user != null && user.usersId != 0)
                    {
                        model.UsersId = user.usersId;
                        model.EmailCust = user.email;
                        model.CourierId = userIdLogged;
                        model.EmailCourier = userEmail;
                        ReclamacionesManager pk = new ReclamacionesManager();
                        pk.Insert(model);
                        ViewBag.SavedOk = "1";
                        string body = "", subject = model.Subject;
                        body = System.IO.File.ReadAllText(RootUrl + "/" + ConfigurationManager.AppSettings["FileReclaimsToUser"].ToString());
                        body = string.Format(body, model.EmailCust, model.Subject, model.Description);
                        SendEmail(subject, model.EmailCust, body, true);
                        body = System.IO.File.ReadAllText(RootUrl + "/" + ConfigurationManager.AppSettings["FileReclaimsToUser"].ToString());
                        body = string.Format(body, model.EmailCourier, model.Subject, model.Description);
                        SendEmail(subject, model.EmailCourier, body, true);
                        body = System.IO.File.ReadAllText(RootUrl + "/" + ConfigurationManager.AppSettings["FileReclaimsToUser"].ToString());
                        body = string.Format(body, ConfigurationManager.AppSettings["AdminEmail"].ToString(), model.Subject, model.Description);
                        SendEmail(subject, ConfigurationManager.AppSettings["AdminEmail"].ToString(), body, true);
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Los datos no se han completado correctamente";
                    }
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
        public ActionResult Deleteclaims(Reclamaciones model)
        {
            getCookies();
            if (ModelState.IsValid)
            {
                try
                {
                    model.CourierId = userIdLogged;
                    model.EmailCourier = userEmail;
                    ReclamacionesManager pk = new ReclamacionesManager();
                    pk.Delete(model);
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
        [HttpPost]
        public ActionResult EditApproveReclaims(Reclamaciones model)
        {
            getCookies();
            if (ModelState.IsValid)
            {
                try
                {
                    model.CourierId = userIdLogged;
                    model.EmailCourier = userEmail;
                    ReclamacionesManager pk = new ReclamacionesManager();
                    model.StatusId = 2;
                    pk.Update(model);
                    ViewBag.SavedOk = "1";
                    string body = "", subject = model.Subject;
                    body = System.IO.File.ReadAllText(RootUrl + "/" + ConfigurationManager.AppSettings["FileReclaimsToAdmin"].ToString());
                    body = string.Format(body, model.EmailCust, model.Subject, model.AnswerInfo);
                    SendEmail(subject, model.EmailCust, body, true);
                    body = System.IO.File.ReadAllText(RootUrl + "/" + ConfigurationManager.AppSettings["FileReclaimsToAdmin"].ToString());
                    body = string.Format(body, model.EmailCourier, model.Subject, model.AnswerInfo);
                    SendEmail(subject, model.EmailCourier, body, true);
                    body = System.IO.File.ReadAllText(RootUrl + "/" + ConfigurationManager.AppSettings["FileReclaimsToAdmin"].ToString());
                    body = string.Format(body, ConfigurationManager.AppSettings["AdminEmail"].ToString(), model.Subject, model.AnswerInfo);
                    SendEmail(subject, ConfigurationManager.AppSettings["AdminEmail"].ToString(), body, true);
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
        public ActionResult EditApproveReclaims(int recl_id)
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
        public ActionResult ViewReclaims(int recl_id)
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
        public ActionResult EditReclaims(int recl_id)
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
        public ActionResult EditReclaims(Reclamaciones model)
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