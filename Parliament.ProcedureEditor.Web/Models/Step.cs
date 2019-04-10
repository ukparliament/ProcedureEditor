using System.Collections.Generic;

namespace Parliament.ProcedureEditor.Web.Models
{
    public class Step
    {
        public int Id { get; set; }
        public string TripleStoreId { get; set; }
        public string ProcedureStepName { get; set; }
        public string ProcedureStepDescription { get; set; }
        public string ProcedureStepScopeNote { get; set; }
        public string ProcedureStepLinkNote { get; set; }
        public string ProcedureStepDateNote { get; set; }
        public IEnumerable<int> Houses { get; set; }

        public IEnumerable<string> HouseNames { get; set; }
        public int? CommonlyActualisedAlongsideProcedureStepId { get; set; }
        public StepPublication Publication { get; set; }
    }
}