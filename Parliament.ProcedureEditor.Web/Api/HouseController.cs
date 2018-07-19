using Dapper;
using Parliament.ProcedureEditor.Web.Models;
using System.Collections.Generic;

namespace Parliament.ProcedureEditor.Web.Api
{

    public class HouseController : BaseApiController
    {
        public List<House> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select Id, TripleStoreId, HouseName
                from House");
            return GetItems<House>(command);
        }

    }
 
}