using System;

namespace Parliament.ProcedureEditor.Web.Models
{
    public class BusinessItemCandidateModel
    {
        public int WorkPackagedId { get; set; }
        public string WorkPackagedTripleStoreId { get; set; }
        public string WorkPackagedThingName { get; set; }
        public string ProcedureName { get; set; }
        public DateTimeOffset? MadeDate { get; set; }
        public DateTimeOffset? LaidDate { get; set; }
        public string WebUrl { get; set; }
        public int? Number { get; set; }
        public string Prefix { get; set; }
        public int ProcedureId { get; set; }
    }
}