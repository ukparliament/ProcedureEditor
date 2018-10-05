using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{
    public class LayingItemController : BaseApiController
    {
        [HttpGet]
        [ContentNegotiation("layingitem", ContentType.HTML)]
        public IHttpActionResult GetView()
        {
            return RenderView("Index");
        }

        [HttpGet]
        [ContentNegotiation("layingitem", ContentType.JSON)]
        public List<LayingItem> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select li.Id, li.ProcedureBusinessItemId,
                    li.ProcedureWorkPackagedId, li.LayingDate, li.LayingBodyId, li.PersonTripleStoreId,
	                lb.LayingBodyName, b.TripleStoreId,
                    coalesce(si.ProcedureStatutoryInstrumentName, nsi.ProcedureProposedNegativeStatutoryInstrumentName) as WorkPackagedThingName
                    from ProcedureLaying li
                join LayingBody lb on lb.Id=li.LayingBodyId
                join ProcedureBusinessItem b on b.Id=li.ProcedureBusinessItemId
                join ProcedureWorkPackagedThing wp on wp.Id=li.ProcedureWorkPackagedId
                left join ProcedureStatutoryInstrument si on si.Id=wp.Id
                left join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=wp.Id");
            return GetItems<LayingItem>(command);
        }

        [HttpGet]
        [ContentNegotiation("layingitem/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetView(int id)
        {
            return RenderView("View", id);
        }

        [HttpGet]
        [ContentNegotiation("layingitem/{id:int}", ContentType.JSON)]
        public LayingItem Get(int id)
        {
            CommandDefinition command = new CommandDefinition(@"select li.Id, li.ProcedureBusinessItemId,
                    li.ProcedureWorkPackagedId, li.LayingDate, li.LayingBodyId, li.PersonTripleStoreId,
	                lb.LayingBodyName, b.TripleStoreId,
                    coalesce(si.ProcedureStatutoryInstrumentName, nsi.ProcedureProposedNegativeStatutoryInstrumentName) as WorkPackagedThingName
                    from ProcedureLaying li
                join LayingBody lb on lb.Id=li.LayingBodyId
                join ProcedureBusinessItem b on b.Id=li.ProcedureBusinessItemId
                join ProcedureWorkPackagedThing wp on wp.Id=li.ProcedureWorkPackagedId
                left join ProcedureStatutoryInstrument si on si.Id=wp.Id
                left join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=wp.Id
                where li.Id=@Id",
                new { Id = id });
            return GetItem<LayingItem>(command);
        }

        [HttpGet]
        [ContentNegotiation("layingitem/edit/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetEdit(int id)
        {
            return RenderView("Edit", new LayingItemEditParameters() { LayingItemId = id });
        }

        [HttpGet]
        [ContentNegotiation("layingitem/add", ContentType.HTML)]
        public IHttpActionResult GetAdd()
        {
            return RenderView("Edit", new LayingItemEditParameters());
        }

        [HttpGet]
        [ContentNegotiation("layingitem/add", ContentType.HTML)]
        public IHttpActionResult GetAdd(int businessItemId)
        {
            return RenderView("Edit", new LayingItemEditParameters() { BusinessItemId = businessItemId });
        }

        [HttpPut]
        [ContentNegotiation("layingitem/{id:int}", ContentType.JSON)]
        public bool Put(int id, [FromBody]LayingItem layingItem)
        {
            if ((layingItem == null) ||
                (layingItem.ProcedureBusinessItemId == 0) ||
                (layingItem.ProcedureWorkPackagedId == 0))
                return false;
            CommandDefinition command = new CommandDefinition(@"update ProcedureLaying
                set ProcedureBusinessItemId=@ProcedureBusinessItemId,
                    ProcedureWorkPackagedId=@ProcedureWorkPackagedId,
                    LayingDate=@LayingDate,
                    LayingBodyId=@LayingBodyId,
                    PersonTripleStoreId=@PersonTripleStoreId,
                    ModifiedBy=@ModifiedBy,
                    ModifiedAt=@ModifiedAt
                where Id=@Id",
                new
                {
                    ProcedureBusinessItemId = layingItem.ProcedureBusinessItemId,
                    ProcedureWorkPackagedId = layingItem.ProcedureWorkPackagedId,
                    LayingDate = layingItem.LayingDate,
                    LayingBodyId = layingItem.LayingBodyId,
                    PersonTripleStoreId = layingItem.PersonTripleStoreId,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    Id = id
                });
            return Execute(command);
        }

        [HttpPost]
        [ContentNegotiation("layingitem", ContentType.JSON)]
        public bool Post([FromBody]LayingItem layingItem)
        {
            if ((layingItem == null) ||
                (layingItem.ProcedureBusinessItemId == 0) ||
                (layingItem.ProcedureWorkPackagedId == 0))
                return false;
            CommandDefinition command = new CommandDefinition(@"insert into ProcedureLaying
                (ProcedureBusinessItemId, ProcedureWorkPackagedId,
	                LayingDate, LayingBodyId, PersonTripleStoreId,
                    ModifiedBy,ModifiedAt)
                values(@ProcedureBusinessItemId, @ProcedureWorkPackagedId,
	                @LayingDate, @LayingBodyId, @PersonTripleStoreId,
                    @ModifiedBy,@ModifiedAt)",
                new
                {
                    ProcedureBusinessItemId = layingItem.ProcedureBusinessItemId,
                    ProcedureWorkPackagedId = layingItem.ProcedureWorkPackagedId,
                    LayingDate = layingItem.LayingDate,
                    LayingBodyId = layingItem.LayingBodyId,
                    PersonTripleStoreId = layingItem.PersonTripleStoreId,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow
                });
            return Execute(command);
        }

        [HttpDelete]
        [ContentNegotiation("layingitem/{id:int}", ContentType.JSON)]
        public bool Delete(int id)
        {
            CommandDefinition command = new CommandDefinition(@"delete from ProcedureLaying where Id=@Id", new { Id = id });
            return Execute(command);
        }
    }
}