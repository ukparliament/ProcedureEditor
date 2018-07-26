using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{

    public class LayingBodyController : BaseApiController
    {
        [HttpGet]
        [ContentNegotiation("layingbody", ContentType.JSON)]
        public List<LayingBody> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select Id, TripleStoreId, LayingBodyName
                from LayingBody
                where IsDeleted=0");
            return GetItems<LayingBody>(command);
        }

    }
 
}