using IntelliPack.DataAccessLayer.DataManagers;
using IntelliPack.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IntelliPackWeb.Base
{
    public class BaseController : Controller
    {
        protected int usersIdGlobal = 0;
        protected int userIdLogged = 0;
        protected int RoleId = 0;
        protected string userEmail = "";
        protected string ErrorMessage { get; set; }
        protected string RootUrl = System.Web.HttpContext.Current.Server.MapPath("~/");
        
            protected void SendEmail(string subject, string userName, string body, bool isHtml, string attachmentUrl)
        {
            try
            {

                SmtpClient client = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["SmtpServer"].ToString(),
                    Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"].ToString()),
                    Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpUser"].ToString(), ConfigurationManager.AppSettings["SmtpPass"].ToString()),
                    Timeout = int.Parse(ConfigurationManager.AppSettings["SmtpTimeOut"].ToString()),
                };
                MailMessage mm = new MailMessage(ConfigurationManager.AppSettings["SmtpFrom"].ToString(), userName, subject, body);
                Attachment attachment = new Attachment(attachmentUrl);
                mm.Attachments.Add(attachment);
                mm.IsBodyHtml = isHtml;
                client.Send(mm);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                LogError("Sending Email", e.Message);
            }
        }
        protected void SendEmail(string subject, string userName, string body, bool isHtml)
        {
            try
            {

                SmtpClient client = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["SmtpServer"].ToString(),
                    Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"].ToString()),                   
                    Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpUser"].ToString(), ConfigurationManager.AppSettings["SmtpPass"].ToString()),
                    Timeout = int.Parse(ConfigurationManager.AppSettings["SmtpTimeOut"].ToString()),
                };
                MailMessage mm = new MailMessage(ConfigurationManager.AppSettings["SmtpFrom"].ToString(), userName, subject, body);
                mm.IsBodyHtml = isHtml;
                client.Send(mm);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                LogError("Sending Email",e.Message);
            }
        }
        public void LogError(string origin, string filterContext)
        {
            StringBuilder builder = new StringBuilder();
            builder
                .AppendLine("----------")
                .AppendLine(DateTime.Now.ToString())
                .AppendFormat("Source:\t{0}", origin)
                .AppendLine()
                .AppendFormat("Message:\t{0}", filterContext)
                .AppendLine();

            string filePath = RootUrl+"/Error.log";
            
            using (StreamWriter writer = System.IO.File.AppendText(filePath))
            {
                writer.Write(builder.ToString());
                writer.Flush();
            }
        }
        protected void SendEmail(string to, string subject, string body)
        {
            try
            {
                SmtpClient client = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["SmtpServer"].ToString(),
                    Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"].ToString()),
                    Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpUser"].ToString(), ConfigurationManager.AppSettings["SmtpPass"].ToString()),
                    Timeout = int.Parse(ConfigurationManager.AppSettings["SmtpTimeOut"].ToString()),
                };
                MailMessage mm = new MailMessage(ConfigurationManager.AppSettings["SmtpFrom"].ToString(), to, subject, body);
                client.Send(mm);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                LogError("Sending Email", e.Message);
            }
        }
        protected bool getCookies()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket =
                       FormsAuthentication.Decrypt(authCookie.Value);
                var user = authTicket.Name;
                int userID = 0;
                int.TryParse(user, out userID);
                if (userID > 0)
                {
                    UsersManager usuarioManager = new UsersManager();
                    Users usuario = usuarioManager.GetUsers(userID);
                    ViewBag.RoleId = usuario.Id_Rol;
                    RoleId = usuario.Id_Rol;
                    ViewBag.is_admin = usuario.is_admin;
                    ViewBag.UserName = usuario.username;
                    usersIdGlobal = ViewBag.userId = usuario.usersId;
                    ViewBag.userIdInfo = userIdLogged = userID;
                    userEmail = usuario.email;
                    return true;
                }                
            }
            
            return false;
        }

        protected IEnumerable<Users> GetTypeOfPayment()
        {
            try
            {
                var _man = new UsersManager();

                var result = _man.GetUsersCourier();
                return result;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
            }
            return null;
        }

        protected IEnumerable<Users> GetListCouriers()
        {
            try
            {
                UsersManager _man = new UsersManager();

                var result = _man.GetUsersCourier();
                return result;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
            }
            return null;
        }
        protected IEnumerable<SelectListItem> GetDrpCourier()
        {
            List<SelectListItem> ls = new List<SelectListItem>();
            try
            {
                UsersManager _man = new UsersManager();

                var result = _man.GetUsersCourier();
                foreach (var temp in result)
                {
                    ls.Add(new SelectListItem() { Text = temp.name.ToString(), Value = temp.usersId.ToString() });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
            }
            return ls;
        }

        protected IEnumerable<SelectListItem> GetDrpCities()
        {
            List<SelectListItem> ls = new List<SelectListItem>();
            try
            {
                CityManager _man = new CityManager();

                var result = _man.GetCities();
                foreach (var temp in result)
                {
                    ls.Add(new SelectListItem() { Text = temp.city_description.ToString(), Value = temp.city_code.ToString() });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
            }
            return ls;
        }

        protected void Set_Message(string message)
        {
            ViewBag.Message = message;
        }

        protected string Upload_File(string Id_Element, string File_Name)
        {
            var file = Request.Files[Id_Element];
            var extension = Path.GetExtension(file.FileName).Replace(".", "");
            if (extension != null && extension != "")
            {
                var fileName = File_Name + "." + extension;

                string path = Path.Combine(Server.MapPath("~/content/"), fileName.ToString());

                file.SaveAs(path);
                return fileName;
            }
            return "";
        }
    }
}