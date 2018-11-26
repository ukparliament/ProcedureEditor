using System.Collections.Generic;

namespace Parliament.ProcedureEditor.Web.Models
{
    public class Route
    {
        public int Id { get; set; }
        public string TripleStoreId { get; set; }
        public int FromProcedureStepId { get; set; }
        public int ToProcedureStepId { get; set; }
        public int ProcedureRouteTypeId { get; set; }
        public IEnumerable<int> Procedures { get; set; }

        public string ProcedureRouteTypeName { get; set; }
        public string FromProcedureStepName { get; set; }
        public string ToProcedureStepName { get; set; }
        public IEnumerable<string> FromProcedureStepHouseNames { get; set; }
        public IEnumerable<string> ToProcedureStepHouseNames { get; set; }
    }
}