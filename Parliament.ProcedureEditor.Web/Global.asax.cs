using Microsoft.ApplicationInsights.Extensibility;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Parliament.ProcedureEditor.Web
{

    public class Global : HttpApplication
    {

        protected void Application_Start()
        {
            TelemetryConfiguration.Active.TelemetryInitializers.Add(new AIInitializer());
            GlobalFilters.Filters.Add(new System.Web.Mvc.AuthorizeAttribute());
            GlobalConfiguration.Configuration.Filters.Add(new System.Web.Http.AuthorizeAttribute());
            RouteTable.Routes.MapMvcAttributeRoutes();
            GlobalConfiguration.Configuration.Routes.MapHttpRoute("api", "api/{controller}/{id}", new
            {
                id = RouteParameter.Optional
            });
        }

    }
}