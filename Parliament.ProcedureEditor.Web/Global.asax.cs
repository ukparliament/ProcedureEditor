using System.Web;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web
{

    public class Global : HttpApplication
    {

        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Filters.Add(new System.Web.Http.AuthorizeAttribute());
            GlobalConfiguration.Configuration.MapHttpAttributeRoutes();
            GlobalConfiguration.Configuration.EnsureInitialized();
        }

        protected void Application_AuthenticateRequest()
        {
            if ((Context.Request.IsAuthenticated == false) && 
                (Context.Request.Path.Contains("/login") == false) &&
                (Context.Request.HttpMethod!="POST"))
            {
                Context.Response.Redirect("~/login");
            }
        }

    }
}