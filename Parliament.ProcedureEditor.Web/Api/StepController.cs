﻿using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{

    public class StepController : BaseApiController
    {
        [HttpPost]
        [ContentNegotiation("step/search", ContentType.JSON)]
        public List<Step> Search(StepSearchParameters searchParameters)
        {
            if (searchParameters == null)
                return null;
            CommandDefinition command = new CommandDefinition(@"select s.Id, s.TripleStoreId, s.ProcedureStepName, s.ProcedureStepDescription from (
	                select unionsteps.step from (
		                select wp.Id as wp, s.Id as step from ProcedureWorkPackagedThing wp
		                join ProcedureRoute r on r.ProcedureId=wp.ProcedureId
		                join ProcedureStep s on s.Id=r.FromProcedureStepId
		                where wp.Id in @ids
		                union
		                select wp.Id as wp, s.Id as step from ProcedureWorkPackagedThing wp
		                join ProcedureRoute r on r.ProcedureId=wp.ProcedureId
		                join ProcedureStep s on s.Id=r.ToProcedureStepId
		                where wp.Id in @ids
		                group by wp.Id, s.Id) unionsteps
	                group by unionsteps.step
	                having count(unionsteps.wp)=@wpCount) intersectsteps
                join ProcedureStep s on s.Id=intersectsteps.step;
                select h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName from ProcedureStepHouse h
                join House hh on hh.Id=h.HouseId",
                new { ids = searchParameters.WorkPackagedIds, wpCount=searchParameters.WorkPackagedIds.Count() });
            Tuple<List<Step>, List<StepHouse>> tuple = GetItems<Step, StepHouse>(command);

            tuple.Item1
                .ForEach(s => s.Houses = tuple.Item2
                    .Where(h => h.ProcedureStepId == s.Id)
                    .Select(h => h.HouseId)
                    .ToList());

            tuple.Item1
                .ForEach(s => s.HouseNames = tuple.Item2
                    .Where(h => h.ProcedureStepId == s.Id)
                    .Select(h => h.HouseName)
                    .ToList());

            return tuple.Item1;
        }

        [HttpGet]
        [ContentNegotiation("step", ContentType.HTML)]
        public IHttpActionResult GetView()
        {
            return RenderView("Index");
        }

        [HttpGet]
        [ContentNegotiation("step", ContentType.JSON)]
        public List<Step> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select s.Id, s.TripleStoreId, s.ProcedureStepName,
                s.ProcedureStepDescription from ProcedureStep s;
                select h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName from ProcedureStepHouse h
                join House hh on hh.Id=h.HouseId");
            Tuple<List<Step>, List<StepHouse>> tuple = GetItems<Step, StepHouse>(command);

            tuple.Item1
                .ForEach(s => s.Houses = tuple.Item2
                    .Where(h => h.ProcedureStepId == s.Id)
                    .Select(h => h.HouseId)
                    .ToList());

            tuple.Item1
                .ForEach(s => s.HouseNames = tuple.Item2
                    .Where(h => h.ProcedureStepId == s.Id)
                    .Select(h => h.HouseName)
                    .ToList());

            return tuple.Item1;
        }

        [HttpGet]
        [ContentNegotiation("step/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetView(int id)
        {
            return RenderView("View", id);
        }

        [HttpGet]
        [ContentNegotiation("step/{id:int}", ContentType.JSON)]
        public Step Get(int id)
        {
            CommandDefinition command = new CommandDefinition(@"select Id, TripleStoreId, ProcedureStepName,
                ProcedureStepDescription from ProcedureStep
                where Id=@Id;
                select h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName from ProcedureStepHouse h
                join House hh on hh.Id=h.HouseId
                where h.ProcedureStepId=@Id",
                new { Id = id });
            Tuple<Step, List<StepHouse>> tuple = GetItem<Step, StepHouse>(command);

            tuple.Item1.Houses = tuple.Item2
                    .Select(h => h.HouseId)
                    .ToList();

            tuple.Item1.HouseNames=tuple.Item2
                    .Select(h => h.HouseName)
                    .ToList();

            return tuple.Item1;
        }

        [HttpGet]
        [ContentNegotiation("step/edit/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetEdit(int id)
        {
            return RenderView("Edit", id);
        }

        [HttpGet]
        [ContentNegotiation("step/add", ContentType.HTML)]
        public IHttpActionResult GetAdd()
        {
            return RenderView("Edit");
        }

        [HttpPut]
        [ContentNegotiation("step/{id:int}", ContentType.JSON)]
        public bool Put(int id, [FromBody]Step step)
        {
            if ((step == null) ||
                (string.IsNullOrWhiteSpace(step.ProcedureStepName)))
                return false;
            List<CommandDefinition> updates = new List<CommandDefinition>();
            updates.Add(new CommandDefinition(@"update ProcedureStep
                set ProcedureStepName=@ProcedureStepName,
                    ProcedureStepDescription=@ProcedureStepDescription,
                    ModifiedBy=@ModifiedBy,
                    ModifiedAt=@ModifiedAt
                where Id=@Id",
                new
                {
                    ProcedureStepName = step.ProcedureStepName.Trim(),
                    ProcedureStepDescription = step.ProcedureStepDescription,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    Id = id
                }));
            updates.Add(new CommandDefinition(@"delete from ProcedureStepHouse where ProcedureStepId=@Id",
                new { Id = id }));
            if (step.Houses != null)
                updates.AddRange(step.Houses.Select(h => new CommandDefinition(@"insert into ProcedureStepHouse
                    (ProcedureStepId, HouseId)
                    values (@Id, @HouseId)",
                        new
                        {
                            Id = id,
                            HouseId = h
                        })));
            return Execute(updates.ToArray());
        }

        [HttpPost]
        [ContentNegotiation("step", ContentType.JSON)]
        public bool Post([FromBody]Step step)
        {
            if ((step == null) ||
                (string.IsNullOrWhiteSpace(step.ProcedureStepName)))
                return false;
            string tripleStoreId = GetTripleStoreId();
            if (string.IsNullOrWhiteSpace(tripleStoreId))
                return false;
            List<CommandDefinition> updates = new List<CommandDefinition>();
            updates.Add(new CommandDefinition(@"insert into ProcedureStep
                (ProcedureStepName,ProcedureStepDescription,
                    ModifiedBy,ModifiedAt,TripleStoreId)
                values(@ProcedureStepName,@ProcedureStepDescription,
                    @ModifiedBy,@ModifiedAt,@TripleStoreId)",
                new
                {
                    ProcedureStepName = step.ProcedureStepName.Trim(),
                    ProcedureStepDescription = step.ProcedureStepDescription,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    TripleStoreId = tripleStoreId
                }));
            if (step.Houses != null)
                updates.AddRange(step.Houses.Select(h => new CommandDefinition(@"insert into ProcedureStepHouse
                    (ProcedureStepId, HouseId)
                    select Id, @HouseId from ProcedureStep where TripleStoreId=@TripleStoreId",
                    new
                    {
                        TripleStoreId = tripleStoreId,
                        HouseId = h
                    })));
            return Execute(updates.ToArray());
        }

        [HttpDelete]
        [ContentNegotiation("step/{id:int}", ContentType.JSON)]
        public bool Delete(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@StepId", id);
            parameters.Add("@IsSuccess", dbType: System.Data.DbType.Boolean, direction: System.Data.ParameterDirection.Output);
            CommandDefinition command = new CommandDefinition("DeleteStep",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);
            if (Execute(command))
                return parameters.Get<bool>("@IsSuccess");
            else
                return false;
        }
    }

}