using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{
    public class ProcedureController : BaseApiController
    {
        [HttpGet]
        [ContentNegotiation("~/", ContentType.HTML)]
        [ContentNegotiation("procedure", ContentType.HTML)]
        public IHttpActionResult GetView()
        {
            return RenderView("Index");
        }

        [HttpGet]
        [ContentNegotiation("procedure", ContentType.JSON)]
        public List<Procedure> Get()
        {
            CommandDefinition command = new CommandDefinition("select Id, TripleStoreId, ProcedureName, ProcedureDescription from [Procedure]");
            return GetItems<Procedure>(command);
        }

        [HttpGet]
        [ContentNegotiation("procedure/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetView(int id)
        {
            return RenderView("View", id);
        }

        [HttpGet]
        [ContentNegotiation("procedure/{id:int}", ContentType.JSON)]
        public Procedure Get(int id)
        {
            CommandDefinition command = new CommandDefinition("select Id, TripleStoreId, ProcedureName, ProcedureDescription from [Procedure] where Id=@Id", new { Id = id });
            return GetItem<Procedure>(command);
        }

        [HttpGet]
        [ContentNegotiation("procedure/edit/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetEdit(int id)
        {
            return RenderView("Edit", id);
        }

        [HttpGet]
        [ContentNegotiation("procedure/add", ContentType.HTML)]
        public IHttpActionResult GetAdd()
        {
            return RenderView("Edit");
        }

        [HttpPut]
        [ContentNegotiation("procedure/{id:int}", ContentType.JSON)]
        public bool Put(int id, [FromBody]Procedure procedure)
        {
            if ((procedure == null) || (string.IsNullOrWhiteSpace(procedure.ProcedureName)))
                return false;
            CommandDefinition command = new CommandDefinition(@"update [Procedure]
                set ProcedureName=@ProcedureName,
                    ProcedureDescription=@ProcedureDescription,
                    ModifiedBy=@ModifiedBy,
                    ModifiedAt=@ModifiedAt
                where Id=@Id",
                new
                {
                    ProcedureName = procedure.ProcedureName.Trim(),
                    ProcedureDescription = procedure.ProcedureDescription,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    Id = id
                });
            return Execute(command);
        }

        [HttpPost]
        [ContentNegotiation("procedure", ContentType.JSON)]
        public bool Post(Procedure procedure)
        {
            if ((procedure == null) || (string.IsNullOrWhiteSpace(procedure.ProcedureName)))
                return false;
            string tripleStoreId = GetTripleStoreId();
            if (string.IsNullOrWhiteSpace(tripleStoreId))
                return false;
            CommandDefinition command = new CommandDefinition(@"insert into [Procedure] (TripleStoreId, ProcedureName, ProcedureDescription, ModifiedBy, ModifiedAt)
                values (@TripleStoreId, @ProcedureName, @ProcedureDescription, @ModifiedBy, @ModifiedAt)",
                new
                {
                    TripleStoreId = tripleStoreId,
                    ProcedureName = procedure.ProcedureName.Trim(),
                    ProcedureDescription = procedure.ProcedureDescription,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTime.UtcNow
                });
            return Execute(command);
        }

        [HttpDelete]
        [ContentNegotiation("procedure/{id:int}", ContentType.JSON)]
        public bool Delete(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ProcedureId", id);
            parameters.Add("@IsSuccess", dbType: System.Data.DbType.Boolean, direction: System.Data.ParameterDirection.Output);
            CommandDefinition command = new CommandDefinition("DeleteProcedure",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);
            if (Execute(command))
                return parameters.Get<bool>("@IsSuccess");
            else
                return false;
        }

    }
}