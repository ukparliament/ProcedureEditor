using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{
    public class HtmlTemplateController : BaseApiController
    {
        
        [HttpGet]
        [Route("template/{templateName}")]
        public IHttpActionResult Get(string templateName)
        {
            return RenderView(templateName);
        }

    }
}