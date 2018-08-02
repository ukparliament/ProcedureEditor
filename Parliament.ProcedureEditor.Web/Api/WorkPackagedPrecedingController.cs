using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{

    public class WorkPackagedPrecedingController : BaseApiController
    {
        [HttpGet]
        [ContentNegotiation("workpackagepreceding", ContentType.HTML)]
        public IHttpActionResult GetView()
        {
            return RenderView("Index");
        }

        [HttpGet]
        [ContentNegotiation("workpackagepreceding", ContentType.JSON)]
        public List<WorkPackagedPreceding> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select p.Id, p.ProcedureProposedNegativeStatutoryInstrumentId,
	                p.ProcedureStatutoryInstrumentId, nsi.ProcedureProposedNegativeStatutoryInstrumentName,
	                si.ProcedureStatutoryInstrumentName
                from ProcedureWorkPackagedThingPreceding p
                join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=p.ProcedureProposedNegativeStatutoryInstrumentId
                join ProcedureStatutoryInstrument si on si.Id=p.ProcedureStatutoryInstrumentId
                where p.IsDeleted=0");
            return GetItems<WorkPackagedPreceding>(command);
        }

        [HttpGet]
        [ContentNegotiation("workpackagepreceding/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetView(int id)
        {
            return RenderView("View", id);
        }

        [HttpGet]
        [ContentNegotiation("workpackagepreceding/{id:int}", ContentType.JSON)]
        public WorkPackagedPreceding Get(int id)
        {
            CommandDefinition command = new CommandDefinition(@"select p.Id, p.ProcedureProposedNegativeStatutoryInstrumentId,
	                p.ProcedureStatutoryInstrumentId, nsi.ProcedureProposedNegativeStatutoryInstrumentName,
	                si.ProcedureStatutoryInstrumentName
                from ProcedureWorkPackagedThingPreceding p
                join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=p.ProcedureProposedNegativeStatutoryInstrumentId
                join ProcedureStatutoryInstrument si on si.Id=p.ProcedureStatutoryInstrumentId
                where p.IsDeleted=0 and p.Id=@Id",
                new { Id = id });
            return GetItem<WorkPackagedPreceding>(command);
        }

        [HttpGet]
        [ContentNegotiation("workpackagepreceding/edit/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetEdit(int id)
        {
            return RenderView("Edit", id);
        }

        [HttpGet]
        [ContentNegotiation("workpackagepreceding/add", ContentType.HTML)]
        public IHttpActionResult GetAdd()
        {
            return RenderView("Edit");
        }

        [HttpPut]
        [ContentNegotiation("workpackagepreceding/{id:int}", ContentType.JSON)]
        public bool Put(int id, [FromBody]WorkPackagedPreceding workPackagedPreceding)
        {
            if ((workPackagedPreceding == null) ||
                (workPackagedPreceding.ProcedureProposedNegativeStatutoryInstrumentId == 0) ||
                (workPackagedPreceding.ProcedureStatutoryInstrumentId == 0))
                return false;
            CommandDefinition command = new CommandDefinition(@"update ProcedureWorkPackagedThingPreceding
                set ProcedureProposedNegativeStatutoryInstrumentId=@ProcedureProposedNegativeStatutoryInstrumentId,
                    ProcedureStatutoryInstrumentId=@ProcedureStatutoryInstrumentId,
                    ModifiedBy=@ModifiedBy,
                    ModifiedAt=@ModifiedAt
                where Id=@Id",
                new
                {
                    ProcedureProposedNegativeStatutoryInstrumentId = workPackagedPreceding.ProcedureProposedNegativeStatutoryInstrumentId,
                    ProcedureStatutoryInstrumentId = workPackagedPreceding.ProcedureStatutoryInstrumentId,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    Id = id
                });
            return Execute(command);
        }

        [HttpPost]
        [ContentNegotiation("workpackagepreceding", ContentType.JSON)]
        public bool Post([FromBody]WorkPackagedPreceding workPackagedPreceding)
        {
            if ((workPackagedPreceding == null) ||
                (workPackagedPreceding.ProcedureProposedNegativeStatutoryInstrumentId == 0) ||
                (workPackagedPreceding.ProcedureStatutoryInstrumentId == 0))
                return false;
            CommandDefinition command = new CommandDefinition(@"insert into ProcedureWorkPackagedThingPreceding
                (ProcedureProposedNegativeStatutoryInstrumentId,ProcedureStatutoryInstrumentId,
                    ModifiedBy,ModifiedAt)
                values(@ProcedureProposedNegativeStatutoryInstrumentId,@ProcedureStatutoryInstrumentId,
                    @ModifiedBy,@ModifiedAt)",
                new
                {
                    ProcedureProposedNegativeStatutoryInstrumentId = workPackagedPreceding.ProcedureProposedNegativeStatutoryInstrumentId,
                    ProcedureStatutoryInstrumentId = workPackagedPreceding.ProcedureStatutoryInstrumentId,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow
                });
            return Execute(command);
        }

        [HttpDelete]
        [ContentNegotiation("workpackagepreceding/{id:int}", ContentType.JSON)]
        public bool Delete(int id)
        {
            CommandDefinition command = new CommandDefinition(@"update ProcedureWorkPackagedThingPreceding
                set IsDeleted=1,
                    ModifiedBy=@ModifiedBy,
                    ModifiedAt=@ModifiedAt
                where Id=@Id",
                new
                {
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    Id = id
                });
            return Execute(command);
        }
    }

}