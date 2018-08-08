using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{

    public class RouteTypeController : BaseApiController
    {
        [HttpGet]
        [ContentNegotiation("routetype", ContentType.JSON)]
        public List<RouteType> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select Id, ProcedureRouteTypeName
                from ProcedureRouteType");
            return GetItems<RouteType>(command);
        }

    }
 
}