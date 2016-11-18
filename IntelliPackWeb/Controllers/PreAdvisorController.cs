using IntelliPack.DataAccessLayer.DataManagers;
using IntelliPack.DataAccessLayer.Models;
using IntelliPackWeb.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntelliPackWeb.Controllers
{
    public class PreAdvisorController : BaseController
    {
        [Authorize]
        ////[RequireHttps]
        public ActionResult Manage()
        {
            ViewBag.PackageTitles = "Preavisos Creados Pendientes";
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
        //[RequireHttps]
        public ActionResult Concludas()
        {
            ViewBag.PackageTitles = "Preavisos Concluidos Entregados";
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
        //[RequireHttps]
        public ActionResult AddPreavisos()
        {
            getCookies();
            return View();
        }
        [Authorize]
        [HttpPost]
        //[RequireHttps]
        public ActionResult DeletePreAdvisor(PreAviso model)
        {
            getCookies();
            if (ModelState.IsValid)
            {
                try
                {
                    PreAvisoManager um = new PreAvisoManager();
                    model.Weights = 0;
                    model.usersId = userIdLogged;
                    um.Delete(model);
                    ViewBag.DeleteOk = "1";
                    return RedirectToAction("Manage");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Ha Ocurrido un Error: Intente en unos momentos.";
                }
            }
            return View(model);
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult DeletePreAdvisor(string tracking_code, int user_id)
        {
            try
            {
                getCookies();
                PreAvisoManager um = new PreAvisoManager();
                var model = um.GetPreAdvisorById(user_id, tracking_code);
                if (model != null)
                {
                    return View(model);
                }
            }
            catch
            {

            }
            return RedirectToAction("Manage");
        }
        [Authorize]
        [HttpPost]
        //[RequireHttps]
        public ActionResult EditPreAdvisor(PreAviso model)
        {
            getCookies();
            if (ModelState.IsValid)
            {
                try
                {
                    try
                    {
                        string path = "";
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            var file = Request.Files[i];

                            var extension = Path.GetExtension(file.FileName).Replace(".", "");

                            var fileName = model.invoice;
                            
                            path = Path.Combine(RootUrl + "/preavisoFiles", fileName);
                            try
                            {
                                System.IO.File.Delete(path);
                            }
                            catch
                            { }
                            /// Guardando el archivo
                            if (ConfigurationManager.AppSettings["ExtensionsAllowed"].ToString().ToLower().Contains(extension.ToLower()))
                            {
                                try
                                {
                                    file.SaveAs(path);
                                }
                                catch (Exception ex) { throw ex; }
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "Ha Ocurrido un Error: Formato de archivo invalido, deben ser: " + ConfigurationManager.AppSettings["ExtensionsAllowed"].ToString().ToLower();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = "Ha Ocurrido un Error: " + ex.Message.ToString();
                    }
                    PreAvisoManager um = new PreAvisoManager();
                    model.Weights = 0;
                    model.usersId = userIdLogged;
                    um.Update(model);
                    ViewBag.SavedOk = "1";
                    return RedirectToAction("Manage");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Ha Ocurrido un Error: Intente en unos momentos.";
                }
            }
            return View(model);
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult EditPreAdvisor(string tracking_code, int user_id)
        {
            try
            {
                getCookies();
                PreAvisoManager um = new PreAvisoManager();
                var model = um.GetPreAdvisorById(user_id, tracking_code);
                if (model != null)
                {
                    return View(model);
                }
            }
            catch
            {

            }
            return RedirectToAction("Manage");
        }
        
        [Authorize]
        [HttpPost]
        //[RequireHttps]
        public ActionResult AddPreavisos(PreAviso model)
        {
            getCookies();
            if (ModelState.IsValid)
            {
                try
                {
                    try
                    {
                        string path ="";
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            var file = Request.Files[i];
                            var extension = Path.GetExtension(file.FileName).Replace(".", "");                           
                            var fileName = Guid.NewGuid().ToString() + "." + extension;
                            model.invoice = fileName;
                            path = Path.Combine(RootUrl+ "/preavisoFiles", fileName);                            
                            /// Guardando el archivo
                            if (ConfigurationManager.AppSettings["ExtensionsAllowed"].ToString().ToLower().Contains(extension.ToLower()))
                            {
                                try
                                {
                                    file.SaveAs(path);
                                }
                                catch (Exception ex) { throw ex; }
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "Ha Ocurrido un Error: Formato de archivo invalido, deben ser: "+ ConfigurationManager.AppSettings["ExtensionsAllowed"].ToString().ToLower();
                            }
                        }                        
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = "Ha Ocurrido un Error: " + ex.Message.ToString();
                    }
                    PreAvisoManager um = new PreAvisoManager();
                    model.Weights = 0;
                    model.usersId = userIdLogged;
                    um.Insert(model);
                    ViewBag.SavedOk = "1";
                    string body = System.IO.File.ReadAllText(RootUrl + "/" + ConfigurationManager.AppSettings["PreAdvisor"].ToString());
                    body = string.Format(body, userEmail, model.tracking_code);
                    SendEmail("Nueva PreAlerta Registrada", ConfigurationManager.AppSettings["AdminEmail"].ToString(), body, true);
                    return RedirectToAction("Manage");
                }
                catch(Exception ex)
                {
                    ViewBag.ErrorMessage = "Ha Ocurrido un Error: Intente en unos momentos.";
                }
            }
            return View(model);
        }
        [Authorize]
        //[RequireHttps]
        public ActionResult Download(string fileName)
        {
            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(RootUrl + "/preavisoFiles", fileName));

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch
            {
                return Json("El Archivo no existe.", JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        [HttpPost]
        //[RequireHttps]
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
        //[RequireHttps]
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
        //[RequireHttps]
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
        //[RequireHttps]
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
        //[RequireHttps]
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