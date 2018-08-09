using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{

    public class BusinessItemController : BaseApiController
    {

        [HttpGet]
        [ContentNegotiation("businessitem", ContentType.JSON)]
        public List<BusinessItem> SearchByWorkPackage(int workPackageId)
        {
            CommandDefinition command = new CommandDefinition(@"select b.Id, b.TripleStoreId, b.WebLink,
                b.ProcedureWorkPackageId, b.BusinessItemDate,
                coalesce(si.ProcedureStatutoryInstrumentName, nsi.ProcedureProposedNegativeStatutoryInstrumentName) as WorkPackagedThingName,
                wp.ProcedureId, p.ProcedureName from ProcedureBusinessItem b
                join ProcedureWorkPackagedThing wp on wp.Id=b.ProcedureWorkPackageId
                left join ProcedureStatutoryInstrument si on si.Id=wp.Id
                left join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=wp.Id
                join [Procedure] p on p.Id=wp.ProcedureId
                where b.ProcedureWorkPackageId=@ProcedureWorkPackageId;
                select s.Id, s.ProcedureBusinessItemId, s.ProcedureStepId from ProcedureBusinessItemProcedureStep s
                join ProcedureBusinessItem b on b.Id=s.ProcedureBusinessItemId
                where b.ProcedureWorkPackageId=@ProcedureWorkPackageId",
                new { ProcedureWorkPackageId = workPackageId });
            Tuple<List<BusinessItem>, List<BusinessItemStep>> tuple = GetItems<BusinessItem, BusinessItemStep>(command);

            tuple.Item1
                .ForEach(b => b.Steps = tuple.Item2
                    .Where(s => s.ProcedureBusinessItemId == b.Id)
                    .Select(s => s.ProcedureStepId)
                    .ToList());

            return tuple.Item1;
        }

        [HttpGet]
        [ContentNegotiation("businessitem", ContentType.JSON)]
        public List<BusinessItem> SearchByStep(int stepId)
        {
            CommandDefinition command = new CommandDefinition(@"select b.Id, b.TripleStoreId, b.WebLink,
                b.ProcedureWorkPackageId, b.BusinessItemDate,
                coalesce(si.ProcedureStatutoryInstrumentName, nsi.ProcedureProposedNegativeStatutoryInstrumentName) as WorkPackagedThingName,
                wp.ProcedureId, p.ProcedureName from ProcedureBusinessItem b
                join ProcedureWorkPackagedThing wp on wp.Id=b.ProcedureWorkPackageId
                left join ProcedureStatutoryInstrument si on si.Id=wp.Id
                left join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=wp.Id
                join [Procedure] p on p.Id=wp.ProcedureId
                where exists (select top 1 bs.Id from ProcedureBusinessItemProcedureStep bs where bs.ProcedureBusinessItemId=b.Id and bs.ProcedureStepId=@StepId);
                select s.Id, s.ProcedureBusinessItemId, s.ProcedureStepId from ProcedureBusinessItemProcedureStep s
                join ProcedureBusinessItem b on b.Id=s.ProcedureBusinessItemId
                where s.ProcedureStepId=@StepId",
                new { StepId = stepId });
            Tuple<List<BusinessItem>, List<BusinessItemStep>> tuple = GetItems<BusinessItem, BusinessItemStep>(command);

            tuple.Item1
                .ForEach(b => b.Steps = tuple.Item2
                    .Where(s => s.ProcedureBusinessItemId == b.Id)
                    .Select(s => s.ProcedureStepId)
                    .ToList());

            return tuple.Item1;
        }

        [HttpGet]
        [ContentNegotiation("businessitem", ContentType.HTML)]
        public IHttpActionResult GetView()
        {
            return RenderView("Index");
        }

        [HttpGet]
        [ContentNegotiation("businessitem", ContentType.JSON)]
        public List<BusinessItem> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select b.Id, b.TripleStoreId, b.WebLink,
                b.ProcedureWorkPackageId, b.BusinessItemDate,
                coalesce(si.ProcedureStatutoryInstrumentName, nsi.ProcedureProposedNegativeStatutoryInstrumentName) as WorkPackagedThingName,
                wp.ProcedureId, p.ProcedureName from ProcedureBusinessItem b
                join ProcedureWorkPackagedThing wp on wp.Id=b.ProcedureWorkPackageId
                left join ProcedureStatutoryInstrument si on si.Id=wp.Id
                left join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=wp.Id
                join [Procedure] p on p.Id=wp.ProcedureId;
                select s.Id, s.ProcedureBusinessItemId, s.ProcedureStepId from ProcedureBusinessItemProcedureStep s
                join ProcedureBusinessItem b on b.Id=s.ProcedureBusinessItemId");
            Tuple<List<BusinessItem>, List<BusinessItemStep>> tuple = GetItems<BusinessItem, BusinessItemStep>(command);

            tuple.Item1
                .ForEach(b => b.Steps = tuple.Item2
                    .Where(s => s.ProcedureBusinessItemId == b.Id)
                    .Select(s => s.ProcedureStepId)
                    .ToList());

            return tuple.Item1;
        }

        [HttpGet]
        [ContentNegotiation("businessitem/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetView(int id)
        {
            return RenderView("View", id);
        }

        [HttpGet]
        [ContentNegotiation("businessitem/{id:int}", ContentType.JSON)]
        public BusinessItem Get(int id)
        {
            CommandDefinition command = new CommandDefinition(@"select b.Id, b.TripleStoreId, b.WebLink,
                b.ProcedureWorkPackageId, b.BusinessItemDate,
                coalesce(si.ProcedureStatutoryInstrumentName, nsi.ProcedureProposedNegativeStatutoryInstrumentName) as WorkPackagedThingName,
                wp.ProcedureId, p.ProcedureName from ProcedureBusinessItem b
                join ProcedureWorkPackagedThing wp on wp.Id=b.ProcedureWorkPackageId
                left join ProcedureStatutoryInstrument si on si.Id=wp.Id
                left join ProcedureProposedNegativeStatutoryInstrument nsi on nsi.Id=wp.Id
                join [Procedure] p on p.Id=wp.ProcedureId
                where b.Id=@Id;
                select Id, ProcedureBusinessItemId, ProcedureStepId from ProcedureBusinessItemProcedureStep
                where ProcedureBusinessItemId=@Id",
                new { Id = id });
            Tuple<BusinessItem, List<BusinessItemStep>> tuple = GetItem<BusinessItem, BusinessItemStep>(command);

            tuple.Item1.Steps = tuple.Item2
                    .Select(s => s.ProcedureStepId)
                    .ToList();

            return tuple.Item1;
        }

        [HttpGet]
        [ContentNegotiation("businessitem/edit/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetEdit(int id)
        {
            return RenderView("Edit", new BusinessItemEditParameters() { BusinessItemId = id });
        }

        [HttpGet]
        [ContentNegotiation("businessitem/add", ContentType.HTML)]
        public IHttpActionResult GetAdd()
        {
            return RenderView("Edit", new BusinessItemEditParameters());
        }

        [HttpGet]
        [ContentNegotiation("businessitem/add", ContentType.HTML)]
        public IHttpActionResult GetAdd(int workPackageId)
        {
            return RenderView("Edit", new BusinessItemEditParameters() { WorkPackageId = workPackageId });
        }

        [HttpPut]
        [ContentNegotiation("businessitem/{id:int}", ContentType.JSON)]
        public bool Put(int id, [FromBody]BusinessItem businessItem)
        {
            if ((businessItem == null) ||
                (businessItem.ProcedureWorkPackageId == 0))
                return false;
            List<CommandDefinition> updates = new List<CommandDefinition>();
            updates.Add(new CommandDefinition(@"update ProcedureBusinessItem
                set WebLink=@WebLink,
                    ProcedureWorkPackageId=@ProcedureWorkPackageId,
                    BusinessItemDate=@BusinessItemDate,
                    ModifiedBy=@ModifiedBy,
                    ModifiedAt=@ModifiedAt
                where Id=@Id",
                new
                {
                    WebLink = businessItem.WebLink,
                    ProcedureWorkPackageId = businessItem.ProcedureWorkPackageId,
                    BusinessItemDate = businessItem.BusinessItemDate,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    Id = id
                }));
            updates.Add(new CommandDefinition(@"delete from ProcedureBusinessItemProcedureStep where ProcedureBusinessItemId=@Id",
                new { Id = id }));
            if (businessItem.Steps != null)
                updates.AddRange(businessItem.Steps.Select(s => new CommandDefinition(@"insert into ProcedureBusinessItemProcedureStep
                    (ProcedureBusinessItemId, ProcedureStepId)
                    values (@Id, @ProcedureStepId)",
                        new
                        {
                            Id = id,
                            ProcedureStepId = s
                        })));
            return Execute(updates.ToArray());
        }

        [HttpPost]
        [ContentNegotiation("businessitem", ContentType.JSON)]
        public bool Post([FromBody]BusinessItem businessItem)
        {
            if ((businessItem == null) ||
                (businessItem.ProcedureWorkPackageId == 0))
                return false;
            string tripleStoreId = GetTripleStoreId();
            if (string.IsNullOrWhiteSpace(tripleStoreId))
                return false;
            List<CommandDefinition> updates = new List<CommandDefinition>();
            updates.Add(new CommandDefinition(@"insert into ProcedureBusinessItem
                (WebLink,LayingBodyId,ProcedureWorkPackageId,BusinessItemDate,
                    ModifiedBy,ModifiedAt,TripleStoreId)
                values(@WebLink,@LayingBodyId,@ProcedureWorkPackageId,@BusinessItemDate,
                    @ModifiedBy,@ModifiedAt,@TripleStoreId)",
                new
                {
                    WebLink = businessItem.WebLink,
                    ProcedureWorkPackageId = businessItem.ProcedureWorkPackageId,
                    BusinessItemDate = businessItem.BusinessItemDate,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    TripleStoreId = tripleStoreId
                }));
            if (businessItem.Steps != null)
                updates.AddRange(businessItem.Steps.Select(s => new CommandDefinition(@"insert into ProcedureBusinessItemProcedureStep
                    (ProcedureBusinessItemId, ProcedureStepId)
                    select Id, @ProcedureStepId from ProcedureBusinessItem where TripleStoreId=@TripleStoreId",
                        new
                        {
                            TripleStoreId = tripleStoreId,
                            ProcedureStepId = s
                        })));
            return Execute(updates.ToArray());
        }

        [HttpDelete]
        [ContentNegotiation("businessitem/{id:int}", ContentType.JSON)]
        public bool Delete(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ModifiedBy", EMail);
            parameters.Add("@BusinessItemId", id);
            parameters.Add("@IsSuccess", dbType: System.Data.DbType.Boolean, direction: System.Data.ParameterDirection.Output);
            CommandDefinition command = new CommandDefinition("DeleteBusinessItem",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);
            if (Execute(command))
                return parameters.Get<bool>("@IsSuccess");
            else
                return false;
        }
    }

}