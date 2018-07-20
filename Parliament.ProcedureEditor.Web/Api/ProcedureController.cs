using Dapper;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{
    public class ProcedureController : BaseApiController
    {
        public List<Procedure> Get()
        {
            CommandDefinition command = new CommandDefinition("select Id, TripleStoreId, ProcedureName from [Procedure] where IsDeleted=0");
            return GetItems<Procedure>(command);
        }

        public Procedure Get(int id)
        {
            CommandDefinition command = new CommandDefinition("select Id, TripleStoreId, ProcedureName from [Procedure] where Id=@Id", new { Id = id });
            return GetItem<Procedure>(command);
        }

        /*public bool CanDelete(int id)
        {
            CommandDefinition command = new CommandDefinition(@"select top 1 Id from ProcedureRoute where IsDeleted=0 and ProcedureId=@Id
	            union
	            select top 1 Id from ProcedureWorkPackageableThing where IsDeleted=0 and ProcedureId=@Id",
                new { Id = id });
            List<int> result = GetItems<int>(command);

            return result.Count == 0;
        }*/

        public bool Put(int id, [FromBody]Procedure procedure)
        {
            if ((procedure == null) || (string.IsNullOrWhiteSpace(procedure.ProcedureName)))
                return false;
            CommandDefinition command = new CommandDefinition(@"update [Procedure]
                set ProcedureName=@ProcedureName,
                    ModifiedBy=@ModifiedBy,
                    ModifiedAt=@ModifiedAt
                where Id=@Id",
                new
                {
                    ProcedureName = procedure.ProcedureName.Trim(),
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    Id = id
                });
            return Execute(command);
        }

        public bool Post(Procedure procedure)
        {
            if ((procedure == null) || (string.IsNullOrWhiteSpace(procedure.ProcedureName)))
                return false;
            string tripleStoreId = GetTripleStoreId();
            if (string.IsNullOrWhiteSpace(tripleStoreId))
                return false;
            CommandDefinition command = new CommandDefinition(@"insert into [Procedure] (TripleStoreId, ProcedureName, ModifiedBy, ModifiedAt)
                values (@TripleStoreId, @ProcedureName, @ModifiedBy, @ModifiedAt)",
                new
                {
                    TripleStoreId = tripleStoreId,
                    ProcedureName = procedure.ProcedureName.Trim(),
                    ModifiedBy = EMail,
                    ModifiedAt = DateTime.UtcNow
                });
            return Execute(command);
        }

        public bool Delete(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ModifiedBy", EMail);
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