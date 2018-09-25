using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{
    public class SolrStatutoryInstrumentController : BaseApiController
    {
        [HttpGet]
        [ContentNegotiation("solrfeed", ContentType.HTML)]
        public IHttpActionResult GetView()
        {
            return RenderView("Index");
        }

        [HttpGet]
        [ContentNegotiation("solrfeed", ContentType.JSON)]
        public List<SolrStatutoryInstrument> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select s.Id, s.Title,
                s.SIPrefix, s.SINumber, s.WebUrl, s.ComingIntoForceNote,
                s.ComingIntoForceDate, s.MadeDate, s.SIProcedure, s.IsStatutoryInstrument
                from SolrStatutoryInstrumentData s
                where s.TripleStoreId is null and s.IsDeleted=0");
            return GetItems<SolrStatutoryInstrument>(command);
        }

        [HttpGet]
        [ContentNegotiation("solrfeed/{id:int}", ContentType.JSON)]
        public SolrStatutoryInstrument Get(int id)
        {
            CommandDefinition command = new CommandDefinition(@"select s.Id, s.Title,
                s.SIPrefix, s.SINumber, s.WebUrl, s.ComingIntoForceNote,
                s.ComingIntoForceDate, s.MadeDate, s.SIProcedure, s.IsStatutoryInstrument
                from SolrStatutoryInstrumentData s
                where s.TripleStoreId is null and s.IsDeleted=0 and s.Id=@Id",
                new { Id = id });
            return GetItem<SolrStatutoryInstrument>(command);
        }

        [HttpGet]
        [ContentNegotiation("solrfeed/edit/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetEdit(int id)
        {
            return RenderView("Edit", id);
        }

        [HttpPost]
        [ContentNegotiation("solrfeed/{id:int}", ContentType.JSON)]
        public bool Post(int id, [FromBody]WorkPackaged workPackaged)
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
            parameters.Add("@SolarFeedId", id);
            parameters.Add("@ModifiedBy", EMail);
            CommandDefinition command = new CommandDefinition("CreateWorkPackaged",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);
            return Execute(command);
        }

        [HttpDelete]
        [ContentNegotiation("solrfeed/{id:int}", ContentType.JSON)]
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
