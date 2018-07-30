using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{

    public class WorkPackageController : BaseApiController
    {
        [HttpGet]
        [ContentNegotiation("workpackage", ContentType.JSON)]
        public List<WorkPackageable> Search(string searchText)
        {
            CommandDefinition command = new CommandDefinition(@"select w.Id, w.TripleStoreId, w.ProcedureWorkPackageableThingName,
                w.StatutoryInstrumentNumber, w.StatutoryInstrumentNumberPrefix, w.StatutoryInstrumentNumberYear, w.ComingIntoForceNote,
                w.WebLink, w.ProcedureWorkPackageableThingTypeId, w.ComingIntoForceDate, w.TimeLimitForObjectionEndDate,
                w.ProcedureWorkPackageTripleStoreId, w.ProcedureId, p.ProcedureName,
                (select max(b.BusinessItemDate) from ProcedureBusinessItem b where b.ProcedureWorkPackageId=w.Id and b.IsDeleted=0) as MostRecentBusinessItemDate
                from ProcedureWorkPackageableThing w
                join [Procedure] p on p.Id=w.ProcedureId
                where w.IsDeleted=0 and ((w.ProcedureWorkPackageableThingName like @SearchText) or (upper(w.TripleStoreId)=@TripleStoreId))",
                new { SearchText = $"%{searchText}%", TripleStoreId = searchText.ToUpper() });
            return GetItems<WorkPackageable>(command);
        }

        [HttpGet]
        [ContentNegotiation("workpackage", ContentType.JSON)]
        public List<WorkPackageable> Search(int procedureId)
        {
            CommandDefinition command = new CommandDefinition(@"select w.Id, w.TripleStoreId, w.ProcedureWorkPackageableThingName,
                w.StatutoryInstrumentNumber, w.StatutoryInstrumentNumberPrefix, w.StatutoryInstrumentNumberYear, w.ComingIntoForceNote,
                w.WebLink, w.ProcedureWorkPackageableThingTypeId, w.ComingIntoForceDate, w.TimeLimitForObjectionEndDate,
                w.ProcedureWorkPackageTripleStoreId, w.ProcedureId, p.ProcedureName,
                (select max(b.BusinessItemDate) from ProcedureBusinessItem b where b.ProcedureWorkPackageId=w.Id and b.IsDeleted=0) as MostRecentBusinessItemDate
                from ProcedureWorkPackageableThing w
                join [Procedure] p on p.Id=w.ProcedureId
                where w.IsDeleted=0 and w.ProcedureId=@ProcedureId",
                new { ProcedureId = procedureId });
            return GetItems<WorkPackageable>(command);
        }

        [HttpGet]
        [ContentNegotiation("workpackage", ContentType.HTML)]
        public IHttpActionResult GetView()
        {
            return RenderView("Index");
        }

        [HttpGet]
        [ContentNegotiation("workpackage", ContentType.JSON)]
        public List<WorkPackageable> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select w.Id, w.TripleStoreId, w.ProcedureWorkPackageableThingName,
                w.StatutoryInstrumentNumber, w.StatutoryInstrumentNumberPrefix, w.StatutoryInstrumentNumberYear, w.ComingIntoForceNote,
                w.WebLink, w.ProcedureWorkPackageableThingTypeId, w.ComingIntoForceDate, w.TimeLimitForObjectionEndDate,
                w.ProcedureWorkPackageTripleStoreId, w.ProcedureId, p.ProcedureName,
                (select max(b.BusinessItemDate) from ProcedureBusinessItem b where b.ProcedureWorkPackageId=w.Id and b.IsDeleted=0) as MostRecentBusinessItemDate
                from ProcedureWorkPackageableThing w
                join [Procedure] p on p.Id=w.ProcedureId
                where w.IsDeleted=0");
            return GetItems<WorkPackageable>(command);
        }

        [HttpGet]
        [ContentNegotiation("workpackage/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetView(int id)
        {
            return RenderView("View", id);
        }

        [HttpGet]
        [ContentNegotiation("workpackage/{id:int}", ContentType.JSON)]
        public WorkPackageable Get(int id)
        {
            CommandDefinition command = new CommandDefinition(@"select w.Id, w.TripleStoreId, w.ProcedureWorkPackageableThingName,
                w.StatutoryInstrumentNumber, w.StatutoryInstrumentNumberPrefix, w.StatutoryInstrumentNumberYear, w.ComingIntoForceNote,
                w.WebLink, w.ProcedureWorkPackageableThingTypeId, w.ComingIntoForceDate, w.TimeLimitForObjectionEndDate,
                w.ProcedureWorkPackageTripleStoreId, w.ProcedureId, p.ProcedureName,
                (select max(b.BusinessItemDate) from ProcedureBusinessItem b where b.ProcedureWorkPackageId=w.Id and b.IsDeleted=0) as MostRecentBusinessItemDate
                from ProcedureWorkPackageableThing w
                join [Procedure] p on p.Id=w.ProcedureId
                where w.Id=@Id",
                new { Id = id });
            return GetItem<WorkPackageable>(command);
        }

        [HttpGet]
        [ContentNegotiation("workpackage/edit/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetEdit(int id)
        {
            return RenderView("Edit", id);
        }

        [HttpGet]
        [ContentNegotiation("workpackage/add", ContentType.HTML)]
        public IHttpActionResult GetAdd()
        {
            return RenderView("Edit");
        }

        [HttpPut]
        [ContentNegotiation("workpackage/{id:int}", ContentType.JSON)]
        public bool Put(int id, [FromBody]WorkPackageable workPackageable)
        {
            if ((workPackageable == null) ||
                (string.IsNullOrWhiteSpace(workPackageable.ProcedureWorkPackageableThingName)) ||
                (workPackageable.ProcedureId == 0))
                return false;
            CommandDefinition command = new CommandDefinition(@"update ProcedureWorkPackageableThing
                set ProcedureWorkPackageableThingName=@ProcedureWorkPackageableThingName,
                    StatutoryInstrumentNumber=@StatutoryInstrumentNumber,
                    StatutoryInstrumentNumberPrefix=@StatutoryInstrumentNumberPrefix,
                    StatutoryInstrumentNumberYear=@StatutoryInstrumentNumberYear,
                    ComingIntoForceNote=@ComingIntoForceNote,
                    WebLink=@WebLink,
                    ProcedureWorkPackageableThingTypeId=@ProcedureWorkPackageableThingTypeId,
                    ComingIntoForceDate=@ComingIntoForceDate,
                    TimeLimitForObjectionEndDate=@TimeLimitForObjectionEndDate,
                    ProcedureId=@ProcedureId,
                    ModifiedBy=@ModifiedBy,
                    ModifiedAt=@ModifiedAt
                where Id=@Id",
                new
                {
                    ProcedureWorkPackageableThingName = workPackageable.ProcedureWorkPackageableThingName.Trim(),
                    StatutoryInstrumentNumber = workPackageable.StatutoryInstrumentNumber,
                    StatutoryInstrumentNumberPrefix = workPackageable.StatutoryInstrumentNumberPrefix,
                    StatutoryInstrumentNumberYear = workPackageable.StatutoryInstrumentNumberYear,
                    ComingIntoForceNote = workPackageable.ComingIntoForceNote,
                    WebLink = workPackageable.WebLink,
                    ProcedureWorkPackageableThingTypeId = workPackageable.ProcedureWorkPackageableThingTypeId,
                    ComingIntoForceDate = workPackageable.ComingIntoForceDate,
                    TimeLimitForObjectionEndDate = workPackageable.TimeLimitForObjectionEndDate,
                    ProcedureId = workPackageable.ProcedureId,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    Id = id
                });
            return Execute(command);
        }

        [HttpPost]
        [ContentNegotiation("workpackage", ContentType.JSON)]
        public bool Post([FromBody]WorkPackageable workPackageable)
        {
            if ((workPackageable == null) ||
                (string.IsNullOrWhiteSpace(workPackageable.ProcedureWorkPackageableThingName)) ||
                (workPackageable.ProcedureId == 0))
                return false;
            string tripleStoreId = GetTripleStoreId();
            string workPackageTripleStoreId = GetTripleStoreId();
            if ((string.IsNullOrWhiteSpace(tripleStoreId)) ||
                (string.IsNullOrWhiteSpace(workPackageTripleStoreId)))
                return false;
            CommandDefinition command = new CommandDefinition(@"insert into ProcedureWorkPackageableThing
                (ProcedureWorkPackageableThingName,StatutoryInstrumentNumber,StatutoryInstrumentNumberPrefix,
                    StatutoryInstrumentNumberYear,ComingIntoForceNote,WebLink,ProcedureWorkPackageableThingTypeId,
                    ComingIntoForceDate,TimeLimitForObjectionEndDate,ProcedureId,ModifiedBy,ModifiedAt,
                    TripleStoreId,ProcedureWorkPackageTripleStoreId)
                values(@ProcedureWorkPackageableThingName,@StatutoryInstrumentNumber,@StatutoryInstrumentNumberPrefix,
                    @StatutoryInstrumentNumberYear,@ComingIntoForceNote,@WebLink,@ProcedureWorkPackageableThingTypeId,
                    @ComingIntoForceDate,@TimeLimitForObjectionEndDate,@ProcedureId,@ModifiedBy,@ModifiedAt,
                    @TripleStoreId,@ProcedureWorkPackageTripleStoreId)",
                new
                {
                    ProcedureWorkPackageableThingName = workPackageable.ProcedureWorkPackageableThingName.Trim(),
                    StatutoryInstrumentNumber = workPackageable.StatutoryInstrumentNumber,
                    StatutoryInstrumentNumberPrefix = workPackageable.StatutoryInstrumentNumberPrefix,
                    StatutoryInstrumentNumberYear = workPackageable.StatutoryInstrumentNumberYear,
                    ComingIntoForceNote = workPackageable.ComingIntoForceNote,
                    WebLink = workPackageable.WebLink,
                    ProcedureWorkPackageableThingTypeId = workPackageable.ProcedureWorkPackageableThingTypeId,
                    ComingIntoForceDate = workPackageable.ComingIntoForceDate,
                    TimeLimitForObjectionEndDate = workPackageable.TimeLimitForObjectionEndDate,
                    ProcedureId = workPackageable.ProcedureId,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    TripleStoreId = tripleStoreId,
                    ProcedureWorkPackageTripleStoreId = workPackageTripleStoreId
                });
            return Execute(command);
        }

        [HttpDelete]
        [ContentNegotiation("workpackage/{id:int}", ContentType.JSON)]
        public bool Delete(int id)
        {
            CommandDefinition command = new CommandDefinition(@"update ProcedureWorkPackageableThing
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