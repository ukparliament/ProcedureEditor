using Newtonsoft.Json;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Web.Http;
using System.Web.Security;

namespace Parliament.ProcedureEditor.Web.Api
{
    [AllowAnonymous]
    public class AuthenticationController : BaseApiController
    {
        [HttpGet]
        [ContentNegotiation("~/login", ContentType.HTML)]
        public IHttpActionResult Login()
        {
            return RenderView("Login");
        }

        [HttpPost]
        [Route("~/login")]
        public IHttpActionResult Login(UserLogin userLogin)
        {
            if ((ModelState.IsValid) && (FormsAuthentication.Authenticate(userLogin.EMail, userLogin.Password)))
            {
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, userLogin.EMail,
                    DateTime.Now, DateTime.Now.AddDays(1), true, JsonConvert.SerializeObject(userLogin));

                FormsAuthentication.SetAuthCookie(userLogin.EMail, true, "ProcedureEditorAuth");
                return Redirect(new Uri("/procedure", UriKind.Relative));
            }
            else
                return RenderView("Login");
        }

        [HttpGet]
        [Route("~/logout")]
        public IHttpActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Login();
        }
    }
}
