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
            CommandDefinition command = new CommandDefinition(@"select p.Id, p.WorkPackagedIsFollowedById, p.WorkPackagedIsPrecededById, 
	                si.ProcedureStatutoryInstrumentName as WorkPackagedIsFollowedByName,
	                nsi.ProcedureProposedNegativeStatutoryInstrumentName as WorkPackagedIsPrecededByName
                from ProcedureWorkPackagedThingPreceding p
                join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=p.WorkPackagedIsPrecededById
				join ProcedureStatutoryInstrument si on si.Id=p.WorkPackagedIsFollowedById");
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
            CommandDefinition command = new CommandDefinition(@"select p.Id, p.WorkPackagedIsFollowedById, p.WorkPackagedIsPrecededById, 
	                si.ProcedureStatutoryInstrumentName as WorkPackagedIsFollowedByName,
	                nsi.ProcedureProposedNegativeStatutoryInstrumentName as WorkPackagedIsPrecededByName
                from ProcedureWorkPackagedThingPreceding p
                join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=p.WorkPackagedIsPrecededById
				join ProcedureStatutoryInstrument si on si.Id=p.WorkPackagedIsFollowedById
                where p.Id=@Id",
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
                (workPackagedPreceding.WorkPackagedIsFollowedById == 0) ||
                (workPackagedPreceding.WorkPackagedIsPrecededById == 0))
                return false;
            CommandDefinition command = new CommandDefinition(@"update ProcedureWorkPackagedThingPreceding
                set WorkPackagedIsFollowedById=@WorkPackagedIsFollowedById,
                    WorkPackagedIsPrecededById=@WorkPackagedIsPrecededById,
                    ModifiedBy=@ModifiedBy,
                    ModifiedAt=@ModifiedAt
                where Id=@Id",
                new
                {
                    WorkPackagedIsFollowedById = workPackagedPreceding.WorkPackagedIsFollowedById,
                    WorkPackagedIsPrecededById = workPackagedPreceding.WorkPackagedIsPrecededById,
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
                (workPackagedPreceding.WorkPackagedIsFollowedById == 0) ||
                (workPackagedPreceding.WorkPackagedIsPrecededById == 0))
                return false;
            CommandDefinition command = new CommandDefinition(@"insert into ProcedureWorkPackagedThingPreceding
                (WorkPackagedIsFollowedById,WorkPackagedIsPrecededById,
                    ModifiedBy,ModifiedAt)
                values(@WorkPackagedIsFollowedById,@WorkPackagedIsPrecededById,
                    @ModifiedBy,@ModifiedAt)",
                new
                {
                    WorkPackagedIsFollowedById = workPackagedPreceding.WorkPackagedIsFollowedById,
                    WorkPackagedIsPrecededById = workPackagedPreceding.WorkPackagedIsPrecededById,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow
                });
            return Execute(command);
        }

        [HttpDelete]
        [ContentNegotiation("workpackagepreceding/{id:int}", ContentType.JSON)]
        public bool Delete(int id)
        {
            CommandDefinition command = new CommandDefinition(@"delete from ProcedureWorkPackagedThingPreceding where Id=@Id", new { Id = id });
            return Execute(command);
        }
    }

}