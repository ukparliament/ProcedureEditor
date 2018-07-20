using Newtonsoft.Json;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace Parliament.ProcedureEditor.Web.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("login")]
    public class LoginController : Controller
    {
        [Route]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [Route]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin userLogin)
        {
            if ((ModelState.IsValid) && (FormsAuthentication.Authenticate(userLogin.EMail, userLogin.Password)))
            {
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, userLogin.EMail, 
                    DateTime.Now, DateTime.Now.AddDays(1), true, JsonConvert.SerializeObject(userLogin));

                FormsAuthentication.SetAuthCookie(userLogin.EMail, true, "ProcedureEditorAuth");
                return Redirect("~/");
            }
            else
                return View();
        }

        [Route("logout")]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


    }


}