using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api.Configuration
{
    /// <summary>
    /// Action result returning Html response
    /// </summary>
    public class HtmlContentActionResult : IHttpActionResult
    {
        private readonly HttpRequestMessage requestMessage;
        private readonly object data;
        private readonly string view;
        private readonly string controllerName;

        public HtmlContentActionResult(HttpRequestMessage httpRequestMessage, string controller = null, string viewName = null, object dataObject = null)
        {
            requestMessage = httpRequestMessage;
            view = viewName;
            data = dataObject;
            controllerName = controller;
        }

        /// <summary>
        /// Generates the view based on controller name, view name and data model using Razor engine.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            System.Web.Mvc.Controller mvcController = createController();

            HttpResponseMessage response = requestMessage.CreateResponse(HttpStatusCode.OK);
            using (StringWriter writer = new StringWriter())
            {
                System.Web.Mvc.ViewEngineResult viewEngineResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(mvcController.ControllerContext, view);
                if (data != null)
                    mvcController.ViewData = new System.Web.Mvc.ViewDataDictionary(data);    
                System.Web.Mvc.ViewContext viewContext = new System.Web.Mvc.ViewContext(mvcController.ControllerContext, viewEngineResult.View, mvcController.ViewData, mvcController.TempData, writer);
                viewEngineResult.View.Render(viewContext, writer);
                response.Content = new StringContent(writer.GetStringBuilder().ToString());
            }

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return Task.FromResult(response);
        }

        /// <summary>
        /// Creates fake (empty) Mvc controller to allow render the view
        /// </summary>
        /// <returns></returns>
        private System.Web.Mvc.Controller createController()
        {
            System.Web.Mvc.Controller mvcController = new Controllers.MvcController();

            System.Web.HttpContextWrapper wrapper = new System.Web.HttpContextWrapper(System.Web.HttpContext.Current);
            System.Web.Routing.RouteData routeData = new System.Web.Routing.RouteData();
            routeData.Values.Add("controller", controllerName);

            mvcController.ControllerContext = new System.Web.Mvc.ControllerContext(wrapper, routeData, mvcController);

            return mvcController;
        }
    }
}