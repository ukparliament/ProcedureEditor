using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{

    public class WorkPackageableTypeController : BaseApiController
    {
        [HttpGet]
        [ContentNegotiation("workpackageabletype", ContentType.JSON)]
        public List<WorkPackageableType> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select Id, ProcedureWorkPackageableThingTypeName
                from ProcedureWorkPackageableThingType
                where IsDeleted=0");
            return GetItems<WorkPackageableType>(command);
        }

    }
 
}