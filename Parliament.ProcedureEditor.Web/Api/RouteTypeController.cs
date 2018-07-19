using Dapper;
using Parliament.ProcedureEditor.Web.Models;
using System.Collections.Generic;

namespace Parliament.ProcedureEditor.Web.Api
{

    public class RouteTypeController : BaseApiController
    {
        public List<RouteType> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select Id, ProcedureRouteTypeName
                from ProcedureRouteType
                where IsDeleted=0");
            return GetItems<RouteType>(command);
        }

    }
 
}