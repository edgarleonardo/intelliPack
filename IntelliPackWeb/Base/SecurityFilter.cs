using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IntelliPackWeb.Base
{
    public class SecurityFilter : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpCookie authCookie =
              filterContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            var User = "";
            var Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var Action = filterContext.ActionDescriptor.ActionName;
            bool isAuthenticated = false;
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket =
                       FormsAuthentication.Decrypt(authCookie.Value);
                var identity = new GenericIdentity(authTicket.Name, "Forms");
                var principal = new GenericPrincipal(identity, new string[] { authTicket.UserData });
                filterContext.HttpContext.User = principal;
                User = authTicket.Name;
                isAuthenticated = true;
            }

            if (!isAuthenticated)
            {
                if (Controller.ToLower() == "home" || (Controller.ToLower() == "account" &&
                    (Action.ToLower() == "login" || Action.ToLower() ==  "viewmap" || Action.ToLower() == "userlocation" || Action.ToLower() == "register" || Action.ToLower() == "forgotpassword" || Action.ToLower() == "userregister")))
                {
                    return;
                }
                else
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
            }
        }
    }
}