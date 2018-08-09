using System;

namespace Parliament.ProcedureEditor.Web.Models
{
    public class LayingItem
    {
        public int Id { get; set; }
        
        public int ProcedureBusinessItemId { get; set; }
        public int ProcedureWorkPackagedId { get; set; }
        public int LayingBodyId { get; set; }
        public DateTimeOffset? LayingDate { get; set; }
        public string PersonTripleStoreId { get; set; }

        public string TripleStoreId { get; set; }
        public string WorkPackagedThingName { get; set; }
        public string LayingBodyName { get; set; }
    }
}