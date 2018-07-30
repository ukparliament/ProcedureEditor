using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{

    public class WorkPackageablePrecedingController : BaseApiController
    {
        [HttpGet]
        [ContentNegotiation("workpackagepreceding", ContentType.HTML)]
        public IHttpActionResult GetView()
        {
            return RenderView("Index");
        }

        [HttpGet]
        [ContentNegotiation("workpackagepreceding", ContentType.JSON)]
        public List<WorkPackageablePreceding> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select p.Id, p.PrecedingProcedureWorkPackageableThingId,
                p.FollowingProcedureWorkPackageableThingId, 
                pw.ProcedureWorkPackageableThingName as PrecedingProcedureWorkPackageableThingName,
                fw.ProcedureWorkPackageableThingName as FollowingProcedureWorkPackageableThingName from ProcedureWorkPackageableThingPreceding p
                join ProcedureWorkPackageableThing pw on pw.Id=p.PrecedingProcedureWorkPackageableThingId
                join ProcedureWorkPackageableThing fw on fw.Id=p.FollowingProcedureWorkPackageableThingId
                where p.IsDeleted=0");
            return GetItems<WorkPackageablePreceding>(command);
        }

        [HttpGet]
        [ContentNegotiation("workpackagepreceding/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetView(int id)
        {
            return RenderView("View", id);
        }

        [HttpGet]
        [ContentNegotiation("workpackagepreceding/{id:int}", ContentType.JSON)]
        public WorkPackageablePreceding Get(int id)
        {
            CommandDefinition command = new CommandDefinition(@"select p.Id, p.PrecedingProcedureWorkPackageableThingId,
                p.FollowingProcedureWorkPackageableThingId, 
                pw.ProcedureWorkPackageableThingName as PrecedingProcedureWorkPackageableThingName,
                fw.ProcedureWorkPackageableThingName as FollowingProcedureWorkPackageableThingName
                from ProcedureWorkPackageableThingPreceding p
                join ProcedureWorkPackageableThing pw on pw.Id=p.PrecedingProcedureWorkPackageableThingId
                join ProcedureWorkPackageableThing fw on fw.Id=p.FollowingProcedureWorkPackageableThingId
                where p.Id=@Id",
                new { Id = id });
            return GetItem<WorkPackageablePreceding>(command);
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
        public bool Put(int id, [FromBody]WorkPackageablePreceding workPackageablePreceding)
        {
            if ((workPackageablePreceding == null) ||
                (workPackageablePreceding.PrecedingProcedureWorkPackageableThingId == 0) ||
                (workPackageablePreceding.FollowingProcedureWorkPackageableThingId == 0))
                return false;
            CommandDefinition command = new CommandDefinition(@"update ProcedureWorkPackageableThingPreceding
                set PrecedingProcedureWorkPackageableThingId=@PrecedingProcedureWorkPackageableThingId,
                    FollowingProcedureWorkPackageableThingId=@FollowingProcedureWorkPackageableThingId,
                    ModifiedBy=@ModifiedBy,
                    ModifiedAt=@ModifiedAt
                where Id=@Id",
                new
                {
                    PrecedingProcedureWorkPackageableThingId = workPackageablePreceding.PrecedingProcedureWorkPackageableThingId,
                    FollowingProcedureWorkPackageableThingId = workPackageablePreceding.FollowingProcedureWorkPackageableThingId,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    Id = id
                });
            return Execute(command);
        }

        [HttpPost]
        [ContentNegotiation("workpackagepreceding", ContentType.JSON)]
        public bool Post([FromBody]WorkPackageablePreceding workPackageablePreceding)
        {
            if ((workPackageablePreceding == null) ||
                (workPackageablePreceding.PrecedingProcedureWorkPackageableThingId == 0) ||
                (workPackageablePreceding.FollowingProcedureWorkPackageableThingId == 0))
                return false;
            CommandDefinition command = new CommandDefinition(@"insert into ProcedureWorkPackageableThingPreceding
                (PrecedingProcedureWorkPackageableThingId,FollowingProcedureWorkPackageableThingId,
                    ModifiedBy,ModifiedAt)
                values(@PrecedingProcedureWorkPackageableThingId,@FollowingProcedureWorkPackageableThingId,
                    @ModifiedBy,@ModifiedAt)",
                new
                {
                    PrecedingProcedureWorkPackageableThingId = workPackageablePreceding.PrecedingProcedureWorkPackageableThingId,
                    FollowingProcedureWorkPackageableThingId = workPackageablePreceding.FollowingProcedureWorkPackageableThingId,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow
                });
            return Execute(command);
        }

        [HttpDelete]
        [ContentNegotiation("workpackagepreceding/{id:int}", ContentType.JSON)]
        public bool Delete(int id)
        {
            CommandDefinition command = new CommandDefinition(@"update ProcedureWorkPackageableThingPreceding
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