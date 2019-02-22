﻿using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{
    public class SolrFeedController : BaseApiController
    {
        [HttpGet]
        [ContentNegotiation("solrfeed/treaty", ContentType.HTML)]
        public IHttpActionResult GetViewTreaty()
        {
            return RenderView("Treaty/Index");
        }

        [HttpGet]
        [ContentNegotiation("solrfeed/statutoryinstrument", ContentType.HTML)]
        public IHttpActionResult GetViewStatutoryInstrument()
        {
            return RenderView("StatutoryInstrument/Index");
        }

        [HttpGet]
        [ContentNegotiation("solrfeed/businessitem", ContentType.HTML)]
        public IHttpActionResult GetViewBusinessItem()
        {
            return RenderView("BusinessItem/Index");
        }

        [HttpGet]
        [ContentNegotiation("solrfeed/treaty", ContentType.JSON)]
        public List<SolrTreaty> GetTreaties()
        {
            CommandDefinition command = new CommandDefinition(@"select s.Id, s.TripleStoreId, s.Title,
                s.WebUrl, s.Prefix, s.Number, s.Series as Citation from SolrTreatyData s
                where s.TripleStoreId is null and s.IsDeleted=0");
            return GetItems<SolrTreaty>(command);
        }

        [HttpGet]
        [ContentNegotiation("solrfeed/statutoryinstrument", ContentType.JSON)]
        public List<SolrStatutoryInstrument> GetStatutoryInstruments()
        {
            CommandDefinition command = new CommandDefinition(@"select s.Id, s.TripleStoreId, s.Title,
                s.SIPrefix, s.SINumber, s.WebUrl, s.ComingIntoForceNote,
                s.ComingIntoForceDate, s.MadeDate, s.LaidDate, s.SIProcedure, s.IsStatutoryInstrument
                from SolrStatutoryInstrumentData s
                where s.TripleStoreId is null and s.IsDeleted=0");
            return GetItems<SolrStatutoryInstrument>(command);
        }

        [HttpGet]
        [ContentNegotiation("solrfeed/businessitem", ContentType.JSON)]
        public List<SolrStatutoryInstrument> GetWorkPackaged()
        {
            CommandDefinition command = new CommandDefinition(@"select s.Id, s.TripleStoreId,
                coalesce(si.ProcedureStatutoryInstrumentName, nsi.ProcedureProposedNegativeStatutoryInstrumentName) as Title,
                si.StatutoryInstrumentNumberPrefix as SIPrefix, si.StatutoryInstrumentNumber as SINumber,
                s.WebUrl, si.ComingIntoForceNote,
                si.ComingIntoForceDate, s.MadeDate, s.LaidDate, p.ProcedureName as SIProcedure,
                case when si.ProcedureStatutoryInstrumentName is null then cast (0 as bit) else cast (1 as bit) end as IsStatutoryInstrument
                from SolrStatutoryInstrumentData s
                join ProcedureWorkPackagedThing wp on wp.TripleStoreId=s.TripleStoreId
                join [Procedure] p on p.Id=wp.ProcedureId
                left join ProcedureStatutoryInstrument si on si.Id=wp.Id
                left join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=wp.Id
                where s.TripleStoreId is not null and s.IsDeleted=0 and not exists (select 1 from SolrBusinessItem sbi where sbi.SolrStatutoryInstrumentDataId=s.Id)");
            return GetItems<SolrStatutoryInstrument>(command);
        }

        [HttpGet]
        [ContentNegotiation("solrfeed/treaty/{id:int}", ContentType.JSON)]
        public SolrTreaty GetTreaty(int id)
        {
            CommandDefinition command = new CommandDefinition(@"select s.Id, s.TripleStoreId, s.Title,
                s.WebUrl, s.Prefix, s.Number, s.Series as Citation from SolrTreatyData s
                where s.TripleStoreId is null and s.IsDeleted=0 and s.Id=@Id",
                new { Id = id });
            return GetItem<SolrTreaty>(command);
        }

        [HttpGet]
        [ContentNegotiation("solrfeed/statutoryinstrument/{id:int}", ContentType.JSON)]
        public SolrStatutoryInstrument GetStatutoryInstrument(int id)
        {
            CommandDefinition command = new CommandDefinition(@"select s.Id, s.TripleStoreId, s.Title,
                s.SIPrefix, s.SINumber, s.WebUrl, s.ComingIntoForceNote,
                s.ComingIntoForceDate, s.MadeDate, s.LaidDate, s.SIProcedure, s.IsStatutoryInstrument
                from SolrStatutoryInstrumentData s
                where s.TripleStoreId is null and s.IsDeleted=0 and s.Id=@Id",
                new { Id = id });
            return GetItem<SolrStatutoryInstrument>(command);
        }

        [HttpGet]
        [ContentNegotiation("solrfeed/businessitem/{id:int}", ContentType.JSON)]
        public SolrStatutoryInstrument GetWorkPackaged(int id)
        {
            CommandDefinition command = new CommandDefinition(@"select s.Id, s.TripleStoreId,
                coalesce(si.ProcedureStatutoryInstrumentName, nsi.ProcedureProposedNegativeStatutoryInstrumentName) as Title,
                si.StatutoryInstrumentNumberPrefix as SIPrefix, si.StatutoryInstrumentNumber as SINumber,
                s.WebUrl, si.ComingIntoForceNote,
                si.ComingIntoForceDate, s.MadeDate, s.LaidDate, p.ProcedureName as SIProcedure,
                case when si.ProcedureStatutoryInstrumentName is null then cast (0 as bit) else cast (1 as bit) end as IsStatutoryInstrument
                from SolrStatutoryInstrumentData s
                join ProcedureWorkPackagedThing wp on wp.TripleStoreId=s.TripleStoreId
                join [Procedure] p on p.Id=wp.ProcedureId
                left join ProcedureStatutoryInstrument si on si.Id=wp.Id
                left join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=wp.Id
                where s.TripleStoreId is not null and s.IsDeleted=0 and s.Id=@Id and not exists (select 1 from SolrBusinessItem sbi where sbi.SolrStatutoryInstrumentDataId=s.Id)",
                new { Id = id });
            return GetItem<SolrStatutoryInstrument>(command);
        }

        [HttpGet]
        [ContentNegotiation("solrfeed/treaty/edit/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetEditTreaty(int id)
        {
            return RenderView("Treaty/Edit", id);
        }

        [HttpGet]
        [ContentNegotiation("solrfeed/statutoryinstrument/edit/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetEditStatutoryInstrument(int id)
        {
            return RenderView("StatutoryInstrument/Edit", id);
        }

        [HttpGet]
        [ContentNegotiation("solrfeed/businessitem/edit/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetEditBusinessItem(int id)
        {
            return RenderView("BusinessItem/Edit", id);
        }

        [HttpPost]
        [ContentNegotiation("solrfeed/treaty/{id:int}", ContentType.JSON)]
        public bool PostTreaty(int id, [FromBody]WorkPackaged workPackaged)
        {
            if ((workPackaged == null) ||
                (string.IsNullOrWhiteSpace(workPackaged.WorkPackagedThingName)) ||
                (workPackaged.ProcedureId == 0) ||
                (workPackaged.NonTreatySeriesMembership == null))
                return false;
            string tripleStoreId = GetTripleStoreId();
            string workPackageTripleStoreId = GetTripleStoreId();
            string seriesMembershipTripleStoreId = GetTripleStoreId();
            string seriesMembershipTreatyTripleStoreId = GetTripleStoreId();
            if ((string.IsNullOrWhiteSpace(tripleStoreId)) ||
                (string.IsNullOrWhiteSpace(workPackageTripleStoreId))||
                (string.IsNullOrWhiteSpace(seriesMembershipTripleStoreId)) ||
                (string.IsNullOrWhiteSpace(seriesMembershipTreatyTripleStoreId)))
                return false;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TripleStoreId", tripleStoreId);
            parameters.Add("@WebLink", workPackaged.WebLink);
            parameters.Add("@ProcedureWorkPackageTripleStoreId", workPackageTripleStoreId);
            parameters.Add("@ProcedureId", workPackaged.ProcedureId);
            parameters.Add("@WorkPackagedKind", (int)WorkPackagedType.Treaty);
            parameters.Add("@WorkPackagedThingName", workPackaged.WorkPackagedThingName);
            parameters.Add("@StatutoryInstrumentNumber", workPackaged.StatutoryInstrumentNumber);
            parameters.Add("@StatutoryInstrumentNumberPrefix", workPackaged.StatutoryInstrumentNumberPrefix);
            parameters.Add("@ComingIntoForceNote", workPackaged.ComingIntoForceNote);
            parameters.Add("@ComingIntoForceDate", workPackaged.ComingIntoForceDate);
            parameters.Add("@LeadGovernmentOrganisationTripleStoreId", workPackaged.LeadGovernmentOrganisationTripleStoreId);
            parameters.Add("@SeriesCitation", workPackaged.NonTreatySeriesMembership.Citation);
            parameters.Add("@SeriesTreatyCitation", workPackaged?.TreatySeriesMembership?.Citation);
            parameters.Add("@SeriesMembershipTripleStoreId", seriesMembershipTripleStoreId);
            parameters.Add("@SeriesMembershipTreatyTripleStoreId", seriesMembershipTreatyTripleStoreId);
            parameters.Add("@IsCountrySeriesMembership", workPackaged.NonTreatySeriesMembership.SeriesMembershipKind == SeriesMembershipType.Country);
            parameters.Add("@IsEuropeanUnionSeriesMembership", workPackaged.NonTreatySeriesMembership.SeriesMembershipKind == SeriesMembershipType.EuropeanUnion);
            parameters.Add("@IsMiscellaneousSeriesMembership", workPackaged.NonTreatySeriesMembership.SeriesMembershipKind == SeriesMembershipType.Miscellaneous);
            parameters.Add("@IsTreatySeriesMembership", workPackaged.TreatySeriesMembership != null);
            parameters.Add("@SolarFeedId", id);
            parameters.Add("@ModifiedBy", EMail);
            CommandDefinition command = new CommandDefinition("CreateWorkPackaged",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);
            return Execute(command);
        }

        [HttpPost]
        [ContentNegotiation("solrfeed/statutoryinstrument/{id:int}", ContentType.JSON)]
        public bool PostStatutoryInstrument(int id, [FromBody]WorkPackaged workPackaged)
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
            parameters.Add("@WorkPackagedKind", (int)workPackaged.WorkPackagedKind);
            parameters.Add("@WorkPackagedThingName", workPackaged.WorkPackagedThingName);
            parameters.Add("@StatutoryInstrumentNumber", workPackaged.StatutoryInstrumentNumber);
            parameters.Add("@StatutoryInstrumentNumberPrefix", workPackaged.StatutoryInstrumentNumberPrefix);
            parameters.Add("@StatutoryInstrumentNumberYear", workPackaged.StatutoryInstrumentNumberYear);
            parameters.Add("@ComingIntoForceNote", workPackaged.ComingIntoForceNote);
            parameters.Add("@ComingIntoForceDate", workPackaged.ComingIntoForceDate);
            parameters.Add("@MadeDate", workPackaged.MadeDate);
            parameters.Add("@SolarFeedId", id);
            parameters.Add("@ModifiedBy", EMail);
            CommandDefinition command = new CommandDefinition("CreateWorkPackaged",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);
            return Execute(command);
        }

        [HttpPost]
        [ContentNegotiation("solrfeed/businessitem/{id:int}", ContentType.JSON)]
        public bool PostBusinessItem(int id, [FromBody]BusinessItemSolrEditModel[] businessItems)
        {
            if ((businessItems == null) ||
                (businessItems.Any() == false))
                return false;
            List<string> tripleStoreIds = new List<string>(); ;
            for (int i = 0; i < businessItems.Length; i++)
            {
                string tripleStoreId = GetTripleStoreId();
                if (string.IsNullOrWhiteSpace(tripleStoreId))
                    return false;
                else
                    tripleStoreIds.Add(tripleStoreId);
            }
            List<CommandDefinition> updates = new List<CommandDefinition>();
            BusinessItemController businessItemController = new BusinessItemController();
            for (int i = 0; i < businessItems.Length; i++)
            {
                string tripleStoreId = tripleStoreIds.Skip(i).Take(1).SingleOrDefault();
                BusinessItem bi = new BusinessItem()
                {
                    WebLink = businessItems[i].WebLink,
                    ProcedureWorkPackages = new int[] { businessItems[i].ProcedureWorkPackageId },
                    BusinessItemDate = businessItems[i].BusinessItemDate,
                    Steps = new int[] { businessItems[i].StepId }
                };
                updates.AddRange(businessItemController.GenerateCreateCommand(bi, new string[] { tripleStoreId }));
                if (businessItems[i].IsLaid)
                    updates.Add(new CommandDefinition(@"insert into ProcedureLaying
                        (ProcedureBusinessItemId,
                            ProcedureWorkPackagedId,
	                        LayingDate, LayingBodyId,
                            ModifiedBy,ModifiedAt)
                        values((select Id from ProcedureBusinessItem where TripleStoreId=@TripleStoreId), 
                            @ProcedureWorkPackagedId,
	                        @LayingDate, @LayingBodyId,
                            @ModifiedBy,@ModifiedAt)",
                    new
                    {
                        TripleStoreId = tripleStoreId,
                        ProcedureWorkPackagedId = businessItems[i].ProcedureWorkPackageId,
                        LayingDate = businessItems[i].LayingDate,
                        LayingBodyId = businessItems[i].LayingBodyId,
                        ModifiedBy = EMail,
                        ModifiedAt = DateTimeOffset.UtcNow
                    }));
                updates.Add(new CommandDefinition(@"insert into SolrBusinessItem (SolrStatutoryInstrumentDataId, TripleStoreId)
                    values(@SolrStatutoryInstrumentDataId, @TripleStoreId)",
                new { SolrStatutoryInstrumentDataId = id, TripleStoreId = tripleStoreId }));
            }

            return Execute(updates.ToArray());
        }

        [HttpDelete]
        [ContentNegotiation("solrfeed/treaty/{id:int}", ContentType.JSON)]
        public bool DeleteTreaty(int id)
        {
            CommandDefinition command = new CommandDefinition(@"update SolrTreatyData
                set IsDeleted=1 where Id=@id",
                new { Id = id });
            if (Execute(command))
                return true;
            else
                return false;
        }

        [HttpDelete]
        [ContentNegotiation("solrfeed/statutoryinstrument/{id:int}", ContentType.JSON)]
        public bool Delete(int id)
        {
            CommandDefinition command = new CommandDefinition(@"update SolrStatutoryInstrumentData
                set IsDeleted=1 where Id=@id",
                new { Id = id });
            if (Execute(command))
                return true;
            else
                return false;
        }

    }
}
