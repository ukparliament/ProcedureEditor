using System;

namespace Parliament.ProcedureEditor.Web.Models
{
    public class WorkPackageable
    {
        public int Id { get; set; }
        public string TripleStoreId { get; set; }
        public string ProcedureWorkPackageableThingName { get; set; }
        public string StatutoryInstrumentNumber { get; set; }
        public string StatutoryInstrumentNumberPrefix { get; set; }
        public int? StatutoryInstrumentNumberYear { get; set; }
        public string ComingIntoForceNote { get; set; }
        public string WebLink { get; set; }
        public int? ProcedureWorkPackageableThingTypeId { get; set; }
        public DateTimeOffset? ComingIntoForceDate { get; set; }
        public DateTimeOffset? TimeLimitForObjectionEndDate { get; set; }
        public string ProcedureWorkPackageTripleStoreId { get; set; }
        public int ProcedureId { get; set; }

        public string ProcedureName { get; set; }
        public DateTimeOffset? MostRecentBusinessItemDate { get; set; }

    }
}