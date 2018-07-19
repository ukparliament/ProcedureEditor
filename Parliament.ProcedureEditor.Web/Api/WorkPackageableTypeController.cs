using Dapper;
using Parliament.ProcedureEditor.Web.Models;
using System.Collections.Generic;

namespace Parliament.ProcedureEditor.Web.Api
{

    public class WorkPackageableTypeController : BaseApiController
    {
        public List<WorkPackageableType> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select Id, ProcedureWorkPackageableThingTypeName
                from ProcedureWorkPackageableThingType
                where IsDeleted=0");
            return GetItems<WorkPackageableType>(command);
        }

    }
 
}