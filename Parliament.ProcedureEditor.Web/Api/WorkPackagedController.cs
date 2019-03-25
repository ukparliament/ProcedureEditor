using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
	                coalesce(si.ProcedureStatutoryInstrumentName, nsi.ProcedureProposedNegativeStatutoryInstrumentName, t.ProcedureTreatyName) as WorkPackagedThingName,
	                coalesce(si.StatutoryInstrumentNumber, t.TreatyNumber) as StatutoryInstrumentNumber,
                    coalesce(si.StatutoryInstrumentNumberPrefix, t.TreatyPrefix) as StatutoryInstrumentNumberPrefix,
                    si.StatutoryInstrumentNumberYear, si.MadeDate, t.LeadGovernmentOrganisationTripleStoreId,
                    coalesce(si.ComingIntoForceNote, t.ComingIntoForceNote) as ComingIntoForceNote,
                    coalesce(si.ComingIntoForceDate, t.ComingIntoForceDate) as ComingIntoForceDate
                from ProcedureWorkPackagedThing wp
                left join ProcedureStatutoryInstrument si on si.Id=wp.Id
                left join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=wp.Id
                left join ProcedureTreaty t on t.Id=wp.Id
                join [Procedure] p on p.Id=wp.ProcedureId
                where wp.ProcedureId=@ProcedureId;
                select t.Id, sm.TripleStoreId, sm.Citation, 
	                case when cs.Id is null then 
		                case when eus.Id is null then 
			                case when ms.Id is null then null
	                else 3 end else 2 end else 1 end as SeriesMembershipId	
                from ProcedureTreaty t
                join ProcedureWorkPackagedThing wp on wp.Id=t.Id
                left join ProcerdureCountrySeriesMembership cs on cs.ProcedureTreatyId=t.Id
                left join ProcerdureEuropeanUnionSeriesMembership eus on eus.ProcedureTreatyId=t.Id
                left join ProcerdureMiscellaneousSeriesMembership ms on ms.ProcedureTreatyId=t.Id
                left join ProcedureSeriesMembership sm on sm.Id=coalesce(cs.Id,eus.Id,ms.Id)
                where wp.ProcedureId=@ProcedureId
                union
                select t.Id, sm.TripleStoreId, sm.Citation, 4 as SeriesMembershipId
                from ProcedureTreaty t
                join ProcedureWorkPackagedThing wp on wp.Id=t.Id
                join ProcerdureTreatySeriesMembership ts on ts.ProcedureTreatyId=t.Id
                join ProcedureSeriesMembership sm on sm.Id=ts.Id
                where wp.ProcedureId=@ProcedureId",
                new { ProcedureId = procedureId });


            Tuple<List<WorkPackaged>, List<SeriesMembership>> tuple = GetItems<WorkPackaged, SeriesMembership>(command);

            tuple.Item1
                .ForEach(wp =>
                    wp.SeriesMemberships = tuple.Item2
                        .Where(t => t.Id == wp.Id)
                        ?.ToArray()
                );

            return tuple.Item1;
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
	                coalesce(si.ProcedureStatutoryInstrumentName, nsi.ProcedureProposedNegativeStatutoryInstrumentName, t.ProcedureTreatyName) as WorkPackagedThingName,
	                coalesce(si.StatutoryInstrumentNumber, t.TreatyNumber) as StatutoryInstrumentNumber,
                    coalesce(si.StatutoryInstrumentNumberPrefix, t.TreatyPrefix) as StatutoryInstrumentNumberPrefix,
                    si.StatutoryInstrumentNumberYear, si.MadeDate, t.LeadGovernmentOrganisationTripleStoreId,
                    coalesce(si.ComingIntoForceNote, t.ComingIntoForceNote) as ComingIntoForceNote,
                    coalesce(si.ComingIntoForceDate, t.ComingIntoForceDate) as ComingIntoForceDate
                from ProcedureWorkPackagedThing wp
                left join ProcedureStatutoryInstrument si on si.Id=wp.Id
                left join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=wp.Id
                left join ProcedureTreaty t on t.Id=wp.Id
                join [Procedure] p on p.Id=wp.ProcedureId;
                select t.Id, sm.TripleStoreId, sm.Citation, 
	                case when cs.Id is null then 
		                case when eus.Id is null then 
			                case when ms.Id is null then null
	                else 3 end else 2 end else 1 end as SeriesMembershipId	
                from ProcedureTreaty t
                left join ProcerdureCountrySeriesMembership cs on cs.ProcedureTreatyId=t.Id
                left join ProcerdureEuropeanUnionSeriesMembership eus on eus.ProcedureTreatyId=t.Id
                left join ProcerdureMiscellaneousSeriesMembership ms on ms.ProcedureTreatyId=t.Id
                left join ProcedureSeriesMembership sm on sm.Id=coalesce(cs.Id,eus.Id,ms.Id)
                union
                select t.Id, sm.TripleStoreId, sm.Citation, 4 as SeriesMembershipId
                from ProcedureTreaty t
                join ProcerdureTreatySeriesMembership ts on ts.ProcedureTreatyId=t.Id
                join ProcedureSeriesMembership sm on sm.Id=ts.Id");

            Tuple<List<WorkPackaged>, List<SeriesMembership>> tuple = GetItems<WorkPackaged, SeriesMembership>(command);

            tuple.Item1
                .ForEach(wp =>
                    wp.SeriesMemberships = tuple.Item2
                        .Where(t => t.Id == wp.Id)
                        ?.ToArray()
                );

            return tuple.Item1;
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
	                coalesce(si.ProcedureStatutoryInstrumentName, nsi.ProcedureProposedNegativeStatutoryInstrumentName, t.ProcedureTreatyName) as WorkPackagedThingName,
	                coalesce(si.StatutoryInstrumentNumber, t.TreatyNumber) as StatutoryInstrumentNumber,
                    coalesce(si.StatutoryInstrumentNumberPrefix, t.TreatyPrefix) as StatutoryInstrumentNumberPrefix,
                    si.StatutoryInstrumentNumberYear, si.MadeDate, t.LeadGovernmentOrganisationTripleStoreId,
                    coalesce(si.ComingIntoForceNote, t.ComingIntoForceNote) as ComingIntoForceNote,
                    coalesce(si.ComingIntoForceDate, t.ComingIntoForceDate) as ComingIntoForceDate
                from ProcedureWorkPackagedThing wp
                left join ProcedureStatutoryInstrument si on si.Id=wp.Id
                left join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=wp.Id
                left join ProcedureTreaty t on t.Id=wp.Id
                join [Procedure] p on p.Id=wp.ProcedureId
                where wp.Id=@Id;
                select t.Id, sm.TripleStoreId, sm.Citation, 
	                case when cs.Id is null then 
		                case when eus.Id is null then 
			                case when ms.Id is null then null
	                else 3 end else 2 end else 1 end as SeriesMembershipId	
                from ProcedureTreaty t
                left join ProcerdureCountrySeriesMembership cs on cs.ProcedureTreatyId=t.Id
                left join ProcerdureEuropeanUnionSeriesMembership eus on eus.ProcedureTreatyId=t.Id
                left join ProcerdureMiscellaneousSeriesMembership ms on ms.ProcedureTreatyId=t.Id
                left join ProcedureSeriesMembership sm on sm.Id=coalesce(cs.Id,eus.Id,ms.Id)
                where t.Id=@Id
                union
                select t.Id, sm.TripleStoreId, sm.Citation, 4 as SeriesMembershipId
                from ProcedureTreaty t
                join ProcerdureTreatySeriesMembership ts on ts.ProcedureTreatyId=t.Id
                join ProcedureSeriesMembership sm on sm.Id=ts.Id
                where t.Id=@Id",
                new { Id = id });

            Tuple<WorkPackaged, List<SeriesMembership>> tuple = GetItem<WorkPackaged, SeriesMembership>(command);

            tuple.Item1.SeriesMemberships = tuple.Item2?.ToArray();

            return tuple.Item1;
        }

        [HttpGet]
        [ContentNegotiation("workpackage/{tripleStoreId}", ContentType.JSON)]
        public WorkPackaged GetByTripleStore(string tripleStoreId)
        {
            CommandDefinition command = new CommandDefinition(@"select wp.Id, wp.TripleStoreId, wp.WebLink, wp.ProcedureWorkPackageTripleStoreId,
	                wp.ProcedureId, p.ProcedureName,
	                (select max(b.BusinessItemDate) from ProcedureBusinessItem b where b.ProcedureWorkPackageId=wp.Id) as MostRecentBusinessItemDate,
	                coalesce(si.ProcedureStatutoryInstrumentName, nsi.ProcedureProposedNegativeStatutoryInstrumentName, t.ProcedureTreatyName) as WorkPackagedThingName,
	                coalesce(si.StatutoryInstrumentNumber, t.TreatyNumber) as StatutoryInstrumentNumber,
                    coalesce(si.StatutoryInstrumentNumberPrefix, t.TreatyPrefix) as StatutoryInstrumentNumberPrefix,
                    si.StatutoryInstrumentNumberYear, si.MadeDate, t.LeadGovernmentOrganisationTripleStoreId,
                    coalesce(si.ComingIntoForceNote, t.ComingIntoForceNote) as ComingIntoForceNote,
                    coalesce(si.ComingIntoForceDate, t.ComingIntoForceDate) as ComingIntoForceDate
                from ProcedureWorkPackagedThing wp
                left join ProcedureStatutoryInstrument si on si.Id=wp.Id
                left join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=wp.Id
                left join ProcedureTreaty t on t.Id=wp.Id
                join [Procedure] p on p.Id=wp.ProcedureId
                where wp.TripleStoreId=@TripleStoreId;
                select t.Id, sm.TripleStoreId, sm.Citation, 
	                case when cs.Id is null then 
		                case when eus.Id is null then 
			                case when ms.Id is null then null
	                else 3 end else 2 end else 1 end as SeriesMembershipId	
                from ProcedureTreaty t
                join ProcedureWorkPackagedThing wp on wp.Id=t.Id
                left join ProcerdureCountrySeriesMembership cs on cs.ProcedureTreatyId=t.Id
                left join ProcerdureEuropeanUnionSeriesMembership eus on eus.ProcedureTreatyId=t.Id
                left join ProcerdureMiscellaneousSeriesMembership ms on ms.ProcedureTreatyId=t.Id
                left join ProcedureSeriesMembership sm on sm.Id=coalesce(cs.Id,eus.Id,ms.Id)
                where wp.TripleStoreId=@TripleStoreId
                union
                select t.Id, sm.TripleStoreId, sm.Citation, 4 as SeriesMembershipId
                from ProcedureTreaty t
                join ProcedureWorkPackagedThing wp on wp.Id=t.Id
                join ProcerdureTreatySeriesMembership ts on ts.ProcedureTreatyId=t.Id
                join ProcedureSeriesMembership sm on sm.Id=ts.Id
                where wp.TripleStoreId=@TripleStoreId",
                new { TripleStoreId = tripleStoreId });

            Tuple<WorkPackaged, List<SeriesMembership>> tuple = GetItem<WorkPackaged, SeriesMembership>(command);

            tuple.Item1.SeriesMemberships = tuple.Item2?.ToArray();

            return tuple.Item1;
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
                (workPackaged.ProcedureId == 0) ||
                ((workPackaged.WorkPackagedKind == WorkPackagedType.Treaty) &&
                (workPackaged.NonTreatySeriesMembership == null)))
                return false;
            string seriesMembershipTripleStoreId = null;
            string seriesMembershipTreatyTripleStoreId = null;
            if (workPackaged.WorkPackagedKind == WorkPackagedType.Treaty)
            {
                if (string.IsNullOrWhiteSpace(workPackaged.NonTreatySeriesMembership.TripleStoreId))
                    seriesMembershipTripleStoreId = GetTripleStoreId();
                else
                    seriesMembershipTripleStoreId = workPackaged.NonTreatySeriesMembership.TripleStoreId;
                if (workPackaged.TreatySeriesMembership != null)
                {
                    if (string.IsNullOrWhiteSpace(workPackaged.TreatySeriesMembership.TripleStoreId))
                        seriesMembershipTreatyTripleStoreId = GetTripleStoreId();
                    else
                        seriesMembershipTreatyTripleStoreId = workPackaged.TreatySeriesMembership.TripleStoreId;
                    if (string.IsNullOrWhiteSpace(seriesMembershipTreatyTripleStoreId))
                        return false;
                }
                if (string.IsNullOrWhiteSpace(seriesMembershipTripleStoreId))
                    return false;
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@WorkPackagedId", id);
            parameters.Add("@WebLink", workPackaged.WebLink);
            parameters.Add("@ProcedureId", workPackaged.ProcedureId);
            parameters.Add("@WorkPackagedKind", (int)workPackaged.WorkPackagedKind);
            parameters.Add("@WorkPackagedThingName", workPackaged.WorkPackagedThingName);
            parameters.Add("@StatutoryInstrumentNumber", workPackaged.StatutoryInstrumentNumber);
            parameters.Add("@StatutoryInstrumentNumberPrefix", workPackaged.StatutoryInstrumentNumberPrefix);
            parameters.Add("@ComingIntoForceNote", workPackaged.ComingIntoForceNote);
            parameters.Add("@ComingIntoForceDate", workPackaged.ComingIntoForceDate);
            parameters.Add("@MadeDate", workPackaged.MadeDate);
            parameters.Add("@LeadGovernmentOrganisationTripleStoreId", workPackaged.LeadGovernmentOrganisationTripleStoreId);
            parameters.Add("@SeriesCitation", workPackaged.NonTreatySeriesMembership?.Citation);
            parameters.Add("@SeriesMembershipTripleStoreId", seriesMembershipTripleStoreId);
            parameters.Add("@SeriesTreatyCitation", workPackaged?.TreatySeriesMembership?.Citation);
            parameters.Add("@SeriesMembershipTreatyTripleStoreId", seriesMembershipTreatyTripleStoreId);
            parameters.Add("@IsCountrySeriesMembership", workPackaged.NonTreatySeriesMembership?.SeriesMembershipKind == SeriesMembershipType.Country);
            parameters.Add("@IsEuropeanUnionSeriesMembership", workPackaged.NonTreatySeriesMembership?.SeriesMembershipKind == SeriesMembershipType.EuropeanUnion);
            parameters.Add("@IsMiscellaneousSeriesMembership", workPackaged.NonTreatySeriesMembership?.SeriesMembershipKind == SeriesMembershipType.Miscellaneous);
            parameters.Add("@IsTreatySeriesMembership", workPackaged.TreatySeriesMembership != null);
            parameters.Add("@ModifiedBy", EMail);
            CommandDefinition command = new CommandDefinition("UpdateWorkPackaged",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);
            return Execute(command);
        }

        [HttpPost]
        [ContentNegotiation("workpackage", ContentType.JSON)]
        public bool Post([FromBody]WorkPackaged workPackaged)
        {
            if ((workPackaged == null) ||
                (string.IsNullOrWhiteSpace(workPackaged.WorkPackagedThingName)) ||
                (workPackaged.ProcedureId == 0) ||
                ((workPackaged.WorkPackagedKind == WorkPackagedType.Treaty) &&
                (workPackaged.NonTreatySeriesMembership == null)))
                return false;
            string tripleStoreId = GetTripleStoreId();
            string workPackageTripleStoreId = GetTripleStoreId();
            string seriesMembershipTripleStoreId = null;
            string seriesMembershipTreatyTripleStoreId = null;
            if (workPackaged.WorkPackagedKind == WorkPackagedType.Treaty)
            {
                seriesMembershipTripleStoreId = GetTripleStoreId();
                seriesMembershipTreatyTripleStoreId = GetTripleStoreId();
                if ((string.IsNullOrWhiteSpace(seriesMembershipTripleStoreId)) ||
                    (string.IsNullOrWhiteSpace(seriesMembershipTreatyTripleStoreId)))
                    return false;
            }
            if ((string.IsNullOrWhiteSpace(tripleStoreId)) ||
                (string.IsNullOrWhiteSpace(workPackageTripleStoreId)))
                return false;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TripleStoreId", tripleStoreId);
            parameters.Add("@WebLink", workPackaged.WebLink);
            parameters.Add("@ProcedureWorkPackageTripleStoreId", workPackageTripleStoreId);
            parameters.Add("@ProcedureId", workPackaged.ProcedureId);
            parameters.Add("@WorkPackagedKind", (int)workPackaged.WorkPackagedKind);
            parameters.Add("@WorkPackagedThingName", workPackaged.WorkPackagedThingName);
            parameters.Add("@StatutoryInstrumentNumber", workPackaged.StatutoryInstrumentNumber);
            parameters.Add("@StatutoryInstrumentNumberPrefix", workPackaged.StatutoryInstrumentNumberPrefix);
            parameters.Add("@StatutoryInstrumentNumberYear", workPackaged.StatutoryInstrumentNumberYear);
            parameters.Add("@ComingIntoForceNote", workPackaged.ComingIntoForceNote);
            parameters.Add("@ComingIntoForceDate", workPackaged.ComingIntoForceDate);
            parameters.Add("@MadeDate", workPackaged.MadeDate);
            parameters.Add("@LeadGovernmentOrganisationTripleStoreId", workPackaged.LeadGovernmentOrganisationTripleStoreId);
            parameters.Add("@SeriesCitation", workPackaged.NonTreatySeriesMembership?.Citation);
            parameters.Add("@SeriesTreatyCitation", workPackaged?.TreatySeriesMembership?.Citation);
            parameters.Add("@SeriesMembershipTripleStoreId", seriesMembershipTripleStoreId);
            parameters.Add("@SeriesMembershipTreatyTripleStoreId", seriesMembershipTreatyTripleStoreId);
            parameters.Add("@IsCountrySeriesMembership", workPackaged.NonTreatySeriesMembership?.SeriesMembershipKind == SeriesMembershipType.Country);
            parameters.Add("@IsEuropeanUnionSeriesMembership", workPackaged.NonTreatySeriesMembership?.SeriesMembershipKind == SeriesMembershipType.EuropeanUnion);
            parameters.Add("@IsMiscellaneousSeriesMembership", workPackaged.NonTreatySeriesMembership?.SeriesMembershipKind == SeriesMembershipType.Miscellaneous);
            parameters.Add("@IsTreatySeriesMembership", workPackaged.TreatySeriesMembership != null);
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