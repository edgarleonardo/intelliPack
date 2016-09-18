using IntelliPack.DataAccessLayer.DataManagers;
using IntelliPack.DataAccessLayer.Models;
using IntelliPackWeb.Base;
using System.Configuration;
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
        [HttpPost]
        [Authorize]
        [RequireHttps]
        public ActionResult Index(EnviosMasivos GuardarEnvio)
        {
            getCookies();
            try
            {
                if (GuardarEnvio != null && string.IsNullOrEmpty(GuardarEnvio.Subject) && string.IsNullOrEmpty(GuardarEnvio.HtmlInfo))
                {
                    EnviosMasivosManager bd = new EnviosMasivosManager();
                    bd.GuardarEnvio(GuardarEnvio);
                    string body = System.IO.File.ReadAllText(RootUrl + "/" + ConfigurationManager.AppSettings["NotificationEmails"].ToString());
                    body = string.Format(body, userEmail, GuardarEnvio.HtmlInfo);


                    UsersManager manager = new UsersManager();
                    var result = manager.GetUsers();
                    foreach (Users usuarios in result)
                    {
                        SendEmail(GuardarEnvio.Subject, usuarios.email, body, true);
                    }
                    ViewBag.CorreoEnviado = "Correo Enviado Exitosamente";
                }
            }
            catch
            {
                ViewBag.CorreoEnviado = "Ha ocurrido un error: Intentelo mas tarde";
            }
            return View(GuardarEnvio);
        }
    }
}