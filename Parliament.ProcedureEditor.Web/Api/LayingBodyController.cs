using Dapper;
using Parliament.ProcedureEditor.Web.Models;
using System.Collections.Generic;

namespace Parliament.ProcedureEditor.Web.Api
{

    public class LayingBodyController : BaseApiController
    {
        public List<LayingBody> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select Id, TripleStoreId, LayingBodyName
                from LayingBody
                where IsDeleted=0");
            return GetItems<LayingBody>(command);
        }

    }
 
}