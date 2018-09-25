using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{

    public class WorkPackagedController : BaseApiController
    {
        [HttpGet]
        [ContentNegotiation("workpackage", ContentType.JSON)]
        public List<WorkPackaged> Search(int procedureId)
        {
            CommandDefinition command = new CommandDefinition(@"select wp.Id, wp.TripleStoreId, wp.WebLink, wp.ProcedureWorkPackageTripleStoreId,
	                wp.ProcedureId, p.ProcedureName,
	                (select max(b.BusinessItemDate) from ProcedureBusinessItem b where b.ProcedureWorkPackageId=wp.Id) as MostRecentBusinessItemDate,
	                coalesce(si.ProcedureStatutoryInstrumentName, nsi.ProcedureProposedNegativeStatutoryInstrumentName) as WorkPackagedThingName,
	                si.StatutoryInstrumentNumber, si.StatutoryInstrumentNumberPrefix, si.StatutoryInstrumentNumberYear,
	                si.ComingIntoForceNote, si.ComingIntoForceDate, si.MadeDate
                from ProcedureWorkPackagedThing wp
                left join ProcedureStatutoryInstrument si on si.Id=wp.Id
                left join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=wp.Id
                join [Procedure] p on p.Id=wp.ProcedureId
                where wp.ProcedureId=@ProcedureId",
                new { ProcedureId = procedureId });
            return GetItems<WorkPackaged>(command);
        }

        [HttpGet]
        [ContentNegotiation("workpackage", ContentType.HTML)]
        public IHttpActionResult GetView()
        {
            return RenderView("Index");
        }

        [HttpGet]
        [ContentNegotiation("workpackage", ContentType.JSON)]
        public List<WorkPackaged> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select wp.Id, wp.TripleStoreId, wp.WebLink, wp.ProcedureWorkPackageTripleStoreId,
	                wp.ProcedureId, p.ProcedureName,
	                (select max(b.BusinessItemDate) from ProcedureBusinessItem b where b.ProcedureWorkPackageId=wp.Id) as MostRecentBusinessItemDate,
	                coalesce(si.ProcedureStatutoryInstrumentName, nsi.ProcedureProposedNegativeStatutoryInstrumentName) as WorkPackagedThingName,
	                si.StatutoryInstrumentNumber, si.StatutoryInstrumentNumberPrefix, si.StatutoryInstrumentNumberYear,
	                si.ComingIntoForceNote, si.ComingIntoForceDate, si.MadeDate
                from ProcedureWorkPackagedThing wp
                left join ProcedureStatutoryInstrument si on si.Id=wp.Id
                left join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=wp.Id
                join [Procedure] p on p.Id=wp.ProcedureId");
            return GetItems<WorkPackaged>(command);
        }

        [HttpGet]
        [ContentNegotiation("workpackage/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetView(int id)
        {
            return RenderView("View", id);
        }

        [HttpGet]
        [ContentNegotiation("workpackage/{id:int}", ContentType.JSON)]
        public WorkPackaged Get(int id)
        {
            CommandDefinition command = new CommandDefinition(@"select wp.Id, wp.TripleStoreId, wp.WebLink, wp.ProcedureWorkPackageTripleStoreId,
	                wp.ProcedureId, p.ProcedureName,
	                (select max(b.BusinessItemDate) from ProcedureBusinessItem b where b.ProcedureWorkPackageId=wp.Id) as MostRecentBusinessItemDate,
	                coalesce(si.ProcedureStatutoryInstrumentName, nsi.ProcedureProposedNegativeStatutoryInstrumentName) as WorkPackagedThingName,
	                si.StatutoryInstrumentNumber, si.StatutoryInstrumentNumberPrefix, si.StatutoryInstrumentNumberYear,
	                si.ComingIntoForceNote, si.ComingIntoForceDate, si.MadeDate
                from ProcedureWorkPackagedThing wp
                left join ProcedureStatutoryInstrument si on si.Id=wp.Id
                left join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=wp.Id
                join [Procedure] p on p.Id=wp.ProcedureId
                where wp.Id=@Id",
                new { Id = id });
            return GetItem<WorkPackaged>(command);
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
        public bool Put(int id, [FromBody]WorkPackaged workPackaged)
        {
            if ((workPackaged == null) ||
                (string.IsNullOrWhiteSpace(workPackaged.WorkPackagedThingName)) ||
                (workPackaged.ProcedureId == 0))
                return false;
            List<CommandDefinition> updates = new List<CommandDefinition>();
            updates.Add(new CommandDefinition("delete from ProcedureStatutoryInstrument where Id=@Id", new { Id = id }));
            updates.Add(new CommandDefinition("delete from ProcedureProposedNegativeStatutoryInstrument where Id=@Id", new { Id = id }));
            updates.Add(new CommandDefinition(@"update ProcedureWorkPackagedThing
                set WebLink=@WebLink,
                    ProcedureId=@ProcedureId,
                    ModifiedBy=@ModifiedBy,
                    ModifiedAt=@ModifiedAt
                where Id=@Id",
                new
                {
                    WebLink = workPackaged.WebLink,
                    ProcedureId = workPackaged.ProcedureId,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    Id = id
                }));
            if (workPackaged.IsStatutoryInstrument)
                updates.Add(new CommandDefinition(@"insert into ProcedureStatutoryInstrument
                    (Id, ProcedureStatutoryInstrumentName, StatutoryInstrumentNumber, 
                    StatutoryInstrumentNumberPrefix, StatutoryInstrumentNumberYear,
	                ComingIntoForceNote, ComingIntoForceDate, MadeDate)
                    values (@Id, @ProcedureStatutoryInstrumentName, @StatutoryInstrumentNumber, 
                    @StatutoryInstrumentNumberPrefix, @StatutoryInstrumentNumberYear,
	                @ComingIntoForceNote, @ComingIntoForceDate, @MadeDate)",
                    new
                    {
                        Id = id,
                        ProcedureStatutoryInstrumentName = workPackaged.WorkPackagedThingName.Trim(),
                        StatutoryInstrumentNumber = workPackaged.StatutoryInstrumentNumber,
                        StatutoryInstrumentNumberPrefix = workPackaged.StatutoryInstrumentNumberPrefix,
                        StatutoryInstrumentNumberYear = workPackaged.StatutoryInstrumentNumberYear,
                        ComingIntoForceNote = workPackaged.ComingIntoForceNote,
                        ComingIntoForceDate = workPackaged.ComingIntoForceDate,
                        MadeDate = workPackaged.MadeDate
                    }));
            else
                updates.Add(new CommandDefinition(@"insert into ProcedureProposedNegativeStatutoryInstrument
                    (Id, ProcedureProposedNegativeStatutoryInstrumentName)
                    values (@Id, @ProcedureProposedNegativeStatutoryInstrumentName)",
                    new
                    {
                        Id = id,
                        ProcedureProposedNegativeStatutoryInstrumentName = workPackaged.WorkPackagedThingName.Trim(),
                    }));
            return Execute(updates.ToArray());
        }

        [HttpPost]
        [ContentNegotiation("workpackage", ContentType.JSON)]
        public bool Post([FromBody]WorkPackaged workPackaged)
        {
            if ((workPackaged == null) ||
                (string.IsNullOrWhiteSpace(workPackaged.WorkPackagedThingName)) ||
                (workPackaged.ProcedureId == 0))
                return false;
            string tripleStoreId = GetTripleStoreId();
            string workPackageTripleStoreId = GetTripleStoreId();
            if ((string.IsNullOrWhiteSpace(tripleStoreId)) ||
                (string.IsNullOrWhiteSpace(workPackageTripleStoreId)))
                return false;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TripleStoreId", tripleStoreId);
            parameters.Add("@WebLink", workPackaged.WebLink);
            parameters.Add("@ProcedureWorkPackageTripleStoreId", workPackageTripleStoreId);
            parameters.Add("@ProcedureId", workPackaged.ProcedureId);
            parameters.Add("@IsStatutoryInstrument", workPackaged.IsStatutoryInstrument);
            parameters.Add("@WorkPackagedThingName", workPackaged.WorkPackagedThingName);
            parameters.Add("@StatutoryInstrumentNumber", workPackaged.StatutoryInstrumentNumber);
            parameters.Add("@StatutoryInstrumentNumberPrefix", workPackaged.StatutoryInstrumentNumberPrefix);
            parameters.Add("@StatutoryInstrumentNumberYear", workPackaged.StatutoryInstrumentNumberYear);
            parameters.Add("@ComingIntoForceNote", workPackaged.ComingIntoForceNote);
            parameters.Add("@ComingIntoForceDate", workPackaged.ComingIntoForceDate);
            parameters.Add("@MadeDate", workPackaged.MadeDate);
            parameters.Add("@ModifiedBy", EMail);
            CommandDefinition command = new CommandDefinition("CreateWorkPackaged",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);
            return Execute(command);
        }

        [HttpDelete]
        [ContentNegotiation("workpackage/{id:int}", ContentType.JSON)]
        public bool Delete(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ModifiedBy", EMail);
            parameters.Add("@WorkPackagedId", id);
            parameters.Add("@IsSuccess", dbType: System.Data.DbType.Boolean, direction: System.Data.ParameterDirection.Output);
            CommandDefinition command = new CommandDefinition("DeleteWorkPackaged",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);
            if (Execute(command))
                return parameters.Get<bool>("@IsSuccess");
            else
                return false;
        }
    }

}