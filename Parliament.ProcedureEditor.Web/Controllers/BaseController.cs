using System.Web.Mvc;

namespace Parliament.ProcedureEditor.Web.Controllers
{
    [RoutePrefix(@"{route:regex((?i)(procedure)|(?i)(workpackage)|(?i)(step)|(?i)(route)|(?i)(businessitem)|(?i)(workpackagepreceding))}")]
    public class BaseController : Controller
    {
        [Route]
        [Route("~/")]
        public ActionResult Index(string route)
        {
            route = route ?? "procedure";
            return View($"{route}/Index");
        }

        [Route("View/{id:int}")]
        public ActionResult Details(string route, int id)
        {
            return View($"{route}/View", id);
        }

        [Route("Edit/{id:int?}")]
        public ActionResult Edit(string route, int? id)
        {
            return View($"{route}/Edit", id);
        }

    }
}