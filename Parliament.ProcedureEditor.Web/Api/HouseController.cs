using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{

    public class HouseController : BaseApiController
    {
        [HttpGet]
        [ContentNegotiation("house", ContentType.JSON)]
        public List<House> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select Id, TripleStoreId, HouseName
                from House");
            return GetItems<House>(command);
        }

    }
 
}