using Dapper;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{

    public class WorkPackageablePrecedingController : BaseApiController
    {
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

        public bool Delete(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ModifiedBy", EMail);
            parameters.Add("@WorkPackageablePrecedingId", id);
            parameters.Add("@IsSuccess", dbType: System.Data.DbType.Boolean, direction: System.Data.ParameterDirection.Output);
            CommandDefinition command = new CommandDefinition("DeleteWorkPackageablePreceding",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);
            if (Execute(command))
                return parameters.Get<bool>("@IsSuccess");
            else
                return false;
        }
    }

}