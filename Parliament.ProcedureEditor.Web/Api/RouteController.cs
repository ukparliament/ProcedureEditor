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
            CommandDefinition command = new CommandDefinition(@"select r.Id, r.TripleStoreId,
                r.FromProcedureStepId, r.ToProcedureStepId, r.ProcedureRouteTypeId,
                rt.ProcedureRouteTypeName, fs.ProcedureStepName as FromProcedureStepName,
                ts.ProcedureStepName as ToProcedureStepName from ProcedureRoute r
                join ProcedureRouteProcedure rp on rp.ProcedureRouteId=r.Id
				join ProcedureRouteType rt on rt.Id=r.ProcedureRouteTypeId
                join ProcedureStep fs on fs.Id=r.FromProcedureStepId
                join ProcedureStep ts on ts.Id=r.ToProcedureStepId
                where rp.ProcedureId=@Id;
                select h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName from ProcedureStepHouse h
                join House hh on hh.Id=h.HouseId
                join ProcedureRoute r on r.FromProcedureStepId=h.ProcedureStepId or r.ToProcedureStepId=h.ProcedureStepId
                join ProcedureRouteProcedure rp on rp.ProcedureRouteId=r.Id
                where rp.ProcedureId=@Id
                group by h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName",
                new { Id = procedureId });
            Tuple<List<Route>, List<StepHouse>> tuple = GetItems<Route, StepHouse>(command);

            tuple.Item1
                .ForEach(r =>
                    r.FromProcedureStepHouseNames = tuple.Item2
                        .Where(h => h.ProcedureStepId == r.FromProcedureStepId)
                        .Select(h => h.HouseName)
                        .ToList());

            tuple.Item1
                .ForEach(r => r.ToProcedureStepHouseNames = tuple.Item2
                    .Where(h => h.ProcedureStepId == r.ToProcedureStepId)
                    .Select(h => h.HouseName)
                    .ToList());

            tuple.Item1
                .ForEach(r => r.Procedures = new int[] { procedureId });

            return tuple.Item1;
        }

        [HttpGet]
        [ContentNegotiation("route", ContentType.JSON)]
        public List<Route> SearchByStep(int stepId)
        {
            CommandDefinition command = new CommandDefinition(@"select r.Id, r.TripleStoreId,
                r.FromProcedureStepId, r.ToProcedureStepId, r.ProcedureRouteTypeId,
                rt.ProcedureRouteTypeName, fs.ProcedureStepName as FromProcedureStepName,
                ts.ProcedureStepName as ToProcedureStepName from ProcedureRoute r
                join ProcedureRouteType rt on rt.Id=r.ProcedureRouteTypeId
                join ProcedureStep fs on fs.Id=r.FromProcedureStepId
                join ProcedureStep ts on ts.Id=r.ToProcedureStepId
                where ((r.FromProcedureStepId=@StepId) or (r.ToProcedureStepId=@StepId));
                select h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName from ProcedureStepHouse h
                join House hh on hh.Id=h.HouseId
                where h.ProcedureStepId=@StepId
                group by h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName;
                select rp.Id, rp.ProcedureRouteId, rp.ProcedureId from ProcedureRoute r
                join ProcedureRouteProcedure rp on rp.ProcedureRouteId=r.Id
                where ((r.FromProcedureStepId=@StepId) or (r.ToProcedureStepId=@StepId))",
                new { StepId = stepId });
            Tuple<List<Route>, List<StepHouse>, List<RouteProcedure>> tuple = GetItems<Route, StepHouse, RouteProcedure>(command);

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

            tuple.Item1
                .ForEach(r => r.Procedures = tuple.Item3
                    .Where(rp=>rp.ProcedureRouteId==r.Id)
                    .Select(rp=>rp.ProcedureId)
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
            CommandDefinition command = new CommandDefinition(@"select r.Id, r.TripleStoreId,
                r.FromProcedureStepId, r.ToProcedureStepId, r.ProcedureRouteTypeId,
                rt.ProcedureRouteTypeName, fs.ProcedureStepName as FromProcedureStepName,
                ts.ProcedureStepName as ToProcedureStepName from ProcedureRoute r
                join ProcedureRouteType rt on rt.Id=r.ProcedureRouteTypeId
                join ProcedureStep fs on fs.Id=r.FromProcedureStepId
                join ProcedureStep ts on ts.Id=r.ToProcedureStepId;
                select h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName from ProcedureStepHouse h
                join House hh on hh.Id=h.HouseId
                group by h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName;
                select rp.Id, rp.ProcedureRouteId, rp.ProcedureId from ProcedureRoute r
                join ProcedureRouteProcedure rp on rp.ProcedureRouteId=r.Id");
            Tuple<List<Route>, List<StepHouse>, List<RouteProcedure>> tuple = GetItems<Route, StepHouse, RouteProcedure>(command);

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

            tuple.Item1
                .ForEach(r => r.Procedures = tuple.Item3
                    .Where(rp => rp.ProcedureRouteId == r.Id)
                    .Select(rp => rp.ProcedureId)
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
            CommandDefinition command = new CommandDefinition(@"select r.Id, r.TripleStoreId,
                r.FromProcedureStepId, r.ToProcedureStepId, r.ProcedureRouteTypeId,
                rt.ProcedureRouteTypeName, fs.ProcedureStepName as FromProcedureStepName,
                ts.ProcedureStepName as ToProcedureStepName from ProcedureRoute r
                join ProcedureRouteType rt on rt.Id=r.ProcedureRouteTypeId
                join ProcedureStep fs on fs.Id=r.FromProcedureStepId
                join ProcedureStep ts on ts.Id=r.ToProcedureStepId
                where r.Id=@Id;
                select h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName from ProcedureStepHouse h
                join House hh on hh.Id=h.HouseId
                join ProcedureRoute r on r.FromProcedureStepId=h.ProcedureStepId or r.ToProcedureStepId=h.ProcedureStepId
                where r.Id=@Id
                group by h.Id, h.ProcedureStepId, h.HouseId, hh.HouseName;
                select rp.Id, rp.ProcedureRouteId, rp.ProcedureId from ProcedureRoute r
                join ProcedureRouteProcedure rp on rp.ProcedureRouteId=r.Id
                where r.Id=@Id",
                new { Id = id });
            Tuple<Route, List<StepHouse>, List<RouteProcedure>> tuple = GetItem<Route, StepHouse, RouteProcedure>(command);

            tuple.Item1.FromProcedureStepHouseNames = tuple.Item2
                    .Where(h => h.ProcedureStepId == tuple.Item1.FromProcedureStepId)
                    .Select(h => h.HouseName)
                    .ToList();

            tuple.Item1.ToProcedureStepHouseNames = tuple.Item2
                    .Where(h => h.ProcedureStepId == tuple.Item1.ToProcedureStepId)
                    .Select(h => h.HouseName)
                    .ToList();

            tuple.Item1.Procedures = tuple.Item3
                    .Where(rp => rp.ProcedureRouteId == tuple.Item1.Id)
                    .Select(rp => rp.ProcedureId)
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
            if ((route == null) || (route.Procedures == null) ||
                (route.FromProcedureStepId == 0) || (route.ToProcedureStepId == 0) ||
                (route.ProcedureRouteTypeId == 0))
                return false;
            List<CommandDefinition> updates = new List<CommandDefinition>();
            updates.Add(new CommandDefinition(@"update ProcedureRoute
                set FromProcedureStepId=@FromProcedureStepId, 
                    ToProcedureStepId=@ToProcedureStepId, 
                    ProcedureRouteTypeId=@ProcedureRouteTypeId,
                    ModifiedBy=@ModifiedBy,
                    ModifiedAt=@ModifiedAt
                where Id=@Id",
                new
                {
                    FromProcedureStepId = route.FromProcedureStepId,
                    ToProcedureStepId = route.ToProcedureStepId,
                    ProcedureRouteTypeId = route.ProcedureRouteTypeId,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    Id = id
                }));
            updates.Add(new CommandDefinition(@"delete from ProcedureRouteProcedure where ProcedureRouteId=@Id",
                new { Id = id }));
            updates.AddRange(route.Procedures.Select(p => new CommandDefinition(@"insert into ProcedureRouteProcedure
                    (ProcedureRouteId, ProcedureId)
                    values (@Id, @ProcedureId)",
                        new
                        {
                            Id = id,
                            ProcedureId = p
                        })));
            return Execute(updates.ToArray());
        }

        [HttpPost]
        [ContentNegotiation("route", ContentType.JSON)]
        public bool Post([FromBody]Route route)
        {
            if ((route == null) || (route.Procedures == null) ||
                (route.FromProcedureStepId == 0) || (route.ToProcedureStepId == 0) ||
                (route.ProcedureRouteTypeId == 0))
                return false;
            string tripleStoreId = GetTripleStoreId();
            if (string.IsNullOrWhiteSpace(tripleStoreId))
                return false;
            List<CommandDefinition> commands = new List<CommandDefinition>();
            commands.Add(new CommandDefinition(@"insert into ProcedureRoute
                (FromProcedureStepId, ToProcedureStepId, ProcedureRouteTypeId,
                    ModifiedBy,ModifiedAt,TripleStoreId)
                values(@FromProcedureStepId, @ToProcedureStepId, @ProcedureRouteTypeId,
                    @ModifiedBy,@ModifiedAt,@TripleStoreId)",
                new
                {
                    FromProcedureStepId = route.FromProcedureStepId,
                    ToProcedureStepId = route.ToProcedureStepId,
                    ProcedureRouteTypeId = route.ProcedureRouteTypeId,
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    TripleStoreId = tripleStoreId
                }));
            commands.AddRange(route.Procedures.Select(p => new CommandDefinition(@"insert into ProcedureRouteProcedure
                    (ProcedureRouteId, ProcedureId)
                    select Id, @ProcedureId from ProcedureRoute where TripleStoreId=@TripleStoreId",
                            new
                            {
                                TripleStoreId = tripleStoreId,
                                ProcedureId = p
                            })));
            return Execute(commands.ToArray());
        }

        [HttpDelete]
        [ContentNegotiation("route/{id:int}", ContentType.JSON)]
        public bool Delete(int id)
        {
            List<CommandDefinition> commands = new List<CommandDefinition>();
            commands.Add(new CommandDefinition(@"delete from ProcedureRouteProcedure where ProcedureRouteId=@Id", new { Id = id }));
            commands.Add(new CommandDefinition(@"delete from ProcedureRoute where Id=@Id", new { Id = id }));
            return Execute(commands.ToArray());
        }
    }

}