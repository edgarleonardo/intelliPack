using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntelliPack.DataAccessLayer.Models;
using IntelliPack.DataAccessLayer.DataManagers;
using IntelliPackWeb.Base;
using System.Web.Security;
using System.Configuration;

namespace IntelliPackWeb.Controllers
{
    public class AccountController : BaseController
    {
        [Authorize]
        [RequireHttps]
        public ActionResult DatosGenerales(int CustId)
        {
            try
            {
                getCookies();
                ViewBag.Couriers = GetDrpCourier();
                UsersManager manager = new UsersManager();
                var result = manager.GetUsers(CustId);

                ViewBag.Success = "Datos Actualizados Satisfactoriamente";
                return View("UserInfo", result);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View("UserInfo", new Users());
        }
        [Authorize]
        [RequireHttps]
        public ActionResult Update(Users model)
        {
            try
            {
                getCookies();
                UsersManager manager = new UsersManager();
                model.email = model.username;
                if (string.IsNullOrEmpty(model.Segundo_apellido))
                {
                    model.Segundo_apellido = "";
                }
                if (string.IsNullOrEmpty(model.Segundo_nombre))
                {
                    model.Segundo_nombre = "";
                }
                manager.Update(model);

                ViewBag.Success = "Datos Actualizados Satisfactoriamente";
                return View("UserInfo", model);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View("UserInfo", model);
        }
        [Authorize]
        [RequireHttps]
        public ActionResult Add(Users model)
        {
            try
            {
                getCookies();
                UsersManager manager = new UsersManager();
                model.email = model.username;
                if (string.IsNullOrEmpty(model.Segundo_apellido))
                {
                    model.Segundo_apellido = "";
                }
                if (string.IsNullOrEmpty(model.Segundo_nombre))
                {
                    model.Segundo_nombre = "";
                }
                manager.Update(model);

                ViewBag.Success = "Datos Actualizados Satisfactoriamente";
                return Content("Datos Actualizados Satisfactoriamente");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return Content(ViewBag.Error);
        }        
             
        [AllowAnonymous]
        [RequireHttps]
        public ActionResult Register()
        {
            ViewBag.Couriers = GetDrpCourier();
            ViewBag.Cities = GetDrpCities();
            return View(new Users());
        }
        [AllowAnonymous]
        [RequireHttps]
        public ActionResult ForgotPassword()
        {
            Users model = new Users();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [RequireHttps]
        public ActionResult ForgotPassword(string email)
        {
            Users model = new Users();
            try
            {
                UsersManager dbObject = new UsersManager();
                var UserResult = dbObject.GetUsersByEmail(email);
                if (UserResult == null || UserResult.Count() == 0)
                {
                    model.ErrorMessage = "El Correo no existe.";
                }
                else
                {
                    model = UserResult.First();

                    string body = System.IO.File.ReadAllText(RootUrl + "/" + ConfigurationManager.AppSettings["FileEmailPasswordResetLocation"].ToString());
                    body = string.Format(body, model.name, model.passwords);
                    /// Sending validation Email
                    SendEmail(ConfigurationManager.AppSettings["ForgotPasswordSubject"].ToString(), model.username, body, true);
                    model.SuccessMessage = "El Password se ha enviado a su correo.";
                }
            }
            catch (Exception ex)
            {
                model.ErrorMessage = ex.Message;
            }
            return View(model);
        }
        [Authorize]
        [RequireHttps]
        public ActionResult Users()
        {
            getCookies();
            UsersManager manager = new UsersManager();
            var result = manager.GetUsers();
            ViewBag.PackageTitles = "Usuarios";
            return View("Users", result);
        }
        [Authorize]
        [RequireHttps]
        public ActionResult MakeAccountValidation()
        {
            getCookies();
            UsersManager manager = new UsersManager();
            var result = manager.GetUsersNotValidated();
            ViewBag.PackageTitles = "Validación De Usuarios";
            return View("MakeAccountValidation", result);
        }
        [Authorize]
        [RequireHttps]
        public ActionResult ValidateAccounts(int Id)
        {
            getCookies();
            UsersManager manager = new UsersManager();
            var result = manager.GetUsersNotValidated();
            ViewBag.PackageTitles = "Validación De Usuarios";
            return View("MakeAccountValidation", result);
        }
        [Authorize]
        [RequireHttps]
        [HttpPost]
        public ActionResult ValidateAccounts(Users user)
        {
            getCookies();
            UsersManager manager = new UsersManager();
            ViewBag.PackageTitles = "Validación De Usuarios";
            if (user != null)
            {
                if (user.validation_key == "1")
                {
                    manager.ValidateUsers(user.usersId);
                    string body = System.IO.File.ReadAllText(RootUrl + "/" + ConfigurationManager.AppSettings["FileEnvioDireccion"].ToString());
                    body = string.Format(body, user.name, user.package_address);
                    SendEmail(ConfigurationManager.AppSettings["AddressEmailSubject"].ToString(), user.email, body, true);
                    return RedirectToAction("MakeAccountValidation");
                }
                else if (user.validation_key == "0")
                {
                    manager.InValidateUsers(user.usersId);
                    return RedirectToAction("MakeAccountValidation");
                }
            }           

            return View("ValidateAccounts", user);
        }
        [Authorize]
        [RequireHttps]
        public ActionResult Admins()
        {
            getCookies();
            UsersManager manager = new UsersManager();
            var result = manager.GetAdmins();
            ViewBag.PackageTitles = "Admins";
            return View("Users", result);
        }
        
       [Authorize]
        [RequireHttps]
        public ActionResult Couriers()
        {
            getCookies();
            UsersManager manager = new UsersManager();
            var result = manager.GetCouriers();
            ViewBag.PackageTitles = "Couriers";
            return View("Users", result);
        }
        [Authorize]
        [RequireHttps]
        public ActionResult Get(int Id, string partial_view)
        {
            getCookies();
            ViewBag.Couriers = GetDrpCourier();
            UsersManager manager = new UsersManager();
            var result = manager.GetUsers(Id);
            return PartialView(partial_view, result);
        }
        [Authorize]
        [RequireHttps]
        public ActionResult AddCourier()
        {
            getCookies();
            ViewBag.Cities = GetDrpCities();
            ViewBag.Couriers = GetDrpCourier();
            return View(new Users());
        }
        [HttpPost]
        [Authorize]
        [RequireHttps]
        public ActionResult AddCourier(Users model)
        {
            try
            {
                getCookies();
                ViewBag.Couriers = GetDrpCourier();
                ViewBag.Cities = GetDrpCities();
                if (ModelState.IsValid)
                {
                    model.email = model.username;
                    model.CourierId = 0;
                    if (string.IsNullOrEmpty(model.Segundo_apellido))
                    {
                        model.Segundo_apellido = "";
                    }
                    if (string.IsNullOrEmpty(model.Segundo_nombre))
                    {
                        model.Segundo_nombre = "";
                    }
                    UsersManager manager = new UsersManager();
                    manager.SetCourier(model);
                    if (manager != null && !string.IsNullOrEmpty(manager.Error_Message))
                    {
                        ViewBag.Message = manager.Error_Message;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(model.last_name ))
                        {
                            model.last_name = "";
                        }
                        string body = System.IO.File.ReadAllText(RootUrl + "/" + ConfigurationManager.AppSettings["FileSubAgentWelcome"].ToString());
                        body = string.Format(body, model.name+" "+model.last_name, model.username, model.passwords);
                        SendEmail(ConfigurationManager.AppSettings["WelcomeEmailSubject"].ToString(), model.username, body, true);
                        ViewBag.Message = "Datos Actualizados Satisfactoriamente";
                    }
                }
                else
                {
                    ViewBag.Message = "Completar el formulario correctamente.";
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Set_Message("Ha Ocurrido Un Error: " + ex.Message);
            }
            return View(model);

        }
        [HttpPost]
        [AllowAnonymous]
        [RequireHttps]
        public ActionResult Register(Users model)
        {
             
            try
            {
                ViewBag.Couriers = GetDrpCourier();
                ViewBag.Cities = GetDrpCities();
                Users userObject = null;
                if (ModelState.IsValid)
                {
                    model.email = model.username;
                    UsersManager manager = new UsersManager();
                    if (string.IsNullOrEmpty(model.Segundo_apellido))
                    {
                        model.Segundo_apellido = "";
                    }
                    if (string.IsNullOrEmpty(model.Segundo_nombre))
                    {
                        model.Segundo_nombre = "";
                    }
                    manager.Set(model);
                    userObject = manager.GetUsersByCedula(model.ID);
                    if (manager != null && !string.IsNullOrEmpty(manager.Error_Message))
                    {
                        ViewBag.Message = manager.Error_Message;
                    }
                    else
                    {
                        if (userObject != null)
                        {                           
                            ViewBag.Message = "Datos Actualizados Satisfactoriamente, su direccion para recibir los paquetes se ha enviado a su correo.";
                        }                           
                    }
                }
                else
                {
                    ViewBag.Message = "Completar el formulario correctamente.";
                }
                return RedirectToAction("UserRegister", new { @Urls = userObject.package_address });
                
            }
            catch (Exception ex)
            {
                Set_Message("Ha Ocurrido Un Error: " + ex.Message);
            }
            return View(model);
        }
        
        [AllowAnonymous]
        [RequireHttps]
        public ActionResult UserRegister(string Urls)
        {
            return View(new Users() { package_address = Urls});
        }
        [AllowAnonymous]
        [RequireHttps]
        public ActionResult Login(string ReturnUrl = "")
        {
            if (getCookies())
            {
                if (string.IsNullOrEmpty(ReturnUrl))
                {
                    ReturnUrl = ConfigurationManager.AppSettings["AuthDefaultUrl"].ToString() + usersIdGlobal.ToString();
                }

                Response.Redirect(ReturnUrl);
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [RequireHttps]
        public ActionResult Login(string userName, string password, string ReturnURL)
        {
            UsersManager user = new UsersManager();
            Users usuario = user.GetAuthentication(userName, password);
            
            if (usuario == null || usuario.usersId == 0 || usuario.username == null || usuario.passwords == "")
            {
                Set_Message("Usuario o Clave Incorrectas.");
                return View();
            }
            else if (getCookies())
            {
                if (string.IsNullOrEmpty(ReturnURL))
                {
                    ReturnURL = ConfigurationManager.AppSettings["AuthDefaultUrl"].ToString() + usuario.usersId;
                }

                Response.Redirect(ReturnURL);
                return View();
            }
            else
            {
                var authTicket = new FormsAuthenticationTicket(1, usuario.usersId.ToString(),
                DateTime.Now, DateTime.Now.AddMinutes(int.Parse(ConfigurationManager.AppSettings["TimeOutMinute"].ToString())), true, usuario.usersId.ToString());
                string cookieContents = FormsAuthentication.Encrypt(authTicket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieContents)
                {
                    Expires = authTicket.Expiration,
                    Path = FormsAuthentication.FormsCookiePath
                };
                Response.Cookies.Add(cookie);
                
                ViewBag.is_admin = usuario.is_admin;
                ViewBag.UserName = usuario.username;
                usersIdGlobal = ViewBag.userId = usuario.usersId;
                if (string.IsNullOrEmpty(ReturnURL))
                {
                    ReturnURL = ConfigurationManager.AppSettings["AuthDefaultUrl"].ToString() + usuario.usersId;
                }

                Response.Redirect(ReturnURL);

                return View();
            }
        }
        [Authorize]
        [RequireHttps]
        public ActionResult Logout()
        {
            if (getCookies())
            {
                FormsAuthentication.SignOut();
                ViewBag.RoleId = null;
                ViewBag.is_admin = null;
                ViewBag.UserName = null;
                usersIdGlobal = 0;
                userIdLogged = 0;
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ErrorAutorizacion()
        {
            return View();
        }
    }
}