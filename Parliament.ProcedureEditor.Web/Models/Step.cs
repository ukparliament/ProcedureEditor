using System.Collections.Generic;

namespace Parliament.ProcedureEditor.Web.Models
{
    public class Step
    {
        public int Id { get; set; }
        public string TripleStoreId { get; set; }
        public string ProcedureStepName { get; set; }
        public string ProcedureStepDescription { get; set; }
        public IEnumerable<int> Houses { get; set; }

        public IEnumerable<string> HouseNames { get; set; }
    }
}