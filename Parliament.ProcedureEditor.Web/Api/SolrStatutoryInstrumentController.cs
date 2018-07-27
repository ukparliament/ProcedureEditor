using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{
    public class SolrStatutoryInstrumentController : BaseApiController
    {
        [HttpGet]
        [ContentNegotiation("solrstatutoryinstrument", ContentType.HTML)]
        public IHttpActionResult GetView()
        {
            return RenderView("Index");
        }

        [HttpGet]
        [ContentNegotiation("solrstatutoryinstrument", ContentType.JSON)]
        public List<SolrStatutoryInstrument> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select s.Id, s.Title,
                s.SIPrefix, s.SINumber, s.WebUrl, s.ComingIntoForceNote,
                s.ComingIntoForceDate, s.MadeDate, s.SIProcedure from SolrStatutoryInstrumentData s
                where s.IsDeleted=0 or s.TripleStoreId is null");
            return GetItems<SolrStatutoryInstrument>(command);
        }

        [HttpGet]
        [ContentNegotiation("solrstatutoryinstrument/{id:int}", ContentType.JSON)]
        public SolrStatutoryInstrument Get(int id)
        {
            CommandDefinition command = new CommandDefinition(@"select s.Id, s.Title,
                s.SIPrefix, s.SINumber, s.WebUrl, s.ComingIntoForceNote,
                s.ComingIntoForceDate, s.MadeDate, s.SIProcedure from SolrStatutoryInstrumentData s
                where s.Id=@Id",
                new { Id = id });
            return GetItem<SolrStatutoryInstrument>(command);
        }

        [HttpGet]
        [ContentNegotiation("solrstatutoryinstrument/edit/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetEdit(int id)
        {
            return RenderView("Edit", id);
        }

        [HttpPost]
        [ContentNegotiation("solrstatutoryinstrument/{id:int}", ContentType.JSON)]
        public bool Post(int id, [FromBody]WorkPackageable workPackageable)
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
            CommandDefinition commandAdd = new CommandDefinition(@"insert into ProcedureWorkPackageableThing
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
            CommandDefinition commandUpdate = new CommandDefinition(@"update SolrStatutoryInstrumentData
                set TripleStoreId=@TripleStoreId where Id=@Id",
                new { TripleStoreId=tripleStoreId, Id = id });
            return Execute(new CommandDefinition[] { commandAdd, commandUpdate });
        }

        [HttpDelete]
        [ContentNegotiation("solrstatutoryinstrument/{id:int}", ContentType.JSON)]
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
