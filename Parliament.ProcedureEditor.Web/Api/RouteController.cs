using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using Parliament.ProcedureEditor.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{

    public class RouteController : BaseApiController
    {
        [HttpGet]
        [ContentNegotiation("route", ContentType.JSON)]
        public List<Route> Search(string searchText)
        {
            CommandDefinition command = new CommandDefinition(@"select r.Id, r.TripleStoreId, r.ProcedureId,
                r.FromProcedureStepId, r.ToProcedureStepId, r.ProcedureRouteTypeId,
                p.ProcedureName, rt.ProcedureRouteTypeName, fs.ProcedureStepName as FromProcedureStepName,
                ts.ProcedureStepName as ToProcedureStepName from ProcedureRoute r
                join [Procedure] p on p.Id=r.ProcedureId
                join ProcedureRouteType rt on rt.Id=r.ProcedureRouteTypeId
                join ProcedureStep fs on fs.Id=r.FromProcedureStepId
                join ProcedureStep ts on ts.Id=r.ToProcedureStepId
                where r.IsDeleted=0 and ((fs.ProcedureStepName like @SearchText) or (ts.ProcedureStepName like @SearchText) or (upper(r.TripleStoreId)=@TripleStoreId))",
                new { SearchText = $"%{searchText}%", TripleStoreId = searchText.ToUpper() });
            return GetItems<Route>(command);
        }

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
                where r.IsDeleted=0 and r.ProcedureId=@Id",
                new { Id=procedureId });
            return GetItems<Route>(command);
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
                where r.IsDeleted=0 and ((r.FromProcedureStepId=@StepId) or (r.ToProcedureStepId=@StepId))",
                new { StepId = stepId});
            return GetItems<Route>(command);
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
                join ProcedureStep ts on ts.Id=r.ToProcedureStepId");
            return GetItems<Route>(command);
        }

        [HttpGet]
        [ContentNegotiation("route/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetView(int id)
        {
            return RenderView("View",id);
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
                where r.Id=@Id",
                new { Id = id });
            return GetItem<Route>(command);
        }

        [HttpGet]
        [ContentNegotiation("route/edit/{id:int}", ContentType.HTML)]
        public IHttpActionResult GetEdit(int id)
        {
            return RenderView("Edit",id);
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
            CommandDefinition command = new CommandDefinition(@"update ProcedureRoute
                set IsDeleted=1,
                    ModifiedBy=@ModifiedBy,
                    ModifiedAt=@ModifiedAt
                where Id=@Id",
                new
                {
                    ModifiedBy = EMail,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    Id = id
                });
            return Execute(command);
        }
    }

}