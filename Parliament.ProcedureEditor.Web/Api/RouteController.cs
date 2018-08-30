using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{

    public class RouteController : BaseApiController
    {
        [HttpGet]
        [ContentNegotiation("route", ContentType.JSON)]
        public List<Route> SearchByProcedure(int procedureId)
        {
            CommandDefinition command = new CommandDefinition(@"select r.Id, r.TripleStoreId, r.ProcedureId,
                r.FromProcedureStepId, r.ToProcedureStepId, r.ProcedureRouteTypeId,
                p.ProcedureName, rt.ProcedureRouteTypeName, fs.ProcedureStepName as FromProcedureStepName,
                ts.ProcedureStepName as ToProcedureStepName from ProcedureRoute r
                join [Procedure] p on p.Id=r.ProcedureId
                join ProcedureRouteType rt on rt.Id=r.ProcedureRouteTypeId
                join ProcedureStep fs on fs.Id=r.FromProcedureStepId
                join ProcedureStep ts on ts.Id=r.ToProcedureStepId
                where r.ProcedureId=@Id;
                select h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName from ProcedureStepHouse h
                join House hh on hh.Id=h.HouseId
                join ProcedureRoute r on r.FromProcedureStepId=h.ProcedureStepId or r.ToProcedureStepId=h.ProcedureStepId
                join [Procedure] p on p.Id=r.ProcedureId
                where r.ProcedureId=@Id
                group by h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName",
                new { Id = procedureId });
            Tuple<List<Route>, List<StepHouse>> tuple = GetItems<Route, StepHouse>(command);

            tuple.Item1
                .ForEach(r => r.FromProcedureStepHouseNames = tuple.Item2
                    .Where(h => h.ProcedureStepId == r.FromProcedureStepId)
                    .Select(h => h.HouseName)
                    .ToList());

            tuple.Item1
                .ForEach(r => r.ToProcedureStepHouseNames = tuple.Item2
                    .Where(h => h.ProcedureStepId == r.ToProcedureStepId)
                    .Select(h => h.HouseName)
                    .ToList());

            return tuple.Item1;
        }

        [HttpGet]
        [ContentNegotiation("route", ContentType.JSON)]
        public List<Route> SearchByStep(int stepId)
        {
            CommandDefinition command = new CommandDefinition(@"select r.Id, r.TripleStoreId, r.ProcedureId,
                r.FromProcedureStepId, r.ToProcedureStepId, r.ProcedureRouteTypeId,
                p.ProcedureName, rt.ProcedureRouteTypeName, fs.ProcedureStepName as FromProcedureStepName,
                ts.ProcedureStepName as ToProcedureStepName from ProcedureRoute r
                join [Procedure] p on p.Id=r.ProcedureId
                join ProcedureRouteType rt on rt.Id=r.ProcedureRouteTypeId
                join ProcedureStep fs on fs.Id=r.FromProcedureStepId
                join ProcedureStep ts on ts.Id=r.ToProcedureStepId
                where ((r.FromProcedureStepId=@StepId) or (r.ToProcedureStepId=@StepId));
                select h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName from ProcedureStepHouse h
                join House hh on hh.Id=h.HouseId
                where h.ProcedureStepId=@StepId
                group by h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName",
                new { StepId = stepId });
            Tuple<List<Route>, List<StepHouse>> tuple = GetItems<Route, StepHouse>(command);

            tuple.Item1
                .ForEach(r => r.FromProcedureStepHouseNames = tuple.Item2
                    .Where(h => h.ProcedureStepId == r.FromProcedureStepId)
                    .Select(h => h.HouseName)
                    .ToList());

            tuple.Item1
                .ForEach(r => r.ToProcedureStepHouseNames = tuple.Item2
                    .Where(h => h.ProcedureStepId == r.ToProcedureStepId)
                    .Select(h => h.HouseName)
                    .ToList());

            return tuple.Item1;
        }

        [HttpGet]
        [ContentNegotiation("route", ContentType.HTML)]
        public IHttpActionResult GetView()
        {
            return RenderView("Index");
        }

        [HttpGet]
        [ContentNegotiation("route", ContentType.JSON)]
        public List<Route> Get()
        {
            CommandDefinition command = new CommandDefinition(@"select r.Id, r.TripleStoreId, r.ProcedureId,
                r.FromProcedureStepId, r.ToProcedureStepId, r.ProcedureRouteTypeId,
                p.ProcedureName, rt.ProcedureRouteTypeName, fs.ProcedureStepName as FromProcedureStepName,
                ts.ProcedureStepName as ToProcedureStepName from ProcedureRoute r
                join [Procedure] p on p.Id=r.ProcedureId
                join ProcedureRouteType rt on rt.Id=r.ProcedureRouteTypeId
                join ProcedureStep fs on fs.Id=r.FromProcedureStepId
                join ProcedureStep ts on ts.Id=r.ToProcedureStepId;
                select h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName from ProcedureStepHouse h
                join House hh on hh.Id=h.HouseId
                group by h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName");
            Tuple<List<Route>, List<StepHouse>> tuple = GetItems<Route, StepHouse>(command);

            tuple.Item1
                .ForEach(r => r.FromProcedureStepHouseNames = tuple.Item2
                    .Where(h => h.ProcedureStepId == r.FromProcedureStepId)
                    .Select(h => h.HouseName)
                    .ToList());

            tuple.Item1
                .ForEach(r => r.ToProcedureStepHouseNames = tuple.Item2
                    .Where(h => h.ProcedureStepId == r.ToProcedureStepId)
                    .Select(h => h.HouseName)
                    .ToList());

            return tuple.Item1;
        }

        [HttpGet]
        [ContentNegotiation("route/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetView(int id)
        {
            return RenderView("View", id);
        }

        [HttpGet]
        [ContentNegotiation("route/{id:int}", ContentType.JSON)]
        public Route Get(int id)
        {
            CommandDefinition command = new CommandDefinition(@"select r.Id, r.TripleStoreId, r.ProcedureId,
                r.FromProcedureStepId, r.ToProcedureStepId, r.ProcedureRouteTypeId,
                p.ProcedureName, rt.ProcedureRouteTypeName, fs.ProcedureStepName as FromProcedureStepName,
                ts.ProcedureStepName as ToProcedureStepName from ProcedureRoute r
                join [Procedure] p on p.Id=r.ProcedureId
                join ProcedureRouteType rt on rt.Id=r.ProcedureRouteTypeId
                join ProcedureStep fs on fs.Id=r.FromProcedureStepId
                join ProcedureStep ts on ts.Id=r.ToProcedureStepId
                where r.Id=@Id;
                select h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName from ProcedureStepHouse h
                join House hh on hh.Id=h.HouseId
                join ProcedureRoute r on r.FromProcedureStepId=h.ProcedureStepId or r.ToProcedureStepId=h.ProcedureStepId
                where r.Id=@Id
                group by h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName",
                new { Id = id });
            Tuple<Route, List<StepHouse>> tuple = GetItem<Route, StepHouse>(command);

            tuple.Item1.FromProcedureStepHouseNames = tuple.Item2
                    .Where(h => h.ProcedureStepId == tuple.Item1.FromProcedureStepId)
                    .Select(h => h.HouseName)
                    .ToList();

            tuple.Item1.ToProcedureStepHouseNames = tuple.Item2
                    .Where(h => h.ProcedureStepId == tuple.Item1.ToProcedureStepId)
                    .Select(h => h.HouseName)
                    .ToList();

            return tuple.Item1;
        }

        [HttpGet]
        [ContentNegotiation("route/edit/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetEdit(int id)
        {
            return RenderView("Edit", id);
        }

        [HttpGet]
        [ContentNegotiation("route/add", ContentType.HTML)]
        public IHttpActionResult GetAdd()
        {
            return RenderView("Edit");
        }

        [HttpPut]
        [ContentNegotiation("route/{id:int}", ContentType.JSON)]
        public bool Put(int id, [FromBody]Route route)
        {
            if ((route == null) || (route.ProcedureId == 0) ||
                (route.FromProcedureStepId == 0) || (route.ToProcedureStepId == 0) ||
                (route.ProcedureRouteTypeId == 0))
                return false;
            CommandDefinition command = new CommandDefinition(@"update ProcedureRoute
                set ProcedureId=@ProcedureId,
                    FromProcedureStepId=@FromProcedureStepId, 
                    ToProcedureStepId=@ToProcedureStepId, 
                    ProcedureRouteTypeId=@ProcedureRouteTypeId,
                    ModifiedBy=@ModifiedBy,
                    ModifiedAt=@ModifiedAt
                where Id=@Id",
                new
                {
                    ProcedureId = route.ProcedureId,
                    FromProcedureStepId = route.FromProcedureStepId,
                    ToProcedureStepId = route.ToProcedureStepId,
                    ProcedureRouteTypeId = route.ProcedureRouteTypeId,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    Id = id
                });
            return Execute(command);
        }

        [HttpPost]
        [ContentNegotiation("route", ContentType.JSON)]
        public bool Post([FromBody]Route route)
        {
            if ((route == null) || (route.ProcedureId == 0) ||
                (route.FromProcedureStepId == 0) || (route.ToProcedureStepId == 0) ||
                (route.ProcedureRouteTypeId == 0))
                return false;
            string tripleStoreId = GetTripleStoreId();
            if (string.IsNullOrWhiteSpace(tripleStoreId))
                return false;
            CommandDefinition command = new CommandDefinition(@"insert into ProcedureRoute
                (ProcedureId, FromProcedureStepId, ToProcedureStepId, ProcedureRouteTypeId,
                    ModifiedBy,ModifiedAt,TripleStoreId)
                values(@ProcedureId, @FromProcedureStepId, @ToProcedureStepId, @ProcedureRouteTypeId,
                    @ModifiedBy,@ModifiedAt,@TripleStoreId)",
                new
                {
                    ProcedureId = route.ProcedureId,
                    FromProcedureStepId = route.FromProcedureStepId,
                    ToProcedureStepId = route.ToProcedureStepId,
                    ProcedureRouteTypeId = route.ProcedureRouteTypeId,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    TripleStoreId = tripleStoreId
                });
            return Execute(command);
        }

        [HttpDelete]
        [ContentNegotiation("route/{id:int}", ContentType.JSON)]
        public bool Delete(int id)
        {
            CommandDefinition command = new CommandDefinition(@"delete from ProcedureRoute where Id=@Id", new { Id = id });
            return Execute(command);
        }
    }

}