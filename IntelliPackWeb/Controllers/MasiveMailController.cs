using IntelliPack.DataAccessLayer.DataManagers;
using IntelliPack.DataAccessLayer.Models;
using IntelliPackWeb.Base;
using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;

namespace IntelliPackWeb.Controllers
{
    public class MasiveMailController : BaseController
    {

        [Authorize]
        [RequireHttps]
        public ActionResult Index()
        {
            getCookies();
            return View(new EnviosMasivos());
        }
        [HttpPost, ValidateInput(false)]
        [Authorize]
        [RequireHttps]
        public ActionResult Index(EnviosMasivos GuardarEnvio)
        {
            getCookies();
            try
            {
                if (GuardarEnvio != null && !string.IsNullOrEmpty(GuardarEnvio.Subject) && !string.IsNullOrEmpty(GuardarEnvio.HtmlInfo))
                {
                    string path = "";
                    string fileName = "";
                    for (int b = 0; b < Request.Files.Count; b++)
                    {
                        var file = Request.Files[b];

                        var extension = Path.GetExtension(file.FileName).Replace(".", "");

                        fileName = Guid.NewGuid().ToString() + "." + extension;
                        if (extension != null && extension.ToLower() == "jpg" || extension.ToLower() == "png" || extension.ToLower() == "gif")
                        {
                            path = RootUrl + "/emailImages/" + fileName;
                            if (System.IO.File.Exists(path))
                            {

                                System.IO.File.Delete(path);
                            }
                            file.SaveAs(path);
                        }
                    }
                    EnviosMasivosManager bd = new EnviosMasivosManager();
                    bd.GuardarEnvio(GuardarEnvio);
                    UsersManager manager = new UsersManager();
                    var result = manager.GetUsers();
                    foreach (Users usuarios in result)
                    {
                            string body = System.IO.File.ReadAllText(RootUrl + "/" + ConfigurationManager.AppSettings["NotificationEmails"].ToString());
                        body = string.Format(body, usuarios.name + " " + usuarios.last_name, GuardarEnvio.HtmlInfo);
                        if (string.IsNullOrEmpty(path))
                        {
                            SendEmail(GuardarEnvio.Subject, usuarios.email, body, true);
                        }
                        else
                        {
                            body = System.IO.File.ReadAllText(RootUrl + "/" + ConfigurationManager.AppSettings["NotificationEmails2"].ToString());
                            body = string.Format(body, usuarios.name + " " + usuarios.last_name, GuardarEnvio.HtmlInfo, fileName);
                            SendEmail(GuardarEnvio.Subject, usuarios.email, body, true);
                        }
                    }
                    ViewBag.CorreoEnviado = "Correo Enviado Exitosamente";
                    //System.IO.File.Delete(path);
                }
                else
                {
                    ViewBag.CorreoEnviado = "Ha ocurrido un error: No puede haber campos vacíos";
                }
            }
            catch(Exception ex)
            {
                ViewBag.CorreoEnviado = "Ha ocurrido un error: Intentelo mas tarde "+ ex.Message;
            }
            return View(GuardarEnvio);
        }
    }
}