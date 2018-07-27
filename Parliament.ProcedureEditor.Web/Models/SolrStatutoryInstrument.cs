using System;

namespace Parliament.ProcedureEditor.Web.Models
{
    public class SolrStatutoryInstrument
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SIPrefix { get; set; }
        public string SINumber { get; set; }
        public string WebUrl { get; set; }
        public string ComingIntoForceNote { get; set; }
        public DateTimeOffset? ComingIntoForceDate { get; set; }
        public DateTimeOffset? MadeDate { get; set; }
        public string SIProcedure { get; set; }
    }
}